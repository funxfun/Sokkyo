using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerMenu : MonoBehaviour {

	public WordGenerator parent;
	public TimerScript timer;

	public SwitchModeButton switchBut;

	public int currentTime;

	public Text displaytime;

	void OnEnable () {
		displaytime.text = currentTime.ToString() + "s";
	}
	
	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			CloseMenu(true);
		}
	}

	public void GetTime(int amount, bool minus){
		if(currentTime > 0 && minus){
			currentTime -= amount;
		}else if(currentTime < 130 && !minus){
			currentTime += amount;
		}
		displaytime.text = currentTime.ToString() + "s";
	}

	public void CloseMenu(bool canceled){
		if(!canceled){
			timer.StartTimer(currentTime);
			Utils.LogEvent("StartedTimerAt_" + currentTime);
			parent.GetStartTimer();
		}else if(canceled){
			switchBut.Icon.sprite = switchBut.fingerIcon;
			Utils.LogEvent("CanceledOutOfTimerMenu");
		}


		parent.isInAMenu = false;

		GetComponent<Animator>().SetTrigger("end");
	}
}