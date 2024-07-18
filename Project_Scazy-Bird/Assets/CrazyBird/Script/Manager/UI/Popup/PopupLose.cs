using Suriyun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using TigerForge;

public class PopupLose : PopupTopDown
{
    [Header("Ui")]
    public Button btn_Backtohome;
    public Button btn_getRewardX5;

    public TMP_Text btn_Backtohome_txt;
    public Image IconVideo;

    public TMP_Text Score_txt;
    public TMP_Text Reward_txt;



    [Header("Anim")]
    public AnimPopupController animController;

    Tweener tweenClaim, tweenBacktoHome;
    private void OnEnable()
    {
        InitButton();
        InitScoreAndReward();
        InitAds();
    }

    public void InitAds()
    {
        if (GameManager.ins.uiController.remainingShowAds.gameObject.activeSelf)
        {
            GameManager.ins.uiController.remainingShowAds.gameObject.SetActive(false);
        }

        AdsHandle.instance.CanShowInterAlways = false;
        AdsHandle.instance.StopInterAlwaysGamePlay();
    }


    public void InitScoreAndReward()
    {
        Score_txt.text = GameManager.ins.YourScore.ToString();

        int rewardCoin = GameManager.ins.YourScore;
        Reward_txt.text = rewardCoin.ToString();
    }

    private void InitButton()
    {
        if (GameManager.ins.YourScore == 0)
        {
            btn_getRewardX5.onClick.AddListener(OnBackToHome);
            btn_Backtohome_txt.text = "Back To Home";
            btn_Backtohome.gameObject.SetActive(false);
            IconVideo.gameObject.SetActive(false);
            btn_Backtohome_txt.rectTransform.offsetMin = new Vector2(0, 0);
            btn_Backtohome_txt.rectTransform.offsetMax = new Vector2(0, 20);

        }
        else
        {
            btn_Backtohome_txt.text = "Get x5";
            btn_Backtohome.onClick.AddListener(OnBackToHome);
            btn_getRewardX5.onClick.AddListener(OnClaimRewardX5);
            btn_Backtohome.gameObject.SetActive(true);
            IconVideo.gameObject.SetActive(true);
            btn_Backtohome_txt.rectTransform.offsetMin = new Vector2(60, 10);
        }
    }

    private void OnClaimRewardX5()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenClaim = btn_getRewardX5.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo); int newCoin = (GameManager.ins.YourScore * 5 * 10) + PlayerDataManager.GetCoin();

        AdManager.instance.ShowReward(delegate
        {

            int newCoin = (GameManager.ins.YourScore * 5) + PlayerDataManager.GetCoin();
            PlayerDataManager.SetCoin(newCoin);

            EventManager.EmitEvent(EventContains.UPDATEUIGAMEPLAY, 0.5f);

            StartCoroutine(IE_DelayBackToHome());
        }, delegate
        {
            GameManager.ins.uiController.PopupNoInternet.gameObject.SetActive(true);
        }, "ShowReward");
    }

    private void OnBackToHome()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenBacktoHome = btn_Backtohome.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        int newScore = PlayerDataManager.GetCoin() + GameManager.ins.YourScore;
        PlayerDataManager.SetCoin(newScore);

        StartCoroutine(IE_DelayBackToHome());
    }

    IEnumerator IE_DelayBackToHome()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundClosePopup);

        GameManager.ins.GameState(E_GameState.Home);
        animController.Close();
    }

    private void RemoveButton()
    {
        btn_Backtohome.onClick.RemoveListener(OnBackToHome);
        btn_getRewardX5.onClick.RemoveListener(OnClaimRewardX5);
        btn_getRewardX5.onClick.AddListener(OnBackToHome);
    }

    public void InitAdsClosePopup()
    {
        AdsHandle.instance.CanShowInterAlways = true;
        AdsHandle.instance.InterAlwaysGamePlay();
    }

    private void OnDisable()
    {
        InitAdsClosePopup();
        StopCoroutine(IE_DelayBackToHome());
        tweenClaim?.Kill();
        RemoveButton();
    }
}
