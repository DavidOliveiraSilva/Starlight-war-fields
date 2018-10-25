using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursorRipple : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 mPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        transform.position = new Vector3(mPosition.x, mPosition.y, transform.position.z);
	}
}
