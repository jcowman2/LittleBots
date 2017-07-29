using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackManager : MonoBehaviour {

    public Transform stackOrigin;
    public Transform stackHead;

    [ReadOnly]
    public List<Stackable> adjacentStackables;

    [ReadOnly]
    public List<Stackable> stackTransit;

    public List<Stackable> stack;

    private bool pickupPressed;

    void Start () {
		
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
