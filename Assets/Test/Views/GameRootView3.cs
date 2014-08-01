using System;
using System.Collections;
using System.Linq;
using UnityEngine;


public partial class GameRootView3 {
    
    public override void TestPropertyChanged(string value) {
        base.TestPropertyChanged(value);
    }
    
    public override void TestVMPropertyChanged(GameSubElementViewModel value) {
        base.TestVMPropertyChanged(value);
    }
    
    public override void TestCollectionAdded(string item) {
        base.TestCollectionAdded(item);
    }
    
    public override void TestCollectionRemoved(string item) {
        base.TestCollectionRemoved(item);
    }
    
    public override void TestVMCollectionAdded(GameSubElementViewBase item) {
        base.TestVMCollectionAdded(item);
    }
    
    public override void TestVMCollectionRemoved(GameSubElementViewBase item) {
        base.TestVMCollectionRemoved(item);
    }
    
    public override ViewBase CreateTestVMCollectionView(GameSubElementViewModel value) {
        return base.CreateTestVMCollectionView(value);
        return null;
    }
    
    public override void Bind() {
    }
}
