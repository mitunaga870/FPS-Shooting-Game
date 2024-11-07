using InvasionPhase;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;

public class HpuiController : MonoBehaviour
{
    [SerializeField]
    private GameObject lifeIconPrefab;
    
    [SerializeField]
    private Transform lifeIconContainer;
    
    [SerializeField]
    private InvasionController invasionController;
    
    [SerializeField]
    private GeneralS2SData generalS2SData;
    
    // 前回読み込んだ時のHP
    private int _prevHp;
    
    // invasionControllerがあるか
    private bool _hasInvasionController;

    private void Start()
    {
        SetHP(generalS2SData.PlayerHp);
        
        // invasionControllerがあるか
        if (invasionController != null)
        {
            _hasInvasionController = true;
        }
    }
    
    private void FixedUpdate()
    {
        // HPが変更されたらUIを更新
        if (!_hasInvasionController || _prevHp == invasionController.PlayerHp) return;
        
        SetHP(invasionController.PlayerHp);
        _prevHp = invasionController.PlayerHp;
    }


    // ReSharper disable once InconsistentNaming
    private void SetHP(int HP)
    {
        for (var i = 0; i < lifeIconContainer.childCount; i++)
        {
            Destroy(lifeIconContainer.GetChild(i).gameObject);
        }
        
        for (var i = 0; i < HP; i++)
        {
            Instantiate(lifeIconPrefab, lifeIconContainer);
        }
    }
}