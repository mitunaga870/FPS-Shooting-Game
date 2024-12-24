using UnityEngine;
using UnityEngine.Audio;

public class SoundController : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    
    [SerializeField]
    private AudioSource bgmAudioSource;
    
    private void Start()
    {
        UpdateVolume();
    }
    
    public void UpdateVolume()
    {
        audioMixer.SetFloat("BGMVolume", PlayerPrefs.GetFloat("BGMVolume", 1));
        audioMixer.SetFloat("SEVolume", PlayerPrefs.GetFloat("SEVolume", 1));
    }
    
    public void ChangeBGM(AudioClip audioClip)
    {
        bgmAudioSource.clip = audioClip;
        bgmAudioSource.Play();
    }
}