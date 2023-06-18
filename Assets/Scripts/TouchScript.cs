using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchScript : MonoBehaviour {

	public ParticleSystem ps;

	void Start () {
		
	}
	
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			Vector2 sp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			transform.position = sp;
			ps.Play();
		}
	}
}
