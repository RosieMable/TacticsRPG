using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class defines different target positions for ui pieces, then works with LayoutAnchor to snap them to their correct position
[RequireComponent(typeof(LayoutAnchor))]
public class PanelPosition : MonoBehaviour
{
    [Serializable]
    public class Position
    {
        public string name;
        public TextAnchor myAnchor;
        public TextAnchor parentAnchor;
        public Vector2 offset;

        public Position(string name)
        {
            this.name = name;
        }

        //extension of the previous construct
        public Position(string name, TextAnchor myAnchor, TextAnchor parentAnchor) : this(name)
        {
            this.myAnchor = myAnchor;
            this.parentAnchor = parentAnchor;
        }

        public Position(string name, TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset) : this(name, myAnchor, parentAnchor)
        {
            this.offset = offset;
        }
    }


    //Access Positions through a dictionary where a string points to an instance of Position
    //Need to use list, since Unity does not display dictionaries in the editor
    [SerializeField] List<Position> positionsList;
    Dictionary<string, Position> positionMap;
    LayoutAnchor anchor;

    private void Awake()
    {
        anchor = GetComponent<LayoutAnchor>();
        positionMap = new Dictionary<string, Position>(positionsList.Count); //Creates a new dictionary as long as the number of positions
        for (int i = positionsList.Count - 1; i >= 0; --i)
            AddPosition(positionsList[i]);
    }

    //Properties to know the current position, whether or not there is a transition, and if so, access the tweener
    public Position currentPosition { get; private set; }
    public Tweener Transition { get; private set; }
    public bool InTransition {  get { return Transition != null; } }

    public Position this[string name]
    {
        get
        {
            if (positionMap.ContainsKey(name))
                return positionMap[name];
            return null;
        }
    }

    //To add and remove positions dynamically
    public void AddPosition(Position p)
    {
        positionMap[p.name] = p;
    }

    public void RemovePosition (Position p)
    {
        if (positionMap.ContainsKey(p.name))
            positionMap.Remove(p.name);
    }

    //Methods to move the panel to one of its specified positions
    public Tweener SetPosition(string positionName, bool animated)
    {
        return SetPosition(this[positionName], animated);
    }
    public Tweener SetPosition(Position p, bool animated)
    {
        currentPosition = p;
        if (currentPosition == null)
            return null;
        if (InTransition)
            Transition.easingControl.Stop();
        if (animated)
        {
            Transition = anchor.MoveToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return Transition;
        }
        else
        {
            anchor.SnapToAnchorPosition(p.myAnchor, p.parentAnchor, p.offset);
            return null;
        }
    }

    //If no Position has been assigned by the time the script reaches start, then assign the first Position in the list as the default position
    private void Start()
    {
        if (currentPosition == null && positionsList.Count > 0)
            SetPosition(positionsList[0], false);
    }
}


