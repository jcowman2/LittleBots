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
}
