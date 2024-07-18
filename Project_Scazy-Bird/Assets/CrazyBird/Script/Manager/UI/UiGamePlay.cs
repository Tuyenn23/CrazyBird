using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TigerForge;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UiGamePlay : MonoBehaviour
{
    [Header("Top")]
    public Button btn_BackToHome;
    public Button btn_Pause;

    public TMP_Text Coin_txt;

    [Header("LevelMode")]
    public TMP_Text Level_txt;
    public TMP_Text AmoutBlockInLevel_txt;
    public TMP_Text CurrentBlock_txt;
    public TMP_Text CountTime_txt;
    float TimeCountDown1;
    [SerializeField] public int MaxTime;

    public int CurrentTimeReal;
    int Second;


    public bool isClockCoutDown;


    [Header("EndLessMode")]
    public TMP_Text yourScore_txt;
    public TMP_Text ScoreCombo_txt;

    public RectTransform ContentTop;
    Tweener TweenMoveTop, tweenPause, tweenBackToHome , tweenScoreScale;


    private void OnEnable()
    {
        //EventManager.StartListening(EventContains.CURRENT_BLOCK, InitUICurrentBlock);
        EventManager.StartListening(EventContains.UPDATE_SCORE, UpdateScore);
        EventManager.StartListening(EventContains.UPDATE_TIME_JUMP, UpdateTimeJump);
        EventManager.StartListening(EventContains.UPDATEUIGAMEPLAY, InitCoin);
        btn_BackToHome.onClick.AddListener(OnBackToHome);
        btn_Pause.onClick.AddListener(OnPause);
        LoadUiLevel();

        AnimHomeUiTop();
        InitCoin();

        InitTime();

        isClockCoutDown = false;

        GameManager.ins.TimeInGame = 0;
        GameManager.ins.TimeSoundBird = 0;

        InitCoin();
    }

    public void UpdateScoreCombo(int Score)
    {
        ScoreCombo_txt.text = $"+{Score}";
    }


    private void Update()
    {
        CountTime();

    }

    public void UpdateTimeJump()
    {
        MaxTime = CurrentTimeReal;
        CountTime_txt.text = $"{MaxTime}s";
    }

    public void InitUiGamePlay()
    {
        Coin_txt.text = PlayerDataManager.GetCoin().ToString();
    }

    public void InitTime()
    {
        MaxTime = 45;
        CurrentTimeReal = MaxTime;
        CountTime_txt.text = $"{MaxTime}s";
    }

    public void ResetTime()
    {
        MaxTime = CurrentTimeReal;
        CountTime_txt.text = $"{MaxTime}s";
    }

    public void InitCoin()
    {
        Coin_txt.text = PlayerDataManager.GetCoin().ToString();
    }

    public void LoadUiLevel()
    {
        if (GameManager.ins.PlayMode == E_PlayMode.LevelMode)
        {
            EventManager.StartListening(EventContains.CURRENT_BLOCK, InitUICurrentBlock);
            InitUIBlockInLevel();
            InitUICurrentBlock();
            InitUiLevel();
            InitUiTimeInLevel();
        }
        else
        {
            InitScoreEndlessMode();
        }
    }

    public void InitScoreEndlessMode()
    {
        tweenScoreScale = yourScore_txt.transform.DOScale(0.8f, 0.2f).SetEase(Ease.Linear).SetLoops(2,LoopType.Yoyo).OnComplete(()=>
        {
            yourScore_txt.text = GameManager.ins.YourScore.ToString();
            tweenScoreScale?.Kill();
        });
    }

    public void UpdateScore()
    {
        InitScoreEndlessMode();
    }

    public void InitUiLevel()
    {
        Level_txt.text = $"Cấp Độ {GameManager.ins.LevelPlaying}";
    }

    public void InitUiTimeInLevel()
    {
        /*MaxTime = GameManager.ins.CurrentLevel.TimeInLevel;*/
    }

    public void InitUICurrentBlock()
    {
        CurrentBlock_txt.text = $"{GameManager.ins.AmoutBlockCurrent}";
    }

    public void InitUIBlockInLevel()
    {
        /*AmoutBlockInLevel_txt.text = $"/ {GameManager.ins.CurrentLevel.AmoutBlockInlevel}";*/
    }

    public void CountTime()
    {
        if (MaxTime < 0 || !GameManager.ins.CanPlayLevel) return;

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

            if (MaxTime <= 5 && !isClockCoutDown)
            {
                SoundManager.Instance.PlayFxSound(SoundManager.Instance.TimeCountDown);
                isClockCoutDown = true;
            }

            GameManager.ins.TimeInGame += 1;
            GameManager.ins.TimeSoundBird += 1;

            if(GameManager.ins.TimeSoundBird >= 10)
            {
                int rand = UnityEngine.Random.Range(0, 2);

                if(rand == 1)
                {
                    SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundChimHot);
                    GameManager.ins.TimeSoundBird = 0;
                }
            }


            if (GameManager.ins.TimeInGame % 30 == 0)
            {
                if(CurrentTimeReal > 15)
                {
                    CurrentTimeReal -= 5;
                }
            }

            if(GameManager.ins.TimeInGame % 20 == 0)
            {
                if (PrefabStorage.ins.player.MaxRange < 5)
                {
                    PrefabStorage.ins.player.MaxRange += 0.5f;
                }

                if(GameManager.ins.CurrentLevel.MinScale > 0.71f)
                {
                    GameManager.ins.CurrentLevel.MinScale -= 0.15f;

                }

            }

            if(MaxTime < 0)
            {
                PrefabStorage.ins.player.Death();
                GameManager.ins.CanPlayLevel = false;
                GameManager.ins.uiController.ProcessWinLose(E_LevelResult.Revive);
            }
        }
    }
    public void AnimHomeUiTop()
    {
        Vector3 thePos = ContentTop.anchoredPosition;
        thePos.y += 300;

        ContentTop.anchoredPosition = thePos;

        TweenMoveTop = ContentTop.DOAnchorPosY(-30f, 0.4f).SetEase(Ease.Linear);
    }
    private void OnBackToHome()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenBackToHome = btn_BackToHome.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        if (AdsHandle.instance.canShowInterBack)
        {
            AdManager.instance.ShowInter(() =>
            {
                AdsHandle.instance.StopInterAfterTime();
                AdsHandle.instance.ShowInterAfterTime();
                AdsHandle.instance.canShowInterBack = false;
            }, null, "ShowInter");
        }

        GameManager.ins.CanPlayLevel = false;
        StartCoroutine(IE_ChangeStateBackToHome());
    }

    private void OnPause()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenPause = btn_Pause.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);

        if (AdsHandle.instance.canShowInterBack)
        {
            AdManager.instance.ShowInter(()=>
            {
                AdsHandle.instance.StopInterAfterTime();
                AdsHandle.instance.ShowInterAfterTime();
                AdsHandle.instance.canShowInterBack = false;
            }, null, "ShowInter");
        }
        GameManager.ins.CanPlayLevel = false;
        StartCoroutine(IE_ChangeStatePause());
    }

    public void RemoveButton()
    {
        btn_BackToHome.onClick.RemoveListener(OnBackToHome);
        btn_Pause.onClick.RemoveListener(OnPause);
        /*EventManager.StopListening(EventContains.CURRENT_BLOCK, InitUICurrentBlock);*/
    }

    public IEnumerator IE_ChangeStatePause()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.ins.GameState(E_GameState.Pause);
    }

    IEnumerator IE_ChangeStateBackToHome()
    {
        yield return new WaitForSeconds(0.2f);
        GameManager.ins.GameState(E_GameState.Home);

    }


    private void OnDisable()
    {
        EventManager.StopListening(EventContains.UPDATE_SCORE, UpdateScore);
        EventManager.StopListening(EventContains.UPDATE_TIME_JUMP, UpdateTimeJump);

        StopCoroutine(IE_ChangeStateBackToHome());
        tweenPause?.Kill();
        TweenMoveTop?.Kill();
        RemoveButton();
    }
}
