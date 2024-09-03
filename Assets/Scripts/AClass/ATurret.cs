#nullable enable
using System;
using CreatePhase;
using Enums;
using InvasionPhase;
using JetBrains.Annotations;
using ScriptableObjects;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace AClass
{
    public abstract class ATurret : MonoBehaviour
    {
        [SerializeField] protected TurretObject turretObject;

        private CreationSceneController _creationSceneController;

        private MazeCreationController _mazeCreationController;

        private InvasionController _invasionController;

        private InvasionMazeController _invasionMazeController;

        private InvasionEnemyController _invasionEnemyController;

        private bool _isInitialized;

        private bool _isEnable;

        protected Phase _phase;

        protected (CreationSceneController?, InvasionController?) GetControllers()
        {
            switch (_phase)
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
            switch (_phase)
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

            _phase = Phase.Create;

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

            _phase = Phase.Invade;

            _isInitialized = true;
        }

        // ================= abstract =================
        public abstract float GetHeight();
        public abstract void AwakeTurret();
    }
}