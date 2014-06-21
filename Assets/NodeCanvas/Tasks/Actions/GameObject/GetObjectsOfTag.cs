using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Variables;

namespace NodeCanvas.Actions{

	[Category("GameObject")]
	public class GetObjectsOfTag : ActionTask{

		[RequiredField] [TagField]
		public string searchTag = "Untagged";
		
		[RequiredField]
		public BBGameObjectList saveAs = new BBGameObjectList{blackboardOnly = true};

		protected override string info{
			get{return "GetObjects '" + searchTag + "' as " + saveAs;}
		}

		protected override void OnExecute(){

			saveAs.value = GameObject.FindGameObjectsWithTag(searchTag).ToList();
			EndAction(true);
		}
	}
}