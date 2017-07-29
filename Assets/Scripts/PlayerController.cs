using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 10f;
    public float jumpForce = 700f;

    [ReadOnly]
    public bool grounded = false;
    [ReadOnly]
    public bool jumped = false;

    public Transform groundCheck;
    public float groundRadius = 0.2f;
    public LayerMask groundMask;

    private Rigidbody2D rb;

	void Start () {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update () {
        if (grounded && Input.GetAxis("Jump") > 0) {
            jumped = true;
            //rb.AddForce(new Vector2(0, jumpForce));
        }
    }

	void FixedUpdate () {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        if (jumped) {
            rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            jumped = false;
            grounded = false;
        }

        float move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * maxSpeed, rb.velocity.y);
	}
}
