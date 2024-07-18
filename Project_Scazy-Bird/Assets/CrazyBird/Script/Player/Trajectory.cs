using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour
{
    public int NumberDots;
    [SerializeField] public GameObject dotsParent;
    [SerializeField] GameObject dotPrefab;
    [SerializeField] float dotSpacing;

    [SerializeField]
    [Range(0.01f, 0.2f)] public float minScale;
    [Range(0.1f, 0.2f)] public float MaxScale;

    public List<Transform> L_dots;

    Vector3 pos;

    float timeStamp;

    private void Start()
    {
        L_dots = new List<Transform>(NumberDots);

        for (int i = 0; i < NumberDots; i++)
        {
            L_dots.Add(null);
        }
        PrePareDots();

        HideTrajection();
    }

    public void ShowTrajection()
    {
        dotsParent.SetActive(true);
    }

    public void HideTrajection()
    {
        dotsParent.SetActive(false);
    }

    public void PrePareDots()
    {
        dotPrefab.transform.localScale = Vector3.one * MaxScale;

        float Scale = MaxScale;

        float ScaleFactor = Scale / NumberDots;

        for (int i = 0; i < NumberDots; i++)
        {
            L_dots[i] = Instantiate(dotPrefab.transform);
            L_dots[i].parent = dotsParent.transform;

            L_dots[i].localScale = Vector3.one * Scale;

            if (Scale > minScale)
                Scale -= ScaleFactor;
        }
    }

    public void UpdateDots(Player player, Vector3 forceApplied)
    {
        timeStamp = dotSpacing;

        for (int i = 0; i < NumberDots; i++)
        {
            if (forceApplied.x > 0)
            {
                pos.x = (player.transform.position.x + forceApplied.x * timeStamp);
            }

            pos.y = (player.transform.position.y + (forceApplied.y * timeStamp)) - (Physics.gravity.magnitude * timeStamp * timeStamp) / 2;

            pos.z = player.transform.position.z - timeStamp;

            L_dots[i].position = pos;

            timeStamp += dotSpacing;
        }
    }

    public void UpdateDotsZ(Player player, Vector3 forceApplied)
    {

        timeStamp = dotSpacing;

        for (int i = 0; i < NumberDots; i++)
        {
            if (forceApplied.z > 0)
            {
                pos.z = (player.transform.position.z + forceApplied.z * timeStamp);
            }

            pos.y = (player.transform.position.y + (forceApplied.y * timeStamp)) - (Physics.gravity.magnitude * timeStamp * timeStamp) / 2;

            pos.x = player.transform.position.x - timeStamp;
            L_dots[i].position = pos;

            timeStamp += dotSpacing;
        }
    }
}
