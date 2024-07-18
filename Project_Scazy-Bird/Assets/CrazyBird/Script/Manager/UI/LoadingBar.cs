using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingBar : MonoBehaviour
{
    public Image image_fill;
    public TMP_Text Loading_txt;
    Tweener TweenTextLoading;
    private void Start()
    {
        Application.targetFrameRate = 60;
        SoundManager.Instance.PlayBGM(SoundManager.Instance.BgMusic);
        Loading();
        TextLoading();
    }

    private void Loading()
    {
        image_fill.fillAmount = 0f;
        image_fill.DOFillAmount(1f, 6f).SetEase(Ease.Linear).OnComplete(() =>
        {
            SceneManager.LoadScene(1);
        });
    }

    public void TextLoading()
    {
        TweenTextLoading = Loading_txt.transform.DOScale(0.9f, 0.8f).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDisable()
    {
        TweenTextLoading?.Kill();
    }
}
