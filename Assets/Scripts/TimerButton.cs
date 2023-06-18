using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimerButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public bool minus;

	public int amount;

	public TimerMenu parent;

	public Text numlabel;
	public Image panel;

	Color startColor;

	void Start () {
		startColor = panel.color;
		Setup();
	}

	void Setup(){
		if(minus){
			numlabel.text = "- " + amount.ToString();
		}else{
			numlabel.text = "+ " + amount.ToString();
		}
	}

	public void OnPointerDown(PointerEventData ped){
		panel.color = new Color(panel.color.r+0.1f, panel.color.g+0.1f, panel.color.b+0.1f);

	}

	public void OnPointerUp(PointerEventData ped){
		panel.color = startColor;

		parent.GetTime(amount,minus);
	}
}