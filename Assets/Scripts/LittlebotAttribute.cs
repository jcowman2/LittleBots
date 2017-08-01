using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittlebotAttribute : MonoBehaviour {

    public Color[] colors;

    //[ReadOnly]
    public Color bodyColor;

    //[ReadOnly]
    public Color faceColor;

    private SpriteRenderer bodySprite;
    private Transform face;
    private SpriteRenderer faceSprite;

	void Start () {
        bodySprite = GetComponent<SpriteRenderer>();
        face = transform.GetChild(0);
        faceSprite = face.GetComponent<SpriteRenderer>();

        if (colors.Length > 1) {
            int bodyIndex = Random.Range(0, colors.Length);
            int faceIndex = Random.Range(0, colors.Length);
            
            while (bodyIndex == faceIndex) {
                faceIndex = Random.Range(0, colors.Length);
            }

            bodySprite.color = colors[bodyIndex] - new Color(0.2f, 0.2f, 0.2f, 0);
            faceSprite.color = colors[faceIndex];
        } else {
            Debug.LogError("Littlebot must have at least 2 color options.");
        }

        bodyColor = bodySprite.color;
        faceColor = faceSprite.color;
	}

}
