using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour {

    public Transform headPoint; //Point at which the stack head is placed
    public Transform tailPoint; //Point at which each stackable is connected

    [ReadOnly]
    public string state;

    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;

    private const int STACK_LAYER = 10;

	void Start () {
        state = R.NOT_STACKED;

        rb = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
	}
	
	void Update () {
		
	}

    public void SetInTransit() {
        Debug.Log(name + " moved to now in transit.");
        state = R.IN_TRANSIT;

        rb.bodyType = RigidbodyType2D.Kinematic;
        boxCollider.enabled = false;
    }

    public void SetStacked() {
        Debug.Log(name + " moved to now stacked.");
        state = R.STACKED;

        gameObject.layer = STACK_LAYER;
        //rb.bodyType = RigidbodyType2D.Kinematic;
        //boxCollider.enabled = true;
    }
}
