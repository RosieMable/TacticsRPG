using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Maintains a reference to its own Re ctTransform and sets the interpolated vector as the anchored position value
public class RectTransformAnchorPositionTweener : Vector3Tweener
{
    RectTransform rt;

    protected override void Awake()

    {
        base.Awake();
        rt = transform as RectTransform;
    }

    protected override void OnUpdate(object sender, EventArgs e)
    {
        base.OnUpdate(sender, e);
        rt.anchoredPosition = currentValue;
    }
}
