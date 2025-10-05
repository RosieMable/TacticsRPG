using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Representation of a game turn
//Each unity able to move or attack
//Move can be undone
//Attacking not undoable

//This is a data container, updating the turn data will be handled through game states
[System.Serializable]
public class Turn
{
    public Unit actor;
    public bool hasUnitMoved;
    public bool hasUnitActed;
    public bool lockMove;
    Tile startTile;
    Directions startDir;

    public void Change(Unit current)
    {
        actor = current;
        hasUnitMoved = false;
        hasUnitActed = false;
        lockMove = false;
        startTile = actor.tile; 
        startDir = actor.dir;
    }

    public void UndoMove()
    {
        hasUnitActed= false;
        actor.Place(startTile);
        actor.dir = startDir;
        actor.Match();
    }
}
