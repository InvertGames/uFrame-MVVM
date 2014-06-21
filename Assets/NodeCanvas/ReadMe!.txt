-------------------------------------------------------------
CHANGE LOG
-------------------------------------------------------------
You can find the complete change log online at:
http://nodecanvas.com/change-log/


-------------------------------------------------------------
EXAMPLE SCENES
-------------------------------------------------------------
The examples demo scenes have been removed from the asset itself as to make the asset smaller and keep the essential stuff in it.
You can though find and download example demo scenes online at:
http://www.nodecanvas.com


-------------------------------------------------------------
IMPORTANT 1.5.0 UPDATE NOTICE
-------------------------------------------------------------
In version 1.5.0 there was some major refactoring in class names and namespaces that had to be done sooner rather than later.
The first thing to do is to remove your old NodeCanvas folder complete if you not already have and re-import anew as you should every time updating.

-------
Regarding namespaces the changes are:

BehaviourTree = BehaviourTrees
FSM           = StateMachines
DialogueTree  = DialogueTrees

-------
Regarding class names, the important ones that you should be aware of are:

NodeGraphContainer    = Graph
NodeBase              = Node
BTContainer           = BehaviourTree
FSMContainer          = FSM
DialogueTreeContainer = DialogueTree

-------
Regarding Methods and Properties there are but two for Tasks:

OnActionEditGUI    = OnTaskInspectorGUI
OnConditionEditGUI = OnTaskInspectorGUI

actionInfo    = info
conditionInfo = info

-------
Here are 2 things that unfortunately will be lost in the update:

-The ScriptControl Tasks like ExecuteFunction, GetProperty etc had to be refactored for the better.
As such the settings for them will now be lost and you will have to re-set those type of Tasks if you use any of those :(

-The Graph names will be reseted :/

Sorry for any inconvenience these changes might have caused.


-------------------------------------------------------------
Thanks for using NodeCanvas!
-------------------------------------------------------------