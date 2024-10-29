#nullable enable
using System;
using System.Collections.Generic;
using CreatePhase;
using DataClass;
using Enums;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.PlayerLoop;

#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。Null 許容として宣言することをご検討ください。

namespace AClass
{
    public abstract class ATurret : MonoBehaviour
    {
        [SerializeField]
        protected TurretObject turretObject;

        private CreationSceneController _creationSceneController;

        private MazeCreationController _mazeCreationController;

        private InvasionController _invasionController;

        private InvasionMazeController _invasionMazeController;

        private InvasionEnemyController _invasionEnemyController;

        private bool _isInitialized;

        private bool _isEnable;

        protected Phase Phase;

        protected (CreationSceneController?, InvasionController?) GetControllers()
        {
            switch (Phase)
            {
                case Phase.Create:
                    return (_creationSceneController, null);
                case Phase.Invade:
                    return (null, _invasionController);
                default:
                    throw new Exception("Invalid phase");
            }
        }

        protected (MazeCreationController?, InvasionMazeController?) GetMazeControllers()
        {
            switch (Phase)
            {
                case Phase.Create:
                    return (_mazeCreationController, null);
                case Phase.Invade:
                    return (null, _invasionMazeController);
                default:
                    throw new Exception("Invalid phase");
            }
        }

        protected InvasionEnemyController? GetInvasionEnemyController()
        {
            return _invasionEnemyController;
        }

        /**
         * 制作phase用の初期化処理
         */
        public void InitializeCreationTurret(CreationSceneController sceneController,
            MazeCreationController mazeController)
        {
            if (_isInitialized) throw new Exception("Already initialized");

            _creationSceneController = sceneController;
            _mazeCreationController = mazeController;

            Phase = Phase.Create;

            _isInitialized = true;
        }

        /**
         * 侵略phase用の初期化処理
         */
        public void InitializeInvasionTurret(InvasionController invasionController,
            InvasionMazeController invasionMazeController, InvasionEnemyController invasionEnemyController)
        {
            if (_isInitialized) throw new Exception("Already initialized");

            _invasionController = invasionController;
            _invasionMazeController = invasionMazeController;
            _invasionEnemyController = invasionEnemyController;

            Phase = Phase.Invade;

            _isInitialized = true;
        }

        // ================= abstract =================
        public abstract float GetHeight();
        public abstract void AwakeTurret();
        public abstract List<TilePosition> GetEffectArea();

        /**
         * タレットの色を変更する
         */
        public void SetColor(Color color)
        {
            GetComponent<Renderer>().material.color = color;
        }
    }
}