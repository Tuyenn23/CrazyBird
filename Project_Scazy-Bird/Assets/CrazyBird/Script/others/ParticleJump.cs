using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleJump : MonoBehaviour
{
    public float time;

    private void OnEnable()
    {
        StartCoroutine(IE_DelayReSpawnParticle());
    }

    IEnumerator IE_DelayReSpawnParticle()
    {
        yield return new WaitForSeconds(time);

        SimplePool.Despawn(gameObject);

    }

    private void OnDisable()
    {
        StopCoroutine(IE_DelayReSpawnParticle());
    }
}
