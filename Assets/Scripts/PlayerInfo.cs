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
            Debug.Log("�÷��̾��� ü���� �� �����Ǿ����ϴ�.");
        }
    }
}