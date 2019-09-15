using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Component made to provide an easy way to move a RectTransform in relationship to its parent RectTransform
[RequireComponent(typeof(RectTransform))]
public class LayoutAnchor : MonoBehaviour
{
    RectTransform myRT;
    RectTransform parentRT;

    private void Awake()
    {
        myRT = transform as RectTransform;
        parentRT = transform.parent as RectTransform;
        if (parentRT == null)
            Debug.Log("This component needs a parent with a RectTransform to work.", gameObject);
    }

    //When positioning the rt, there is the need to know the general offsets to use based on the location of the anchor that we want
    //and the size of the rect transform  itself
    Vector2 GetPosition(RectTransform rt, TextAnchor anchor)
    {
        Vector2 retValue = Vector2.zero;

        switch (anchor)
        {
            case TextAnchor.LowerCenter:
            case TextAnchor.MiddleCenter:
            case TextAnchor.UpperCenter:
                retValue.x += rt.rect.width * .5f;
                break;
            case TextAnchor.LowerRight:
            case TextAnchor.MiddleRight:
            case TextAnchor.UpperRight:
                retValue.x += rt.rect.width;
                break;
        }

        switch (anchor)
        {
            case TextAnchor.MiddleLeft:
            case TextAnchor.MiddleCenter:
            case TextAnchor.MiddleRight:
                retValue.y += rt.rect.height * .5f;
                break;
            case TextAnchor.UpperCenter:
            case TextAnchor.UpperLeft:
            case TextAnchor.UpperRight:
                retValue.y += rt.rect.height;
                break;
        }

        return retValue;

        //The two switch methods are not separated by a break, so as to allow them to fall through each other until a break is reached.
        //This  means that any vertical center anchor settings will modify the return value x by half and any vertical right anchor settings to modify the return value by the full witdth of the rt
    }

    public Vector2 AnchorPosition(TextAnchor myAnchor, TextAnchor parentAnchor, Vector2 offset)
    {
        Vector2 myOffset = GetPosition(myRT, myAnchor);
        Vector2 parentOffset = GetPosition(parentRT, parentAnchor);
        Vector2 anchorCenter = new Vector2(Mathf.Lerp(myRT.anchorMin.x, myRT.anchorMax.x, myRT.pivot.x), Mathf.Lerp(myRT.anchorMin.y, myRT.anchorMax.y, myRT.pivot.y));
        Vector2 myAnchorOffset = new Vector2(parentRT.rect.width * anchorCenter.x, parentRT.rect.height * anchorCenter.y);
        Vector2 myPivotOffset = new Vector2(myRT.rect.width * myRT.pivot.x, myRT.rect.height * myRT.pivot.y);
        Vector2 pos = parentOffset - myAnchorOffset - myOffset + myPivotOffset + offset;
        pos.x = Mathf.RoundToInt(pos.x);
        pos.y = Mathf.RoundToInt(pos.y);
        return pos;
    }

}