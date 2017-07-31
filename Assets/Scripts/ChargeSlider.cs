using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChargeSlider : MonoBehaviour {

    public float blendSpeed;

    [ReadOnly]
    public float tParam;

    [ReadOnly]
    public float blendDeficit;

    private RectTransform rectTransform;
    private GameControl game;
	
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        game = GameObject.FindGameObjectWithTag(R.GAME_CONTROLLER).GetComponent<GameControl>();
    }
	
	void Update () {
        Vector3 scale = rectTransform.localScale;

        blendDeficit = Mathf.Abs(scale.x * 100 - game.chargeLevel);
        if (blendDeficit > 0.01) {
            tParam += Time.deltaTime * blendSpeed;
            rectTransform.localScale = new Vector3(Mathf.Lerp(scale.x, game.chargeLevel / 100f, tParam),
                                                   scale.y);
        } else {
            tParam = 0;
        }

        //rectTransform.localScale = new Vector3(game.chargeLevel / 100f, scale.y);
		/*if (blendDeficit > 0.001) {
            float newScale = Mathf.Min(blendDeficit * Time.deltaTime / blendRate, blendDeficit);
            newScale *= transform.localScale.x > game.chargeLevel ? -1 : 1;

            rectTransform.localScale = new Vector2(scale.x + newScale, scale.y);
        }*/
	}
}
