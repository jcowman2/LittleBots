using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour {

    public Transform backgroundPrefab;

    [ReadOnly]
    public float width;

    [ReadOnly]
    public float height;

    [ReadOnly]
    public Vector3 lastCameraMove;

    private GameControl game;

	void Start () {
        game = GetComponent<GameControl>();

        SpriteRenderer sprite = backgroundPrefab.GetComponent<SpriteRenderer>();
        width = sprite.bounds.size.x;
        height = sprite.bounds.size.y;
	}
	
	
	void Update () {
        backgroundPrefab.position = new Vector3(backgroundPrefab.position.x + game.cameraPos.x - lastCameraMove.x,
                                                backgroundPrefab.position.y + game.cameraPos.y - lastCameraMove.y,
                                                backgroundPrefab.position.z);
        lastCameraMove = game.cameraPos;
	}

}
