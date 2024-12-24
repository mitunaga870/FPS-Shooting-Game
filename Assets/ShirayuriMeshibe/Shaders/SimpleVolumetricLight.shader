Shader "ShirayuriMeshibe/SimpleVolumetricLight"
{ 
	Properties
	{
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _EmissionIntensity ("Emission Intensity", Float) = 1

        _LightSize ("Light Size", Range(1, 1000)) = 1
        _LightLength ("Light Length", Float) = 1

        [Space]
        [Toggle(USE_SOFT_EDGE)] _UseSoftEdge ("Use Soft Edge", Float) = 0
        _FresnelPower ("Fresnel Power", Float) = 1

        [Space]
        _FallOffPower ("FallOff Power", Float) = 4

        [KeywordEnum(NONE, HEIGHT_FALLOFF, DEPTH_FADE)] _FadeMode ("FadeMode", Float) = 0

        [Header(Height FallOff)]
        _HeightFallOffStartPos ("Height FallOff Start Pos(World Y)", Float) = 0
        _HeightFallOffHeight ("Height FallOff Height", Float) = 1

        [Header(Depth Fade(Required Camera Depth Mode))]
        _DepthFadeDensity ("Depth Fade Density", Range(0, 1)) = 0.1

        [Space]
        [Toggle(USE_NOISE_TEXTURE)] _UseNoiseTexture ("Use Noise Texture", Float) = 0
		_NoiseTex ("Noise Texture", 2D) = "white" {}

        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
	}

	SubShader
	{
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "DisableBatching" = "True"
        }

		Pass
		{
            Blend SrcAlpha One
            Cull [_Cull]
            Lighting Off
            ZWrite Off
            ColorMask RGB

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
            #pragma exclude_renderers gles
            #pragma target 3.0

			#pragma multi_compile_instancing
			#pragma instancing_options procedural:vertInstancingSetup
				
			#include "UnityCG.cginc"
            #include "UnityStandardParticleInstancing.cginc"

            #pragma multi_compile_local _ USE_SOFT_EDGE
            #pragma multi_compile_local _ _FADEMODE_HEIGHT_FALLOFF _FADEMODE_DEPTH_FADE
            #pragma multi_compile_local _ USE_NOISE_TEXTURE

            struct appdata
            {
                float4 vertex : POSITION;
#if defined(USE_SOFT_EDGE)
                float4 normal : NORMAL;
                float4 tangent : TANGENT;
#endif
                float2 uv0 : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
#if defined(USE_SOFT_EDGE)
                float vdotn : TEXCOORD1;
#endif
#if defined(_FADEMODE_HEIGHT_FALLOFF)
                float4 posWorld : TEXCOORD2;
#elif defined(_FADEMODE_DEPTH_FADE)
                float4 projPos : TEXCOORD2;
#endif
#if defined(USE_NOISE_TEXTURE)
                float2 uv1 : TEXCOORD3;
#endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

    		uniform float3 _Color;
            uniform float _EmissionIntensity;
            uniform float _LightSize;
            uniform float _LightLength;
            uniform float _FresnelPower;
            uniform float _FallOffPower;

#if defined(_FADEMODE_HEIGHT_FALLOFF)
            uniform float _HeightFallOffStartPos;
            uniform float _HeightFallOffHeight;
#elif defined(_FADEMODE_DEPTH_FADE)
            UNITY_DECLARE_DEPTH_TEXTURE(_CameraDepthTexture);
            uniform float _DepthFadeDensity;
#endif

#if defined(USE_NOISE_TEXTURE)
            uniform sampler2D _NoiseTex;
            uniform float4 _NoiseTex_ST;
#endif

            inline float3 ScaledVolimetricLight(float3 pos)
            {
                pos.xz *= lerp(1.0, _LightSize, pos.y * 0.1);
                pos.y *= _LightLength;
                return pos;
            }

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(v2f, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                float4 vertex = v.vertex;
                // vertex.xz *= lerp(1.0, _LightSize, vertex.y * 0.1);
                // vertex.y *= _LightLength;
                vertex.xyz = ScaledVolimetricLight(vertex.xyz);

                o.pos = UnityObjectToClipPos(vertex);
#if defined(_FADEMODE_HEIGHT_FALLOFF)
                o.posWorld = mul(unity_ObjectToWorld, vertex);
#elif defined(_FADEMODE_DEPTH_FADE)
                o.projPos = ComputeScreenPos(o.pos);

                // MEMO: COMPUTE_EYEDEPTHマクロはv.vertexを使用してしまうため頂点変更したものは直接計算する
                o.projPos.z = -UnityObjectToViewPos(float4(vertex.xyz, 1.0)).z;
#endif

#if defined(USE_SOFT_EDGE)
                float3 binormal = normalize(cross(v.normal.xyz, v.tangent.xyz));
                binormal += v.vertex.xyz;
                binormal = ScaledVolimetricLight(binormal);
                binormal = normalize(binormal - vertex.xyz);

                float3 tangent = v.tangent.xyz + v.vertex.xyz;
                tangent = ScaledVolimetricLight(tangent);
                tangent = normalize(tangent - vertex.xyz);

                float3 viewDir = normalize(mul(unity_ObjectToWorld, ObjSpaceViewDir(vertex)));
                float3 normal = UnityObjectToWorldNormal(cross(tangent, binormal));
                o.vdotn = pow(max(0.0, dot(viewDir, normal)), _FresnelPower);
#endif
                o.uv0 = v.uv0;

#if defined(USE_NOISE_TEXTURE)
                o.uv1 = TRANSFORM_TEX(v.uv0, _NoiseTex);
#endif
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float fallOff = pow(i.uv0.y, _FallOffPower);
                float3 c0 = _Color * _EmissionIntensity * fallOff;

#if defined(USE_SOFT_EDGE)
                c0 *= i.vdotn;
#endif

#if defined(USE_NOISE_TEXTURE)
                float n0 = tex2D(_NoiseTex, i.uv1).r;
                c0.rgb *= n0;
#endif

#if defined(_FADEMODE_HEIGHT_FALLOFF)
                c0 *= smoothstep(_HeightFallOffStartPos, _HeightFallOffStartPos + _HeightFallOffHeight, i.posWorld.y);
#elif defined(_FADEMODE_DEPTH_FADE)
                float depth0 = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float depth1 = i.projPos.z;
                float fade = saturate(_DepthFadeDensity * (depth0 - depth1));
                c0 *= fade;
#endif

                c0 = clamp(c0, 0.0, 30.0);
                float4 f = float4(c0, 1.0);
                return f;
            }
            ENDCG
		}
	}
}