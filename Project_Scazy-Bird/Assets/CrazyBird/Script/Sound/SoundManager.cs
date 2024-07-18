using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource MusicAudio;
    public AudioSource SoundAudio;

    public AudioClip Soundbtn_Click;
    public AudioClip BgMusic;
    public AudioClip HoldJump;
    public AudioClip SoundJump;
    public AudioClip SoundFly;
    public AudioClip SoundClosePopup;
    public AudioClip SoundHighScore;
    public AudioClip SoundNewBlock;
    public AudioClip SoundOpenPopup;
    public AudioClip SoundRoixuong;
    public AudioClip SoundWin;
    public AudioClip SoundLose;
    public AudioClip TimeCountDown;
    public AudioClip SoundChimHot;

    public AudioClip SoundGood;
    public AudioClip SoundGreat;
    public AudioClip SoundPerfect;


    public float bgVol;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SettingMusic(PlayerDataManager.GetMusic());
        SettingFxSound(PlayerDataManager.GetSound());
    }


    #region Play Music And Sound
    public void PlayBGM(AudioClip audioClip)
    {
        MusicAudio.loop = true;
        MusicAudio.clip = audioClip;
        MusicAudio.volume = bgVol;
        MusicAudio.Play();
    }

    public void PlayFxSound(AudioClip clip)
    {
        SoundAudio.PlayOneShot(clip);
    }
    #endregion

    #region Save Setting Music And Sound
    public void SettingMusic(bool isOn)
    {
        bgVol = isOn ? 1 : 0;
        MusicAudio.volume = bgVol;
        MusicAudio.mute = !isOn;
    }


    public void SettingFxSound(bool isOn)
    {
        var vol = isOn ? 1 : 0;
        SoundAudio.volume = vol;
        SoundAudio.mute = !isOn;
    }
    #endregion

    #region Cho nhieu Audiosources
    public void PlayFxSound(AudioClip clip, AudioSource audioSource)
    {
        audioSource.PlayOneShot(clip);
    }
    #endregion

    #region Stop Music and Sound
    public void StopPlayMusic()
    {
        if (MusicAudio) MusicAudio.Stop();
    }
    #endregion

    #region Ap dung Bar Music And Bar Sound
    public void SetMusicVolume(float vol)
    {
        if (MusicAudio) MusicAudio.volume = vol;
    }

    public void SetSoundVolume(float vol)
    {
        if (SoundAudio) SoundAudio.volume = vol;
    }
    #endregion


}
