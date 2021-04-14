using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] public float speed = 0.5f;
    [SerializeField] public float jumpHeight = 0.5f;
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
        if (PlayerMovement.instance == null)
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
        animation = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        startingPos = GetComponent<Transform>().position;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        isOnGround = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        animation.SetBool(Jumped, false);
        isOnGround = true;
    }
}
