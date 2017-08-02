using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChainManager : MonoBehaviour {

    public float firePower;
    public float reactiveForceMultiplier = 1f; //how much stronger launches are felt to the player
    public Vector2 relativeStartPoint;

    //[ReadOnly]
    public Vector3 actualStartPoint;

    //[ReadOnly]
    public List<LinkBehavior> links;

    //[ReadOnly]
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

        actualStartPoint = transform.position + new Vector3(transform.up.x, transform.up.y, 0)
                                              + transform.right * relativeStartPoint.x
                                              + transform.up * relativeStartPoint.y;
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
        if (link != null && link.state != R.LINKED) {
            adjacentLinkables.Add(link);
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        LinkBehavior link = collision.GetComponent<LinkBehavior>();
        if (link != null && link.state != R.LINKED) {
            adjacentLinkables.Remove(link);
        }
    }

    void pickupLinkable() {
        LinkBehavior link = adjacentLinkables[0];
        adjacentLinkables.RemoveAt(0);

        if (links.Count == 0) {
            link.transform.eulerAngles = transform.eulerAngles;
            link.transform.position = actualStartPoint;
            link.MakeLink(rb);
        } else {
            LinkBehavior topLink = links[links.Count - 1];
            link.transform.eulerAngles = topLink.transform.eulerAngles;
            //link.transform.position = topLink.transform.position + link.height * topLink.transform.up;
            link.transform.position = topLink.transform.position + (link.height / 2 + topLink.height / 2) * topLink.transform.up;
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

    private void OnDrawGizmosSelected () {
        Gizmos.color = Color.blue;
        //Gizmos.DrawSphere(transform.position + new Vector3(relativeStartPoint.x * transform.up.x, relativeStartPoint.y * transform.up.y), 0.01f);
        Gizmos.DrawLine(transform.position, actualStartPoint);
    }
}
