//using System;
//using System.Collections.Generic;
//using UnityEngine;

//[UBCategory("UBGUI")]
//public class RectAtWorldPosition : UBAction {

//    public UBVector3 _Position = new UBVector3();
//    public UBInt _Width = new UBInt();
//    public UBInt _Height = new UBInt();
//    public UBVector3 _Offset = new UBVector3();
//    [UBRequireVariable] public UBRect _Result = new UBRect();
//    protected override void PerformExecute(IUBContext context){
//        if (_Result != null){
//            _Result.SetValue( context, UBGUI.RectAtWorldPosition(_Position.GetValue(context),_Width.GetValue(context),_Height.GetValue(context),_Offset.GetValue(context)));
//        }

//    }

//    public override void WriteCode(IUBCSharpGenerator sb){
//        sb.AppendExpression("UBGUI.RectAtWorldPosition(#_Position#, #_Width#, #_Height#, #_Offset#)");
//    }

//}