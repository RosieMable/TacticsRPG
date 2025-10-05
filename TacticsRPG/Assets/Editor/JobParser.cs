using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public static class JobParser
{
    [MenuItem("Pre Production/Parse Jobs")]
    public static void Parse()
    {
        CreateDirectories();
        ParseStartingStats();
        ParseGrowthStats();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    static void CreateDirectories()
    {
        if (!AssetDatabase.IsValidFolder("Assets/Resources/Jobs"))
            AssetDatabase.CreateFolder("Assets/Resources", "Jobs");
    }

    static void ParseStartingStats()
    {
        string readPath = string.Format("{0}/Settings/JobStartingStats.csv", Application.dataPath);
        if (!File.Exists(readPath))
        {
            Debug.LogWarning($"JobStartingStats.csv not found at {readPath}");
            return;
        }
        
        string[] readLines = File.ReadAllLines(readPath);
        for (int i = 1; i < readLines.Length; ++i)
            ParseStartingStats(readLines[i]);
    }

    static void ParseStartingStats(string line)
    {
        string[] elements = line.Split(',');
        if (elements.Length < 10) return;
        
        string jobName = elements[0];
        string assetPath = $"Assets/Resources/Jobs/{jobName}.asset";
        
        // Create or load existing job asset
        Job job = AssetDatabase.LoadAssetAtPath<Job>(assetPath);
        if (job == null)
        {
            job = ScriptableObject.CreateInstance<Job>();
            job.jobName = jobName;
            AssetDatabase.CreateAsset(job, assetPath);
        }

        // Parse base stats from CSV: Name,MHP,MMP,ATK,DEF,MAT,MDF,SPD,MOV,JMP
        job.baseHP = Convert.ToInt32(elements[1]);   // MHP -> HP
        job.baseMP = Convert.ToInt32(elements[2]);   // MMP -> MP
        job.baseATK = Convert.ToInt32(elements[3]);  // ATK -> ATK
        job.baseDEF = Convert.ToInt32(elements[4]);  // DEF -> DEF  
        job.baseINT = Convert.ToInt32(elements[5]);  // MAT -> INT
        job.baseRES = Convert.ToInt32(elements[6]);  // MDF -> RES
        job.baseSPD = Convert.ToInt32(elements[7]);  // SPD -> SPD
        job.baseMOV = Convert.ToInt32(elements[8]);  // MOV -> MOV
        job.baseJMP = Convert.ToInt32(elements[9]);  // JMP -> JMP

        EditorUtility.SetDirty(job);
    }

    static void ParseGrowthStats()
    {
        string readPath = string.Format("{0}/Settings/JobGrowthStats.csv", Application.dataPath);
        if (!File.Exists(readPath))
        {
            Debug.LogWarning($"JobGrowthStats.csv not found at {readPath}");
            return;
        }
        
        string[] readLines = File.ReadAllLines(readPath);
        for (int i = 1; i < readLines.Length; ++i)
            ParseGrowthStats(readLines[i]);
    }

    static void ParseGrowthStats(string line)
    {
        string[] elements = line.Split(',');
        if (elements.Length < 8) return; // Only 8 columns in growth stats
        
        string jobName = elements[0];
        string assetPath = $"Assets/Resources/Jobs/{jobName}.asset";
        
        Job job = AssetDatabase.LoadAssetAtPath<Job>(assetPath);
        if (job == null)
        {
            Debug.LogWarning($"Job {jobName} not found for growth stats. Create starting stats first.");
            return;
        }

        // Parse growth rates from CSV: Name,MHP,MMP,ATK,DEF,MAT,MDF,SPD (no MOV,JMP in growth stats)
        job.hpGrowth = (int)(Convert.ToSingle(elements[1]) * 10);   // Convert 8.4 to 84%
        job.mpGrowth = (int)(Convert.ToSingle(elements[2]) * 10);   // Convert 0.8 to 8%
        job.atkGrowth = (int)(Convert.ToSingle(elements[3]) * 10);  // Convert 8.8 to 88%
        job.defGrowth = (int)(Convert.ToSingle(elements[4]) * 10);  // Convert 9.2 to 92%
        job.intGrowth = (int)(Convert.ToSingle(elements[5]) * 10);  // Convert 1.1 to 11%
        job.resGrowth = (int)(Convert.ToSingle(elements[6]) * 10);  // Convert 7.6 to 76%
        job.spdGrowth = (int)(Convert.ToSingle(elements[7]) * 10);  // Convert 1.1 to 11%
        // Keep default values for MOV and JMP growth since they're not in the CSV
        job.movGrowth = 10; // Default 10%
        job.jmpGrowth = 10; // Default 10%

        EditorUtility.SetDirty(job);
    }
}