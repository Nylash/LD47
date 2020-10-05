using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPositionner : EnhancedMonoBehaviour
{
    public enum Position
    {
        left, right
    }

    [SerializeField] private Position position = Position.left;

    private Position lastPosition;

    protected override void EditorStart()
    {
        if(position == Position.right)
        {
            transform.localPosition = new Vector3(.5f, 0, .5f);
            transform.eulerAngles = new Vector3(0, -45, 0);
        }
        lastPosition = position;
    }

    protected override void EditorUpdate()
    {
        if(position != lastPosition)
        {
            if (position == Position.right)
            {
                transform.localPosition = new Vector3(.5f, 0, .5f);
                transform.eulerAngles = new Vector3(0, -45, 0);
            }
            else
            {
                transform.localPosition = new Vector3(-.5f, 0, .5f);
                transform.eulerAngles = new Vector3(0, -135, 0);
            }
            lastPosition = position;
        }
    }
}
