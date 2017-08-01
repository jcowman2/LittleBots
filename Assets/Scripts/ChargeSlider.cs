using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChargeSlider : MonoBehaviour {

    public float blendSpeed;

    public Color healthyColor; //100
    public Color midColor;     //50
    public Color badColor;     //15
    public Color deathColor;   //0

    [ReadOnly]
    public float relativeColor;

    [ReadOnly]
    public float tParam;

    [ReadOnly]
    public float blendDeficit;

    private RectTransform rectTransform;
    private Image image;
    private GameControl game;
	
	void Start () {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        game = GameObject.FindGameObjectWithTag(R.GAME_CONTROLLER).GetComponent<GameControl>();
    }
	
	void Update () {
        Vector3 scale = rectTransform.localScale;

        if (game.allowChargeSliderLerp) {
            blendDeficit = Mathf.Abs(scale.x * 100 - game.chargeLevel);

            if (blendDeficit > 0.01) {
                tParam += Time.deltaTime * blendSpeed;
                rectTransform.localScale = new Vector3(Mathf.Lerp(scale.x, game.chargeLevel / 100f, tParam),
                                                       scale.y);
            } else {
                tParam = 0;
            }
        } else {
            rectTransform.localScale = new Vector3(game.chargeLevel / 100f, scale.y);
            game.allowChargeSliderLerp = true;
        }
        

        ChangeColor();
	}

    private void ChangeColor() {
        Color newColor;

        if (game.chargeLevel >= 50) {
            relativeColor = (game.chargeLevel - 50f) / 50f;
            newColor = Color.Lerp(midColor, healthyColor, relativeColor);
        } else if (game.chargeLevel >= 15) {
            relativeColor = (game.chargeLevel - 15f) / 25f;
            newColor = Color.Lerp(badColor, midColor, relativeColor);
        } else {
            relativeColor = game.chargeLevel / 15f;
            newColor = Color.Lerp(deathColor, badColor, relativeColor);
        }

        image.color = newColor;
    }
}
