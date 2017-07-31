using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    [ReadOnly]
    public GameObject player;

    //** Camera Endpoints **//
    [ReadOnly]
    public Vector3 topLeft;

    [ReadOnly]
    public Vector3 bottomLeft;

    [ReadOnly]
    public Vector3 topRight;

    [ReadOnly]
    public Vector3 bottomRight;

    [ReadOnly]
    public Vector3 cameraPos;

    private new Camera camera;

    void Start () {
        player = GameObject.FindGameObjectWithTag(R.PLAYER);
        camera = GameObject.FindGameObjectWithTag(R.MAIN_CAMERA).GetComponent<Camera>();
        UpdateCorners();
    }

    void Update() {
        UpdateCorners();
        cameraPos = camera.transform.position;
    }

    void UpdateCorners () {
        topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1));
        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1));
        bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0));
    }

    public void OnFallOutMap(GameObject obj) {
        Debug.Log(obj + " fell through the map at " + obj.transform.position);

        if (obj.CompareTag(R.PLAYER)) {
            OnPlayerFallOutMap();
        } else if (obj.CompareTag(R.LITTLEBOT)) {
            OnLittlebotFallOutMap(obj);
        }
    }

    private void OnPlayerFallOutMap() {
        RestartLevel();
    }

    private void OnLittlebotFallOutMap(GameObject obj) {
        if (obj.GetComponent<LinkBehavior>().state != R.LINKED)
            GameObject.Destroy(obj);
    }

    private void RestartLevel() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void OnDrawGizmosSelected () {
        Gizmos.color = Color.yellow;

        //Camera Corners
        Gizmos.DrawSphere(topLeft, 0.5f);
        Gizmos.DrawSphere(bottomLeft, 0.5f);
        Gizmos.DrawSphere(topRight, 0.5f);
        Gizmos.DrawSphere(bottomRight, 0.5f);
    }
}
