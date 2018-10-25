﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject release;
    public float speed;
    public float delay;
    public float timer = 4;
    private SpriteRenderer sr;
    ParticleSystem ps;
    private float timer_;
    private bool dead = false;
    // Use this for initialization
    private void Awake() {
        sr = GetComponent<SpriteRenderer>();
        ps = GetComponent<ParticleSystem>();
    }
    void Start () {
		
        timer_ = timer;
    }
	
	// Update is called once per frame
	void Update () {
        if (!dead) {
            timer_ -= Time.deltaTime;
            if (timer_ <= 0) {
                AutoDestroy();
            }
            sr.color = new Color(1, sr.color.g - Time.deltaTime / timer, sr.color.b - Time.deltaTime / timer, 1);
            
        }
		
	}
    void AutoDestroy() {
        dead = true;
        GameObject r = Instantiate(release);
        r.transform.position = transform.position;
        ps.Stop();
        CircleCollider2D cc = GetComponent<CircleCollider2D>();
        ParticleSystem pss = transform.Find("Plasma").gameObject.GetComponent<ParticleSystem>();
        pss.Stop();
        cc.enabled = false;
        sr.enabled = false;
        Destroy(gameObject, ps.main.startLifetime.constantMax);
    }
    private void OnCollisionEnter2D(Collision2D collision) {
        AutoDestroy();
    }
}