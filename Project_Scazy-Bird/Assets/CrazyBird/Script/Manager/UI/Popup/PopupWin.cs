using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupWin : MonoBehaviour
{
    public Button btn_Receive;
    public Button btn_Reject;


    private void OnEnable()
    {
        InitButton();
    }

    private void InitButton()
    {
        btn_Receive.onClick.AddListener(OnReceive);
        btn_Reject.onClick.AddListener(OnReject);
    }

    private void OnReceive()
    {
        gameObject.SetActive(false);
    }

    private void OnReject()
    {
        gameObject.SetActive(false);
    }


    private void RemoveButton()
    {
        btn_Receive.onClick.RemoveListener(OnReceive);
        btn_Reject.onClick.RemoveListener(OnReject);
    }

    private void OnDisable()
    {
        RemoveButton();
    }
}
