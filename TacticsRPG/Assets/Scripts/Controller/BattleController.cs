using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    public GameObject heroPrefab;
    public Tile currentTile { get { return board.GetTile(pos); } }
    public Unit currentUnit;
    private void Start()
    {
        ChangeState<InitBattleState>();
    }

    #region Turn
    //Instance of Turn, list of all Unity in battle and AbilityMenuPanelController
    public AbilityMenuPanelController abilityMenuPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    #endregion
}
