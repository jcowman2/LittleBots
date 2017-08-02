using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    //** Base Movement **//
    public float maxSpeed = 10f;
    public float jumpForce = 700f;
    //[ReadOnly]
    public bool grounded = false;
    //[ReadOnly]
    public bool jumped = false;

    //** Collision Detection **//
    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    //** Ball Movement **//
    public Rigidbody2D ball;
    public Transform ballSprite;
    public float ballAccelerateTime;
    public float ballTopRotateSpeed;
    //[ReadOnly]
    public float currentBallSpeed;
    //[ReadOnly]
    public bool ballMoving;

    //private Rigidbody2D ball;

	void Start () {
        //ball = GetComponent<Rigidbody2D>();
        //ball = GameObject.FindGameObjectWithTag(R.PLAYER).GetComponent<Rigidbody2D>();
        ballMoving = false;
    }

    private void Update () {
        if (grounded && Input.GetButtonDown("Jump")) {
            jumped = true;
        }
    }

	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        if (jumped) {
            ball.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumped = false;
            grounded = false;
        }

        float move = Input.GetAxis("Horizontal");
        ball.velocity = new Vector2(move * maxSpeed, ball.velocity.y);

        ballMoving = Mathf.Abs(move) > 0;
        currentBallSpeed = move * ballTopRotateSpeed;

        ballSprite.Rotate(ballSprite.forward, currentBallSpeed * -1);

    }
}
