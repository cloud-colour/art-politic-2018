using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicServant : BaseCorrupter {

    public float radius;
    public float power;
	public Transform forceField;
	public Transform forceParent;

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

            var clone = Instantiate(forceField, Vector3.zero, Quaternion.identity, forceParent);
			clone.localPosition = Vector3.zero;
            var angle = Mathf.Atan2(col.contacts[0].normal.y, col.contacts[0].normal.x) * Mathf.Rad2Deg;
            clone.eulerAngles = new Vector3(0, 0, angle);
			//clone.position = col.contacts[0].point;
            SoundManager.inst.PlaySFXOneShot(15);
        }
    }

    public void Die()
    {
		isHappy = true;
		anim.Play("Action");
        CorrupterManager.Despawn(this);

        SoundManager.inst.PlaySFXOneShot(12);
		this.GetComponent<Collider>().enabled = false;
        //this.gameObject.SetActive(false);
    }
}
