using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabStorage : MonoBehaviour
{
    public static PrefabStorage ins { get; private set; }

    public Player player;
    public CameraVirtual CameraVirtual;
    public Background Background;
    public FollowObject CamFake;
    public GameObject TrajectionParent;
    public GameObject StartPos;
    public GameObject Mushroom;

    [Header("Data Controller")]
    public PlayerSO dataPlayer;

    [Header("Particle")]
    public ParticleSystem FxJump;
    public ParticleSystem FxJumpGood;
    public ParticleSystem FxJumpGrate;
    public ParticleSystem FxJumpPerfect;
    public ParticleSystem FxTrail;


    private void Awake()
    {
        if(ins == null)
        {
            ins = this;
        }    
    }
}
