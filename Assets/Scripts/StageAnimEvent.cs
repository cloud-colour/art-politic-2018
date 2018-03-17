using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageAnimEvent : MonoBehaviour
{
	[SerializeField] Animation uiAnim;

	public void ShowUI()
	{
		uiAnim.gameObject.SetActive(true);
		uiAnim.Play();
	}
}
