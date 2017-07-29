using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChainManager : MonoBehaviour {

    public Vector3 relativeStartPoint;

    [ReadOnly]
    public List<LinkBehavior> links;

    [ReadOnly]
    public List<LinkBehavior> adjacentLinkables;

    private bool pickupPressed;
    private Rigidbody2D rb;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
	}

    void Update () {
        if (Input.GetButtonDown("Pickup")) {
            pickupPressed = true;
        }
    }

    void FixedUpdate () {
        if (pickupPressed) {
            pickupPressed = false;

            if (adjacentLinkables.Count > 0) {
                pickupLinkable(adjacentLinkables[0]);
                adjacentLinkables.RemoveAt(0);
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

    void pickupLinkable(LinkBehavior link) {
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
}
