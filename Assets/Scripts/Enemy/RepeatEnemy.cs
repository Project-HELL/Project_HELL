using UnityEngine;

public class RepeatEnemy : EntityBase
{
    public bool isMoveNegative = false;

    public override Vector2 MoveAI()
    {
        if (isMoveBlockedBefore) {
            isMoveNegative = !isMoveNegative;
            SetFlipX(isMoveNegative);
        }

        return new Vector2(moveSpeed * Time.deltaTime * (isMoveNegative ? 1 : -1), 0);
    }
}