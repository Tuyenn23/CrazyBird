using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class AdsHandle : MonoBehaviour
{
    public static AdsHandle instance;

    [Header("inter Always")]
    public bool CanShowInterAlways;

    [Header("interAFK")]
    public bool Detected;
    public bool CanShowInterAFK;

    [Header("inter back")]
    public bool canShowInterBack;

    Coroutine coAFK, coAlways, CoAftertime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InterAlwaysGamePlay();
        ShowInterAFK();
        ShowInterAfterTime();

        CanShowInterAlways = true;
    }

    public void InterAlwaysGamePlay()
    {
        if(!GameManager.ins.isOpenShop)
        {
            coAlways = StartCoroutine(IE_CountTimeShowPlayInter(40));
        }
    }


    public IEnumerator IE_CountTimeShowPlayInter(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Debug.Log(time);
        GameManager.ins.uiController.remainingShowAds.gameObject.SetActive(true);
    }

    public void StopInterAlwaysGamePlay()
    {
        if(coAlways != null)
        {
            StopCoroutine(coAlways);
        }
    }


    public void ShowInterAFK()
    {
        if(Detected || coAFK != null)
        {
            StopIEShowInterAFK();
        }

        Detected = false;

        coAFK = StartCoroutine(IE_InterAFK(25));
    }


    public void StopIEShowInterAFK()
    {
        CanShowInterAFK = false;
        if (coAFK != null)
        {
            StopCoroutine(coAFK);
        }
    }

    public IEnumerator IE_InterAFK(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        Debug.Log(time);
        CanShowInterAFK = true;
        GameManager.ins.uiController.remainingShowAds.gameObject.SetActive(true);
    }


    public void ShowInterAfterTime()
    {
        CoAftertime = StartCoroutine(IE_InterAfterTime(30));
    }

    public void StopInterAfterTime()
    {
        canShowInterBack = false;

        if (CoAftertime != null)
        {
            StopCoroutine(CoAftertime);
        }
    }

    public IEnumerator IE_InterAfterTime(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        canShowInterBack = true;
    }
    
    public void ChangeInterAFK()
    {
        StartCoroutine(IE_DelayChangeCanShowAds());
    }
    IEnumerator IE_DelayChangeCanShowAds()
    {
        yield return new WaitForSeconds(0.5f);
        CanShowInterAFK = false;
    }
}
