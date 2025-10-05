using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base class for all features (equipment effects, abilities, etc.)
public abstract class Feature : MonoBehaviour
{
    public abstract void Apply(GameObject target);
    public abstract void Remove(GameObject target);
}