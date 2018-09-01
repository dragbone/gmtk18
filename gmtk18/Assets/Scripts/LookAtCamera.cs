using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour {

	// Use this for initialization
	public Camera cameraToLookAt;
	
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var v = cameraToLookAt.transform.position - transform.position;
		v.x = v.z = 0.0f;
		transform.LookAt( cameraToLookAt.transform.position - v); 
		transform.Rotate(0,180,0);
	}
}
