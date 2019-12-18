using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cash : MonoBehaviour {

    public Rigidbody Rigidbody { get; set; }
	// Use this for initialization
	protected void Awake () 
    {
        Rigidbody = gameObject.GetComponent<Rigidbody>();
        Rigidbody.sleepThreshold = 0.3f;
	}
	
	// Update is called once per frame
	public virtual void Update () 
    {
        if (Rigidbody.IsSleeping())
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
