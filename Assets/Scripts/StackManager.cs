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

    void Start () {
		
	}

	void Update () {
		
	}

    private void OnTriggerEnter2D (Collider2D collision) {
        Stackable s = collision.GetComponent<Stackable>();
        if (s != null) {
            adjacentStackables.Add(s);
        }
    }

    private void OnTriggerExit2D (Collider2D collision) {
        Stackable s = collision.GetComponent<Stackable>();
        if (s != null) {
            adjacentStackables.Remove(s);
        }
    }
}
