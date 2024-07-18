
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using TigerForge;
using System.Collections;
using GoogleMobileAds.Api.AdManager;

public class ElementShopPlayer : MonoBehaviour
{
    public E_TypePlayer TypePlayerInShop;

    public TMP_Text ID_txt;
    public List<Button> L_btnTrasition;

    public Image Icon;

    public TMP_Text Price_txt;

    GameObject TickObj;

    private void OnEnable()
    {
        InitButton();
    }

    public void InitButton()
    {
        L_btnTrasition[0].onClick.AddListener(OnPurchaseVideo);
        L_btnTrasition[1].onClick.AddListener(OnPurchaseGold);
        L_btnTrasition[2].onClick.AddListener(OnSelect);
    }

    public void InitDataShop()
    {
        PLAYER player = PrefabStorage.ins.dataPlayer.GetPlayerWithType(TypePlayerInShop);
        Icon.sprite = player.Icon;
        Price_txt.text = player.Coin.ToString();
    }

    public void InitIDOwned()
    {
        for (int i = 0; i < PlayerDataManager.DataShopPlayerModel.GetListSkinOnwed().Count; i++)
        {
            if (TypePlayerInShop == PlayerDataManager.DataShopPlayerModel.GetListSkinOnwed()[i])
            {
                DeActiveAllButton();
                ActiveBtnUse();
                ID_txt.text = TypePlayerInShop.ToString();
                break;
            }
            else
            {
                InitTypeButton();
                ID_txt.text = TypePlayerInShop.ToString();
            }
        }
    }

    public void InitCurrentSkinUsing()
    {
        DeActiveAllButton();
        ActivebtnEquipedAndTick();
    }

    public void InitTypeButton()
    {
        DeActiveAllButton();

        if (PlayerDataManager.GetCoin() >= 1000)
        {
            DeActiveAllButton();
            ActiveBtnPurchaseGold();
        }
        else
        {
            DeActiveAllButton();
            ActiveBtnVideo();
        }
    }


    public void ActiveBtnVideo()
    {
        L_btnTrasition[0].gameObject.SetActive(true);
    }

    public void ActiveBtnPurchaseGold()
    {
        L_btnTrasition[1].gameObject.SetActive(true);
    }


    public void ActiveBtnUse()
    {
        L_btnTrasition[2].gameObject.SetActive(true);
    }

    public void ActivebtnEquipedAndTick()
    {
        GameObject Tick = GameManager.ins.uiController.ShopBuyPlayer.Tick_Obj;
        Tick.transform.SetParent(transform);
        Tick.GetComponent<RectTransform>().anchoredPosition = new Vector2(-5, -25);
        Tick.SetActive(true);

        L_btnTrasition[3].gameObject.SetActive(true);
    }

    public void DeActiveBtnEquiped()
    {
        if (TickObj != null)
        {
            DestroyImmediate(TickObj);
        }
        L_btnTrasition[3].gameObject.SetActive(false);
    }

    private void OnPurchaseVideo()
    {
        AdManager.instance.ShowReward(() =>
        {
            DeActiveAllButton();
            ActiveBtnUse();
            PlayerDataManager.AddSkin(TypePlayerInShop);
        }, () =>
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);

        },"ShowReward");
    }

    private void OnPurchaseGold()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);

        if (PlayerDataManager.GetCoin() >= 1000)
        {
            int newCoin = PlayerDataManager.GetCoin() - 1000;
            PlayerDataManager.SetCoin(newCoin);
            GameManager.ins.uiController.ShopBuyPlayer.InitCoin();

            DeActiveAllButton();
            ActiveBtnUse();
            PlayerDataManager.AddSkin(TypePlayerInShop);
            EventManager.EmitEvent(EventContains.UPDATEMAINUI);
        }

        GameManager.ins.uiController.ShopBuyPlayer.InitElementOwned();
        GameManager.ins.uiController.ShopBuyPlayer.InitCurrentSkinUsing();
    }

    private void OnSelect()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.Soundbtn_Click);
        GameManager.ins.uiController.ShopBuyPlayer.ResetBtnEquiped();
        DeActiveAllButton();
        ActivebtnEquipedAndTick();
        PlayerDataManager.SetCurrentSkinUsing(TypePlayerInShop);

        GameManager.ins.LoadPlayerWhenChooseInShop();
    }

    IEnumerator IE_DelayDeActivePopupNoInternet()
    {
        yield return new WaitForSeconds(1f);
        DeActivePopupNoInternet();
    }

    public void ActivePopupNoInternet()
    {
        GameManager.ins.uiController.PopupNoInternet.gameObject.SetActive(true);
    }

    public void DeActivePopupNoInternet()
    {
        GameManager.ins.uiController.PopupNoInternet.gameObject.SetActive(false);
    }


    public void DeActiveAllButton()
    {
        for (int i = 0; i < L_btnTrasition.Count; i++)
        {
            L_btnTrasition[i].gameObject.SetActive(false);
        }
    }

    public void RemoveButton()
    {
        L_btnTrasition[0].onClick.RemoveListener(OnPurchaseVideo);
        L_btnTrasition[1].onClick.RemoveListener(OnPurchaseGold);
        L_btnTrasition[2].onClick.RemoveListener(OnSelect);
    }
    private void OnDisable()
    {
        StopCoroutine(IE_DelayDeActivePopupNoInternet());
        RemoveButton();
    }

}
