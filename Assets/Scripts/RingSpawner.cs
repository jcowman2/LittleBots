using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingSpawner : MonoBehaviour {

    public Transform ringPrefab;
    public Vector2 verticalSpawnRange;

    public float chanceRing;

    public float chanceHorizontal;
    public float chanceOddAngle;

    private TerrainSpawner terrainSpawner;

	void Start () {
        terrainSpawner = GetComponent<TerrainSpawner>();
	}

    //rings can only be spawned off the left edge of right platforms, and right edge of left platforms.
    public void SpawnRings(GameObject platformObj, string orientation, float spaceFromPreviousPlatform) {
        if (Random.Range(0f, 1f) > chanceRing) {
            return;
        }

        BoxCollider2D platform = platformObj.GetComponent<BoxCollider2D>();

        Vector3 leftCorner = terrainSpawner.GetPlatformLeftCorner(platform);
        Vector3 rightCorner = terrainSpawner.GetPlatformRightCorner(platform);

        Vector3 pos;

        if (orientation == R.RIGHT) {
            pos = new Vector3(Random.Range(leftCorner.x - spaceFromPreviousPlatform, rightCorner.x),
                              Random.Range(verticalSpawnRange.x, verticalSpawnRange.y) + leftCorner.y);
        } else if (orientation == R.LEFT) {
            pos = new Vector3(Random.Range(leftCorner.x, rightCorner.x + spaceFromPreviousPlatform),
                              Random.Range(verticalSpawnRange.x, verticalSpawnRange.y) + leftCorner.y);
        } else {
            throw new System.Exception("Direction must be RIGHT or LEFT in SpawnRings()");
        }

        Transform ring = Instantiate(ringPrefab, pos, Quaternion.identity);

        if (Random.Range(0f, 1f) < chanceHorizontal + chanceOddAngle) {
            if (Random.Range(0, chanceHorizontal + chanceOddAngle) > chanceOddAngle) {
                ring.eulerAngles = new Vector3(ring.eulerAngles.x, ring.eulerAngles.y, -90);
            } else {
                ring.eulerAngles = new Vector3(ring.eulerAngles.x, ring.eulerAngles.y, Random.Range(-90, 90));
            }
        }
        
    }
	
}
