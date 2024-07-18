using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimPopupController : MonoBehaviour
{
    public PopupBase PopupBase;
    public Transform Content;

    public float DurationOpen;
    public float DurationClose;

    private void OnEnable()
    {
        Open();
    }
    public void Open()
    {
        PopupBase.Open(Content, DurationOpen);
    }

    public void Close()
    {
        PopupBase.Close(Content, DurationClose);
    }
}
