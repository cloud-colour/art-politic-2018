using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCorrupter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    virtual protected void Update () {
        transform.Translate(Vector3.left * Time.deltaTime);
	}

    void OnCollisionEnter (Collision col)
    {
        if(col.gameObject.tag == "cash")
        {
            gameObject.SetActive(false);
        }
    }
}
