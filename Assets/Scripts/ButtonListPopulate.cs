using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ButtonListPopulate : MonoBehaviour {
	#if UNITY_EDITOR
	[MenuItem ("Tools/Clear Player Prefs", false, 0)]
	public static void ClearPlayerPrefs() {
		PlayerPrefs.DeleteAll();
		Debug.Log ("Cleared all Player Prefs");
	}
	#endif

	public WordGenerator parent;

	public int rowsPerColumn;

	[Header("Prefabs")]
	public GameObject buttonPrefab;
	public GameObject characterButton;
	public GameObject speechButton;
	public GameObject jokeButton;
	public GameObject nameButton;
	public GameObject emptyPanel;

	public GameObject blankButton;

	public List<WordCategory> Categories = new List<WordCategory>();

	List<WordCategory> buttonsToAdd = new List<WordCategory>();

	List<GameObject> panels = new List<GameObject>();

	void Start () {
		ClearButtons();
		LoadList();
		PopulateList();
	}

	public void SaveList(){
		PlayerPrefs.SetInt("listsaved",1);

		foreach(WordCategory wc in Categories){
			PlayerPrefs.SetInt(wc.tagID,(wc.added ? 1 : 0));
		}
	}

	void LoadList(){
		if(PlayerPrefs.GetInt("listsaved", 0) == 0){
			return;
		}

		foreach(WordCategory wc in Categories){
			wc.added = PlayerPrefs.GetInt(wc.tagID) == 1;
		}
	}

	public void PopulateList(){
		foreach(WordCategory wc in Categories){
			if(wc.added){
				buttonsToAdd.Add(wc);
			}
		}

		float temp = (float)buttonsToAdd.Count/2f;
		if(temp<5){
			temp = Mathf.Ceil(temp);
			rowsPerColumn = (int)temp;
		}else{
			rowsPerColumn = 5;
		}

		float columnCount = (float)buttonsToAdd.Count/(float)rowsPerColumn;
		columnCount = Mathf.Ceil(columnCount);

		float xInterval = 1f/columnCount;
		float xMin = 0f;
		float xMax = xInterval; 

		GameObject newPanel;

		for (int i = 0; i < columnCount; i++) {
			newPanel = Instantiate(emptyPanel,Vector2.zero,Quaternion.identity);

			RectTransform newPanelRect = newPanel.GetComponent<RectTransform>();

			newPanel.transform.SetParent(transform,false);

			newPanelRect.localPosition = new Vector3(0,0,1);

			newPanelRect.anchorMin = new Vector2(xMin,0f);
			newPanelRect.anchorMax = new Vector2(xMax,1f);

			newPanelRect.localScale = Vector3.one;

			newPanelRect.offsetMax = new Vector2(0f,0f);
			newPanelRect.offsetMin = new Vector2(0f,0f);

			panels.Add(newPanel);

			newPanel.name = "Panel " + (i+1).ToString();

			xMin += xInterval;
			xMax += xInterval;
		}

		int currentPanelNum = 0;
		int currentRowNum = 0;
		Transform currentParentPanel = panels[currentPanelNum].transform;

		float yInterval = 1f/(float)rowsPerColumn;
		float yMin = 1f - yInterval;
		float yMax = 1f; 

		for (int buttonIndex = 0; buttonIndex < buttonsToAdd.Count; buttonIndex++) {

			GameObject objectToSpawn;

			if(buttonsToAdd[buttonIndex].tagID == "Scene"){
				objectToSpawn = characterButton;
			}else if(buttonsToAdd[buttonIndex].tagID == "Speech"){
				objectToSpawn = speechButton;
			}else if(buttonsToAdd[buttonIndex].tagID == "Joke"){
				objectToSpawn = jokeButton;
			}else if(buttonsToAdd[buttonIndex].tagID == "Name"){
				objectToSpawn = nameButton;
			}else{
				objectToSpawn = buttonPrefab;
			}

			GameObject newButton = Instantiate(objectToSpawn,Vector2.zero,Quaternion.identity);;
			newButton.transform.SetParent(panels[currentPanelNum].transform,false);

			WordCategoryButton newButtonScript = newButton.GetComponent<WordCategoryButton>();
			newButtonScript.GetSetUp(buttonsToAdd[buttonIndex]);
			parent.buttons.Add(newButtonScript);

			RectTransform newButtonRect = newButton.GetComponent<RectTransform>();

			newButtonRect.localPosition = new Vector3(0,0,1);

			newButtonRect.anchorMin = new Vector2(0f,yMin);
			newButtonRect.anchorMax = new Vector2(1f,yMax);

			newButtonRect.localScale = Vector3.one;

			newButtonRect.offsetMax = new Vector2(-2f,-2f);
			newButtonRect.offsetMin = new Vector2(2f,2f);

			yMin -= yInterval;
			yMax -= yInterval;

			if(currentRowNum == rowsPerColumn-1){
				if(currentPanelNum == panels.Count-1){
					Finish();
				}else{
					currentPanelNum++;
					currentRowNum = 0;

					yMin = 1f - yInterval;
					yMax = 1f; 

				}
			}else{
				currentRowNum++;
			}
		}

		if(parent.buttons.Count % 2 != 0 && parent.buttons.Count>1){

			GameObject lastPanel = Instantiate(blankButton,Vector2.zero,Quaternion.identity);
			lastPanel.transform.SetParent(panels[currentPanelNum].transform,false);

			RectTransform lastPanelRect = lastPanel.GetComponent<RectTransform>();

			lastPanelRect.localPosition = new Vector3(0,0,1);

			lastPanelRect.anchorMin = new Vector2(0f,yMin);
			lastPanelRect.anchorMax = new Vector2(1f,yMax);

			lastPanelRect.localScale = Vector3.one;

			lastPanelRect.offsetMax = new Vector2(-2f,-2f);
			lastPanelRect.offsetMin = new Vector2(2f,2f);

		}

		buttonsToAdd.Clear();

		parent.ChangeCurrentButton(parent.buttons[0]);
	}

	public void ClearButtons(){
		foreach(GameObject g in panels){
			Destroy(g);
		}
		parent.buttons.Clear();
		panels.Clear();
		buttonsToAdd.Clear();
	}

	public void LogCurrentCategories(){
		Dictionary<string, object> wcList = new Dictionary<string, object>();
		foreach(WordCategory wc in Categories){
			if(wc.added){
				wcList.Add(wc.tagID, "_");
			}
		}

		Utils.LogEvent("ClosedMenu");
		Utils.LogEvent("HighlightedCategories", wcList);

	}

	void Finish(){
	}
}