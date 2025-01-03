﻿using System.Collections.Generic;
using AClass;
using CreatePhase;
using Map.UI.lib;
using UI;
using UnityEngine;

namespace Turrets
{
    public class TurretController : MonoBehaviour
    {
        [SerializeField]
        private DeckController deckController;
        
        [SerializeField]
        private GameObject turretUIWrapper;
        
        [SerializeField]
        private MazeCreationController mazeCreationController;

        [SerializeField]
        private TurretIconGenerator _turretIconGenerator;
        
        [SerializeField]
        private DetailViewerController detailViewerController;
        
        /** 追加済みタレットの辞書 */
        private Dictionary<string, TurretIcon> _addedTurrets = new Dictionary<string, TurretIcon>();
        
        private void Start()
        {
            // デッキを読んでUIに表示
            var deckTurrets = deckController.DrawTurrets();
            
            AddTurret(deckTurrets);
        }

        /** turretをUIに追加する */
        public void AddTurret(IEnumerable<ATurret> turrets)
        {
            foreach (var turret in turrets)
            {
                var turretName = turret.GetTurretName();
                
                // 既に追加済みのタレットは追加せず所持数を増やす
                if (_addedTurrets.ContainsKey(turretName))
                {
                    _addedTurrets[turretName].IncreaseCount();
                }
                else
                {
                    var icon = _turretIconGenerator.getIcon(turretName);
                    icon = Instantiate(icon, turretUIWrapper.transform, false);
                    
                    // タレットを追加
                    _addedTurrets[turretName] = icon;

                    icon.Init(mazeCreationController, detailViewerController);
                }
            }
        }
    }
}