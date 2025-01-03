using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
 
public class MatAssign : MonoBehaviour
{
    [MenuItem("Tools/MatAssign")]
    static void MaterialAssign()
    {
        List<Material> mlist = new List<Material>();
        List<Texture2D> tlist = new List<Texture2D>();
 
 
        //選択したフォルダの取得
        Object[] selectedAsset = Selection.GetFiltered (typeof(Object), SelectionMode.Assets);
        foreach (var sel in selectedAsset) {
            Debug.Log (sel.name + " : " + sel.GetType ());
            //選択したものがフォルダなら、フォルダ内にあるマテリアルとテクスチャを検索してリストに追加
            if ((sel.GetType ().ToString () == "UnityEditor.DefaultAsset")){
                var folder = (DefaultAsset)sel;
                var dirpath = AssetDatabase.GetAssetPath(folder);
 
                string[] diraug = {dirpath};
                //マテリアルに対する処理
                var mfiles = AssetDatabase.FindAssets("t:Material",diraug);
                foreach(var mf in mfiles){
                    var mfpath = AssetDatabase.GUIDToAssetPath(mf);
                    var mfasset = AssetDatabase.LoadAllAssetsAtPath(mfpath);
                    foreach(var mfa in mfasset){
                        mlist.Add((Material)mfa);
                    }
                }
                //テクスチャに対する処理
                var tfiles = AssetDatabase.FindAssets("t:Texture2D",diraug);
                foreach(var tf in tfiles){
                    var tfpath = AssetDatabase.GUIDToAssetPath(tf);
                    var tfasset = AssetDatabase.LoadAllAssetsAtPath(tfpath);
                    foreach(var tfa in tfasset){
                        tlist.Add((Texture2D)tfa);
                        Debug.Log(tfa);
                    }
                }
            }
        }
        //マテリアルリストをfor文で回して名前の種類によって処理を分ける
        foreach (var mat in mlist)
        {
            if(mat.name.Contains("_body_"))
            {
                foreach (var tex in tlist)
                {
                    if(tex.name.Contains("_body")){
                        mat.SetTexture("_BaseMap",tex);
                        mat.SetTexture("_ShadeMap",tex);
                    }
                }
            }
            else if(mat.name.Contains("_face_"))
            {
                foreach (var tex in tlist)
                {
                    if(tex.name.Contains("_face")){
                        mat.SetTexture("_BaseMap",tex);
                        mat.SetTexture("_ShadeMap",tex);
                        mat.SetColor("_ShadeColor",Color.white);
                    }
                }
            }
            else if(mat.name.Contains("_head_"))
            {
                foreach (var tex in tlist)
                {
                    if(tex.name.Contains("_head")){
                        mat.SetTexture("_BaseMap",tex);
                    }
                }
            }
            else
            {
                continue;
            }
        }
    }
}
