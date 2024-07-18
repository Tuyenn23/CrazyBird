using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopBuyPlayer : MonoBehaviour
{
    public List<ElementShopPlayer> L_Elements;

    public Button btn_close;

    public TMP_Text AmoutCoin_txt;
    public TMP_Text Coin_text;

    public GameObject Tick_prefab;
    public GameObject Tick_Obj;

    Tweener tweenCloseShop;

    private void OnEnable()
    {
        btn_close.onClick.AddListener(OnClose);
        InitCoin();
        InitTick();
        initDataShop();
        InitElementOwned();
        InitCurrentSkinUsing();
        InitAds();
        
    }

    public void InitAds()
    {
        if (GameManager.ins.uiController.remainingShowAds.gameObject.activeSelf)
        {
            GameManager.ins.uiController.remainingShowAds.gameObject.SetActive(false);
        }

        AdsHandle.instance.CanShowInterAlways = false;
        AdsHandle.instance.StopInterAlwaysGamePlay();
        //AdsHandle.instance.StopIEShowInterAFK();
    }
    private void InitTick()
    {
        Tick_Obj = Instantiate(GameManager.ins.uiController.ShopBuyPlayer.Tick_prefab, transform);
        Tick_Obj.transform.SetParent(transform);
        Tick_Obj.gameObject.SetActive(false);
    }

    public void InitCoin()
    {
        AmoutCoin_txt.text = PlayerDataManager.GetCoin().ToString();
    }

    public void initDataShop()
    {
        for (int i = 0; i < L_Elements.Count; i++)
        {
            L_Elements[i].InitDataShop();
        }
    }

    public void InitElementOwned()
    {
        for (int i = 0; i < L_Elements.Count; i++)
        {
            L_Elements[i].InitIDOwned();
        }
    }

    public void InitCurrentSkinUsing()
    {
        for (int i = 0; i < L_Elements.Count; i++)
        {
            if (L_Elements[i].TypePlayerInShop == PlayerDataManager.GetCurrentSkinUsing())
            {
                L_Elements[i].InitCurrentSkinUsing();
                break;
            }
        }
    }

    public void ResetBtnEquiped()
    {
        for (int i = 0; i < L_Elements.Count; i++)
        {
            L_Elements[i].ActiveBtnUse();
            L_Elements[i].DeActiveBtnEquiped();
        }
    }

    private void OnClose()
    {

        if (AdsHandle.instance.canShowInterBack)
        {
            AdManager.instance.ShowInter(() =>
            {
                AdsHandle.instance.StopInterAfterTime();
                AdsHandle.instance.ShowInterAfterTime();
                AdsHandle.instance.canShowInterBack = false;
            }, null, "ShowInter");
        }

        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        tweenCloseShop = btn_close.transform.DOScale(Vector3.one * 0.9f, 0.1f).SetEase(Ease.Linear).SetLoops(2, LoopType.Yoyo);
        StartCoroutine(IE_DelayCloseShop());
    }

    IEnumerator IE_DelayCloseShop()
    {
        yield return new WaitForSeconds(0.2f);
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundClosePopup);
        gameObject.SetActive(false);
    }

    public void InitAdsClosePopup()
    {
        AdsHandle.instance.CanShowInterAlways = true;
        AdsHandle.instance.InterAlwaysGamePlay();
    }

    private void OnDisable()
    {
        GameManager.ins.isOpenShop = false;
        InitAdsClosePopup();

        Destroy(Tick_Obj);
        tweenCloseShop?.Kill();
        StopCoroutine(IE_DelayCloseShop());
        StopCoroutine(GameManager.ins.uiController.Uihome.IE_DelayOpenShop());
        btn_close.onClick.RemoveListener(OnClose);
    }

}
