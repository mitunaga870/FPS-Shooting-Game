using CreatePhase;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Serialization;

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

    /** ステージデータ */
    [FormerlySerializedAs("_stageData")] [SerializeField]
    private StageObject stageObject;

    /** 一般情報 */
    [SerializeField] private GeneralS2SData _generalS2SData;

    // Start is called before the first frame update
    void Start()
    {
        _mouseXLimit = stageObject.GetMazeColumns(0) * 0.5f;
        _mouseZLimit = stageObject.GetMazeRows(0) * 0.5f;
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