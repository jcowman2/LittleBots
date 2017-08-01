using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameControl : MonoBehaviour {

    //[ReadOnly]
    public GameObject player;

    //** Charge Utilities **//
    //[ReadOnly]
    public float chargeLevel; //between 0 and 100

    public float dechargeRate = 2f;
    public bool doACharge;

    //[ReadOnly]
    public bool allowChargeSliderLerp = true;

    //** Camera Endpoints **//
    //[ReadOnly]
    public Vector3 topLeft;

    //[ReadOnly]
    public Vector3 bottomLeft;

    //[ReadOnly]
    public Vector3 topRight;

    //[ReadOnly]
    public Vector3 bottomRight;

    //[ReadOnly]
    public Vector3 cameraPos;

    //** UI **//
    public Text pointsText;
    public Text timeText;

    public Image gameOverPanel;
    public Text GOpointsText;
    public Text GOtimeText;


    private StaticCapsule staticCapsule;
    private new Camera camera;
    private BackgroundControl background;

    void Awake () {
        staticCapsule = GameObject.FindGameObjectWithTag(R.STATIC_CAPSULE).GetComponent<StaticCapsule>();
    } 

    void Start () {
        Time.timeScale = 1;

        player = GameObject.FindGameObjectWithTag(R.PLAYER);
        camera = GameObject.FindGameObjectWithTag(R.MAIN_CAMERA).GetComponent<Camera>();
        background = GetComponent<BackgroundControl>();

        UpdateCorners();

        if (staticCapsule.gameInProgress) { //Player died, but game is still running
            chargeLevel = staticCapsule.inProgressChargeLevel;
            allowChargeSliderLerp = false;
            pointsText.text = staticCapsule.totalPoints.ToString();
        } else {
            staticCapsule.gameInProgress = true;
            staticCapsule.totalPoints = 0;
            staticCapsule.totalSeconds = 0;

            pointsText.text = "0";
            timeText.text = "0:00";

            chargeLevel = 100;
        }
        
    }

    void Update() {
        UpdateCorners();
        cameraPos = camera.transform.position;

        changeChargeLevel(-1 * dechargeRate * Time.deltaTime);

        if (doACharge) {
            changeChargeLevel(5);
            doACharge = false;
        }

        staticCapsule.totalSeconds += Time.deltaTime;
        string secondsText = ((int)(staticCapsule.totalSeconds % 60)).ToString();
        secondsText = secondsText.Length == 1 ? "0" + secondsText : secondsText;
        timeText.text = ((int)(staticCapsule.totalSeconds / 60)).ToString() + ":" + secondsText;
    }

    void UpdateCorners () {
        topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1));
        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1));
        bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0));
    }

    public void OnFallOutMap(GameObject obj) {
        //Debug.Log(obj + " fell through the map at " + obj.transform.position);

        if (obj.CompareTag(R.PLAYER)) {
            OnPlayerFallOutMap();
        } else if (obj.CompareTag(R.LITTLEBOT)) {
            OnLittlebotFallOutMap(obj);
        }
    }

    private void OnPlayerFallOutMap() {
        staticCapsule.inProgressChargeLevel = chargeLevel;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnLittlebotFallOutMap(GameObject obj) {
        if (obj.GetComponent<LinkBehavior>().state != R.LINKED)
            GameObject.Destroy(obj);
    }

    void OnDrawGizmosSelected () {
        Gizmos.color = Color.yellow;

        //Camera Corners
        Gizmos.DrawSphere(topLeft, 0.5f);
        Gizmos.DrawSphere(bottomLeft, 0.5f);
        Gizmos.DrawSphere(topRight, 0.5f);
        Gizmos.DrawSphere(bottomRight, 0.5f);
    }

    public float changeChargeLevel(float amount) {
        float newChargeLevel = Mathf.Min(chargeLevel + amount, 100f);
        newChargeLevel = Mathf.Max(newChargeLevel, 0);

        int spriteIndex = (int)newChargeLevel / 10 + 1;
        if (spriteIndex == 11) {
            spriteIndex = 10;
        } else if (newChargeLevel == 0) {
            spriteIndex = 0;
            EndGame();
        }

        background.SetSpriteIndex(spriteIndex);

        chargeLevel = newChargeLevel;
        return chargeLevel;
    }

    public void addBotPoint() {
        staticCapsule.totalPoints++;
        pointsText.text = staticCapsule.totalPoints.ToString();
    }

    void EndGame() {
        Time.timeScale = 0;

        gameOverPanel.gameObject.SetActive(true);
        GOpointsText.text = pointsText.text;
        GOtimeText.text = timeText.text;

        pointsText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);

        staticCapsule.gameInProgress = false;
    }
}
