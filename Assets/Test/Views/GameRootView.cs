using System;
using System.Collections;
using System.Linq;
using UnityEngine;
public partial class GameRootView { 
    public override void TestCollectionAdded(string item) {
        base.TestCollectionAdded(item);
    }

    [UFGroup("TestVMProperty")]
    [UnityEngine.HideInInspector()]
    public UnityEngine.GameObject _TestVMPropertyPrefab;
    
    public override void TestVMPropertyChanged(GameSubElementViewModel value) {
        base.TestVMPropertyChanged(value);
        if (value == null && _TestVMProperty != null && _TestVMProperty.gameObject != null) {
            Destroy(_TestVMProperty.gameObject);
        }
        if (_TestVMPropertyPrefab == null ) {
            this._TestVMProperty = ((GameSubElementViewBase)(this.InstantiateView(value)));
        }
        else {
            this._TestVMProperty = ((GameSubElementViewBase)(this.InstantiateView(this._TestVMPropertyPrefab, value)));
        }
    }

}
