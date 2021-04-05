﻿using System;
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

    // Update is called once per frame
    void Update()
    {
        //Move();
    }

    void Move()
    {
        animation.SetBool(Walking, false);

        if (Input.GetKey(KeyCode.A))
        {
            animation.SetBool(Walking, true);
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            sprite.flipX = true;
        }
        
        if (Input.GetKey(KeyCode.D))
        {
            animation.SetBool(Walking, true);
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            sprite.flipX = false;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            animation.SetBool(Jumped, true);
            rigidbody.AddForce(Vector3.up * jumpHeight, ForceMode2D.Impulse);
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        isOnGround = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        animation.SetBool(Jumped, false);
        isOnGround = true;
        Debug.Log(other);
    }
}
