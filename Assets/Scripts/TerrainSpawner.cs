using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainSpawner : MonoBehaviour {

    public Transform terrainContainer;
    public Transform platformPrefab;

    public Vector2 platformSpaceRange;
    public Vector2 platformVertRange;
    public Vector2 platformWidthRange; //Scale, not units

    public bool spawnPlatformRight;
    public bool spawnPlatformLeft;

    //** Camera Endpoints **//
    [ReadOnly]
    public Vector3 topLeft;

    [ReadOnly]
    public Vector3 bottomLeft;

    [ReadOnly]
    public Vector3 topRight;

    [ReadOnly]
    public Vector3 bottomRight;

    //** Platform Bounds **//
    [ReadOnly]
    public Vector3 leftEnd;

    [ReadOnly]
    public Vector3 rightEnd;

    private new Camera camera;
    private Transform leftMostPlatform;
    private Transform rightMostPlatform;

    void Start () {
        camera = GetComponent<Camera>();
        UpdateCorners();

        Transform startingPlatform = terrainContainer.Find("StartingPlatform");
        Vector3 startingEnds = GetPlatformCorners(startingPlatform.GetComponent<BoxCollider2D>());
        leftEnd = new Vector3(startingEnds.x, startingEnds.y);
        rightEnd = new Vector3(startingEnds.z, startingEnds.y);

        leftMostPlatform = startingPlatform;
        rightMostPlatform = startingPlatform;
	}
	
	void Update () {
        UpdateCorners();

        if (spawnPlatformRight) {
            spawnPlatformRight = false;
            GeneratePlatformRight();
        }

        if (spawnPlatformLeft) {
            spawnPlatformLeft = false;
            GeneratePlatformLeft();
        }
	}

    void UpdateCorners() {
        topLeft = camera.ViewportToWorldPoint(new Vector3(0, 1));
        bottomLeft = camera.ViewportToWorldPoint(new Vector3(0, 0));
        topRight = camera.ViewportToWorldPoint(new Vector3(1, 1));
        bottomRight = camera.ViewportToWorldPoint(new Vector3(1, 0));
    }

    //x = leftX, y = y, z = rightX (so that two coords in a flat line can be returned)
    Vector3 GetPlatformCorners(BoxCollider2D box) {
        Vector3 scale = box.transform.localScale;
        return new Vector3(box.transform.position.x - box.size.x * box.transform.localScale.x / 2,
                           box.transform.position.y + box.size.y * box.transform.localScale.y / 2,
                           box.transform.position.x + box.size.x * box.transform.localScale.x / 2);
    }

    Vector3 GetPlatformRightCorner(BoxCollider2D box) {
        Vector3 result = GetPlatformCorners(box);
        return new Vector3(result.z, result.y);
    }

    Vector3 GetPlatformLeftCorner(BoxCollider2D box) {
        Vector3 result = GetPlatformCorners(box);
        return new Vector3(result.x, result.y);
    }

    void GeneratePlatformRight() {
        Transform newPlatform = Object.Instantiate(platformPrefab);
        newPlatform.gameObject.SetActive(false);
        newPlatform.localScale = new Vector2(newPlatform.localScale.x * Random.Range(platformWidthRange.x, platformWidthRange.y),
                                             newPlatform.localScale.y);

        BoxCollider2D newCollider = newPlatform.GetComponent<BoxCollider2D>();
        float width = newCollider.size.x * newPlatform.localScale.x;
        float height = newCollider.size.y * newPlatform.localScale.y;

        float margin = Random.Range(platformSpaceRange.x, platformSpaceRange.y);
        float vertStep = Random.Range(platformVertRange.x, platformVertRange.y);

        newPlatform.position = new Vector2(rightEnd.x + width / 2 + margin,
                                           rightEnd.y - height / 2 + vertStep);

        newPlatform.gameObject.SetActive(true);

        rightMostPlatform = newPlatform;
        rightEnd = GetPlatformRightCorner(newCollider);
    }

    void GeneratePlatformLeft () {
        Transform newPlatform = Object.Instantiate(platformPrefab);
        newPlatform.gameObject.SetActive(false);
        newPlatform.localScale = new Vector2(newPlatform.localScale.x * Random.Range(platformWidthRange.x, platformWidthRange.y),
                                             newPlatform.localScale.y);

        BoxCollider2D newCollider = newPlatform.GetComponent<BoxCollider2D>();
        float width = newCollider.size.x * newPlatform.localScale.x;
        float height = newCollider.size.y * newPlatform.localScale.y;

        float margin = Random.Range(platformSpaceRange.x, platformSpaceRange.y);
        float vertStep = Random.Range(platformVertRange.x, platformVertRange.y);

        newPlatform.position = new Vector2(leftEnd.x - width / 2 - margin,
                                           leftEnd.y - height / 2 + vertStep);

        newPlatform.gameObject.SetActive(true);

        leftMostPlatform = newPlatform;
        leftEnd = GetPlatformLeftCorner(newCollider);
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.yellow;

        //Camera Corners
        Gizmos.DrawSphere(topLeft, 0.5f);
        Gizmos.DrawSphere(bottomLeft, 0.5f);
        Gizmos.DrawSphere(topRight, 0.5f);
        Gizmos.DrawSphere(bottomRight, 0.5f);

        //Platform Ends
        Gizmos.DrawSphere(leftEnd, 0.25f);
        Gizmos.DrawSphere(rightEnd, 0.25f);
    }
}