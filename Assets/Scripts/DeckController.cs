using System;
using System.Collections.Generic;
using System.Linq;
using AClass;
using lib;
using ScriptableObjects;
using ScriptableObjects.S2SDataObjects;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using ATrap = AClass.ATrap;
using Random = UnityEngine.Random;

public class DeckController : MonoBehaviour
{
    [SerializeField]
    private DeckObject deckObject;

    [SerializeField]
    private GeneralS2SData generalS2SData;
    
    [SerializeField]
    private DeckUIController deckUIController;
    
    private List<ATrap> DefaultTraps => deckObject.DefaultTraps;
    private List<ASkill> DefaultSkills => deckObject.DefaultSkills;
    private List<ATurret> DefaultTurrets => deckObject.DefaultTurrets;

    private List<ATrap> _deckTraps = new();
    private List<ASkill> _deckSkills = new();
    private List<ATurret> _deckTurrets = new();

    private List<ATrap> _discardTraps = new();

    private List<ATrap> _handTraps = new();

    public int TrapDeckCount => _deckTraps.Count;
    public int SkillDeckCount => _deckSkills.Count;
    
    public bool HasTrap => _deckTraps.Count > 0;
    public bool HasSkill => _deckSkills.Count > 0;
    public bool HasTurret => _deckTurrets.Count > 0;

    private void Awake()
    {
        // セーブ読み込み
        var saveDataTuple = SaveController.LoadDeckData();
        
        // S2SData読み込み
        var s2SData = generalS2SData.GetDeckData();

        // デッキを初期化
        if (saveDataTuple != null)
        {
            //セーブデータがある時
            var saveData = saveDataTuple.Value;

            // デッキトラップ
            foreach (var trap in saveData.DeckTraps) _deckTraps.Add(trap);
            // ハンドトラップ
            foreach (var trap in saveData.HandTraps) _handTraps.Add(trap);
            // 捨てたトラップ
            foreach (var trap in saveData.DiscardTraps) _discardTraps.Add(trap);
            // スキル
            foreach (var skill in saveData.Skills) _deckSkills.Add(skill);
            // タレット
            foreach (var turret in saveData.Turrets) _deckTurrets.Add(turret);
        }
        else if (s2SData != null)
        {
            // S2SDataがある時
            var s2SDataValue = s2SData.Value;

            // トラップ
            foreach (var trap in s2SDataValue.deckTraps) _deckTraps.Add(trap);
            foreach (var trap in s2SDataValue.handTraps) _handTraps.Add(trap);
            foreach (var trap in s2SDataValue.discardTraps) _discardTraps.Add(trap);
            // スキル
            foreach (var skill in s2SDataValue.skills) _deckSkills.Add(skill);
            // タレット
            foreach (var turret in s2SDataValue.turrets) _deckTurrets.Add(turret);
        }
        else
        {
            // セーブデータがないとき
            _deckTraps.Clear();
            _deckTraps.AddRange(DefaultTraps);

            _deckSkills.Clear();
            _deckSkills.AddRange(DefaultSkills);

            _deckTurrets.Clear();
            _deckTurrets.AddRange(DefaultTurrets);
        }
    }
    
    private void OnDestroy()
    {
        // s2sDataにデッキデータを保存
        generalS2SData.SetDeckData(
            _deckTraps.ToArray(), _handTraps.ToArray(), _discardTraps.ToArray(),
            _deckSkills.ToArray(), _deckTurrets.ToArray());
    }

    private void OnApplicationQuit()
    {
        // セーブ
        Save();
    }
    
    public void Save()
    {
        // セーブ
        SaveController.SaveDeckData(
            _deckTraps.ToArray(), _handTraps.ToArray(), _discardTraps.ToArray(),
            _deckSkills.ToArray(), _deckTurrets.ToArray());
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

    /**
     * スキルを取得
     */
    public List<ASkill> DrawSkills()
    {
        return _deckSkills;
    }

    public void UseSkill(string skillName)
    {
        // 使用したスキルを捨てる
        var skill = _deckSkills.FirstOrDefault(s => s.GetSkillName() == skillName);
        
        if (skill == null) return;
        
        _deckSkills.Remove(skill);
    }

    /**
     * タレットを取得
     */
    public List<ATurret> DrawTurrets()
    {
        return _deckTurrets;
    }

    public (List<ATrap> traps, List<ASkill> skills, List<ATurret> turrets) LoadDeck()
    {
        var traps = new List<ATrap>();
        var skills = new List<ASkill>();
        var turrets = new List<ATurret>();
        
        traps.AddRange(_deckTraps);
        traps.AddRange(_handTraps);
        traps.AddRange(_discardTraps);
        
        skills.AddRange(_deckSkills);
        turrets.AddRange(_deckTurrets);
        
        return (traps, skills, turrets);
    }
}