using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("System Events")]
	[EventListener("OnTriggerEnter", "OnTriggerExit")]
	[AgentType(typeof(Collider))]
	public class CheckTrigger : ConditionTask{

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

		void OnTriggerEnter(Collider other){
			
			if (!specifiedTagOnly || other.gameObject.tag == objectTag){
				stay = true;
				if (CheckType == CheckTypes.TriggerEnter || CheckType == CheckTypes.TriggerStay){
					saveGameObjectAs.value = other.gameObject;
					YieldReturn(true);
				}
			}
		}

		void OnTriggerExit(Collider other){
			
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