using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RemainingShowAds : MonoBehaviour
{
    public TMP_Text RemaingTime_txt;

    Coroutine coCanPlay;

    int MaxTime;
    float TimeCountDown;
    int Second;

    Tweener tweenCountTime;
    private void OnEnable()
    {
        MaxTime = 5;
        RemaingTime_txt.text = $"{MaxTime}";
        StartCountdown(6);
    }

    private void Update()
    {
        /*        if (MaxTime < 0) return;

                TimeCountDown += Time.deltaTime;

                if (TimeCountDown > 1)
                {
                    TimeCountDown -= 1;
                    MaxTime -= 1;

                    Second = MaxTime;

                    if (MaxTime >= 0)
                    {
                        RemaingTime_txt.text = $"{Second}";

                        if (MaxTime == 0)
                        {
                            GameManager.ins.CanPlayLevel = false;
                            AdManager.instance.ShowInter(null, null, "ShowInter");
                        }
                    }


                }*/
    }

    void StartCountdown(float time)
    {
        RemaingTime_txt.text = $"{(int)time}";

        tweenCountTime = DOTween.To(() => time, x => time = x, 1, time).SetEase(Ease.Linear)
            .OnUpdate(() =>
            {
                RemaingTime_txt.text = $"{(int)time}";
            })
            .OnComplete(() =>
            {
                GameManager.ins.CanPlayLevel = false;
                AdManager.instance.ShowInter(null, null, "ShowInter");

                gameObject.SetActive(false);
            }).SetUpdate(true);
    }

    public void PlayCoroutineCanPlay()
    {
        coCanPlay = StartCoroutine(IE_DelayChangeCanPlay());
    }

    public IEnumerator IE_DelayChangeCanPlay()
    {
        yield return new WaitForSeconds(0.1f);

        if (GameManager.ins.CurrentGameState != E_GameState.Home)
        {
            GameManager.ins.CanPlayLevel = true;
        }
    }


    private void OnDisable()
    {
        tweenCountTime?.Kill();
        if (coCanPlay != null)
        {
            StopCoroutine(coCanPlay);
        }
    }
}
