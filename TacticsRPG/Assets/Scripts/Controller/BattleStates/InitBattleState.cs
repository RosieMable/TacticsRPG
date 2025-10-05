using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitBattleState : BattleState
{
    public override void Enter()
    {
        base.Enter();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        board.Load(levelData);
        Point p = new Point((int)levelData.tiles[0].x, (int)levelData.tiles[0].z);
        SelectTile(p);
        SpawnTestUnits();
        yield return null;
        owner.ChangeState<CutSceneState>();
    }

    void SpawnTestUnits()
    {
        // Create unit factory if it doesn't exist
        UnitFactory factory = UnitFactory.Instance;
        if (factory == null)
        {
            GameObject factoryGO = new GameObject("UnitFactory");
            factory = factoryGO.AddComponent<UnitFactory>();
            factory.unitPrefab = owner.heroPrefab;
        }

        // Spawn hero units
        System.Type[] movementTypes = new System.Type[] { typeof(WalkMovement), typeof(FlyMovement), typeof(TeleportMovement) };
        string[] heroNames = { "Hero1", "Hero2", "Hero3" };
        
        for (int i = 0; i < 3; ++i)
        {
            // Use UnitFactory to create units with proper stats
            Unit heroUnit = factory.CreateUnit(heroNames[i], factory.GetRandomJob(), Allegiance.Hero);
            
            // Place on board
            Point p = new Point((int)levelData.tiles[i].x, (int)levelData.tiles[i].z);
            heroUnit.Place(board.GetTile(p));
            heroUnit.Match();
            
            // Override movement component for variety
            Movement currentMovement = heroUnit.GetComponent<Movement>();
            if (currentMovement != null)
                DestroyImmediate(currentMovement);
                
            Movement newMovement = heroUnit.gameObject.AddComponent(movementTypes[i]) as Movement;
            // Range and jumpHeight are automatically calculated from stats
            
            units.Add(heroUnit);
            owner.turnOrderController.AddUnit(heroUnit);
        }

        // Spawn some enemy units
        SpawnEnemyUnits();
    }

    void SpawnEnemyUnits()
    {
        UnitFactory factory = UnitFactory.Instance;
        if (factory == null || levelData.tiles.Count < 6) return;

        string[] enemyNames = { "Enemy1", "Enemy2" };
        
        for (int i = 0; i < 2; ++i)
        {
            Unit enemyUnit = factory.CreateUnit(enemyNames[i], factory.GetRandomJob(), Allegiance.Enemy);
            
            // Place enemies on opposite side of the board
            int tileIndex = levelData.tiles.Count - 1 - i;
            Point p = new Point((int)levelData.tiles[tileIndex].x, (int)levelData.tiles[tileIndex].z);
            enemyUnit.Place(board.GetTile(p));
            enemyUnit.Match();
            
            units.Add(enemyUnit);
            owner.turnOrderController.AddUnit(enemyUnit);
        }
    }
}
