using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TurnOrderController : MonoBehaviour
{
    [SerializeField] List<Unit> allUnits = new List<Unit>();
    Queue<Unit> turnQueue = new Queue<Unit>();
    
    public Unit currentUnit { get; private set; }

    void Start()
    {
        // Initialize turn order based on speed
        CalculateTurnOrder();
    }

    public void CalculateTurnOrder()
    {
        turnQueue.Clear();
        
        // Sort units by speed (highest first)
        var sortedUnits = allUnits
            .Where(u => u != null && !IsKnockedOut(u))
            .OrderByDescending(u => GetUnitSpeed(u))
            .ToList();

        // Add to queue
        foreach (var unit in sortedUnits)
        {
            turnQueue.Enqueue(unit);
        }
    }

    public Unit GetNextUnit()
    {
        // Remove any KO'd units from the queue
        while (turnQueue.Count > 0 && IsKnockedOut(turnQueue.Peek()))
        {
            turnQueue.Dequeue();
        }

        if (turnQueue.Count == 0)
        {
            // Recalculate turn order for next round
            CalculateTurnOrder();
        }

        if (turnQueue.Count > 0)
        {
            currentUnit = turnQueue.Dequeue();
            
            // Process status effects at start of turn
            ProcessStatusEffects(currentUnit);
            
            return currentUnit;
        }

        return null;
    }

    int GetUnitSpeed(Unit unit)
    {
        Stats stats = unit.GetComponent<Stats>();
        if (stats != null && stats.Contains(StatType.SPD))
        {
            return stats.GetStatValue(StatType.SPD);
        }
        return 1; // Default speed
    }

    bool IsKnockedOut(Unit unit)
    {
        var koStatus = unit.GetComponent<KOStatusEffect>();
        return koStatus != null;
    }

    void ProcessStatusEffects(Unit unit)
    {
        var statusEffects = unit.GetComponents<StatusEffect>();
        foreach (var effect in statusEffects)
        {
            effect.OnTurnStart();
        }
    }

    public void AddUnit(Unit unit)
    {
        if (!allUnits.Contains(unit))
        {
            allUnits.Add(unit);
        }
    }

    public void RemoveUnit(Unit unit)
    {
        allUnits.Remove(unit);
        
        // Remove from queue if present
        var tempList = new List<Unit>();
        while (turnQueue.Count > 0)
        {
            var u = turnQueue.Dequeue();
            if (u != unit)
            {
                tempList.Add(u);
            }
        }
        
        foreach (var u in tempList)
        {
            turnQueue.Enqueue(u);
        }
    }
}