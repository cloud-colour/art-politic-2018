using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseCorrupter {

	// Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    override protected void OnCollisionEnter (Collision col)
    {
        if(cashCheck(col))
        {
			isHappy = true;
			transform.localScale = Vector3.one;
			anim.Stop();
			anim.Play("Action");

            GotCash(col.gameObject);
        }
    }

	public void ShootPeople()
	{
		var publicServants = CorrupterManager.GetCorrupterByID((int)CorrupterType.PublicServant);
		if (publicServants.Count > 0)
		{
			PublicServant closestPublicServant = null;
			float closestDistance = Screen.width;
			float distance;
			foreach (var publicServant in publicServants)
			{
				distance = Vector3.Distance(this.transform.position, publicServant.transform.position);
				if(closestDistance > distance)
				{
					closestDistance = distance;
					closestPublicServant = publicServant as PublicServant;
				}
			}
			SoundManager.inst.PlaySFXOneShot(6);
			if(closestPublicServant != null)
				closestPublicServant.Die();
		}
	}
}
