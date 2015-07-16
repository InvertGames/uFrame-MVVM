namespace Invert.uFrame.MVVM {
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    
    
    public class StateMachinePage : StateMachinePageBase {
        public override string Name
        {
            get { return "State Machines"; }
        }

        public override void GetContent(Invert.Core.GraphDesigner.IDocumentationBuilder _) {
            base.GetContent(_);
            _.Paragraph("uFrame utilizes a new concept developed to fit the uFrame and RX paradigm of programming.  " +
                        "We like to call this \"Re-active\" state machines. ");
            _.Paragraph("The main concept of state machines in uFrame MVVM is that when defining them in your " +
                        "diagrams you don't need to focus on anything other than the states and transitions that " +
                        "make up the machine.  Making the machine come to life is a matter of wiring them to " +
                        "computed properties, command bindings, and view bindings.  But initially, you can focus " +
                        "purely on the high-level rather than implementation, once commands and transitions " +
                        "are all wired together transitions and states can be easily tweaked with minimal to " +
                        "zero changes in your code.");
            _.Paragraph("Reactive State Machines employ the concept of data subscriptions causing transitions, rather than polling data every frame, this can give a definite increase in performance.");
            _.Break();
            _.ImageByUrl("http://i.imgur.com/CPymbPH.png");
            _.Break();
            _.Title2("Manually triggering transitions");
            _.Paragraph("To set a transition in code, you'll need to access the state machine property.  This should be the property that is connected to your state machine.");
            _.CodeSnippet("{ViewModel}.{StateMachinePropertyName}Property.Transition(\"NAME OF TRANSITION HERE\");");
            _.Break();
            _.Title2("Manually setting the state");
            _.CodeSnippet("{ViewModel}.{StateMachinePropertyName}Property.SetState<STATE_CLASS_NAME>();");
            _.Title3("or");
            _.CodeSnippet("{ViewModel}.{StateMachinePropertyName}Property.SetState(\"STATE_NAME\");");
            _.Break();



//            _.AlsoSeePages(typeof(UsingStateMachines));
        }
    }
}
