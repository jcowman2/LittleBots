﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChainManager : MonoBehaviour {

    public float firePower;
    public float reactiveForceMultiplier = 1f; //how much stronger launches are felt to the player
    public Vector3 relativeStartPoint;

    [ReadOnly]
    public List<LinkBehavior> links;

    [ReadOnly]
    public List<LinkBehavior> adjacentLinkables;

    private Rigidbody2D rb;
    private bool pickupPressed;
    private bool firePressed;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update () {
        if (Input.GetButtonDown("Pickup")) {
            pickupPressed = true;
        }

        if (Input.GetButtonDown("Fire")) {
            firePressed = true;
        }
    }

    void FixedUpdate () {
        if (pickupPressed) {
            pickupPressed = false;

            if (adjacentLinkables.Count > 0) {
                pickupLinkable();
            }
        }

        if (firePressed) {
            firePressed = false;

            if (links.Count > 0) {
                launchLink();
            }
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        LinkBehavior link = collision.GetComponent<LinkBehavior>();
        if (link != null && link.state == R.UNLINKED) {
            adjacentLinkables.Add(link);
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        LinkBehavior link = collision.GetComponent<LinkBehavior>();
        if (link != null && link.state == R.UNLINKED) {
            adjacentLinkables.Remove(link);
        }
    }

    void pickupLinkable() {
        LinkBehavior link = adjacentLinkables[0];
        adjacentLinkables.RemoveAt(0);

        if (links.Count == 0) {
            link.transform.eulerAngles = new Vector3(0, 0, 0);
            link.transform.position = transform.position + relativeStartPoint;
            link.MakeLink(rb);
        } else {
            LinkBehavior topLink = links[links.Count - 1];
            link.transform.eulerAngles = topLink.transform.eulerAngles;
            link.transform.position = topLink.transform.position + link.height * topLink.transform.up;
            Debug.Log(link.transform.position);
            link.MakeLink(topLink.GetComponent<Rigidbody2D>());
        }

        links.Add(link);
    }

    void launchLink() {
        LinkBehavior link = links[links.Count - 1];
        links.RemoveAt(links.Count - 1);

        link.BreakLink();

        Vector2 forceVector = link.transform.up * firePower;
        link.GetComponent<Rigidbody2D>().AddForceAtPosition(forceVector, link.transform.position, ForceMode2D.Impulse);

        forceVector.Scale(new Vector2(-1, -1));
        if (links.Count > 0) {
            Rigidbody2D nextLinkRigidBody = links[links.Count - 1].GetComponent<Rigidbody2D>();
            nextLinkRigidBody.AddForceAtPosition(forceVector, link.transform.position, ForceMode2D.Impulse);
        }

        forceVector *= reactiveForceMultiplier;
        rb.AddForce(forceVector, ForceMode2D.Impulse);
    }
}
