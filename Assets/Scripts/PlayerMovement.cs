using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Pablo Yamamoto, Santiago Kohn, Gianluca Beltran
 *
 * Script in charge of isntantiating the player class
 *
 */

public class PlayerMovement : MonoBehaviour
{

    // Fetch all player information
    public Animator animation;
    public Rigidbody2D rigidbody;
    public SpriteRenderer sprite;
    private BoxCollider2D groundTrigger;
    public static readonly int Walking = Animator.StringToHash("Walking");
    public static readonly int Jumped = Animator.StringToHash("Jumped");
    public bool isOnGround = true; 
    public static PlayerMovement instance = null;
    public Vector3 startingPos;

    private void Awake()
    {
        if (PlayerMovement.instance == null) // if an instance of the player already exists, destroy this gameobject else create new instance
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animator>(); // fetch animator component
        rigidbody = GetComponent<Rigidbody2D>(); // fetch rigidbody component
        sprite = GetComponent<SpriteRenderer>(); // fetch sprite component
        startingPos = GetComponent<Transform>().position; // fetch players starting position
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnGround = false; // when the players collider exists the ground, isOnGround is declared false
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        animation.SetBool(Jumped, false); // when the players collider lands on the ground the jumping animation is set to false
        isOnGround = true; 
    }
}
