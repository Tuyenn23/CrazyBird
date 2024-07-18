using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupRevive : PopupTopDown
{
    [Header("Ui")]
    public Button Btn_Continue;
    public Button btn_Reject;

    public TMP_Text CountTime_txt;

    public Image TimeFill_img;

    float TimeCountDown1;
    int MaxTime;
    int Second;


    [Header("Anim")]
    public AnimPopupController animController;

    Tweener tweenContinue, tweenReject, tweenFillTime;

    private void OnEnable()
    {
        btn_Reject.gameObject.SetActive(false);
        MaxTime = 10;
        CountTime_txt.text = MaxTime.ToString() + "s";
        InitButton();
        Filltime();
        InitAds();
    }



    private void Start()
    {
        animController = GetComponent<AnimPopupController>();
        CountTime_txt.text = MaxTime.ToString() + "s";
    }

    private void Update()
    {
        CountTimeRevive();
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

    public void Filltime()
    {
        TimeFill_img.fillAmount = 0;

        tweenFillTime = TimeFill_img.DOFillAmount(1, MaxTime).SetEase(Ease.Linear);
    }

    private void InitButton()
    {
        Btn_Continue.onClick.AddListener(OnContinue);
        btn_Reject.onClick.AddListener(OnReject);
    }

    public void CountTimeRevive()
    {
        if (MaxTime <= 0)
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundClosePopup);
            GameManager.ins.uiController.ProcessWinLose(E_LevelResult.lose);
            gameObject.SetActive(false);
            return;
        }

        TimeCountDown1 += Time.deltaTime;

        if (TimeCountDown1 > 1)
        {
            TimeCountDown1 -= 1;
            MaxTime -= 1;

            Second = MaxTime;

            if (MaxTime >= 0)
            {
                CountTime_txt.text = $"{Second}s";
            }


            if (MaxTime <= 7)
            {
                if (!btn_Reject.gameObject.activeSelf)
                {
                    btn_Reject.gameObject.SetActive(true);
                }
            }
        }
    }

    private void OnReject()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenReject = btn_Reject.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        AdManager.instance.ShowInter(null, null, "ShowInter");
        StartCoroutine(IE_DelayReject());
    }

    IEnumerator IE_DelayReject()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundClosePopup);
        GameManager.ins.uiController.ProcessWinLose(E_LevelResult.lose);
    }

    private void OnContinue()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenContinue = Btn_Continue.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        AdManager.instance.ShowReward(delegate
        {
            GameManager.ins.uiController.UigamePlay.isClockCoutDown = false;
            StartCoroutine(IE_changeStateRevive());

        }, delegate
        {
            GameManager.ins.uiController.PopupNoInternet.gameObject.SetActive(true);
        }, "ShowReward");

    }

    public IEnumerator IE_changeStateRevive()
    {
        yield return new WaitForSeconds(0.2f);

        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundClosePopup);
        GameManager.ins.uiController.UigamePlay.ResetTime();
        GameManager.ins.CurrentLevel.L_BlockInLevel[GameManager.ins.CurrentLevel.L_BlockInLevel.Count - 2].ResetBlockRevive();
        GameManager.ins.GameState(E_GameState.Revive);
        GameManager.ins.CurrentGameState = E_GameState.GamePlay;

    }

    private void RemoveButton()
    {
        Btn_Continue.onClick.RemoveListener(OnContinue);
        btn_Reject.onClick.RemoveListener(OnReject);
    }
    public void InitAdsClosePopup()
    {
        AdsHandle.instance.CanShowInterAlways = true;
        AdsHandle.instance.InterAlwaysGamePlay();
    }


    private void OnDisable()
    {
        InitAdsClosePopup();

        tweenFillTime?.Kill();
        StopCoroutine(IE_DelayReject());
        StopCoroutine(IE_changeStateRevive());
        RemoveButton();
    }
}
