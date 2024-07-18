using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraVirtual : MonoBehaviour
{
    private CinemachineVirtualCamera _cam;

    public Vector3 _startpos;

    private void Start()
    {
        _cam = GetComponent<CinemachineVirtualCamera>();
        _startpos = transform.position;

    }

    public void ResetStartPosVirtualCamera()
    {
        _cam.transform.position = _startpos;
    }


    public void UpdateCameraFollowX(Block block, float blockPos)
    {
        _cam.transform.DOMove(new Vector3(_cam.transform.position.x + Math.Abs(blockPos), transform.position.y, transform.position.z), 0.25f);
    }

    public void UpdateCameraFollowZ(Block block, float position)
    {
        _cam.transform.DOMove(new Vector3(_cam.transform.position.x,_cam.transform.position.y, transform.position.z + Math.Abs(position)), 0.25f).SetEase(Ease.Linear);
    }

    public void UpdateCameraToPosX(Block block, float blockPos)
    {
        _cam.transform.DOMove(new Vector3(block.transform.position.x, transform.position.y, transform.position.z), 0.25f);
    }
}
