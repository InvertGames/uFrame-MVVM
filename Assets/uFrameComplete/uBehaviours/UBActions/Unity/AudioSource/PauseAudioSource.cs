using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Pauses playing the .")]
[UBCategory("AudioSource")]
public class PauseAudioSource : UBAction {

	[UBRequired] public UBObject _AudioSource = new UBObject(typeof(AudioSource));
	protected override void PerformExecute(IUBContext context){
		_AudioSource.GetValueAs<AudioSource>(context).Pause();
	}

	public override string ToString(){
	return string.Format("Call {0}'s Pause w/ ", _AudioSource.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_AudioSource#.Pause()");
	}

}