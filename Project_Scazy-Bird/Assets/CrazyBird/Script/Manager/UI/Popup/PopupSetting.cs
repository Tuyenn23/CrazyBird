using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupSetting : MonoBehaviour
{
    public Button btn_Music;
    public Button btn_Sound;

    public Image Img_NotiMusic;
    public Image Img_NotiSound;


    

    private void OnEnable()
    {
        btn_Music.onClick.AddListener(OnToggleBtnMusic);
        btn_Sound.onClick.AddListener(OnToggleBtnSound);

        InitSound();
        InitMusic();
    }

    public void InitSound()
    {
        bool isOn = PlayerDataManager.GetSound();

        if (isOn)
        {
            Img_NotiSound.gameObject.SetActive(false);
        }
        else
        {
            Img_NotiSound.gameObject.SetActive(true);
        }
    }

    public void InitMusic()
    {
        bool isOn = PlayerDataManager.GetMusic();

        if (isOn)
        {
            Img_NotiMusic.gameObject.SetActive(false);
        }
        else
        {
            Img_NotiMusic.gameObject.SetActive(true);
        }
    }

    private void OnToggleBtnMusic()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);

        bool isOn = PlayerDataManager.GetMusic();

        if (isOn)
        {
            Img_NotiMusic.gameObject.SetActive(true);
        }
        else
        {
            Img_NotiMusic.gameObject.SetActive(false);
        }

        SoundManager.Instance.SettingMusic(!isOn);
        PlayerDataManager.SetMusic(!isOn);
    }

    private void OnToggleBtnSound()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);

        bool isOn = PlayerDataManager.GetSound();

        if(isOn)
        {
            Img_NotiSound.gameObject.SetActive(true);
        }
        else
        {
            Img_NotiSound.gameObject.SetActive(false);
        }

        SoundManager.Instance.SettingFxSound(!isOn);
        PlayerDataManager.SetSound(!isOn);
    }

    private void OnDisable()
    {
        btn_Music.onClick.RemoveListener(OnToggleBtnMusic);
        btn_Sound.onClick.RemoveListener(OnToggleBtnSound);
    }
}
