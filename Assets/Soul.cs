using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soul : MonoBehaviour {
    public GameObject skill;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    public void AutoDestroy() {
        ParticleSystem ps = GetComponent<ParticleSystem>();
        ps.Stop();
        Destroy(gameObject, ps.main.startLifetime.constantMax);
    }
}
