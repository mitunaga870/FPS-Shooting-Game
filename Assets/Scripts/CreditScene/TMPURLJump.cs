using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

//https://ekifurulab.com/textmeshprolink/

public class TMPURLJump : MonoBehaviour , IPointerClickHandler 
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //3.クリックした時のマウスカーソルの位置を取得します。
        Vector2 pos = Input.mousePosition;

        //4.このスクリプトのついたゲームオブジェクトから「TextMeshProUGUI」コンポーネントを取得し、text変数に代入。
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();

        //5.text=TextMeshProUGUIコンポーネントのルートにもっとも近いCanvasオブジェクトを代入。
        Canvas canvas = text.canvas;

        //6.カメラの変数を生成して、canvasのrenderModeを三項条件演算子で評価してカメラを代入。
        Camera camera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;

        //7.FindIntersectingLink(TMP_Text, Vector3, Camera)で指定された位置にのリンクのインデックスを返します。(存在する場合)
        int index = TMP_TextUtilities.FindIntersectingLink(text, pos, camera);

        //8.インデックスに要素が存在しない場合は-1を返すので-1では無い場合に処理を続行します。
        if (index != -1)
        {
            //9.テキストオブジェクトにリンク情報IDを取得します。
            TMP_LinkInfo linkUrlInfo = text.textInfo.linkInfo[index];

            //10.文字列urlにGetLinkIDで取得したリンクIDを文字列として返して代入。
            string url = linkUrlInfo.GetLinkID();

            //11.Web ブラウザーで url の場所を開きます。
            Application.OpenURL(url);
        }
    }
}
