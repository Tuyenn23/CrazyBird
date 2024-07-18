using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Player",fileName ="Player")]
public class PlayerSO : ScriptableObject
{
    public List<PLAYER> L_Player;

    public PLAYER GetPlayerWithType(E_TypePlayer _typePlayer)
    {
        for (int i = 0; i < L_Player.Count; i++)
        {
            if (L_Player[i].TypePlayer == _typePlayer)
            {
                return L_Player[i];
            }
        }

        return null;
    }
}

[Serializable]
public class PLAYER
{
    public E_TypePlayer TypePlayer;
    public Sprite Icon;
    public int Coin;
}
