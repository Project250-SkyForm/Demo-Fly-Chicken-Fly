using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float speed = -2f; // control scroll speed from unity
    private Vector3 StartPos; // initial starting position

    // Start is called before the first frame update
    void Start()
    {
        StartPos = transform.position; // get initial starting pos of background
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(translation: Vector3.down * speed * Time.deltaTime); // move bg based on speed
        if (transform.position.y > StartPos.y + 10f) // starting pos of y: -14, check if background has gone below 
        {
            StartPos.y += 5;
            transform.position = StartPos; // reset background to initial position 
        }
            
    }
}
