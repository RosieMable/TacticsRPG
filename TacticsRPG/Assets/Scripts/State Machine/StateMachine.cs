using System.Collections;
using UnityEngine;

//Purpose: Maintain a reference to the current state and handle switching from one state to another
public class StateMachine : MonoBehaviour
{
    public virtual State CurrentState
    {
        get { return _currentState; }
        set { Transition (value);  }
    }

    protected State _currentState;
    protected bool _inTransition;

    public virtual T GetState<T> () where T : State
    {
        T target = GetComponent<T>();
        if (target == null)
            target = gameObject.AddComponent<T>();
        return target;
    }

    public virtual void ChangeState<T>() where T : State
    {
        CurrentState = GetState<T>();
    }

    protected virtual void Transition (State value)
    {
        if (_currentState == value || _inTransition)
            return;

        _inTransition = true;

        if (_currentState != null)
            _currentState.Exit(); //Exits from the old state

        _currentState = value; // changes to the new state

        if (_currentState != null)
            _currentState.Enter(); //Enters new state

        _inTransition = false;
    }
}
