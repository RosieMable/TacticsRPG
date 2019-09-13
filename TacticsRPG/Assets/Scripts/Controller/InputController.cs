using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    /*
     * Whenever our Repeaters report input, 
     * I will want to share this as an event. 
     * I will make it static so that other scripts
     * merely need to know about this class and not its instances. 
     * We will implement this EventHandler using generics 
     * so that we can specify the type of EventArgs –
     * we will use our InfoEventArgs and specify its type as a Point. 
     * */

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;

    //Event to check which "fire button" is being pressed
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    //Define which buttons to check for
    string[] _buttons = new string[] { "Fire1", "Fire2", "Fire3" };


    void Update()
    {
        int x = _hor.Update();
        int y = _ver.Update();
        if (x != 0 || y!= 0)
        {
            if (moveEvent != null)

                moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        }

        for (int i = 0; i < 3; ++i)
        {
            if (Input.GetButtonUp(_buttons[i]))
            {
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }
}

//Private class to implement a "repeat" functionality
//Keep holding the same button, moves through the tiles semi-quickly

    class Repeater
{
    //Pause to wait between initial press of the button and the point at which it will starts repeating
    const float threshold = .5f;
    //Determins the speed at which the input will repeat
    const float rate = .25f;
    //target point in time which must be passed before new events will be registered 
    float _next;
    //Are we holding the button?
    bool _hold;
    string _axis;

    public Repeater (string axisName)
    {
        _axis = axisName;
    }

    //Methods return either -1, 0 or 1
    // 0 - User is not pressing the button or they are waiting for the repeat event
    public int Update()
    {
        int retValue = 0;
        int value = Mathf.RoundToInt(Input.GetAxisRaw(_axis));

        if(value != 0)
        {
            if (Time.time > _next)
            {
                retValue = value;
                _next = Time.time + (_hold ? rate : threshold);
                _hold = true;
            }
            else
            {
                _hold = false;
                _next = 0;
            }
        }

        return retValue;
    }
}
