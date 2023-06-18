using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LockedInButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	public Image icon;
	public Text category;
	public Image lockImg;

	public Image button;

	Color startColor;

	WordGenerator parent;

	float count;
	bool counting;

	string myID;

	void Start () {
		parent = GetComponentInParent<WordGenerator>();
		startColor = button.color;
		count = -0.5f;
	}
	
	void Update () {
		if(counting && count<1f){
			count+=0.05f;
			lockImg.fillAmount = 1f - count;
		}
	}

	public void GetNewInfo(WordCategoryButton incButton){
		icon.sprite = incButton.Icon.sprite;
		category.text = incButton.category;
		myID = incButton.myID;
	}

	public void OnPointerDown(PointerEventData ped){
		if(parent.isInTimedMode){
			return;
		}

		button.color = new Color(button.color.r+0.1f, button.color.g+0.1f, button.color.b+0.1f);
		if(!counting){
			counting = true;
		}
	}

	public void OnPointerUp(PointerEventData ped){

		if(parent.isInTimedMode){
			return;
		}

		if(counting){
			if(count>=1f){
				GetLongPush();
			}else if(count<1f){
				GetStandardPush();
			}
			counting = false;
		}

		button.color = startColor;
		count = -0.5f;
	}

	public void GetStandardPush(){
		parent.NewWord();
		lockImg.fillAmount = 1f;

		Utils.LogEvent("BigButtonPush_" + myID);

	}

	public void GetLongPush(){
		parent.NewWord();
		lockImg.fillAmount = 0f;
		parent.UnlockCategory();

		Utils.LogEvent("BigButtonUnlock_" + myID);

	}
}
