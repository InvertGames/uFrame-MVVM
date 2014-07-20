using System;
using System.Collections.Generic;
using UnityEngine;

//[UBCategory("Convert")]
//public class ToStringByString : UBAction {

//    public UBString _Value = new UBString();
//    [UBRequireVariable] public UBString _Result = new UBString();
//    protected override void PerformExecute(IUBContext context){
//        if (_Result != null){
//            _Result.SetValue( context, Convert.ToString(_Value.GetValue(context)));
//        }

//    }

//    public override void WriteCode(IUBCSharpGenerator sb){
//    sb.AppendExpression("Convert.ToString(#_Value#)");
//    }

//}