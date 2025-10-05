using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleController : StateMachine
{
    [Header("Core Components")]
    public CameraRig cameraRig;
    public Board board;
    public LevelData levelData;
    public Transform tileSelectionIndicator;
    public Point pos;

    [Header("Prefabs")]
    public GameObject heroPrefab;
    public GameObject enemyPrefab;

    [Header("Battle Management")]
    public Tile currentTile { get { return board.GetTile(pos); } }
    public Unit currentUnit;
    public TurnOrderController turnOrderController;
    public VictoryConditions victoryConditions;

    private void Awake()
    {
        // Initialize turn order controller
        if (turnOrderController == null)
            turnOrderController = gameObject.AddComponent<TurnOrderController>();
            
        // Initialize victory conditions
        if (victoryConditions == null)
            victoryConditions = gameObject.AddComponent<VictoryConditions>();
            
        // Ensure conversation controller exists as child for cut scenes
        ConversationController conversationController = GetComponentInChildren<ConversationController>();
        if (conversationController == null)
        {
            GameObject conversationGO = new GameObject("ConversationController");
            conversationGO.transform.SetParent(transform);
            conversationController = conversationGO.AddComponent<ConversationController>();
        }
    }

    private void Start()
    {
        ChangeState<InitBattleState>();
    }

    #region Turn Management
    //Instance of Turn, list of all Unity in battle and AbilityMenuPanelController
    public AbilityMenuPanelController abilityMenuPanelController;
    public StatPanelController statPanelController;
    public Turn turn = new Turn();
    public List<Unit> units = new List<Unit>();

    #endregion
}
