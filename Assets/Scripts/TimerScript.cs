using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour {

	float StartTime;

	float timeLeft;

	public Text display;

	//[HideInInspector]
	public bool counting;

	public GameObject parentObject;

	public Image circle;

	AudioSource aSource;

	public Image backing;

	void Start () {
		display.enabled = false;
		circle.enabled = false;

		backing.enabled = false;

		aSource = GetComponent<AudioSource>();
	}
	
	void Update () {
		if(counting && timeLeft>0f){
			timeLeft -= Time.deltaTime;
			display.text = Mathf.Round(timeLeft).ToString();

			float fillamount = (float)timeLeft/(float)StartTime;
			circle.fillAmount = fillamount;

		}else if(counting && timeLeft<=0){
			FinishTimer();
		}
	}

	void FinishTimer(){
		parentObject.SendMessage("GetFinishTimer");

		aSource.Play();

		Utils.LogEvent("FinishedTimer");

		StartTimer(StartTime);
	}

	public void PauseTimer(){
		if(timeLeft>0){
			counting = false;
		}
	}

	public void UnpauseTimer(){
		if(timeLeft>0){
			counting = true;
		}
	}

	public void StopTimer(){
		counting = false;
		display.enabled = false;
		circle.enabled = false;
		backing.enabled = false;
	}

	public void StartTimer(float incTime){
		parentObject.SendMessage("GetStartTimer");
		circle.fillAmount = 1f;
		StartTime = incTime;
		timeLeft = StartTime;
		counting = true;
		display.enabled = true;
		circle.enabled = true;
		backing.enabled = true;
	}
}