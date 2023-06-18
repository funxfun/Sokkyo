using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class OptionButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
	Image button;

	Color startColor;

	public TimerScript timer;

	public WordGenerator parent;

	public bool isCloseButton;

	void Start () {
		button = GetComponent<Image>();
		startColor = button.color;
	}

	public void OnPointerDown(PointerEventData ped){
		button.color = new Color(button.color.r+0.1f, button.color.g+0.1f, button.color.b+0.1f);
	}

	public void OnPointerUp(PointerEventData ped){
		button.color = startColor;

		if(!isCloseButton){
			if(parent.isInTimedMode){
				timer.PauseTimer();
			}
		}else if(isCloseButton){
			if(parent.isInTimedMode){
				timer.UnpauseTimer();
			}
		}

		parent.GetOptionPush(isCloseButton);
	}
}