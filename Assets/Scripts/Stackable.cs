using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stackable : MonoBehaviour {

    [ReadOnly]
    public string state;

	void Start () {
        state = R.NOT_STACKED;
	}
	
	void Update () {
		
	}

    public void SetInTransit() {
        Debug.Log(name + " moved now in transit.");
        state = R.IN_TRANSIT;
    }
}
