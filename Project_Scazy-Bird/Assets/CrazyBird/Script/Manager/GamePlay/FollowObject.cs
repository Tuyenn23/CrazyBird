using Cinemachine;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _cam;

    public Vector3 posCamDefault;

    private void Awake()
    {
        posCamDefault = _cam.transform.localPosition;
    }
    private void Start()
    {
        InitCamera();
    }

    public void UpdateVirtualCam()
    {
        _cam.transform.localPosition = posCamDefault;
    }
    public void InitCamera()
    {
        transform.position = new Vector3(0, 2, 0);

    }
    public void UpdateCamera(Block block)
    {
        transform.DOMove(new Vector3(block.transform.position.x, transform.position.y, block.transform.position.z), 0.25f);
    }
}
