using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCapsule : MonoBehaviour {

    [ReadOnly]
    public bool gameInProgress;

    [ReadOnly]
    public float inProgressChargeLevel;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }

}
