//using System;
//using System.Collections.Generic;
//using UnityEngine;

//[UBHelp(@"Moves a transform in a direction at the specified speed.")]
//[UBCategory("Movement")]
//public class MoveOverTime : UBAction {

//    [UBHelp(@"The tranform to move")]
//    [UBRequired] public UBTransform _Transform = new UBTransform();
//    [UBHelp(@"The speed to move the transform")]
//    [UBRequired] public UBFloat _Speed = new UBFloat();
//    [UBHelp(@"The direction to move the transform in.")]
//    [UBRequired] public UBVector3 _Direction = new UBVector3();
//    protected override void PerformExecute(IUBContext context){
//        Movement.MoveOverTime(_Transform.GetValue(context),_Speed.GetValue(context),_Direction.GetValue(context));
//    }

//    public override void WriteCode(IUBCSharpGenerator sb){
//        sb.AppendExpression("Movement.MoveOverTime(#_Transform#, #_Speed#, #_Direction#)");
//    }

//}