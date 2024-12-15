using AClass;
using DataClass;
using Reward;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    /**
     * カメラの初期高さ
     */
    private const float CameraHeight = 10;

    /**
     * 制限のオフセット
     */
    private const float LimitOffset = 2.5f;

    /**
     * 高さ制限の幅
     */
    private const float HeightLimit = 5;

    /**
     * マウス感度
     */
    private const float MouseSensitivity = 0.8f;

    /**
     * 迷路情報
     */
    [SerializeField]
    private AMazeController _mazeController;

    /**
     * 一般情報
     */
    [SerializeField]
    private GeneralS2SData _generalS2SData;
    
    /**
     * デッキの高さ
     */
    [SerializeField]
    private DeckUIController _deckUIController;
    
    /**
     * リワードUI
     */
    [SerializeField]
    private RewardUIController _rewardUIController;

    /**
     * マウスの横制限
     */
    private float _mouseXLimit;

    /**
     * マウスの縦制限
     */
    private float _mouseZLimit;

    private StageData _stageData => _mazeController.StageData;
    
    // UI要素があるかどうかのフラグ
    private bool _hasRewardUI;

    // Start is called before the first frame update
    private void Start()
    {
        _mouseXLimit = _stageData.mazeColumn * 0.5f;
        _mouseZLimit = _stageData.mazeRow * 0.5f;
        
        // UI要素があるかどうかのフラグ
        _hasRewardUI = _rewardUIController != null;
    }

    // Update is called once per frame
    private void Update()
    {
        var cam = GetComponent<MainCamera>();
        
        // 各種UIが表示されている場合はカメラの移動を制限
        if (
         _hasRewardUI && _rewardUIController.IsRewardUIShowing
        ) {
            return;
        }

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