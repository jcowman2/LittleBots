using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticCapsule : MonoBehaviour {

    [ReadOnly]
    public bool gameInProgress;

    [ReadOnly]
    public float inProgressChargeLevel;

    [ReadOnly]
    public int totalPoints;

    [ReadOnly]
    public float totalSeconds;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }

}
