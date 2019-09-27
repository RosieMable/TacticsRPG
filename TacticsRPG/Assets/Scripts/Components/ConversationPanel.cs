using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConversationPanel : MonoBehaviour
{
    //To display the conv messages
    public Text message;
    //To display the avatar sprite
    public Image speaker;
    //Arrow that indicates if there is more text to read
    public GameObject arrow;
    //To control the tweener to ease in and out the conv panel
    public PanelPosition panel;

    private void Start()
    {
        Vector3 pos = arrow.transform.localPosition;
        arrow.transform.localPosition = new Vector3(pos.x, pos.y + 5, pos.z);
        Tweener t = arrow.transform.MoveToLocal(new Vector3(pos.x, pos.y - 5, pos.z), 0.5f, EasingEquations.EaseInQuad);
        t.easingControl.loopType = EasingControl.LoopType.PingPong;
        t.easingControl.loopCount = -1;

    }

    //Loops through messages and displays the arrow if there are more than one message
    public IEnumerator Display (SpeakerData sd)
    {
        speaker.sprite = sd.speaker;
        speaker.SetNativeSize();
        for (int i = 0; i < sd.messages.Count; ++i)
        {
            message.text = sd.messages[i];
            arrow.SetActive(i + 1 < sd.messages.Count);
            yield return null;
        }
    }
}
