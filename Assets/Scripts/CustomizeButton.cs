using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomizeButton : MonoBehaviour, IPointerUpHandler, IPointerDownHandler {

	public Image button;
	public Image Icon;
	public Text label;
	public string category;

	int indexNum;

	public CustomizeButtonPopulater myParent;
	public ButtonListPopulate populateParent;

	Color startColor;

	bool lockedIn;

	public string myID;

	void Start () {
		
	}

	public void GetSetUp(WordCategory newBut, int indexNumber){
		startColor = Icon.color;

		gameObject.name = newBut.tagID;

		myParent = GetComponentInParent<CustomizeButtonPopulater>();
		populateParent = myParent.buttonListParent;
		startColor = button.color;

		Icon.sprite = newBut.icon;
		category = newBut.tagID;
		label.text = newBut.tagID;
		gameObject.name = category + " Button";

		indexNum = indexNumber;

		myID = newBut.tagID;

		if(myParent.buttonListParent.Categories[indexNum].added){
			TurnOnIfSelected();
		}else{
			button.color = Color.grey;
			populateParent.Categories[indexNum].added = false;
			lockedIn = false;
		}
	}

	public void OnPointerDown(PointerEventData ped){
		button.color = new Color(button.color.r+0.1f, button.color.g+0.1f, button.color.b+0.1f);
	}

	public void OnPointerUp(PointerEventData ped){
		if(lockedIn){
			if(myParent.buttonsAddedToMainApp>1){
				Unlock();
			}else{

				Utils.LogEvent("TriedToRemoveLastCategoryButton");

				button.color = startColor;
			}
		}else if(!lockedIn){
			if(myParent.buttonsAddedToMainApp<10){
				LockIn();
			}else if(myParent.buttonsAddedToMainApp>=10){
				//Debug.Log("already have 10 buttons locked in");

				Utils.LogEvent("TriedToHighlightMoreThan10Categories");


				button.color = Color.grey;
			}
		}
	}

	void TurnOnIfSelected(){
		button.color = startColor;
		populateParent.Categories[indexNum].added = true;
		myParent.buttonsAddedToMainApp++;
		lockedIn = true;
		myParent.UpdateHeader();
	}

	void LockIn(){
		button.color = startColor;
		populateParent.Categories[indexNum].added = true;
		myParent.buttonsAddedToMainApp++;
		lockedIn = true;
		myParent.UpdateHeader();
		populateParent.SaveList();

		Utils.LogEvent("AddCategory_" + myID);
	}

	void Unlock(){
		button.color = Color.grey;
		populateParent.Categories[indexNum].added = false;
		if(myParent.buttonsAddedToMainApp>0){
			myParent.buttonsAddedToMainApp--;
		}
		myParent.UpdateHeader();
		lockedIn = false;
		populateParent.SaveList();

		Utils.LogEvent("RemoveCategory_" + myID);
	}
}