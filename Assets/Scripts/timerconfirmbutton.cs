using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class timerconfirmbutton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public TimerMenu parent;

	public bool isCancel;

	public Image panel;
	Color startColor;

	void Start () {
		startColor = panel.color;

	}
	
	void Update () {
		
	}
	public void OnPointerDown(PointerEventData ped){
		panel.color = new Color(panel.color.r+0.1f, panel.color.g+0.1f, panel.color.b+0.1f);

	}

	public void OnPointerUp(PointerEventData ped){
		panel.color = startColor;

		parent.CloseMenu(isCancel);
	}
}