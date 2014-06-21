using UnityEngine;
using System.Collections;
using NodeCanvas.Variables;

namespace NodeCanvas.Conditions{

	[Category("System Events")]
	[AgentType(typeof(Collider))]
	[EventListener("OnCollisionEnter", "OnCollisionExit")]
	public class CheckCollision : ConditionTask {

		public enum CheckTypes
		{
			CollisionEnter = 0,
			CollisionExit  = 1,
			CollisionStay  = 2
		}

		public CheckTypes checkType = CheckTypes.CollisionEnter;
		public bool specifiedTagOnly;
		[TagField]
		public string objectTag = "Untagged";
		public BBGameObject saveGameObjectAs = new BBGameObject{blackboardOnly = true};

		private bool stay;

		protected override string info{
			get {return checkType.ToString() + ( specifiedTagOnly? (" '" + objectTag + "' tag") : "" );}
		}

		protected override bool OnCheck(){
			if (checkType == CheckTypes.CollisionStay)
				return stay;
			return false;
		}

		void OnCollisionEnter(Collision info){
			
			if (!specifiedTagOnly || info.gameObject.tag == objectTag){
				stay = true;
				if (checkType == CheckTypes.CollisionEnter || checkType == CheckTypes.CollisionStay){
					saveGameObjectAs.value = info.gameObject;
					YieldReturn(true);
				}
			}
		}

		void OnCollisionExit(Collision info){
			
			if (!specifiedTagOnly || info.gameObject.tag == objectTag){
				stay = false;
				if (checkType == CheckTypes.CollisionExit){
					saveGameObjectAs.value = info.gameObject;
					YieldReturn(true);
				}
			}
		}
	}
}