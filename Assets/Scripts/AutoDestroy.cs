using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour {

	public float duration;

	// Use this for initialization
	void Start () {
		Destroy(this.gameObject,duration);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
