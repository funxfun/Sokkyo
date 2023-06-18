using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordCategoryButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

	[HideInInspector]
	public WordGenerator parent;

	public Image Icon;

	[HideInInspector]
	public Image lockImg;
	public Text label;

	public Image button;
	public Color startColor;
	public Color highlightcolor;

	TextAsset words;

	[HideInInspector]
	public string category;

	[HideInInspector]
	public float count;
	bool counting;

	[HideInInspector]
	public List<string> Unused = new List<string>();
	[HideInInspector]
	public List<string> Used = new List<string>();

	[HideInInspector]
	public char lineSeperater = '\n';

	[HideInInspector]
	public string myID;

	void Start () {
	}
	
	public virtual void Update () {
		if(counting && count<1f){
			count+=0.05f;
			lockImg.fillAmount = count;
		}
	}

	public virtual void GetSetUp(WordCategory incEntry){
		parent = GetComponentInParent<WordGenerator>();
		button = GetComponent<Image>();

		count = -0.5f;

		Icon.sprite = incEntry.icon;
		category = incEntry.tagID;
		words = incEntry.words;
		label.text = incEntry.tagID;
		lockImg = parent.lockImage;
		gameObject.name = category + " Button";
		myID = incEntry.tagID;

		if(words!=null){
			string[] wordsTemp = words.text.Split (lineSeperater);
			foreach(string s in wordsTemp){
				Unused.Add(s);
			}	
		}
	}

	public void unhighlight(){
		button.color = startColor;
	}

	public void highlight(){
		button.color = highlightcolor;
	}

	public virtual void getNewWord(){
		int idx = Random.Range(0,Unused.Count);
		string newWord = Unused[idx].ToLower();
		newWord = newWord.Substring(0,1).ToUpper() + newWord.Substring(1);

		Used.Add(Unused[idx]);
		Unused.Remove(Unused[idx]);

		if(Unused.Count<=0){
			foreach(string s in Used){
				Unused.Add(s);
			}
			Used.Clear();
		}

		parent.RecieveNewWord(newWord);
	}

	public void OnPointerDown(PointerEventData ped){
		button.color = new Color(button.color.r+0.1f, button.color.g+0.1f, button.color.b+0.1f);
		if(parent.isInTimedMode){
			return;
		}
		if(!counting){
			counting = true;
		}
	}

	public void OnPointerUp(PointerEventData ped){
		button.color = startColor;

		if(parent.isInTimedMode){
			parent.ChangeCurrentButton(this);
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

		count = -0.5f;
	}

	public void GetStandardPush(){
		parent.ChangeCurrentButton(this);
		parent.NewWord();
		lockImg.fillAmount = 0f;

		Utils.LogEvent("CategoryButtonPush_" + myID);
	}

	public void GetLongPush(){
		if(!parent.categoryLocked){
			parent.LockInCategory(this);
			lockImg.fillAmount = 1f;

			Utils.LogEvent("CategoryLockIn_" + myID);

		}else if (parent.categoryLocked && parent.currentCategory == category){
			parent.UnlockCategory();

			Utils.LogEvent("CategoryUnlock_" + myID);

		}
	}
}