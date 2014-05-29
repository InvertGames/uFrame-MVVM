using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace Invert.uFrame.Editor.ElementDesigner
{
    public interface IDiagramCommand
    {
        Type For { get; }
        void Execute(object item);
        List<CommandHook> Hooks { get; }
        string Name { get; }
        string CanPerform(object arg);
    }

    public abstract class DiagramCommand : IDiagramCommand
    {
        private List<CommandHook> _hooks;
    

        public abstract Type For { get; }

        public void Execute(object item)
        {
            Perform(item);
        }

        public abstract void Perform(object arg);

        public List<CommandHook> Hooks
        {
            get { return _hooks ?? (_hooks = new List<CommandHook>()); }
            set { _hooks = value; }
        }

        public virtual string Name
        {
            get { return this.GetType().Name.Replace("Command",""); }
        }

        public string CanExecute(object arg)
        {
            return null;
        }

        public abstract string CanPerform(object arg);
    }
    public abstract class DiagramCommand<TFor> : DiagramCommand
    {
        public override Type For
        {
            get { return typeof(TFor); }
        }

        public sealed override void Perform(object item)
        {
            Perform((TFor)item);
        }

        public abstract void Perform(TFor item);

        public override string CanPerform(object arg)
        {
            return CanPerform((TFor) arg);
        }

        public abstract string CanPerform(TFor item);

    }

    public abstract class ContextMenuCommand<TFor> : DiagramCommand<TFor>
    {
        public abstract IEnumerable<ContextMenuItem> GetMenuOptions();
    }

    public interface IToolbarCommand
    {
        ToolbarPosition Position { get; }
    }
    public enum ToolbarPosition
    {
        Left,
        Right
    }
    public abstract class ToolbarCommand : DiagramCommand<ElementsDiagram>, IToolbarCommand
    {
        public virtual ToolbarPosition Position { get { return ToolbarPosition.Right; } }

        public override string CanPerform(ElementsDiagram item)
        {
            if (item == null) return "No Diagram Open";
            return null;
        }
    }

    public class SaveCommand : ToolbarCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            item.Repository.Save();
            item.DeselectAll();
            item.Refresh();
        }
    }

    public class AutoLayoutCommand : ToolbarCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            item.LayoutDiagram();
            item.Refresh();
        }
    }

    public class PopToFilterCommand : ToolbarCommand, IDynamicOptionsCommand
    {

        public override void Perform(ElementsDiagram item)
        {

            item.Data.PopToFilter(SelectedOption.Name);
            item.Refresh(true);
        }

        public IEnumerable<ContextMenuItem> GetOptions(ElementsDiagram item)
        {

            yield return new ContextMenuItem() { Name = item.Data.SceneFlowFilter.Name, Checked = item.Data.CurrentFilter == item.Data.SceneFlowFilter };
            foreach (var filter in item.Data.FilterPath)
            {
                yield return new ContextMenuItem() {Name = filter.Name,Checked = item.Data.CurrentFilter == filter};
            }
        }

        public override ToolbarPosition Position
        {
            get { return ToolbarPosition.Left; }
        }

        public ContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get{ return MultiOptionType.Buttons; } }
    }

    public class AddNewCommand : ToolbarCommand, IParentCommand
    {
        public override void Perform(ElementsDiagram item)
        {
            // No implementation
        }

        public ContextMenuItem SelectedOption { get; set; }
        public MultiOptionType OptionsType { get{return MultiOptionType.DropDown;} }
    }

    

    public enum MultiOptionType
    {
        DropDown,
        Buttons
    }
    public interface IDynamicOptionsCommand
    {
        IEnumerable<ContextMenuItem> GetOptions(ElementsDiagram item);
        ContextMenuItem SelectedOption { get; set; }
        MultiOptionType OptionsType { get; }
    }

    public interface IParentCommand
    {
        
    }
    public abstract class ContextDiagramCommand<TFor,TCommand> : DiagramCommand<TFor>, IContextMenuItemCommand
    {
        

        public bool Checked { get; set; }

        public Type ContextItemFor
        {
            get { return typeof (TCommand); }
        }
    }

    public interface IContextMenuItem
    {
        string Name { get; }
        bool Checked { get; set; }
    }

    public interface IContextMenuItemCommand : IContextMenuItem
    {
        Type ContextItemFor { get; }
    }
    public class ContextMenuItem 
    {
 
        public string Name { get; set; }


        public bool Checked { get; set; }
    }
    public class CommandHook
    {
        public CommandHookLifetime Lifetime { get; set; }
        public Action Action { get; set; }
    }

    public enum CommandHookMode
    {
        Before,
        After
    }
    public enum CommandHookLifetime
    {
        NextExecute,
        Everytime
    }
}