using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Samples an animation at a given time for any animated properties.")]
[UBCategory("GameObject")]
public class SampleAnimation : UBAction {

	[UBRequired] public UBGameObject _GameObject = new UBGameObject();
	
	[UBRequired] public UBObject _Animation = new UBObject(typeof(AnimationClip));
	
	[UBRequired] public UBFloat _Time = new UBFloat();
	protected override void PerformExecute(IUBContext context){
		_GameObject.GetValue(context).SampleAnimation(_Animation.GetValueAs<AnimationClip>(context),_Time.GetValue(context));
	}

	public override string ToString(){
	return string.Format("Call {0}'s SampleAnimation w/ {1} and {2}", _GameObject.ToString(RootContainer), _Animation.ToString(RootContainer), _Time.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_GameObject#.SampleAnimation(#_Animation#, #_Time#)");
	}

}