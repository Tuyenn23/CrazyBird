using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PopupBase : MonoBehaviour
{
    public abstract void Open(Transform Content , float Duration);
    public abstract void Close(Transform Content, float Duration);
}
