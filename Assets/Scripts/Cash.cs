using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour {

    private Rigidbody rigid;
	// Use this for initialization
	void Start () 
    {
        rigid = GetComponent<Rigidbody>();
        rigid.sleepThreshold = 0.3f;
	}
	
	// Update is called once per frame
	void Update () 
    {
        
        if (rigid.IsSleeping())
        {
            gameObject.layer = LayerMask.NameToLayer("CleanCash");
        }
	}

    void OnCollisionEnter (Collision col)
    {
//        if(col.gameObject.tag == "corrupter")
//        {
//            gameObject.SetActive(false);
//        }
    }

    public void Reset()
    {
        gameObject.layer = LayerMask.NameToLayer("Cash");
    }
}
