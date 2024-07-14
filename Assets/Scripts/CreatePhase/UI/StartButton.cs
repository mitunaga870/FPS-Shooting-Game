using System;
using UnityEngine;

namespace CreatePhase.UI
{
    public class StartButton : MonoBehaviour
    {
        /** 制作フェーズのコントローラ */
        [SerializeField] private CreationSceneController creationSceneController;

        private void Start()
        {
            // ボタンを押したら侵攻フェーズに移動
            GetComponent<UnityEngine.UI.Button>().onClick
                .AddListener(() => creationSceneController.GoToInvasionPhase());
        }
    }
}