using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndParticleSystem : MonoBehaviour {
    private ParticleSystem ps;
	// Use this for initialization
	void Start () {
        ps = GetComponent<ParticleSystem>();
        Destroy(gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
