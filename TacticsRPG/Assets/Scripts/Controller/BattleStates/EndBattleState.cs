using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(EndBattleSequence());
    }

    IEnumerator EndBattleSequence()
    {
        // Show victory/defeat message
        // Award experience
        // Save game state
        // etc.
        
        yield return new WaitForSeconds(3f);
        
        // Return to main menu or world map
        SceneManager.LoadScene(0); // Assumes scene 0 is main menu
    }
}