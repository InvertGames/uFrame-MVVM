using UnityEngine;
using System.Collections;
using NodeCanvas;
using NodeCanvas.DialogueTrees;

//Sample UI for subtitles.
//1. You must subscribe to the event OnActorSpeaking
//2. When the event is dispatched a method with the same name as the event is called with a DialogueSpeechInfo object
//3. Display the text, play audio if you want and when finish call DoneSpeaking() at the DialogueSpeechInfo
public class DialogueSubtitlesGUI : MonoBehaviour{

	public GUISkin skin;
	public bool showOverActor;

	private DialogueActor talkingActor;
	private string displayText;

	//Subscribe to the events
	void Awake(){
		EventHandler.Subscribe(this, DLGEvents.OnActorSpeaking);
		EventHandler.Subscribe(this, DLGEvents.OnDialogueStarted);
		EventHandler.Subscribe(this, DLGEvents.OnDialogueFinished);
		EventHandler.Subscribe(this, DLGEvents.OnDialoguePaused);
		enabled = false;
	}

	//Function with same name as the event is called when the event is dispatched by the Dialogue Tree
	void OnActorSpeaking(DialogueSpeechInfo speech){
		
		enabled = true;
		talkingActor = speech.actor;
		StartCoroutine(talkingActor.ProcessStatement(speech.statement, speech.DoneSpeaking));
	}

	void OnDialogueStarted(){
		//We could do something here...
	}

	void OnDialoguePaused(){
		OnDialogueFinished();
	}

	void OnDialogueFinished(){
		StopAllCoroutines();
		displayText = null;
		enabled = false;
		if (talkingActor)
			talkingActor.speech = null;
	}

	void OnGUI(){

		GUI.skin = skin;

		if (talkingActor)
			displayText = talkingActor.speech;
		
		if (string.IsNullOrEmpty(displayText) || !Camera.main){
			enabled = false;
			return;
		}

		//calculate the size needed
		Vector2 finalSize= new GUIStyle("box").CalcSize(new GUIContent(displayText));
		Rect speechRect= new Rect(0,0,0,0);
		speechRect.width = finalSize.x;
		speechRect.height = finalSize.y;

		Vector3 talkPos= Camera.main.WorldToScreenPoint(talkingActor.dialoguePosition);
		talkPos.y = Screen.height - talkPos.y;

		//if show over actor and the actor's dialoguePosition is in screen, show the tet above the actor at that dialoguePosition
		if (showOverActor && Camera.main.rect.Contains( new Vector2(talkPos.x/Screen.width, talkPos.y/Screen.height) )){

			Vector2 newCenter = speechRect.center;
			newCenter.x = talkPos.x;
			newCenter.y = talkPos.y - speechRect.height/2;
			speechRect.center = newCenter;

		//else just show the subtitles at the bottom along with his portrait if any
		} else {

			speechRect = new Rect(10, Screen.height - 60, Screen.width - 20, 50);
			Rect nameRect = new Rect(0, 0, 200, 28);
			Vector2 newCenter = nameRect.center;
			newCenter.x = speechRect.center.x;
			newCenter.y = speechRect.y - 24;
			nameRect.center = newCenter;
			GUI.Box(nameRect, talkingActor.actorName);

			if (talkingActor.portrait){
				Rect portraitRect= new Rect(10, Screen.height - talkingActor.portrait.height - 70, talkingActor.portrait.width, talkingActor.portrait.height);
				GUI.DrawTexture(portraitRect, talkingActor.portrait);
			}
		}

		GUI.Box(speechRect, displayText);
	}
}
