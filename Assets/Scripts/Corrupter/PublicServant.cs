using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicServant : BaseCorrupter {

    public float radius;
    public float power;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    override protected void OnCollisionEnter (Collision col)
    {
        if (cashCheck(col))
        {
            Rigidbody cash = col.gameObject.GetComponent<Rigidbody>();
            cash.velocity = Vector3.zero;
            cash.AddExplosionForce(power, transform.position, radius);
			cash.gameObject.layer = LayerMask.NameToLayer("CleanCash");
        }
    }

    public void Die()
    {
        CorrupterManager.Despawn(this);
        this.gameObject.SetActive(false);
    }
}
