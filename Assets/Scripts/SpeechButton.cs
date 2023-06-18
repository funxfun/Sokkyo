using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechButton : WordCategoryButton {

	public TextAsset character;
	public TextAsset topic;

	List<string> chaUnused = new List<string>();
	List<string> chaUsed = new List<string>();

	List<string> topUnused = new List<string>();
	List<string> topUsed = new List<string>();
	
	public override void Update () {
		base.Update();
	}

	public override void getNewWord(){
		string newCha = chaUnused[Random.Range(0,chaUnused.Count)];
		string newTop = topUnused[Random.Range(0,topUnused.Count)];

		string newWord = newCha + " " + newTop.ToLower();
		newWord = newWord.Substring(0,1).ToUpper() + newWord.Substring(1);

		chaUsed.Add(newCha);
		chaUnused.Remove(newCha);

		topUsed.Add(newTop);
		topUnused.Remove(newTop);

		if(chaUnused.Count<=0){
			foreach(string s in chaUsed){
				chaUnused.Add(s);
			}
			chaUsed.Clear();
		}

		if(topUnused.Count<=0){
			foreach(string s in topUsed){
				topUnused.Add(s);
			}
			topUsed.Clear();
		}

		parent.RecieveNewWord(newWord);
	}

	public override void GetSetUp(WordCategory incEntry){
		parent = GetComponentInParent<WordGenerator>();
		button = GetComponent<Image>();
		startColor = button.color;

		count = -0.5f;

		Icon.sprite = incEntry.icon;
		category = incEntry.tagID;
		label.text = incEntry.tagID;
		lockImg = parent.lockImage;
		gameObject.name = category + " Button";

		myID = "Speech";

		string[] wordsTemp = character.text.Split (lineSeperater);
		foreach(string s in wordsTemp){
			chaUnused.Add(s);
		}
		wordsTemp = topic.text.Split(lineSeperater);
		foreach(string s in wordsTemp){
			topUnused.Add(s);
		}
	}
}