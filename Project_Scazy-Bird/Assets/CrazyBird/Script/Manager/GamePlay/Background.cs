using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    public void MoveToPlayer()
    {
        transform.position = new Vector3(GameManager.ins.CurrentLevel.L_BlockInLevel[GameManager.ins.CurrentLevel.L_BlockInLevel.Count - 1].transform.position.x, transform.position.y, GameManager.ins.CurrentLevel.L_BlockInLevel[GameManager.ins.CurrentLevel.L_BlockInLevel.Count - 2].transform.position.z);
    }
}
