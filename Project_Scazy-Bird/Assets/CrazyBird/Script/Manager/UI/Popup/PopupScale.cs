using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupScale : PopupBase
{
    [SerializeField] private AnimationCurve animCurveOpen;
    [SerializeField] private AnimationCurve animCurveClose;
    private Tweener tweenScaleOpen;
    private Tweener tweenScaleClose;
    public override void Open(Transform content, float duration)
    {
        content.localScale = Vector3.one * 0.2f;
        tweenScaleOpen = content.DOScale(Vector3.one, duration)
            .SetUpdate(true)
            .SetEase(animCurveOpen);
    }
    public override void Close(Transform content, float duration)
    {
        Vector3 theScale = Vector3.one * 0.4f;
        tweenScaleClose = content.DOScale(theScale, duration)
           .SetUpdate(true)
           .SetEase(animCurveClose)
           .OnComplete(() =>
           {
               gameObject.SetActive(false);
           });
    }
    private void OnDisable()
    {
        tweenScaleClose?.Kill(true);
        tweenScaleOpen?.Kill(true);
    }
}
