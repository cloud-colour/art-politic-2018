using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DebugCanvas : MonoBehaviour {

	[SerializeField] Slider slider;
	[SerializeField] Animator anim;
	[SerializeField] Button btn;
	public TextMeshPro restartText;

	bool show;
	// Use this for initialization

	public static DebugCanvas Inst;

	void Awake()
	{
		Inst = this;
	}

	void Start ()
	{
		slider.value = GameConfiguration.GetInstance().mouseSensitivity;
	}

	public void SetSensitivity()
	{
		GameConfiguration.GetInstance().mouseSensitivity = slider.value;
	}

	public void Restart()
	{
		GameConfiguration.GetInstance().mouseSensitivity = 1;
		GameSceneManager.GetInstance().ReturnToWelcome();
	}

	public void TogglePanel()
	{
		show = !show;
		EventSystem.current.SetSelectedGameObject(null);
		anim.SetBool("Show",show);
	}

	public void SetRestartText(float remaining)
	{
		if(remaining > 10)
		{
			restartText.SetText("");
		}
		else
		{
			restartText.SetText("Restart in "+remaining.ToString("0"));
		}
	}
}
