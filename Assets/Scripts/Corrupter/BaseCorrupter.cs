using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCorrupter : MonoBehaviour {
    [HideInInspector]
    public CorrupterManager CorrupterManager;
    [HideInInspector]
    public int CorrupterID;

	protected Animation anim;
	protected bool isHappy;

	// Use this for initialization
	protected void Start () {
		anim = GetComponent<Animation>();
		Invoke("DoRandomStuff", Random.Range(1f, 5f));
	}

	void DoRandomStuff()
	{
		if (isHappy)
			return;

		int rand = Random.Range(1, 3);
		if (rand == 0)//flip facing
		{
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
		else //play wiggle 1 or 2
		{
			anim.Blend("Wiggle" + rand);
		}

		Invoke("DoRandomStuff", Random.Range(1f, 5f));
	}

	// Update is called once per frame
	virtual protected void Update () {
//        transform.Translate(Vector3.left * Time.deltaTime);
	}

    virtual protected void OnCollisionEnter (Collision col)
    {
        if(cashCheck(col))
        {
            GotCash(col.gameObject);
        }
    }

    virtual protected bool cashCheck(Collision col)
    {
        return col.gameObject.tag == "cash" && col.gameObject.activeInHierarchy;
    }

    virtual protected void GotCash(GameObject cash)
    {
        SoundManager.inst.PlaySFXOneShot(4);
        CorrupterManager.Despawn(this);
        cash.SetActive(false);
		this.GetComponent<BoxCollider>().enabled = false;
    }
    
	//called in "Action" animation
	public void DespawnCorrupter()
	{
		CorrupterManager.Despawn(this);
	}
}
