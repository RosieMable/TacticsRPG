using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HitRateCalculator
{
    public static int CalculateHitRate(Unit attacker, Unit target, Ability ability)
    {
        if (attacker == null || target == null || ability == null)
            return 0;

        Stats attackerStats = attacker.GetComponent<Stats>();
        Stats targetStats = target.GetComponent<Stats>();

        if (attackerStats == null || targetStats == null)
            return 50; // Default hit rate

        // Base hit rate
        int baseHitRate = 75;

        // Factor in attacker's accuracy
        if (attackerStats.Contains(StatType.ACC))
        {
            baseHitRate += attackerStats.GetStatValue(StatType.ACC);
        }

        // Factor in target's evasion
        if (targetStats.Contains(StatType.EVA))
        {
            baseHitRate -= targetStats.GetStatValue(StatType.EVA);
        }

        // Factor in facing direction (back attacks are more accurate)
        FacingDirection facing = GetFacingDirection(attacker, target);
        switch (facing)
        {
            case FacingDirection.Back:
                baseHitRate += 20; // +20% for back attacks
                break;
            case FacingDirection.Side:
                baseHitRate += 10; // +10% for side attacks
                break;
            case FacingDirection.Front:
                // No bonus for front attacks
                break;
        }

        // Clamp between 5% and 95%
        return Mathf.Clamp(baseHitRate, 5, 95);
    }

    public static bool RollHit(int hitRate)
    {
        int roll = Random.Range(1, 101); // 1-100
        return roll <= hitRate;
    }

    static FacingDirection GetFacingDirection(Unit attacker, Unit target)
    {
        if (attacker.tile == null || target.tile == null)
            return FacingDirection.Front;

        // Calculate direction from target to attacker
        Vector3 attackDirection = attacker.tile.center - target.tile.center;
        attackDirection.y = 0; // Ignore height
        attackDirection.Normalize();

        // Get target's facing direction
        Vector3 targetFacing = target.dir.ToVector();

        // Calculate angle between target facing and attack direction
        float angle = Vector3.Angle(targetFacing, attackDirection);

        if (angle <= 45f)
            return FacingDirection.Front;
        else if (angle >= 135f)
            return FacingDirection.Back;
        else
            return FacingDirection.Side;
    }
}

public enum FacingDirection
{
    Front,
    Side,
    Back
}