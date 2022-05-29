using System.Collections;
using System.Collections.Generic;
using ModularCharacters;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragToRotate : MonoBehaviour
{
    public Transform target;
    public float speed = 10;
    
    public void Drag(BaseEventData baseEventData)
    {
        if (baseEventData is PointerEventData pointerEventData)
            target.Rotate(0,-pointerEventData.delta.x * speed, 0);
    }
}
