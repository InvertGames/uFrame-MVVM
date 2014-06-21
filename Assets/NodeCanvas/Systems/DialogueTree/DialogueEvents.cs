using UnityEngine;
using System;
using System.Collections.Generic;

namespace NodeCanvas.DialogueTrees{

	///The various events that are send through the EventHandler and from the Dialogue Tree
	public enum DLGEvents{
		
		OnActorSpeaking,
		OnDialogueOptions,
		OnDialogueStarted,
		OnDialoguePaused,
		OnDialogueFinished
	}

	///Send along with a OnDialogueOptions event. Holds information of the options, time available as well as a callback to be called providing the selected option
	public class DialogueOptionsInfo{

		public Dictionary<Statement, int> finalOptions = new Dictionary<Statement, int>();
		public float availableTime = 0;

		public Action<int> SelectOption;

		public DialogueOptionsInfo(Dictionary<Statement, int> finalOptions, float availableTime, Action<int> callback){
			this.finalOptions = finalOptions;
			this.availableTime = availableTime;
			this.SelectOption = callback;
		}
	}

	///Send along with a OnActorSpeaking event. Holds info about the actor speaking, the statement that being said as well as a callback to be called when dialogue is done showing
	public class DialogueSpeechInfo{

		public DialogueActor actor;
		public Statement statement;
		
		public Action DoneSpeaking;

		public DialogueSpeechInfo(DialogueActor actor, Statement statement, Action callback){
			this.actor = actor;
			this.statement = statement;
			this.DoneSpeaking = callback;
		}
	}
}