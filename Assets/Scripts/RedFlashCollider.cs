using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlashCollider : MonoBehaviour
{
	[SerializeField] Animation anim;
	float interval = 0;

	void OnTriggerEnter()
	{
		if(interval > 1f)
		{
			interval = 0;
			anim.Stop();
			anim.Play("FlashRed");
		}
	}

	void Update()
	{
		interval += Time.deltaTime;
	}
}
