Shader "ShirayuriMeshibe/SimpleLightSource"
{
    Properties
    {
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _EmissionIntensity ("Emission Intensity", Float) = 1

        [Toggle(USE_EMISSION_TEXTURE)] _UseEmissionTexture ("Use Emission Texture", Float) = 0
        [KeywordEnum(R, RGB)] _EmissionTextureChannel ("Emission Texture Channel", Float) = 0
		[NoScaleOffset] _EmissionTexture ("Emission Texture", 2D) = "white" {}

        [Enum(UnityEngine.Rendering.CullMode)] _Cull("Cull", Float) = 2
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
            "DisableBatching" = "True"
        }

        Pass
        {
            Cull [_Cull]

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma exclude_renderers gles
            #pragma target 3.0

			#pragma multi_compile_instancing
			#pragma instancing_options procedural:vertInstancingSetup

            #include "UnityCG.cginc"
            #include "UnityStandardParticleInstancing.cginc"

            #pragma shader_feature_local _ USE_EMISSION_TEXTURE
            #pragma shader_feature_local _EMISSIONTEXTURECHANNEL_R _EMISSIONTEXTURECHANNEL_RGB
            #pragma shader_feature_local _ USE_OFF_COLOR

    		uniform float3 _Color;
            uniform float _EmissionIntensity;

#if defined(USE_EMISSION_TEXTURE)
            uniform sampler2D _EmissionTexture;
#endif

            struct VertexInput
            {
                float4 vertex : POSITION;
#if defined(USE_EMISSION_TEXTURE)
                float3 uv0 : TEXCOORD0;
#endif
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct VertexOutput
            {
                float4 pos : SV_POSITION;
#if defined(USE_EMISSION_TEXTURE)
                float3 uv0 : TEXCOORD0;
#endif
                UNITY_VERTEX_OUTPUT_STEREO
            };

            VertexOutput vert(VertexInput v)
            {
                VertexOutput o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_OUTPUT(VertexOutput, o);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            float4 frag(VertexOutput i) : SV_TARGET
            {
                float3 c0 = _Color.rgb * _EmissionIntensity;
#if defined(USE_EMISSION_TEXTURE)
                float4 c1 = tex2D(_EmissionTexture, i.uv0);
    #if defined(_EMISSIONTEXTURECHANNEL_R)
                c0.rgb *= c1.r;
    #elif defined(_EMISSIONTEXTURECHANNEL_RGB)
                c0.rgb *= Luminance(c1.rgb);
    #endif
#endif
                float4 f = float4(c0, 1.0);
                return f;
            }
            ENDCG
        }
        UsePass "VertexLit/SHADOWCASTER"
    }
}
