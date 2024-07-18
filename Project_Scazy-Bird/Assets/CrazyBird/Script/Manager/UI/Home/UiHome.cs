using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiHome : MonoBehaviour
{
    public Button btn_play;
    public Button btn_setting;

    //public TMP_Text Level_txt;

    public TMP_Text Score_txt;

    [Header("Coin")]
    public TMP_Text AmoutCoin_txt;


    [Header("Shop And Popup")]
    public Button btn_ShopPlayer;
    public Button invitationPanel;


    public RectTransform ContentTop;
    public RectTransform ContentBottom;
    public RectTransform ContentMidldeLeft;
    public RectTransform ContentSetting;

    public Tweener TweenMoveTop, TweenMoveMidleLeft, TweenMoveBottom, tweenPlay, tweenOpen, tweenOpenSetting;


    bool isOpenSetting;



    private void OnEnable()
    {
        StopAllCoroutines();

        btn_play.onClick.AddListener(OnPlayGame);
        btn_ShopPlayer.onClick.AddListener(OnOpenShop);
        btn_setting.onClick.AddListener(Onsetting);
        invitationPanel.onClick.AddListener(OnOpenInvitation);
        EventManager.StartListening(EventContains.UPDATEMAINUI, InitMainUI);    

        InitAnimUIHome();
        InitMainUI();

    }

    public void InitMainUI()
    {
        AmoutCoin_txt.text = PlayerDataManager.GetCoin().ToString();
        /*Level_txt.text = PlayerDataManager.GetCurrentLevel().ToString();*/
        Score_txt.text = PlayerDataManager.GetBestScore().ToString();
    }

    public void InitAnimUIHome()
    {
        AnimHomeBottom();
        AnimHomeMidleLeft();
        AnimHomeUiTop();

    }

    public void AnimHomeUiTop()
    {
        Vector3 thePos = ContentTop.anchoredPosition;
        thePos.y += 300;

        ContentTop.anchoredPosition = thePos;

        TweenMoveTop = ContentTop.DOAnchorPosY(-70f, 0.4f).SetEase(Ease.Linear);
    }


    public void AnimHomeMidleLeft()
    {
        Vector3 thePos = ContentMidldeLeft.anchoredPosition;


        thePos.x -= 300;

        ContentMidldeLeft.anchoredPosition = thePos;

        TweenMoveTop = ContentMidldeLeft.DOAnchorPosX(0f, 0.4f).SetEase(Ease.Linear);
    }

    public void AnimHomeBottom()
    {
        Vector3 thePos = ContentBottom.anchoredPosition;


        thePos.y -= 300;

        ContentBottom.anchoredPosition = thePos;

        TweenMoveTop = ContentBottom.DOAnchorPosY(10f, 0.4f).SetEase(Ease.Linear);
    }
    private void Onsetting()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenOpenSetting = btn_setting.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        StartCoroutine(IE_DelayOpenSetting());
    }

    public IEnumerator IE_DelayOpenSetting()
    {
        yield return new WaitForSeconds(0.2f);

        if (!ContentSetting.gameObject.activeSelf)
        {
            ContentSetting.gameObject.SetActive(true);
        }
        else
        {
            ContentSetting.gameObject.SetActive(false);
        }
    }

    private void OnPlayGame()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenPlay = btn_play.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        StartCoroutine(IE_DelayChangeState());
    }

    private void OnOpenShop()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenOpen = btn_ShopPlayer.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        StartCoroutine(IE_DelayOpenShop());
    }

    public IEnumerator IE_DelayChangeState()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.ins.GameState(E_GameState.GamePlay);
    }

    public IEnumerator IE_DelayOpenShop()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.ins.uiController.ProcessStateShop(E_ShopState.ShopBuyPlayer);
    }

    private void OnOpenInvitation()
    {
        GameManager.ins.uiController.ProcessStateShop(E_ShopState.Invitation);
    }

    public void KillTweenMove()
    {
        tweenPlay?.Kill();
        tweenOpen?.Kill();
        TweenMoveTop?.Kill();
        TweenMoveMidleLeft?.Kill();
        TweenMoveBottom?.Kill();
    }

    private void OnDisable()
    {
        StopCoroutine(IE_DelayOpenSetting());
        StopCoroutine(IE_DelayOpenShop());
        KillTweenMove();
        btn_setting.onClick.RemoveListener(Onsetting);
        btn_play.onClick.RemoveListener(OnPlayGame);
    }
}
