using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MidScreenTouch : MonoBehaviour, IPointerDownHandler {

	public WordGenerator parent;

	public void OnPointerDown(PointerEventData ped){
		if(!parent.isInTimedMode && parent.canPush){

			if(parent.categoryLocked){
				parent.NewWord();
				Utils.LogEvent("MidscreenPushLocked_" + parent.currentCategory);
			}else{
				parent.RandomFromOnesLockedIn();
				Utils.LogEvent("PushedMidScreenRandom_" + parent.currentCategory);
			}
		}
	}
}