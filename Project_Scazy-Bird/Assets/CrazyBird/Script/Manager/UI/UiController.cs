using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using TigerForge;
using UnityEngine;

public class UiController : MonoBehaviour
{
    public UiGamePlay UigamePlay;
    public UiHome Uihome;

    [Header("Popup")]

    public PopupWin PopupWin;
    public PopupLose PopupLose;
    public PopupPause PopupPause;
    public PopupRevive PopupRevive;
    public ShopBuyPlayer ShopBuyPlayer;
    public PopupNoInternet PopupNoInternet;
    public RemainingShowAds remainingShowAds;
    public PopupCombo PopupCombo;


    private void Start()
    {
        InitPopup();
    }

    public void InitPopup()
    {
        DeActivePopupWin();
        DeActivePopupLose();
    }

    public void ProcessWinLose(E_LevelResult levelResult)
    {
        switch (levelResult)
        {
            case E_LevelResult.Win:
                GameManager.ins.CanPlayLevel = false;
                DeActivePopupLose();
                ActivePopupWin();
                break;
            case E_LevelResult.lose:
                GameManager.ins.CanPlayLevel = false;
                GameManager.ins.uiController.DeActivePopupRevive();
                DeActivePopupWin();
                ActivePopupLose();
                StopCoroutine(IE_DelayActivePopupRevive());
                break;

            case E_LevelResult.Revive:
                GameManager.ins.CanPlayLevel = false;
                StartCoroutine(IE_DelayActivePopupRevive());

                break;
            default:
                break;
        }
    }


    public IEnumerator IE_DelayActivePopupRevive()
    {
        yield return new WaitForSeconds(1f);
        if(!GameManager.ins._isBackToHome)
        {
            ActivePopupRevive();
        }
    }

    public void ActivePopupRevive()
    {
        if(!PopupRevive.gameObject.activeSelf)
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundOpenPopup);
            PopupRevive.gameObject.SetActive(true);
        }
    }
    public void DeActivePopupRevive()
    {
        PopupRevive.gameObject.SetActive(false);
    }

    public void ActiveUiGamePlay()
    {
        UigamePlay.gameObject.SetActive(true);
    }

    public void DeActiveUiGamePlay()
    {
        UigamePlay.gameObject.SetActive(false);
    }
    public void ActiveUiHome()
    {
        Uihome.gameObject.SetActive(true);
    }

    public void DeActiveUiHome()
    {
        Uihome.gameObject.SetActive(false);
    }

    public void ActivePopupWin()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundWin);
        PopupWin.gameObject.SetActive(true);
    }

    public void DeActivePopupWin()
    {
        PopupWin.gameObject.SetActive(false);
    }

    public void ActivePopupLose()
    {
        if (GameManager.ins.YourScore > PlayerDataManager.GetBestScore())
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundHighScore);
            PlayerDataManager.SetBestScore(GameManager.ins.YourScore);
        }
        else
        {
            SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundLose);
        }
        PopupLose.gameObject.SetActive(true);
    }

    public void DeActivePopupLose()
    {
        PopupLose.gameObject.SetActive(false);
    }

    public void ActivePopupPause()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundOpenPopup);
        PopupPause.gameObject.SetActive(true);
    }

    public void DeActivePopupPause()
    {
        PopupPause.gameObject.SetActive(false);
    }


    public void ProcessStateShop(E_ShopState StateShop)
    {
        switch (StateShop)
        {
            case E_ShopState.ShopBuyPlayer:
                OpenShopBuyPlayer();
                break;
            case E_ShopState.Invitation:
                break;
            default:
                break;
        }
    }


    private void OpenShopBuyPlayer()
    {
        SoundManager.Instance.PlayFxSound(SoundManager.Instance.SoundOpenPopup);
        ShopBuyPlayer.gameObject.SetActive(true);
    }
}
