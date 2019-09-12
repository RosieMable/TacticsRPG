using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Point
{
    public int x;
    public int y;

    //Constructor
    public Point (int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    //Operators Overloading
    //Plus +
    public static Point operator + (Point a, Point b)
    {
        return new Point(a.x + b.x, a.y + b.y);
    }
    //Minus -
    public static Point operator - (Point p1, Point p2)
    {
        return new Point(p1.x - p2.x, p1.y - p2.y);
    }
    //Equals ==
    public static bool operator == (Point a, Point b)
    {
        return a.x == b.x && a.y == b.y;
    }

    //Not equal !=
    public static bool operator != (Point a, Point b)
    {
        return !(a == b);
    }

    //we have implemented the equality operator, 
    //it is expected that we will also override Equals and GetHashCode

    public override bool Equals(object obj)
    {
        if (obj is Point)
        {
            Point p = (Point)obj;
            return x == p.x && y == p.y;
        }
        return false;
    }

    public bool Equals (Point p)
    {
        return x == p.x && y == p.y;
    }

    public override int GetHashCode()
    {
        return x ^ y;
    }

    /*
     * Rather than having to manually print out the x and y values
     * of our struct, it would be nice to be able to just log
     * the Point and let it determine 
     * how to print out its own contents.
     * */

    public override string ToString()
    {
        return string.Format("({0},{1})", x, y);
    }
}
