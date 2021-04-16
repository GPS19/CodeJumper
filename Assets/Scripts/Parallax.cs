using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script to handle parallax effect
 */

public class Parallax : MonoBehaviour
{
    private float startPos;
    public GameObject cam;
    public float parallaxEffect;
    
    // Start is called before the first frame update
    void Start() // fetch starting position of background
    {
        startPos = transform.position.x;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        float dist = (cam.transform.position.x * parallaxEffect); // declare distance to move the background, the closer the image the smalled the number

        transform.position = new Vector3(startPos + dist, transform.position.y, transform.position.z); // update the position of the background
    }
}
