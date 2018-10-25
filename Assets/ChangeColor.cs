using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour {
    private SpriteRenderer sr;
    private float timer;
	// Use this for initialization
	void Start () {
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            float red = Random.Range(0.0f, 1.0f);
            sr.color = new Color(red, Random.Range(0.0f, 0.6f), 1 - red);
            timer = Random.Range(2.0f, 4.0f);
        }
	}
}
