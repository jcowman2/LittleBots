using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

	public void OnFallOutMap(GameObject obj) {
        Debug.Log(obj + " fell through the map at " + obj.transform.position);

        if (obj.CompareTag(R.PLAYER)) {
            OnPlayerFallOutMap();
        }
    }

    private void OnPlayerFallOutMap() {
        RestartLevel();
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
