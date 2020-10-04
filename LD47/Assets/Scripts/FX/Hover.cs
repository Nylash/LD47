using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Hover : MonoBehaviour
{
    [SerializeField] private Vector2 MinMaxHeight = new Vector2(-1,1);
    [SerializeField] private float HoverLoopDuration = 2; 
    [SerializeField] private AnimationCurve HeightCurve = null;
    private float StartHeight = 0;
    private float TimeElapsed = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartHeight = transform.position.y;
        TimeElapsed = Random.Range(0, HoverLoopDuration);
    }

    // Update is called once per frame
    void Update()
    {
        TimeElapsed += Time.deltaTime;

        if (TimeElapsed >= HoverLoopDuration)
        {
            TimeElapsed -= HoverLoopDuration;
        }
        
        float alpha = TimeElapsed / HoverLoopDuration;
        Vector3 position = transform.position;
        position.y = StartHeight + Mathf.Lerp(MinMaxHeight.x, MinMaxHeight.y, HeightCurve.Evaluate(alpha));
        transform.position = position;
    }
    
    
}
