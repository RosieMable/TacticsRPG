using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitFactory : MonoBehaviour
{
    public static UnitFactory Instance { get; private set; }

    [Header("Prefabs")]
    public GameObject unitPrefab;
    public List<Job> availableJobs = new List<Job>();
    
    [Header("Materials")]
    public Material allyMaterial;
    public Material enemyMaterial;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Unit CreateUnit(string unitName, Job job, Allegiance allegiance = Allegiance.Hero)
    {
        GameObject unitGO = Instantiate(unitPrefab);
        unitGO.name = unitName;

        Unit unit = unitGO.GetComponent<Unit>();
        if (unit == null)
        {
            unit = unitGO.AddComponent<Unit>();
        }

        // Set allegiance
        unit.allegiance = allegiance;

        // Apply material based on allegiance
        SetUnitMaterial(unitGO, allegiance);

        // Add and configure stats
        Stats stats = unitGO.GetComponent<Stats>();
        if (stats == null)
        {
            stats = unitGO.AddComponent<Stats>();
        }

        ConfigureStats(stats, job);

        // Apply job
        if (job != null)
        {
            job.ApplyToUnit(unitGO);
        }

        // Add equipment component
        Equipment equipment = unitGO.GetComponent<Equipment>();
        if (equipment == null)
        {
            equipment = unitGO.AddComponent<Equipment>();
        }

        // Add movement component based on job/unit type
        AddMovementComponent(unitGO, job);

        return unit;
    }

    void SetUnitMaterial(GameObject unit, Allegiance allegiance)
    {
        Renderer renderer = unit.GetComponentInChildren<Renderer>();
        if (renderer != null)
        {
            switch (allegiance)
            {
                case Allegiance.Hero:
                    renderer.material = allyMaterial;
                    break;
                case Allegiance.Enemy:
                    renderer.material = enemyMaterial;
                    break;
            }
        }
    }

    void ConfigureStats(Stats stats, Job job)
    {
        // Initialize basic stats
        stats.AddStat(StatType.Level, 1);
        stats.AddStat(StatType.EXP, 0);
        
        // Let the job set the base stats
        if (job != null)
        {
            job.ApplyToUnit(stats.gameObject);
        }
        else
        {
            // Default stats if no job
            stats.AddStat(StatType.HP, 20);
            stats.AddStat(StatType.MP, 10);
            stats.AddStat(StatType.ATK, 5);
            stats.AddStat(StatType.DEF, 5);
            stats.AddStat(StatType.INT, 5);
            stats.AddStat(StatType.RES, 5);
            stats.AddStat(StatType.SPD, 5);
            stats.AddStat(StatType.MOV, 3);
            stats.AddStat(StatType.JMP, 1);
        }
        
        // Add secondary stats
        stats.AddStat(StatType.ACC, 75);
        stats.AddStat(StatType.EVA, 10);
        stats.AddStat(StatType.CRT, 5);
    }

    void AddMovementComponent(GameObject unit, Job job)
    {
        // Default to walking movement
        Movement movement = unit.GetComponent<Movement>();
        if (movement == null)
        {
            movement = unit.AddComponent<WalkMovement>();
        }

        // Movement range and jumpHeight are automatically calculated from stats via properties
    }

    public Job GetRandomJob()
    {
        if (availableJobs.Count > 0)
        {
            return availableJobs[Random.Range(0, availableJobs.Count)];
        }
        return null;
    }
}