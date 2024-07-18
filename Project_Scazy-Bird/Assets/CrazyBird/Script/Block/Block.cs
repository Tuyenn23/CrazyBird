using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public int id;
    public Transform Point_y;
    Tweener MoveTween, tweenElastic;

    public Tweener tweenSink;



    public void ResetBlockRevive()
    {
        tweenSink?.Kill();
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);

        SickBlock();
    }
    public void MoveToTargetPoint()
    {
        MoveTween = transform.DOMove(new Vector3(transform.position.x, 0f, transform.position.z), 0.5f).SetEase(Ease.Linear);
    }

    public void SickBlock()
    {
        tweenSink = transform.DOMoveY(-Point_y.position.y, GameManager.ins.uiController.UigamePlay.CurrentTimeReal + 0.5f).SetEase(Ease.Linear);
    }

    public void Elastic()
    {
        tweenElastic = transform.DOScaleY(transform.localScale.y - (transform.localScale.y * 0.451f), 0.2f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
    }


    private void OnDisable()
    {
        tweenSink?.Kill();
        tweenElastic?.Kill();
        MoveTween?.Kill();
    }
}
