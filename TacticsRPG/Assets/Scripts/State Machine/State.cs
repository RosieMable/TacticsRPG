using System.Collections;
using UnityEngine;

//Used as a template class
//The other states should be concrete and inherite from this
public abstract class State : MonoBehaviour
{
    //The idea is that if the state changes, it will call Enter and Exit methods
    public virtual void Enter()
    {
        AddListeners();
    }

    public virtual void Exit()
    {
        RemoveListeners();
    }

    protected virtual void OnDestroy()
    {
        RemoveListeners();
    }

    protected virtual void AddListeners() {

    }

    protected virtual void RemoveListeners()
    {

    }
}
