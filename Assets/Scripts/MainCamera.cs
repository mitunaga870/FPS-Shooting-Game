using AClass;
using DataClass;
using ScriptableObjects.S2SDataObjects;
using Shop;
using UI;
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
     * ショップUI
     */
    [SerializeField]
    private ShopController _shopController;

    /**
     * マウスの横制限
     */
    private float _mouseXLimit;

    /**
     * マウスの縦制限
     */
    private float _mouseZLimit;
    
    // ================= UIの表示情報 =================
    private bool _hasDeckUI;
    private bool _hasShopController;

    private StageData _stageData => _mazeController.StageData;

    // Start is called before the first frame update
    private void Start()
    {
        _mouseXLimit = _stageData.mazeColumn * 0.5f;
        _mouseZLimit = _stageData.mazeRow * 0.5f;
        
        if (_deckUIController != null)
            _hasDeckUI = true;
        
        if (_shopController != null)
            _hasShopController = true;
    }

    // Update is called once per frame
    private void Update()
    {
        var cam = GetComponent<MainCamera>();
        
        // デッキUIが表示されている場合はカメラの移動を制限
        if (_hasDeckUI && _deckUIController.IsDeckUIShowing)
            return;
        // ショップUIが表示されている場合はカメラの移動を制限
        if (_hasShopController && _shopController.IsShopUIShowing)
            return;

        // マウスの位置でカメラを移動
        if (Input.GetMouseButton(2))
        {
            var moveX = Input.GetAxis("Mouse X") * MouseSensitivity;
            var moveZ = Input.GetAxis("Mouse Y") * MouseSensitivity;
            cam.transform.localPosition -= new Vector3(moveX, 0, moveZ);
        }

        // マウスホイールでズーム
        if (Input.mouseScrollDelta.y != 0 && !_deckUIController.IsDeckUIShowing)
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