using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UBehaviours.Actions;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public class DebugExecutionHandler3 : IUBExecutionHandler
{
    public List<BehaviourError> Errors
    {
        get { return _errors; }
        set { _errors = value; }
    }

    private DebugInfo _debugInfo = new DebugInfo();
    private Queue<ExeuctionStackItem> _executionStack = new Queue<ExeuctionStackItem>();

    private static List<BehaviourError> _errors = new List<BehaviourError>();

    public Queue<ExeuctionStackItem> ExecutionStack
    {
        get { return _executionStack; }
        set { _executionStack = value; }
    }

    public DebugInfo DebugInfo
    {
        get { return _debugInfo; }
        set { _debugInfo = value; }
    }

    public void Update()
    {
        if (EditorApplication.isPaused)
            return;

        var stack = ExecutionStack.ToArray();
        foreach (var item in stack)
        {
            //var stackItem = ExecutionStack.Peek();
            if (item.Action.Breakpoint || DebugInfo.IsStepping)
            {
                DebugInfo.CurrentBreakPoint = item.Action;
                DebugInfo.CurrentAction = item.Action;
                item.Action.IsCurrentlyActive = true;
                EditorApplication.isPaused = true;
                return;
            }
            ExecutionStack.Dequeue().Execute();
            //stackItem.Context.StackTrace.Pop();
            item.Action.IsCurrentlyActive = false;
        }

    }

    public void ExecuteSheet(IUBContext context, UBActionSheet sheet)
    {

        if (sheet.IsForward)
        {
            if (sheet.ForwardTo != null)
            {
                ExecuteSheet(context, sheet.ForwardTo.Sheet);
                return;
            }
        }

        Update();
        context.StackTrace.Push(sheet);
        foreach (var action in sheet.Actions)
        {
            ExecutionStack.Enqueue(new ExeuctionStackItem(context, action));
            Update();
        }
        context.StackTrace.Pop();
    }

    public void TriggerBegin(UBTrigger ubTrigger)
    {

    }

    public void TriggerEnd(UBTrigger ubTrigger)
    {

    }

    public void Reset()
    {
        ExecutionStack.Clear();
        DebugInfo = new DebugInfo();
        Errors.Clear();
    }
}
public class DebugExecutionHandler2 : DefaultExecutionHandler
{
    public List<BehaviourError> Errors
    {
        get { return _errors; }
        set { _errors = value; }
    }

    private DebugInfo _debugInfo = new DebugInfo();

    private static List<BehaviourError> _errors = new List<BehaviourError>();

    public DebugInfo DebugInfo
    {
        get { return _debugInfo; }
        set { _debugInfo = value; }
    }

    public void ExecutePerFrame(IUBContext context, UBActionSheet sheet)
    {
        if (sheet.ForwardTo != null)
        {
            if (sheet.ForwardTo.Sheet != null)
            {
                ExecutePerFrame(context, sheet.ForwardTo.Sheet);
                return;
            }
        }

        //var execStart = Time.realtimeSinceStartup;
        context.StackTrace.Push(sheet);

        foreach (var action in sheet.Actions)
        {
            if (!action.Enabled) continue;

            context.StackTrace.Push(action);
            if (action.Breakpoint || DebugInfo.IsStepping)
            {
                DebugInfo.Context = context;
                EditorApplication.isPaused = true;
                DebugInfo.CurrentAction = action;
            }


            ExecuteAction(context, action);
            context.StackTrace.Pop();
        }
        context.StackTrace.Pop();

    }

    public override void ExecuteSheet(IUBContext context, UBActionSheet sheet)
    {

        //try
        //{
        //Debug.Log(sheet.FullName);
        //base.ExecuteSheet(context,sheet);
        ExecutePerFrame(context, sheet);
        //}
        //catch (Exception ex)
        //{
        //    // Coroutine issues
        //}

    }

    protected override void ExecuteAction(IUBContext context, UBAction action)
    {

        context.StackTrace.Push(action);
        try
        {
            //Debug.Log("Executing: " + action.ToString());
            base.ExecuteAction(context, action);
        }
        catch (Exception ex)
        {
            var error = new BehaviourError()
            {
                Message = action.GetType().Name + ": " + ex.Message,
                SourceObject =  context as UnityEngine.Object,
                Source = action,
            };
            error.Remove = () => Errors.Remove(error);
            Errors.Add(error);
            EditorWindow.GetWindow<UBExplorerWindow>().Repaint();
#if DEBUG
            Debug.LogError(ex);
#endif
        }
        context.StackTrace.Pop();
    }
}

public class DebugExecutionHandler : DefaultExecutionHandler
{
    public List<BehaviourError> Errors
    {
        get { return _errors; }
        set { _errors = value; }
    }

