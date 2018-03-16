using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
	[SerializeField] Animator mainAnim;

	[Header("Debug")]
	[SerializeField] bool skipIntro;
	// Use this for initialization
	void Start ()
	{
		if(skipIntro)
		{
			mainAnim.SetTrigger("DebugSkipIntro");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
