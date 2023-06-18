using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : WordCategoryButton {

	public TextAsset adjective;
	public TextAsset character;
	public TextAsset quirk;

	public List<string> adjUnused = new List<string>();
	public List<string> adjUsed = new List<string>();

	public List<string> chaUnused = new List<string>();
	public List<string> chaUsed = new List<string>();

	public List<string> quiUnused = new List<string>();
	public List<string> quiUsed = new List<string>();

	public override void Update () {
		base.Update();
	}

	public override void getNewWord(){
		string newAdj = adjUnused[Random.Range(0,adjUnused.Count)];
		string newCha = chaUnused[Random.Range(0,chaUnused.Count)];
		string newQui = quiUnused[Random.Range(0,quiUnused.Count)];

		string newWord = newAdj + " " + newCha + " who " + newQui.ToLower() + ".";
		newWord = newWord.Substring(0,1).ToUpper() + newWord.Substring(1);

		adjUsed.Add(newAdj);
		adjUnused.Remove(newAdj);

		chaUsed.Add(newCha);
		chaUnused.Remove(newCha);

		quiUsed.Add(newQui);
		quiUnused.Remove(newQui);

		if(adjUnused.Count<=0){
			foreach(string s in adjUsed){
				adjUnused.Add(s);
			}
			adjUsed.Clear();
		}

		if(chaUnused.Count<=0){
			foreach(string s in chaUsed){
				chaUnused.Add(s);
			}
			chaUsed.Clear();
		}

		if(quiUnused.Count<=0){
			foreach(string s in quiUsed){
				quiUnused.Add(s);
			}
			quiUsed.Clear();
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

		myID = "Situation";

		string[] wordsTemp = adjective.text.Split (lineSeperater);
		foreach(string s in wordsTemp){
			adjUnused.Add(s);
		}
		wordsTemp = character.text.Split(lineSeperater);
		foreach(string s in wordsTemp){
			chaUnused.Add(s);
		}
		wordsTemp = quirk.text.Split(lineSeperater);
		foreach(string s in wordsTemp){
			quiUnused.Add(s);
		}
	}
}