//  PerlinNoiseGenerator.cs
//  http://kan-kikuchi.hatenablog.com/entry/PlayFromFirstScene
//
//  Created by kan.kikuchi on 2019.02.05.

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
  /// <summary>
  /// エディタを別メニューで再生するためのクラス
  /// </summary>
  [InitializeOnLoad]//エディター起動時にコンストラクタが呼ばれるように
  public static class EditorPlayer {

    //=================================================================================
    //初期化
    //=================================================================================

    /// <summary>
    /// コンストラクタ(InitializeOnLoad属性によりエディター起動時に呼び出される)
    /// </summary>
    static EditorPlayer() {
      EditorApplication.playModeStateChanged += OnChangedPlayMode;
    }

    //=================================================================================
    //プレイモードの変更
    //=================================================================================

    //プレイモードが変更された
    private static void OnChangedPlayMode(PlayModeStateChange state) {
      //エディタの実行が開始された時に、最初のシーンをnullにする(普通の再生ボタンを押した時に使われないように)
      if (state == PlayModeStateChange.EnteredPlayMode) {
        EditorSceneManager.playModeStartScene = null;
      }
    }

    //=================================================================================
    //再生
    //=================================================================================

    /// <summary>
    /// Scenes in Buildの一番上に登録されているシーンから再生を開始する
    /// </summary>
    [MenuItem("Tools/Play From First Scene")]
    public static void PlayFromFirstScene() {
      //シーンが設定されてるかチェックし、されていなければエラーを出して終了
      if(EditorBuildSettings.scenes.Length == 0){
        Debug.LogError("Scenes in Build にシーンが登録されていません!");
        return;
      }

      //Scenes in Buildの一番上に登録されているシーンのパスを取得し、再生開始
      Play(EditorBuildSettings.scenes[0].path);
    }

    /// <summary>
    /// 最初のシーンをパスで指定し、再生を開始する
    /// </summary>
    public static void Play(string scenePath) {
      //SceneAssetをロード
      SceneAsset sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

      //シーンが存在しない場合はエラーを出して終了
      if(sceneAsset == null){
        Debug.LogError(scenePath + "というシーンアセットは存在しません!");
        return;
      }

      //エディタ実行時の最初のシーンに取得したシーンを設定
      EditorSceneManager.playModeStartScene = sceneAsset;

      //エディタの再生開始
      EditorApplication.isPlaying = true;
    }

  }
}
