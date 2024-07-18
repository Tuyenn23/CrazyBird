using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupLeftRight : PopupBase
{
    public float PosX;
    [SerializeField] private AnimationCurve OpenCurve;
    [SerializeField] private AnimationCurve CloseCurve;

    private Tweener TweenOpen;
    private Tweener TweenClose;

    public override void Close(Transform Content, float Duration)
    {
        TweenClose = Content.transform.DOLocalMoveX(Content.position.x - 1000, Duration).SetUpdate(true).SetEase(OpenCurve).OnComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }

    public override void Open(Transform Content, float Duration)
    {
        Vector3 ContentPosition = Content.localPosition;

        ContentPosition.x += 500;

        Content.localPosition = ContentPosition;

        TweenOpen = Content.transform.DOLocalMoveX(PosX, Duration).SetUpdate(true).SetEase(OpenCurve);

    }

    private void OnDisable()
    {
        TweenOpen?.Kill();
        TweenClose?.Kill();
    }
}
