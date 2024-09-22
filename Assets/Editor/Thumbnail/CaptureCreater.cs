
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CaptureCreater : EditorWindow
{

	// このディレクトリ以下のprefabのキャプチャを全て取得
	UnityEngine.Object searchDirectory;
	List<GameObject> objList = new List<GameObject>();
	string dirPath = "Assets/images/Captures/"; // 出力先ディレクトリ(Assets/Captures/以下に出力されます)
	int width = 100; // キャプチャ画像の幅
	int height = 100; // キャプチャ画像の高さ
	
	[MenuItem("Window/CaptureCreater")]
	static void ShowWindow()
	{
		EditorWindow.GetWindow (typeof (CaptureCreater));
	}
	
	void OnGUI()
	{
		// Unity EditorのUI
		GUILayout.BeginHorizontal();
		GUILayout.Label("Search Directory : ", GUILayout.Width(110));
		searchDirectory = EditorGUILayout.ObjectField(searchDirectory, typeof(UnityEngine.Object), true);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Save directory : ", GUILayout.Width(110));
		dirPath = (string)EditorGUILayout.TextField(dirPath);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Width : ", GUILayout.Width(110));
		width = EditorGUILayout.IntField(width);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("Height : ", GUILayout.Width(110));
		height = EditorGUILayout.IntField(height);
		GUILayout.EndHorizontal();
		EditorGUILayout.Space();
		
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button(new GUIContent("Capture")))
		{

			if(searchDirectory == null) return;
			
			// 出力先ディレクトリを生成
			if (!System.IO.File.Exists(dirPath))
			{
				System.IO.Directory.CreateDirectory(dirPath);
			}

			 objList.Clear();
			
			// 指定ディレクトリ内のprefabを全て取り出してListに入れる
			string replaceDirectoryPath = AssetDatabase.GetAssetPath(searchDirectory);
			string[] filePaths = Directory.GetFiles( replaceDirectoryPath , "*.*" );
			foreach(string filePath in filePaths)
			{
				GameObject obj =  AssetDatabase.LoadAssetAtPath( filePath , typeof(GameObject)) as GameObject;
				if(obj != null){
					 objList.Add(obj);
				}
			}
			
			EditorCoroutine.Start(Exec(objList));

			GUILayout.EndHorizontal();
			EditorGUILayout.Space();
		}
	}

	// List内のGameObjectを配置しつつ、キャプチャを取得
	IEnumerator Exec(List<GameObject> objList){
		foreach(GameObject obj in objList)
		{
			// Instantiateして向きを調整して取りやすい位置に
			GameObject unit = Instantiate(obj , Vector3.zero , Quaternion.identity) as GameObject;
			unit.transform.eulerAngles = new Vector3(270.0f , 0.0f , 0.0f);
			
			yield return new EditorCoroutine.WaitForSeconds(1.0f);

			Capture(obj.name);

			// キャプチャ撮った後は捨てる
			DestroyImmediate(unit);
		}
	}
	
	void Capture(string fileName)
	{
		Vector3 nowPos 	= Camera.main.transform.position;
		float nowSize 	= Camera.main.orthographicSize;
		
		// カメラ調整
		Camera.main.transform.position 	= new Vector3 (nowPos.x, nowPos.y, nowPos.z);
		Camera.main.orthographicSize 	= 100.0f;
		
		// RenderTextureを生成して、これに現在のSceneに映っているものを書き込む
		RenderTexture renderTexture = new RenderTexture(width, height, 24);
		Camera.main.targetTexture = renderTexture;
		Camera.main.Render();
		RenderTexture.active = renderTexture;
		Texture2D texture2D = new Texture2D(width, height, TextureFormat.ARGB32, false);
		texture2D.ReadPixels( new Rect(0, 0, width, height), 0, 0);
		Camera.main.targetTexture = null;
		
		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				Color c = texture2D.GetPixel(x,y);
				c = new Color(c.r , c.g , c.b , c.a);
				texture2D.SetPixel(x , y , c);
			}
		}

		// textureのbyteをファイルに出力
		byte[] bytes = texture2D.EncodeToPNG();
		System.IO.File.WriteAllBytes( dirPath  + fileName + ".png", bytes );
		Debug.Log("textureのbyteをファイルに出力");
		
		// 後処理
		Camera.main.targetTexture = null;
		RenderTexture.active = null;
		renderTexture.Release();
		
		// カメラを元に戻す
		Camera.main.transform.position = nowPos;
		Camera.main.orthographicSize = nowSize;
		Resources.UnloadUnusedAssets();
		System.GC.Collect();
	}
}