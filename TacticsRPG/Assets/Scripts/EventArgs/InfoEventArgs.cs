using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Class that can hold a single field of any data type
public class InfoEventArgs<T> : EventArgs
{
    public T info;

    public InfoEventArgs()
    {
        //Empty construct, it inits itself
        //default keyword handes both reference and value types
        info = default(T);
    }

    public InfoEventArgs(T info)
    {
        this.info = info;
    }
}
