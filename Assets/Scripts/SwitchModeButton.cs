using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SwitchModeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	public Sprite timerIcon;
	public Sprite fingerIcon;

	public Image Icon;

	Image button;

	Color startColor;

	public TimerScript timer;

	public WordGenerator parent;

	void Start () {
		button = GetComponent<Image>();
		startColor = button.color;
	}

	public void OnPointerDown(PointerEventData ped){
		button.color = new Color(button.color.r+0.1f, button.color.g+0.1f, button.color.b+0.1f);
	}

	public void OnPointerUp(PointerEventData ped){
		button.color = startColor;

		if(parent.isInTimedMode){
			SwitchBackToTouch();
			Utils.LogEvent("WentBackToTouchControl");
			Icon.sprite = fingerIcon;
			parent.isInTimedMode = false;
		}else if(!parent.isInTimedMode){
			OpenMenu();
			Utils.LogEvent("OpenedTimerMenu");
			Icon.sprite = timerIcon;
		}
	}

	void OpenMenu(){
		parent.timerSetScreen.SetActive(true);

		parent.isInAMenu = true;

	}

	void SwitchBackToTouch(){
		timer.StopTimer();		
		parent.GetFinishTimer();


		parent.isInAMenu = false;

	}
}