using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour {
    public GameObject target;
    [Range(0.0f, 1.0f)]
    public float softness;
    private bool active;
    public float maxOffset;
    [Range(0.01f, 1f)]
    public float offsetVelocity;
    private float currentMira;
	// Use this for initialization
	void Start () {
        
		if(target == null) {
            active = false;
        } else {
            active = true;
        }
        //i know: active = target != null
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (active) {
            float p = softness;
            if(p == 1) {
                p = 0.999f;
            }
            Vector3 position = new Vector3(target.transform.position.x * (1 - p) + transform.position.x * p, target.transform.position.y * (1 - p) + transform.position.y * p, transform.position.z);
            float mira = target.GetComponent<Player>().GetAim();
            
            currentMira = Mathf.LerpAngle(currentMira, mira, offsetVelocity);
            transform.position = new Vector3(position.x + maxOffset*Mathf.Cos(currentMira*Mathf.Deg2Rad), position.y + maxOffset * Mathf.Sin(currentMira * Mathf.Deg2Rad), transform.position.z);
        }
	}
    float distance(Vector2 a, Vector2 b) {
        return Mathf.Sqrt(Mathf.Pow(a.x - b.x, 2) + Mathf.Pow(a.y - b.y, 2));
    }
    
}
