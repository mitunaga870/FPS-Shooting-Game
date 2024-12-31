using System;
using CreatePhase;
using InvasionPhase;
using Map;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SettingUIController : MonoBehaviour
    {
        [SerializeField]
        private Slider _bgmSlider;
        
        [SerializeField]
        private Slider _seSlider;
        
        [SerializeField]
        private DeckController _deckController;
        
        [SerializeField]
        private MapController _mapController;
        
        [SerializeField]
        private WalletController _walletController;
        
        [SerializeField]
        private S2SDataInitializer _s2SDataInitializer;
        
        [SerializeField]
        private CreationSceneController _creationSceneController;
        
        [SerializeField]
        private InvasionController _invasionController;
        
        [SerializeField]
        private InvasionMazeController _invasionMazeController;
        
        [SerializeField]
        private SoundController _soundController;
        
        public void OpenSetting() {
            gameObject.SetActive(true);
        }

        public void CloseButton() {
            gameObject.SetActive(false);
        }

        public void OnTitleButton() {
            if (_creationSceneController != null)
                _creationSceneController.Save();
            if (_invasionController != null)
                _invasionController.Save();
            if (_deckController != null)
                _deckController.Save();
            if (_invasionMazeController != null)
                _invasionMazeController.Save();
            if (_mapController != null)
                _mapController.SaveMap();
            if (_s2SDataInitializer != null)
                _s2SDataInitializer.Save();
            if (_walletController != null)
                _walletController.Save();
            
            UnityEngine.SceneManagement.SceneManager.LoadScene("Title");
        }

        private void Start()
        {
            _bgmSlider.value = PlayerPrefs.GetFloat("BGMVolume", 1);
            _seSlider.value = PlayerPrefs.GetFloat("SEVolume", 1);
        }
        
        public void OnBGMVolumeChanged()
        {
            // 倍率を対数に変換
            var realFactor = _bgmSlider.value;
            var dbFactor =
                realFactor != 0 ? Math.Log10(realFactor) * 10 : -100;
            
            PlayerPrefs.SetFloat("BGMVolume", (float) dbFactor);
            PlayerPrefs.Save();
            
            _soundController.UpdateVolume();
        }
        
        public void OnSEVolumeChanged()
        {
            // 倍率を対数に変換
            var realFactor = _seSlider.value;
            var dbFactor =
                realFactor != 0 ? Math.Log10(realFactor) * 10 : -100;
            
            PlayerPrefs.SetFloat("SEVolume", (float) dbFactor);
            PlayerPrefs.Save();
            
            _soundController.UpdateVolume();
        }
    }
}
