using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;


public partial class NewView {
    
    /// Subscribes to the property and is notified anytime the value changes.
    public virtual void SpeedChanged(Single value) {
    }
    
    /// Subscribes to the property and is notified anytime the value changes.
    public virtual void DistanceToPlayerChanged(Single value) {
    }
}
