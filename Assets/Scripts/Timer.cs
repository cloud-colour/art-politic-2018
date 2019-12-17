using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
	TextMeshProUGUI text;
	float timer = 30;
	float elapse;

	void Start()
	{
		text = this.GetComponent<TextMeshProUGUI>();
	}

	void Update ()
	{
		if(timer > 20)
		{
			elapse += Time.deltaTime;
		}
		else if(timer > 10)
		{
			elapse += Time.deltaTime*2;
		}
		else if(timer > 1)
		{
			elapse += Time.deltaTime*3;
		}
		else
		{
			timer = Random.Range(0.5f,0.99f);
		}

		timer = timer - Time.deltaTime/Mathf.CeilToInt(elapse/10f);
		text.text = timer.ToString("00.00");
	}
}
