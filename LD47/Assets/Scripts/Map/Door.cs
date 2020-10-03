using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : InteractableObject
{
    protected override void Start()
    {
        base.Start();
    }

    public override void InteractEnter()
    {
        transform.position -= new Vector3(0, .5f, 0);
    }

    public override void InteractExit()
    {
        transform.position += new Vector3(0, .5f, 0);
    }
}