    private DebugInfo _debugInfo = new DebugInfo();

    private static List<BehaviourError> _errors = new List<BehaviourError>();

    public DebugInfo DebugInfo
    {
        get { return _debugInfo; }
        set { _debugInfo = value; }
    }

    public IEnumerator ExecutePerFrame(IUBContext context, UBActionSheet sheet)
    {
        if (EditorApplication.isPaused || DebugInfo.IsDebugging)
        {
            yield return null;
        }
        if (sheet.ForwardTo != null)
        {
            if (sheet.ForwardTo.Sheet != null)
            {
                yield return context.StartCoroutine(ExecutePerFrame(context, sheet.ForwardTo.Sheet));
                yield break;
            }
        }

        //var execStart = Time.realtimeSinceStartup;
        context.StackTrace.Push(sheet);

        for (int index = 0; index < sheet.Actions.Count; index++)
        {
            var action = sheet.Actions[index];
            if (!action.Enabled) continue;

            context.StackTrace.Push(action);
            if (action.Breakpoint || (DebugInfo.IsStepping && DebugInfo.CurrentActionSheet == sheet))
            {
                DebugInfo.Context = context;
                DebugInfo.CurrentActionSheet = sheet;
                EditorApplication.isPaused = true;
                DebugInfo.CurrentAction = action;
            }
            while (EditorApplication.isPaused)
            {
                yield return null;
            }
            action.IsCurrentlyActive = false;
            ExecuteAction(context, action);
            context.StackTrace.Pop();
        }
        context.StackTrace.Pop();
    }

    public override void ExecuteSheet(IUBContext context, UBActionSheet sheet)
    {

        try
        {
            //Debug.Log(sheet.FullName);
            //base.ExecuteSheet(context,sheet);
            context.StartCoroutine(ExecutePerFrame(context, sheet));
        }
        catch (Exception ex)
        {
            // Coroutine issues
            var error = new BehaviourError()
            {
                Message = ex.Message,
                Source = sheet,
                SourceObject = sheet.RootContainer as Object,
            };
            error.Remove = () => Errors.Remove(error);
            Errors.Add(error);
        }
    }

    protected override void ExecuteAction(IUBContext context, UBAction action)
    {

        context.StackTrace.Push(action);
        try
        {
            //Debug.Log("Executing: " + action.ToString());
            base.ExecuteAction(context, action);
        }
        catch (Exception ex)
        {
            var error = new BehaviourError()
            {
                Message = action.GetType().Name + ": " + ex.Message,
                SourceObject = action.RootContainer as UnityEngine.Object,
                Source = action,
            };
            error.Remove = () => Errors.Remove(error);
            Errors.Add(error);
            EditorWindow.GetWindow<UBExplorerWindow>().Repaint();
#if DEBUG
            Debug.LogError(ex);
#endif
        }
        context.StackTrace.Pop();
    }

    public void Reset()
    {

    }
}

public class DebugInfo
{
    private UBAction _currentAction;

    private UBActionSheet _currentActionSheet;

    //private Stack<IContextItem> _stackTrace = new Stack<IContextItem>();

    public IUBContext Context { get; set; }

    public UBAction CurrentAction
    {
        get { return _currentAction; }
        set
        {
            if (CurrentBreakPoint != null)
            {
                CurrentBreakPoint.IsCurrentlyActive = false;
                CurrentBreakPoint = null;
            }

            _currentAction = value;

            if (value != null && (value.Breakpoint || IsStepping))
            {
                value.IsCurrentlyActive = true;
                CurrentBreakPoint = value;
                IsBreak = true;
                Selection.activeObject = _currentAction.RootContainer as UnityEngine.Object;
                EditorUtility.SetDirty(_currentAction.RootContainer as UnityEngine.Object);
            }
        }
    }

    public UBActionSheet CurrentActionSheet
    {
        get { return _currentActionSheet; }
        set
        {
            _currentActionSheet = value;
        }
    }

    public UBAction CurrentBreakPoint { get; set; }

    public bool IsBreak
    {
        get;
        set;
    }

    public bool IsDebugging
    {
        get
        {
            return CurrentBreakPoint != null || IsStepping;
        }
    }

    public bool IsStepping { get; set; }
    public IUBContext SteppingContext { get; set; }

    //public Stack<IContextItem> StackTrace
    //{
    //    get { return _stackTrace; }
    //    set { _stackTrace = value; }
    //}
}
public class ExeuctionStackItem
{
    public ExeuctionStackItem(IUBContext context, UBAction action)
    {
        Context = context;
        Action = action;
    }

    public UBAction Action { get; set; }
    public IUBContext Context { get; set; }

    public void Execute()
    {
        Action.Execute(Context);
    }
}