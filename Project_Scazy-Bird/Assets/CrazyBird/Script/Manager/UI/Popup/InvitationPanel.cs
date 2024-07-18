using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InvitationPanel : PopupTopDown
{
    AnimPopupController _animController;
    public Button btn_close;
    public Button btn_send;

    private void Start()
    {
        _animController = GetComponent<AnimPopupController>();
    }
    private void OnEnable()
    {
        btn_close.onClick.AddListener(OnCLose);
    }

    private void OnCLose()
    {
        _animController.Close();
    }
}
