using System;
using System.Collections.Generic;
using UnityEngine;
using UBehaviours.Actions;

[UBHelp(@"Plays an AudioSource's sound.")]
[UBCategory("AudioSource")]
public class PlayAudioSource : UBAction {

	[UBRequired] public UBObject _AudioSource = new UBObject(typeof(AudioSource));
	protected override void PerformExecute(IUBContext context){
		_AudioSource.GetValueAs<AudioSource>(context).Play();
	}

	public override string ToString(){
	return string.Format("Call {0}'s Play w/ ", _AudioSource.ToString(RootContainer));
	}

	public override void WriteCode(IUBCSharpGenerator sb){
		sb.AppendExpression("#_AudioSource#.Play()");
	}

}