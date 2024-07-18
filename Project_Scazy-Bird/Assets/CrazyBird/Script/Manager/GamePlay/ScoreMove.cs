using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TigerForge;
using TMPro;
using UnityEngine;

public class ScoreMove : MonoBehaviour
{
    public TMP_Text ScoreTxt;

    Tweener TweenCoin;

    private void OnEnable()
    {
        StartCoroutine(IE_DelayDeactiveScore());
    }
    public void InitScore(int Score)
    {
        ScoreTxt.text = $"+{Score}";
    }

    public void AnimCoin()
    {
        TweenCoin = GetComponent<RectTransform>().DOAnchorPosY(200f, 0.3f).SetEase(Ease.Linear);
    }

    IEnumerator IE_DelayDeactiveScore()
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        StopCoroutine(IE_DelayDeactiveScore());
        TweenCoin?.Kill();
    }
}
