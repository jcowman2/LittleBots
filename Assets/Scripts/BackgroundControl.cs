using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundControl : MonoBehaviour {

    public Transform backgroundInstance;

    public List<Sprite> sprites; //list of possible sprites

    [ReadOnly]
    public int currentSpriteIndex = 0;

    [ReadOnly]
    public float width;

    [ReadOnly]
    public float height;

    [ReadOnly]
    public Vector3 lastCameraMove;

    private GameControl game;
    private SpriteRenderer spriteRenderer;

	void Start () {
        game = GetComponent<GameControl>();

        spriteRenderer = backgroundInstance.GetComponent<SpriteRenderer>();
        width = spriteRenderer.bounds.size.x;
        height = spriteRenderer.bounds.size.y;

        if (sprites.Count > 0) {
            spriteRenderer.sprite = sprites[sprites.Count - 1];
            currentSpriteIndex = sprites.Count - 1;
        }
	}
	
	
	void Update () {
        backgroundInstance.position = new Vector3(backgroundInstance.position.x + game.cameraPos.x - lastCameraMove.x,
                                                  backgroundInstance.position.y + game.cameraPos.y - lastCameraMove.y,
                                                  backgroundInstance.position.z);
        lastCameraMove = game.cameraPos;
	}

    public void SetSpriteIndex(int index) {
        if (index != currentSpriteIndex) {
            spriteRenderer.sprite = sprites[index];
            currentSpriteIndex = index;
        }
    }

}
