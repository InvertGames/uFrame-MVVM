using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Invert.StateMachine;


public class EnemyStateAI : EnemyStateAIBase {
    
    public EnemyStateAI(ViewModel vm, string propertyName) : 
            base(vm, propertyName) {
    }
}
