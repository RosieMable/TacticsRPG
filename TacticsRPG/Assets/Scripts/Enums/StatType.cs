using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatType
{
    // Basic Stats
    Level,
    EXP,
    HP,
    MP,
    
    // Primary Attributes
    ATK,    // Attack
    DEF,    // Defense
    INT,    // Intelligence
    RES,    // Resistance
    SPD,    // Speed
    MOV,    // Movement
    JMP,    // Jump
    
    // Secondary Stats
    CTR,    // Counter
    AVD,    // Avoid
    ACC,    // Accuracy
    
    // Status Conditions
    EVA,    // Evasion
    CRT     // Critical
}

public enum StatModType
{
    Flat,
    Percentage
}