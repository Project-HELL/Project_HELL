using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public float hp;
    public void Damege(float damege)
    {
        hp -= damege;

        if(hp <= 0)
        {
            Debug.Log("플레이어의 체력이 다 소진되었습니다.");
        }
    }
}