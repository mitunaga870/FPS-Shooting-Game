using System.Collections.Generic;
using System.Linq;
using AClass;
using lib;
using Unity.VisualScripting;
using UnityEngine;
using ATrap = AClass.ATrap;

public class DeckController : MonoBehaviour
{
    [SerializeField] private List<ATrap> defaultTraps;
    [SerializeField] private List<ASkill> defaultSkills;
    [SerializeField] private List<ATurret> defaultTurrets;

    private List<ATrap> _deckTraps = new List<ATrap>();
    private List<ASkill> _deckSkills = new List<ASkill>();
    private List<ATurret> _deckTurrets = new List<ATurret>();

    private List<ATrap> _discardTraps = new List<ATrap>();
    private List<ASkill> _discardSkills = new List<ASkill>();
    private List<ATurret> _discardTurrets = new List<ATurret>();

    private List<ATrap> _handTraps = new List<ATrap>();
    private List<ASkill> _handSkills = new List<ASkill>();
    private List<ATurret> _handTurrets = new List<ATurret>();


    private void Awake()
    {
        // セーブ読み込み
        var saveDataTuple = SaveController.LoadDeckData();

        // デッキを初期化
        if (saveDataTuple != null)
        {
            //セーブデータがある時
            var saveData = saveDataTuple.Value;

            // トラップ
            foreach (var trap in saveData.Traps)
            {
                _deckTraps.Add(TrapGenerator.GenerateTrap(trap.Trap));
            }
        }
        else
        {
            // セーブデータがないとき
            _deckTraps.Clear();
            _deckTraps.AddRange(defaultTraps);

            _deckSkills.Clear();
            _deckSkills.AddRange(defaultSkills);

            _deckTurrets.Clear();
            _deckTurrets.AddRange(defaultTurrets);
        }
    }

    public List<ATrap> DrowTraps(int amount = 1)
    {
        // 結果を格納するリスト
        var result = new List<ATrap>();

        // トラップが足りない場合はデッキをシャッフル
        if (_deckTraps.Count < amount)
        {
            // 今の分を手札に加える
            _handTraps.AddRange(_deckTraps);
            result.AddRange(_deckTraps);

            // 山札を消す
            _deckTraps.Clear();

            // 加えた分量から減らす
            amount -= _deckTraps.Count;

            // 捨てたトラップをデッキに戻す
            ResetTrapDeck();

            // デッキが補充されたかで判別、でっき切れの時はそのまま返す
            if (_deckTraps.Count > 0)
                // デッキがしっかり補充されたなら残りを引く
                result.AddRange(DrowTraps(amount));
        }
        else
        {
            // 引ききれる場合はランダムに取り出す
            for (var i = 0; i < amount; i++)
            {
                // ランダムな1枚を取り出して手札に加える
                var rand = Random.Range(0, _deckTraps.Count);

                _handTraps.Add(_deckTraps[rand]);

                // 引いたものを結果に加える
                result.Add(_deckTraps[rand]);

                // 引いたものをデッキから削除
                _deckTraps.RemoveAt(rand);
            }
        }

        return result;
    }

    /**
     * トラップデッキをリセットする
     * 捨てたトラップをデッキに戻す
     */
    public void ResetTrapDeck()
    {
        _deckTraps.AddRange(_discardTraps);
        _discardTraps.Clear();
    }

    /**
     * 手札を捨てる
     */
    public void DiscardHandTrap()
    {
        _discardTraps.AddRange(_handTraps);
        _handTraps.Clear();
    }

    /**
     * 山札にトラップを追加
     */
    public void AddTrap(ATrap trap)
    {
        _deckTraps.Add(trap);
    }

    /**
     * 山札にトラップを追加
     */
    public void AddTrapRange(List<ATrap> selectedTrap)
    {
        _deckTraps.AddRange(selectedTrap);
    }

    /**
     * 山札にタレットを追加
     */
    public void AddTurret(ATurret turret)
    {
        _deckTurrets.Add(turret);
    }

    /**
     * 山札にタレットを追加
     */
    public void AddTurretRange(List<ATurret> selectedTurret)
    {
        _deckTurrets.AddRange(selectedTurret);
    }

    /**
     * 山札にスキルを追加
     */
    public void AddSkill(ASkill skill)
    {
        _deckSkills.Add(skill);
    }

    /**
     * 山札にスキルを追加
     */
    public void AddSkillRange(List<ASkill> selectedSkill)
    {
        _deckSkills.AddRange(selectedSkill);
    }
}