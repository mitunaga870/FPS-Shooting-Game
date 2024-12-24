using System;
using CreatePhase;
using Enums;
using InvasionPhase;
using ScriptableObjects.S2SDataObjects;
using UnityEngine;
using UnityEngine.Events;

namespace Chat
{
    /**
     * チャットのコントローラ
     * 進行状況を読み取り・CSVよりテキストを表示させる
     */
    public class ChatController : MonoBehaviour
    {
        #region SerializeField

        [SerializeField]
        private MessageBoxController messageBoxController;
    
        [SerializeField]
        private CreationSceneController creationSceneController;
        
        [SerializeField]
        private InvasionController invasionController;
        
        [SerializeField]
        private DeckController deckController;
        
        [SerializeField]
        private GeneralS2SData generalS2SData;
        
        [SerializeField]
        private ChatS2SData chatS2SData;
        
        #endregion

        public void StartChat()
        {
            _isStartedChat = true;
        }
        
        #region private 

        private Phase _currentPhase;
        
        private bool _isInitialized;
        
        private bool _isStartedChat;

        private void Start()
        {
            // コントローラーのアタッチ情報を取得し、現在のシーンを判定
            if (creationSceneController != null && invasionController == null)
                _currentPhase = Phase.Create;
            else if (creationSceneController == null && invasionController != null)
                _currentPhase = Phase.Invade;
            else
                throw new System.Exception("Both CreationSceneController and InvasionController are null.");
            
            _isInitialized = true;
        }
        
        private void Update()
        {
            if (!_isInitialized)
                return;
            
            if (!_isStartedChat)
                return;
            
            switch (_currentPhase)
            {
                case Phase.Create:
                    CheckCreatePhaseChat();
                    break;
                case Phase.Invade:
                    CheckInvasionPhaseChat();
                    break;
                default:
                    throw new Exception("Unknown phase.");
            }
        }
        
        #endregion

        #region CreatePhase
        
        /**
         * コントローラーから情報見てチャット表示メソッドを呼び出す
         */
        private void CheckCreatePhaseChat()
        {
            
            if (!chatS2SData.ShowedFirstBattle)
                ShowFirstBattleChat();
            if (!chatS2SData.ShowedFirstTurret && deckController.HasTurret)
                ShowFirstTurretChat();
        }

        /**
         * リロール周りは受動的に表示
         */
        public void ShowRerollChat()
        {
            if (!chatS2SData.ShowedFirstReroll)
                ShowFirstRerollChat();
            
            // ここにリロールのチャット表示
        }
        
        /**
         * 初回バトルのチャットを表示
         */
        private void ShowFirstBattleChat()
        {
            chatS2SData.ShowedFirstBattle = true;
            
            messageBoxController.SetMessage("First Battle");
            
            // csvからテキストを取得
            var textCSV = Resources.Load<TextAsset>("ChatTexts/First");
            var text = textCSV.text;
            
            // 初回は２行目
            var lines = text.Split('\n');
            var wholeMessage = lines[1];
            
            // カンマで分割
            var messages = wholeMessage.Split(',');
            messageBoxController.SetMessages(messages);
        }

        /**
         * 初回リロールのチャットを表示
         */
        private void ShowFirstRerollChat()
        {
            chatS2SData.ShowedFirstReroll = true;
        }

        /**
         * 初回turretのチャットを表示
         */
        private void ShowFirstTurretChat()
        {
            chatS2SData.ShowedFirstTurret = true;
        }

        /**
         * OPのチャットを表示
         */
        public void ShowOPChat(UnityAction onEndMessages)
        {
            chatS2SData.ShowedOP = true;
            
            // CSVからテキストを取得
            var textCSV = Resources.Load<TextAsset>("ChatTexts/First");
            var text = textCSV.text;
            
            // OPは１行目
            var lines = text.Split('\n');
            var wholeMessage = lines[0];
            
            // カンマで分割
            var messages = wholeMessage.Split(',');
            messageBoxController.SetMessages(messages, onEndMessages);
        }
        
        #endregion 
        
        #region InvasionPhase
        
        private bool _showedInvadeChat;

        /**
         * コントローラーから情報見てチャット表示メソッドを呼び出す
         */
        private void CheckInvasionPhaseChat()
        {
            if (!_showedInvadeChat && invasionController.GameState != GameState.BeforeStart)
                ShowInvadeChat();
        }

        /**
         * バトル中のチャットを表示
         */
        private void ShowInvadeChat()
        {
            _showedInvadeChat = true;
        }
        
        /**
         * しょっぷのチャットを表示
         * 受動的に表示
         */
        public void ShowShopChat()
        {
            if (!chatS2SData.ShowedFirstShop)
                ShowFirstShopChat();
        }
        
        /**
         * 初回ショップのチャットを表示
         */
        private void ShowFirstShopChat()
        {
            chatS2SData.ShowedFirstShop = true;
            
            // csvからテキストを取得
            var textCSV = Resources.Load<TextAsset>("ChatTexts/First");
            var text = textCSV.text;
            
            // 初回は３行目
            var lines = text.Split('\n');
            var wholeMessage = lines[2];
            
            // カンマで分割
            var messages = wholeMessage.Split(',');
            messageBoxController.SetMessages(messages);
        }
        
        #endregion
    }
}