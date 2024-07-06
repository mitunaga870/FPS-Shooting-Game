using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /** カメラの初期高さ */
    private const float CameraHeight = 10;

    /** 制限のオフセット */
    private const float LimitOffset = 2.5f;

    /** 高さ制限の幅 */
    private const float HeightLimit = 5;

    /** マウス感度 */
    private const float MouseSensitivity = 0.8f;

    /** マウスの横制限 */
    private float _mouseXLimit;

    /** マウスの縦制限 */
    private float _mouseZLimit;

    /** 迷路コントローラ */
    [SerializeField] private MazeController mazeController;

    // Start is called before the first frame update
    void Start()
    {
        _mouseXLimit = mazeController.MazeData.MazeColumns * 0.5f;
        _mouseZLimit = mazeController.MazeData.MazeRows * 0.5f;
        Debug.Log(_mouseXLimit);
        Debug.Log(_mouseZLimit);
    }

    // Update is called once per frame
    void Update()
    {
        var cam = GetComponent<MainCamera>();

        // マウスの位置でカメラを移動
        if (Input.GetMouseButton(2))
        {
            var moveX = Input.GetAxis("Mouse X") * MouseSensitivity;
            var moveZ = Input.GetAxis("Mouse Y") * MouseSensitivity;
            cam.transform.localPosition -= new Vector3(moveX, 0, moveZ);
        }

        // マウスホイールでズーム
        if (Input.mouseScrollDelta.y != 0)
        {
            var moveY = Input.mouseScrollDelta.y;
            cam.transform.localPosition -= new Vector3(0, moveY, 0);
        }


        // カメラの位置を制限
        var pos = cam.transform.localPosition;
        pos.x = Mathf.Clamp(pos.x, -_mouseXLimit + LimitOffset, _mouseXLimit - LimitOffset);
        pos.z = Mathf.Clamp(pos.z, -_mouseZLimit + LimitOffset, _mouseZLimit - LimitOffset);
        pos.y = Mathf.Clamp(pos.y, CameraHeight - HeightLimit, CameraHeight + HeightLimit);
        cam.transform.localPosition = pos;
    }
}