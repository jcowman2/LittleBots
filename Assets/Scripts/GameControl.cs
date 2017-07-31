using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    [ReadOnly]
    public GameObject player;

    //** Charge Utilities **//
    [ReadOnly]
    public int chargeLevel; //between 0 and 100

    public float dechargeTime = 0.5f; //Seconds for losing one point of charge

    [ReadOnly]
    public float timeSinceDecharge = 0f;

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
    private BackgroundControl background;

    void Start () {
        player = GameObject.FindGameObjectWithTag(R.PLAYER);
        camera = GameObject.FindGameObjectWithTag(R.MAIN_CAMERA).GetComponent<Camera>();
        background = GetComponent<BackgroundControl>();

        UpdateCorners();
        chargeLevel = 100;
    }

    void Update() {
        UpdateCorners();
        cameraPos = camera.transform.position;

        timeSinceDecharge += Time.deltaTime;
        if (timeSinceDecharge >= dechargeTime) {
            changeChargeLevel(-1);
            timeSinceDecharge = 0;
        }
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

    public int changeChargeLevel(int amount) {
        int newChargeLevel = Mathf.Min(chargeLevel + amount, 100);

        background.SetSpriteIndex(Mathf.Min(newChargeLevel / 10 + 1, 10));

        chargeLevel = newChargeLevel;
        if (chargeLevel < 0) {
            chargeLevel = 0;
        }
        return chargeLevel;
    }
}
