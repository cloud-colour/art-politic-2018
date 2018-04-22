using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedFlashCollider : MonoBehaviour
{
	[SerializeField] Animation anim;

	void OnTriggerEnter()
	{
		anim.Stop();
		anim.Play("FlashRed");
	}
}
