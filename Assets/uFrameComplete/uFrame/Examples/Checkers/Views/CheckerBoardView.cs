
using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class CheckerBoardView { 
    //public override void CheckersAdded(CheckerViewBase item) {
    //    base.CheckersAdded(item);
    //    this._CheckersList.Add(item);
    //}
    //public override void CheckersRemoved(CheckerViewBase item) {
    //    base.CheckersRemoved(item);
    //    Debug.Log("Checker Removed");
    //    this._CheckersList.Remove(item);
    //    if (item != null && item.gameObject != null) UnityEngine.Object.Destroy(item.gameObject);
    //}
    //public override void PlatesAdded(CheckerPlateViewBase item) {
    //    base.PlatesAdded(item);
    //    this._PlatesList.Add(item);
    //}
    //public override void PlatesRemoved(CheckerPlateViewBase item) {
    //    base.PlatesRemoved(item);
    //    this._PlatesList.Remove(item);
    //    if (item != null && item.gameObject != null) UnityEngine.Object.Destroy(item.gameObject);
    //}
    //public override ViewBase CreatePlatesView(CheckerPlateViewModel value) {
        
    //    return this.InstantiateView(value);
    //}
    //public override ViewBase CreateCheckersView(CheckerViewModel value) {
        
    //    return this.InstantiateView(value);
    //}

    
    public override void Bind() {
        base.Bind();
    }
}
