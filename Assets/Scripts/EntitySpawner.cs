using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour {

    //** These are taken in coordinates offset from the camera's edges **//
    public Vector2 spawnRectBoundsX;
    public Vector2 spawnRectBoundsY;

    public bool spawnLittlebot;
    public Transform littlebotPrefab;
    public List<Transform> littlebots;
    public Vector2 littlebotSpawnForceRange;
    public Vector2 littlebotSpawnTorqueRange;
    public bool littlebotCanRotateOnSpawn;
    public int littlebotSpawnQuantity;
    public float littlebotSpawnTime;
    [ReadOnly]
    public float littlebotTimer;

    private GameControl game;

	void Start () {
        game = GetComponent<GameControl>();
	}
	
	void Update () {
		if (spawnLittlebot) {
            spawnLittlebot = false;
            SpawnLittlebot();
        }

        littlebotTimer += Time.deltaTime;
        if (littlebotTimer >= littlebotSpawnTime) {
            SpawnLittlebot(littlebotSpawnQuantity);
            littlebotTimer = 0;
        }
	}

    void SpawnLittlebot(int num = 1) {
        while (num-- > 0) {
            Vector3 pos = new Vector2(Random.Range(game.topLeft.x + spawnRectBoundsX.x, game.topRight.x + spawnRectBoundsX.y),
                                      Random.Range(game.topLeft.y + spawnRectBoundsY.x, game.topRight.y + spawnRectBoundsY.y));
            Transform newBot;
            if (littlebots.Count > 0) {
                newBot = Object.Instantiate(littlebots[Random.Range(0, littlebots.Count)], pos, Quaternion.identity);
            } else {
                newBot = Object.Instantiate(littlebotPrefab, pos, Quaternion.identity);
            }

            newBot.eulerAngles = new Vector3(0, 0, Random.Range(0f, 360f));

            Rigidbody2D rb = newBot.GetComponent<Rigidbody2D>();
            rb.AddForce(newBot.up * -1 * Random.Range(littlebotSpawnForceRange.x, littlebotSpawnForceRange.y));
            rb.AddTorque(Random.Range(littlebotSpawnTorqueRange.x, littlebotSpawnTorqueRange.y));
        }
    }
}
