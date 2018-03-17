using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SequenceManager : MonoBehaviour
{
	public static SequenceManager instance;
	void Awake()
	{
		if(instance == null)
			instance = this;
	}

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

	public void StartEndSequence()
	{
		mainAnim.SetTrigger("End");
	}

	// Update is called once per frame
	void Update ()
	{
		
	}
}
