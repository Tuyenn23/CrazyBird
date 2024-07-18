using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using TigerForge;
using System.Net.NetworkInformation;
using JetBrains.Annotations;

public class PopupPause : PopupTopDown
{
    [Header("Ui")]
    public Button Btn_BackToGame;
    public Button btn_getRewardX5;
    public TMP_Text btn_getRewardX5_txt;

    public TMP_Text Score_txt;
    public TMP_Text Reward_txt;

    public Image IconVideo;

    [Header("Anim")]
    public AnimPopupController animController;

    Tweener TweenClaim, TweenBackToGame;

    private void OnEnable()
    {
        Time.timeScale = 0;
        InitScoreAndReward();
        InitButton();

        InitAds();
    }

    private void Start()
    {
        animController = GetComponent<AnimPopupController>();
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
        Reward_txt.text = GameManager.ins.YourScore.ToString();
    }

    private void InitButton()
    {
        if (GameManager.ins.YourScore == 0)
        {
            btn_getRewardX5.onClick.AddListener(OnBackToGame);
            btn_getRewardX5_txt.text = "Continue";
            Btn_BackToGame.gameObject.SetActive(false);
            IconVideo.gameObject.SetActive(false);
            btn_getRewardX5_txt.rectTransform.offsetMin = new Vector2(0, 0);
            btn_getRewardX5_txt.rectTransform.offsetMax = new Vector2(0, 20);

        }
        else
        {
            btn_getRewardX5_txt.text = "Get x5";
            Btn_BackToGame.onClick.AddListener(OnBackToGame);
            btn_getRewardX5.onClick.AddListener(OnClaimRewardX5);

            Btn_BackToGame.gameObject.SetActive(true);
            IconVideo.gameObject.SetActive(true);
            btn_getRewardX5_txt.rectTransform.offsetMin = new Vector2(60, 0);
        }
    }

    private void OnClaimRewardX5()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        TweenClaim = btn_getRewardX5.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        AdManager.instance.ShowReward(delegate
        {

            int newCoin = (GameManager.ins.YourScore * 5) + PlayerDataManager.GetCoin();
            PlayerDataManager.SetCoin(newCoin);

            EventManager.EmitEvent(EventContains.UPDATEUIGAMEPLAY, 0.5f);
            DelayBacktohome();

        }, delegate
        {
            GameManager.ins.uiController.PopupNoInternet.gameObject.SetActive(true);
        }, "ShowReward");
    }

    IEnumerator IE_DelayBackToHome()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        GameManager.ins.GameState(E_GameState.Home);
        animController.Close();
    }

    private void OnBackToGame()
    {
        Time.timeScale = 1;
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        TweenBackToGame = Btn_BackToGame.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        StartCoroutine(IE_DelayBackToGame());
    }
    public void DelayBacktohome()
    {
        StartCoroutine(IE_DelayBackToHome());
    }

    IEnumerator IE_DelayBackToGame()
    {
        GameManager.ins.CanPlayLevel = false;

        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundClosePopup);
        GameManager.ins.CurrentGameState = E_GameState.GamePlay;
        GameManager.ins.CanPlayLevel = true;
        animController.Close();
    }

    private void RemoveButton()
    {
        Btn_BackToGame.onClick.RemoveListener(OnBackToGame);
        btn_getRewardX5.onClick.RemoveListener(OnClaimRewardX5);
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
        StopCoroutine(IE_DelayBackToGame());
        TweenClaim?.Kill();
        RemoveButton();
    }
}
