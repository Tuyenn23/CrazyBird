using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerSystem : MonoBehaviour
{
    public void Start()
    {
        StartCoroutine(Load());
    }
    IEnumerator Load()
    {
        yield return null;
        GameManager.ins.LoadLevelMode(GameManager.ins.PlayMode);

        yield return new WaitForEndOfFrame();
        GameManager.ins.uiController.UigamePlay.LoadUiLevel();
    }

}
