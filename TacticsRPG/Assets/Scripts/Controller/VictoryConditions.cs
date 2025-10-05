using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class VictoryConditions : MonoBehaviour
{
    BattleController battleController;
    
    public VictoryType victoryCondition = VictoryType.DefeatAllEnemies;
    public bool checkEveryTurn = true;

    void Awake()
    {
        battleController = GetComponent<BattleController>();
    }

    void Start()
    {
        if (checkEveryTurn)
        {
            // Check victory conditions at the start of each turn
            InvokeRepeating(nameof(CheckVictoryConditions), 1f, 1f);
        }
    }

    public void CheckVictoryConditions()
    {
        VictoryResult result = EvaluateVictoryConditions();
        
        if (result != VictoryResult.None)
        {
            EndBattle(result);
        }
    }

    VictoryResult EvaluateVictoryConditions()
    {
        List<Unit> heroUnits = GetAliveUnits(Allegiance.Hero);
        List<Unit> enemyUnits = GetAliveUnits(Allegiance.Enemy);

        switch (victoryCondition)
        {
            case VictoryType.DefeatAllEnemies:
                if (enemyUnits.Count == 0)
                    return VictoryResult.Victory;
                if (heroUnits.Count == 0)
                    return VictoryResult.Defeat;
                break;

            case VictoryType.SurviveXTurns:
                // Implementation would depend on turn tracking
                break;

            case VictoryType.ProtectUnit:
                // Would need to specify which unit to protect
                break;

            case VictoryType.ReachLocation:
                // Would need to specify target location
                break;
        }

        return VictoryResult.None;
    }

    List<Unit> GetAliveUnits(Allegiance allegiance)
    {
        return battleController.units
            .Where(u => u != null && u.allegiance == allegiance && !IsKnockedOut(u))
            .ToList();
    }

    bool IsKnockedOut(Unit unit)
    {
        var koStatus = unit.GetComponent<KOStatusEffect>();
        return koStatus != null;
    }

    void EndBattle(VictoryResult result)
    {
        Debug.Log($"Battle ended with result: {result}");
        
        // Stop checking victory conditions
        CancelInvoke(nameof(CheckVictoryConditions));
        
        // Transition to end battle state
        battleController.ChangeState<EndBattleState>();
        
        // You could trigger events here for UI updates, experience distribution, etc.
    }
}

public enum VictoryType
{
    DefeatAllEnemies,
    SurviveXTurns,
    ProtectUnit,
    ReachLocation
}

public enum VictoryResult
{
    None,
    Victory,
    Defeat
}