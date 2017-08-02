using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour {

    private AudioSource audioSource;

    void Awake() {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Start () {
        audioSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		if (Input.GetButtonDown("Mute")) {
            Debug.Log("MUTE");
            audioSource.mute = !audioSource.mute;
        }
	}
}
