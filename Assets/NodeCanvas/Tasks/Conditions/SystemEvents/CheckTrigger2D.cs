using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("System Events")]
	[Name("Check Trigger 2D")]
	[EventListener("OnTriggerEnter2D", "OnTriggerExit2D", "OnTriggerStay2D")]
	[AgentType(typeof(Collider2D))]
	public class CheckTrigger2D : ConditionTask{

		public enum CheckTypes
		{
			TriggerEnter = 0,
			TriggerExit  = 1,
			TriggerStay  = 2
		}

		public CheckTypes CheckType = CheckTypes.TriggerEnter;
		public bool specifiedTagOnly;
		[TagField]
		public string objectTag = "Untagged";
		public BBGameObject saveGameObjectAs = new BBGameObject{blackboardOnly = true};

		private bool stay;

		protected override string info{
			get {return CheckType.ToString() + ( specifiedTagOnly? (" '" + objectTag + "' tag") : "" );}
		}

		protected override bool OnCheck(){
			if (CheckType == CheckTypes.TriggerStay)
				return stay;
			return false;
		}

		void OnTriggerEnter2D(Collider2D other){
			
			if (!specifiedTagOnly || other.gameObject.tag == objectTag){
				stay = true;
				if (CheckType == CheckTypes.TriggerEnter || CheckType == CheckTypes.TriggerStay){
					saveGameObjectAs.value = other.gameObject;
					YieldReturn(true);
				}
			}
		}

		void OnTriggerExit2D(Collider2D other){
			
			if (!specifiedTagOnly || other.gameObject.tag == objectTag){
				stay = false;
				if (CheckType == CheckTypes.TriggerExit){
					saveGameObjectAs.value = other.gameObject;				
					YieldReturn(true);
				}
			}
		}
	}
}