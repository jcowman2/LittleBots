using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallZone : MonoBehaviour {

    private GameControl gameControl;

    void Start () {
        gameControl = GameObject.FindGameObjectWithTag(R.GAME_CONTROLLER).GetComponent<GameControl>();
    }

    private void OnTriggerEnter2D (Collider2D collision) {
        gameControl.OnFallOutMap(collision.gameObject);
    }

}
