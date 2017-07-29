using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour {

    public float stackSpeed;
    public float connectMargin;     //how close a new stackable has to be to the head in order to be considered "connected"
    public Transform stackOrigin;
    public Transform stackHead;

    [ReadOnly]
    public List<Stackable> adjacentStackables;

    [ReadOnly]
    public List<Stackable> stackTransit;

    [ReadOnly]
    public List<Stackable> stack;

    private bool pickupPressed;

    void Start () {
        stackHead.position = stackOrigin.position;
	}

	void Update () {
		if (Input.GetButtonDown("Pickup")) {
            pickupPressed = true;
        }
	}

    void FixedUpdate() {
        if (pickupPressed) {
            pickupPressed = false;
            pickupStackable();
        }

        HandleStackTransit();
    }

    //Called every fixed update
    void HandleStackTransit() {
        float stackingStep = stackSpeed * Time.deltaTime;
        List<Stackable> toBeDeleted = new List<Stackable>();

        foreach (Stackable s in stackTransit) {
            s.transform.position = Vector2.MoveTowards(s.transform.position, stackHead.position, stackingStep);

            if (Util.CloseEnough(s.transform, stackHead, connectMargin)) {
                s.SetStacked();
                s.transform.parent = transform;

                toBeDeleted.Add(s);
                stack.Add(s);

                s.transform.eulerAngles = Vector3.zero;
                s.tailPoint.position = stackHead.position;
                stackHead.position = s.headPoint.position;
            }
        }

        foreach (Stackable s in toBeDeleted) {
            stackTransit.Remove(s);
        }
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        Stackable s = collision.GetComponent<Stackable>();
        if (s != null && s.state == R.NOT_STACKED) {
            adjacentStackables.Add(s);
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        Stackable s = collision.GetComponent<Stackable>();
        if (s != null && s.state == R.NOT_STACKED) {
            adjacentStackables.Remove(s);
        }
    }

    public Stackable pickupStackable() {
        if (adjacentStackables.Count == 0) {
            return null;
        }

        Stackable newGuy = adjacentStackables[0];
        adjacentStackables.RemoveAt(0);

        newGuy.SetInTransit();
        stackTransit.Add(newGuy);

        return newGuy;
    }
}
