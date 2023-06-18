using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameButton : WordCategoryButton {

	public TextAsset adjective;
	public TextAsset name;

	List<string> adjUnused = new List<string>();
	List<string> adjUsed = new List<string>();

	List<string> nameUnused = new List<string>();
	List<string> nameUsed = new List<string>();
	
	public override void Update () {
		base.Update();
	}

	public override void getNewWord(){
		string adj1 = adjUnused[Random.Range(0,adjUnused.Count)];
		string adj2 = adjUnused[Random.Range(0,adjUnused.Count)];

		string name = nameUnused[Random.Range(0,nameUnused.Count)];

		string newWord = adj1 + ", " + adj2 + " " + name;
		newWord = newWord.Substring(0,1).ToUpper() + newWord.Substring(1);

		adjUsed.Add(adj1);
		adjUnused.Remove(adj1);
		adjUsed.Add(adj2);
		adjUnused.Remove(adj2);

		nameUsed.Add(name);
		nameUnused.Remove(name);

		if(adjUnused.Count<=0){
			foreach(string s in adjUsed){
				adjUnused.Add(s);
			}
			adjUsed.Clear();
		}

		if(nameUnused.Count<=0){
			foreach(string s in nameUsed){
				nameUnused.Add(s);
			}
			nameUsed.Clear();
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

		myID = "Name";

		string[] wordsTemp = adjective.text.Split (lineSeperater);
		foreach(string s in wordsTemp){
			adjUnused.Add(s);
		}
		wordsTemp = name.text.Split(lineSeperater);
		foreach(string s in wordsTemp){
			nameUnused.Add(s);
		}
	}
}