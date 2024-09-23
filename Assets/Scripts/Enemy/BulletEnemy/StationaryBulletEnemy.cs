using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryBulletEnemy : BulletEnemyBase
{
    public override Vector2 MoveAI()
    {
        return Vector2.zero;
    }
}
