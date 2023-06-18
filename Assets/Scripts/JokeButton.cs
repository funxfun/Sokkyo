using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JokeButton : WordCategoryButton {

	public TextAsset mainList;
	public TextAsset fodder;
	public TextAsset character;

	List<string> mainUnused = new List<string>();
	List<string> mainUsed = new List<string>();

	List<string> fodderUnused = new List<string>();
	List<string> fodderUsed = new List<string>();

	List<string> characterUnused = new List<string>();
	List<string> characterUsed = new List<string>();


	public override void Update () {
		base.Update();
	}

	public override void getNewWord(){
		string newString = mainUnused[Random.Range(0,mainUnused.Count)];

		if(newString == "Worlds worst"){
			string newCha = characterUnused[Random.Range(0,characterUnused.Count)];
			newString = "Worlds worst " + newCha;

			characterUsed.Add(newCha);
			characterUnused.Remove(newCha);
		}else if(newString == "Famous last words of a"){
			string newCha = characterUnused[Random.Range(0,characterUnused.Count)];
			newString = "Famous last words of a " + newCha;

			characterUsed.Add(newCha);
			characterUnused.Remove(newCha);
		}else if(newString == "The worst advice from a"){
			string newCha = characterUnused[Random.Range(0,characterUnused.Count)];
			newString = "The worst advice from a " + newCha;

			characterUsed.Add(newCha);
			characterUnused.Remove(newCha);
		}else if(newString =="Sex with me is like"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "Sex with me is like " + newFod.ToLower();

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else if(newString == "Things you can say about"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "Things you can say about " + newFod.ToLower() + " but not your partner";

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else if(newString =="Rejected product taglines for"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "Rejected product taglines for " + newFod.ToLower();

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else if(newString =="Rejected product names for"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "Rejected product names for " + newFod.ToLower();

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else if(newString =="The worst advice you've ever heard regarding"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "The worst advice you've ever heard regarding " + newFod.ToLower();

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else if(newString =="Unlikely confessions regarding"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "Unlikely confessions regarding " + newFod.ToLower();

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else if(newString == "Unlikely confessions of"){
			string newCha = characterUnused[Random.Range(0,characterUnused.Count)];
			newString = "Unlikely confessions of a " + newCha;

			characterUsed.Add(newCha);
			characterUnused.Remove(newCha);
		}else if(newString =="Unlikely infomercial for"){
			string newFod = fodderUnused[Random.Range(0,fodderUnused.Count)];
			newString = "Unlikely infomercial for " + newFod.ToLower();

			fodderUsed.Add(newFod);
			fodderUnused.Remove(newFod);
		}else{
			mainUsed.Add(newString);
			mainUnused.Remove(newString);
		}

		if(mainUnused.Count<=3){
			foreach(string s in mainUsed){
				mainUnused.Add(s);
			}
			mainUsed.Clear();
		}

		if(characterUnused.Count<=0){
			foreach(string s in characterUsed){
				characterUnused.Add(s);
			}
			characterUsed.Clear();
		}

		if(fodderUnused.Count<=0){
			foreach(string s in fodderUsed){
				fodderUnused.Add(s);
			}
			fodderUsed.Clear();
		}

		parent.RecieveNewWord(newString);
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

		myID = "OneLiner";

		string[] wordsTemp = mainList.text.Split (lineSeperater);
		foreach(string s in wordsTemp){
			mainUnused.Add(s);
		}
		wordsTemp = fodder.text.Split(lineSeperater);
		foreach(string s in wordsTemp){
			fodderUnused.Add(s);
		}
		wordsTemp = character.text.Split(lineSeperater);
		foreach(string s in wordsTemp){
			characterUnused.Add(s);
		}
	}
}