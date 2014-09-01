//public class ControllerActionCommand : ICommand
//{
//    public ControllerActionCommand(string controller, string action)
//        : this(GameManager.ActiveGame[controller], action, null)
//    {
//    }

//    public ControllerActionCommand(string controller, string action, object[] arguments) :
//        this(GameManager.ActiveGame[controller], action, arguments)
//    {
//    }

//    public ControllerActionCommand(UFrameBehaviour controller, string action)
//        : this(controller, action, null)
//    {
//    }

//    public ControllerActionCommand(UFrameBehaviour controller, string action, object[] arguments)
//    {
//        Controller = controller;
//        Action = action;
//        Arguments = arguments;
//    }

//    public string Action
//    {
//        get;
//        set;
//    }

//    public object[] Arguments { get; set; }

//    protected UFrameBehaviour Controller
//    {
//        get;
//        set;
//    }

//    public void Execute()
//    {
//        if (Controller == null)
//        {
//            throw new Exception("Controller is not set.");
//        }
//        var method = Controller.GetType().GetMethod(Action);

//        if (method == null)
//        {
//            throw new Exception(string.Format("Event '{0}' was not found on {1}", Action, Controller.ToString()));
//        }

//        method.Invoke(Controller, Arguments);

//    }
//}