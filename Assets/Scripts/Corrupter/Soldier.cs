using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : BaseCorrupter {

	Animation anim;
	bool isHappy;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animation>();
		Invoke("DoRandomStuff",Random.Range(1f,5f));
	}

	void DoRandomStuff()
	{
		if(isHappy)
			return;

		int rand = Random.Range(1,3);
		if(rand == 0)//flip facing
		{
			transform.localScale = new Vector3(transform.localScale.x*-1,transform.localScale.y,transform.localScale.z);
		}
		else //play wiggle 1 or 2
		{
			anim.Play("Wiggle"+rand);
		}

		Invoke("DoRandomStuff",Random.Range(1f,5f));
	}

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
			PublicServant closestPublicServant = new PublicServant();
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
			SoundManager.inst.PlaySFX(6);
			closestPublicServant.Die();
		}
	}
}
