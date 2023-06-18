using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordGenerator : MonoBehaviour {

	public LockedInButton lockedinbutton;

	public GameObject mainButtons;

	public Text currentWord;
	public Text currentCat;

	public bool categoryLocked;

	[HideInInspector]
	public string currentCategory;

	[HideInInspector]
	public bool canPush;

	[HideInInspector]
	public bool isInTimedMode;

	public List<WordCategoryButton> buttons = new List<WordCategoryButton>();

	public WordCategoryButton currentSelectedButton;

	public Image lockImage;

	public GameObject buttonSelectPanel;

	public ButtonListPopulate buttonListPop;

	public GameObject lockInAnim;
	public GameObject UnlockAnim;

	public GameObject timerSetScreen;

	public bool isInAMenu;

	void Awake () {
#if UNITY_ANDROID
        Screen.fullScreen = false;
#endif

		Amplitude amplitude = Amplitude.Instance;
		amplitude.logging = true;
		amplitude.init("a9d5930b92947b5171639869f1fcb2ec");
		Utils.LogEvent("OpenedApp");

        // Enable line below to enable logging if you are having issues setting up OneSignal. (logLevel, visualLogLevel)
        // OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.INFO, OneSignal.LOG_LEVEL.INFO);
        OneSignal.StartInit("c580bbdf-9748-4d91-bd20-eee5d8b3a50f")
            .HandleNotificationOpened(HandleNotificationOpened)
            .EndInit();
          
        OneSignal.inFocusDisplayType = OneSignal.OSInFocusDisplayOption.Notification;
    }

    // Gets called when the player opens the notification.
    private static void HandleNotificationOpened(OSNotificationOpenedResult result) {
    }

	void Start () {
		canPush = true;
		isInAMenu = false;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.Escape) && !isInAMenu){
			Application.Quit();
		}
	}

	public void ChangeCurrentButton(WordCategoryButton newBut){
		if(currentSelectedButton!=null){
			currentSelectedButton.unhighlight();
		}

		currentCategory = newBut.category;
		currentSelectedButton = newBut;

		currentSelectedButton.highlight();

		currentCat.text = currentCategory.ToString();
	}

	public void LockInCategory(WordCategoryButton incButton){
		lockInAnim.SetActive(true);

		categoryLocked = true;

		lockImage.fillAmount = 1f;

		lockedinbutton.gameObject.SetActive(true);
		lockedinbutton.GetNewInfo(incButton);
		mainButtons.SetActive(false);

		ChangeCurrentButton(incButton);

		NewWord();
	}

	public void UnlockCategory(){
		categoryLocked = false;

		canPush = true;

		lockImage.fillAmount = 0f;

		UnlockAnim.SetActive(true);

		lockedinbutton.gameObject.SetActive(false);
		mainButtons.SetActive(true);
	}

	public void NewWord(){
		if(isInTimedMode){
			return;
		}

		if(currentWord == null){
			Debug.Log("Current word is null");
			return;
		}
		canPush = false;
		currentSelectedButton.getNewWord();
	}

	public void RandomFromOnesLockedIn(){
		WordCategoryButton newBut = buttons[Random.Range(0,buttons.Count)].GetComponent<WordCategoryButton>();
		ChangeCurrentButton(newBut);
		NewWord();
	}

	public void RecieveNewWord(string incWord){
		string newWord = incWord;

		currentWord.text = newWord;

		currentWord.gameObject.GetComponent<Animator>().SetTrigger("new");

		if(!isInTimedMode){
			canPush = true;
		}
	}

	public void GetFinishTimer(){
		isInTimedMode = false;
		canPush = true;
		NewWord();
	}

	public void GetStartTimer(){
		isInTimedMode = true;
		canPush = false;
	}

	public void GetOptionPush(bool wasCloseButton){
		if(wasCloseButton){
			buttonSelectPanel.GetComponent<Animator>().SetTrigger("end");
			buttonListPop.PopulateList();

			buttonListPop.LogCurrentCategories();

			isInAMenu = false;

		}else if(!wasCloseButton){
			buttonSelectPanel.SetActive(true);
			buttonListPop.ClearButtons();

			Utils.LogEvent("OpenedMenu");

			isInAMenu = true;

		}
	}
}