using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkBehavior : MonoBehaviour {

    public int scorePoints = 1;

    public float height;

    //[ReadOnly]
    public string state;

    private HingeJoint2D hinge;
	
	void Start () {
        hinge = GetComponent<HingeJoint2D>();
        hinge.enabled = false;
        state = R.NEWBORN;
	}
	
	public void MakeLink(Rigidbody2D link) {
        hinge.connectedBody = link;
        hinge.enabled = true;
        state = R.LINKED;
    }

    public void BreakLink() {
        hinge.connectedBody = null;
        hinge.enabled = false;
        state = R.UNLINKED;
    }
}
