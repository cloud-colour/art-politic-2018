using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseCorrupter {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    override protected void OnCollisionEnter (Collision col)
    {
        if(cashCheck(col))
        {
            var publicServants = CorrupterManager.GetCorrupterByID((int)CorrupterType.PublicServant);
            if (publicServants.Count > 0)
            {
                PublicServant[] closestPublicServantArray = new PublicServant[1];
                float closestDistance = Screen.width;
                float distance;
                foreach (var publicServant in publicServants)
                {
                    distance = Vector3.Distance(this.transform.position, publicServant.transform.position);
                    if(closestDistance > distance)
                    {
                        closestDistance = distance;
                        closestPublicServantArray[0] = publicServant as PublicServant;
                    }
                }
                SoundManager.inst.PlaySFXOneShot(6);
                if(closestPublicServantArray[0] != null)
                    closestPublicServantArray[0].Die();
            }

            GotCash(col.gameObject);
        }
    }
}
