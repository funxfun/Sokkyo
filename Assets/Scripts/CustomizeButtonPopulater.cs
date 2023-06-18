using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomizeButtonPopulater : MonoBehaviour {

	public GameObject emptyPanel;

	public GameObject emptyButton;

	public ButtonListPopulate buttonListParent;

	List<WordCategory> buttonsToAdd = new List<WordCategory>();

	public int buttonsAddedToMainApp;

	public int rowsPerColumn;

	public List<GameObject> panels = new List<GameObject>();

	public GameObject myPanel;

	public Text header;

	public WordGenerator parent;

	void Start () {
		PopulateList();
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape)){
			parent.GetOptionPush(true);
		}
	}

	public void UpdateHeader(){
		header.text = "Selected: " + buttonsAddedToMainApp + "/10";
	}

	void PopulateList(){
		foreach(WordCategory wc in buttonListParent.Categories){
			buttonsToAdd.Add(wc);
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

			GameObject newButton = Instantiate(emptyButton,Vector2.zero,Quaternion.identity);;
			newButton.transform.SetParent(panels[currentPanelNum].transform);

			CustomizeButton newButtonScript = newButton.GetComponent<CustomizeButton>();
			newButtonScript.GetSetUp(buttonListParent.Categories[buttonIndex],buttonIndex);

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

		UpdateHeader();

		buttonsToAdd.Clear();
	}

	void Finish(){
		//Debug.Log("Well Done");
	}
}