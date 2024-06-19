using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    /** カメラの初期高さ */
    private const float CAMERA_HEIGHT = 10;

    /** 制限のオフセット */
    private const float LIMIT_OFFSET = 2.5f;

    /** 高さ制限の幅 */
    private const float HEIGHT_LIMIT = 5;

    /** マウス感度 */
    private const float MOUSE_SENSITIVETY = 0.8f;

    /** マウスの横制限 */
    private float mouseXLimit;

    /** マウスの縦制限 */
    private float mouseZLimit;

    /** 迷路コントローラ */
    [SerializeField] private MazeController mazeController;

    // Start is called before the first frame update
    void Start()
    {
        mouseXLimit = mazeController.MazeData.MAZE_COLUMNS * 0.5f;
        mouseZLimit = mazeController.MazeData.MAZE_ROWS * 0.5f;
        Debug.Log(mouseXLimit);
        Debug.Log(mouseZLimit);
    }

    // Update is called once per frame
    void Update()
    {
        var cam = GetComponent<Camera>();

        // マウスの位置でカメラを移動
        if (Input.GetMouseButton(2))
        {
            float moveX = Input.GetAxis("Mouse X") * MOUSE_SENSITIVETY;
            float moveZ = Input.GetAxis("Mouse Y") * MOUSE_SENSITIVETY;
            cam.transform.localPosition -= new Vector3(moveX, 0, moveZ);
        }

        // マウスホイールでズーム
        if (Input.mouseScrollDelta.y != 0)
        {
            float moveY = Input.mouseScrollDelta.y;
            cam.transform.localPosition -= new Vector3(0, moveY, 0);
        }


        // カメラの位置を制限
        var pos = cam.transform.localPosition;
        pos.x = Mathf.Clamp(pos.x, -mouseXLimit + LIMIT_OFFSET, mouseXLimit - LIMIT_OFFSET);
        pos.z = Mathf.Clamp(pos.z, -mouseZLimit + LIMIT_OFFSET, mouseZLimit - LIMIT_OFFSET);
        pos.y = Mathf.Clamp(pos.y, CAMERA_HEIGHT - HEIGHT_LIMIT, CAMERA_HEIGHT + HEIGHT_LIMIT);
        cam.transform.localPosition = pos;
    }
}