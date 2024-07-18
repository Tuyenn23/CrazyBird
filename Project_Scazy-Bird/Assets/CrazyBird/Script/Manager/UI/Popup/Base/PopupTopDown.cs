using Cinemachine.Utility;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupTopDown : PopupBase
{
    public float PosY;
    [SerializeField] private AnimationCurve OpenCurve;
    [SerializeField] private AnimationCurve CloseCurve;

    private Tweener TweenOpen;
    private Tweener TweenClose;

    public override void Open(Transform Content, float Duration)
    {
        Vector3 ThePos = Content.localPosition;
        ThePos.y = -2000;
        Content.localPosition = ThePos;

        TweenOpen = Content.DOLocalMoveY(PosY, Duration).SetEase(OpenCurve).SetUpdate(true);
    }

    public override void Close(Transform Content, float Duration)
    {
        TweenClose = Content.transform.DOMoveY(Content.localPosition.y - 1000, Duration).SetUpdate(true).SetEase(CloseCurve).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }


    private void OnDisable()
    {
        TweenOpen?.Kill();
        TweenClose?.Kill();
    }
}
