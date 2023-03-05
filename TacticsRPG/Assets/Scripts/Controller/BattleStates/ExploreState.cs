using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Gives back to the player the possibility to move the cursor freely around the board to plan strategies
public class ExploreState : BattleState
{
    protected override void OnMove(object sender, InfoEventArgs<Point> e)
    {
        SelectTile(e.info + pos);
    }

    protected override void OnFire(object sender, InfoEventArgs<int> e)
    {
        if (e.info == 0)
            owner.ChangeState<CommandSelectionState>();
    }
}
