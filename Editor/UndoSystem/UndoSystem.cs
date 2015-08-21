using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Invert.Common.UI;
using Invert.Core.GraphDesigner.Unity;
using Invert.Data;
using Invert.IOC;
using Invert.Json;
using uFrame.Serialization;
using UnityEngine;
using JSON = Invert.Json.JSON;

namespace Invert.Core.GraphDesigner
{
    public class UndoCommand : Command
    {

    } 

    public class RedoCommand : Command
    {

    }

    public class TestyCommand : Command
    {
        
    }
    public class UndoSystem : DiagramPlugin,
        IDataRecordPropertyBeforeChange,
        IDataRecordInserted,
        IDataRecordRemoved,
        IExecuteCommand<UndoCommand>,
        IExecuteCommand<RedoCommand>,
        IExecuteCommand<TestyCommand>,
        IToolbarQuery,
        ICommandExecuting,
        ICommandExecuted,
        IDrawExplorer,
        IKeyboardEvent
    {
        public string CurrentUndoGroupId = null;
        private List<UndoItem> _undoItems;

        public List<UndoItem> UndoItems
        {
            get { return _undoItems ?? (_undoItems = new List<UndoItem>()); }
            set { _undoItems = value; }
        }

        public void BeforePropertyChanged(IDataRecord record, string name, object previousValue, object nextValue)
        {
            if (IsUndoRedo) return;
            if (CurrentUndoGroupId == null || record is UndoItem || record is RedoItem) return;

            var undoItem = new UndoItem();
            undoItem.Time = DateTime.Now;
            undoItem.Group = CurrentUndoGroupId;
            undoItem.DataRecordId = record.Identifier;
            undoItem.Data = InvertJsonExtensions.SerializeObject(record).ToString();
            undoItem.RecordType = record.GetType().AssemblyQualifiedName;
            undoItem.Type = UndoType.Changed;
            undoItem.Name = CurrentName;
            UndoItems.Add(undoItem);  
        }

        public void RecordInserted(IDataRecord record)
        {
            if (IsUndoRedo) return;
            if (CurrentUndoGroupId == null || record is UndoItem || record is RedoItem) return;
            var undoItem = new UndoItem();
            undoItem.Time = DateTime.Now;
            undoItem.Group = CurrentUndoGroupId;
            undoItem.DataRecordId = record.Identifier;
            undoItem.Data = InvertJsonExtensions.SerializeObject(record).ToString();
            undoItem.RecordType = record.GetType().AssemblyQualifiedName;
            undoItem.Type = UndoType.Inserted;
            undoItem.Name = CurrentName;

            UndoItems.Add(undoItem);
        }

        public void RecordRemoved(IDataRecord record)
        {
            if (IsUndoRedo) return;
            if (CurrentUndoGroupId == null || record is UndoItem || record is RedoItem) return;
            var undoItem = new UndoItem();
            undoItem.Time = DateTime.Now;
            undoItem.Group = CurrentUndoGroupId;
            undoItem.DataRecordId = record.Identifier;
            undoItem.Data = InvertJsonExtensions.SerializeObject(record).ToString();
            undoItem.RecordType = record.GetType().AssemblyQualifiedName;
            undoItem.Name = CurrentName;
            undoItem.Type = UndoType.Removed;
            UndoItems.Add(undoItem);
        }
        public bool IsUndoRedo { get; set; }
        public void Execute(UndoCommand command)
        {
            Repository = Container.Resolve<IRepository>();
            var undoGroup = Repository.All<UndoItem>().GroupBy(p=>p.Group).LastOrDefault();
            if (undoGroup == null) return;
            IsUndoRedo = true;
            foreach (var undoItem in undoGroup)
            {
                // Create redo item
                var redoItem = new RedoItem();
                redoItem.Data = undoItem.Data;
                redoItem.Group = undoItem.Group;
                redoItem.DataRecordId = undoItem.DataRecordId;
                redoItem.Name = undoItem.Name;
                redoItem.Time = undoItem.Time;
                redoItem.Type = undoItem.Type;
                redoItem.RecordType = undoItem.RecordType;
                redoItem.UndoData = InvertJsonExtensions.SerializeObject(undoItem).ToString();

                if (undoItem.Type == UndoType.Inserted)
                {
                    var record = Repository.GetById<IDataRecord>(undoItem.DataRecordId);
                    redoItem.Data = InvertJsonExtensions.SerializeObject(record).ToString();
                    Repository.Remove(record);
                    redoItem.Type = UndoType.Removed;
                }
                else if (undoItem.Type == UndoType.Removed)
                {
                    
                    var obj = InvertJsonExtensions.DeserializeObject(Type.GetType(undoItem.RecordType), JSON.Parse(undoItem.Data).AsObject) as IDataRecord;
                    Repository.Add(obj);
                    redoItem.Type = UndoType.Inserted;
                    redoItem.Data = InvertJsonExtensions.SerializeObject(obj).ToString();
                }
                else 
                {
                    var record = Repository.GetById<IDataRecord>(undoItem.DataRecordId);
                    // We don't want to signal any events on deserialization
                    record.Repository = null;
                    redoItem.Data = InvertJsonExtensions.SerializeObject(record).ToString();
                    InvertJsonExtensions.DeserializeExistingObject(record, JSON.Parse(undoItem.Data).AsObject);
                    record.Changed = true;
                    record.Repository = Repository;

                }
                Repository.Remove(undoItem);
                Repository.Add(redoItem);
            }
            IsUndoRedo = false;
        }

        public void Execute(RedoCommand command)
        {
            IsUndoRedo = true;
            Repository = Container.Resolve<IRepository>();
            var redoGroup = Repository.All<RedoItem>().GroupBy(p=>p.Group).LastOrDefault();
            if (redoGroup == null) return;
            foreach (var redoItem in redoGroup)
            {
                 // Create redo item
                var undoItem = InvertJsonExtensions.DeserializeObject(typeof (UndoItem), JSON.Parse(redoItem.UndoData)) as UndoItem;
                

                if (redoItem.Type == UndoType.Inserted)
                {
                    var record = Repository.GetById<IDataRecord>(redoItem.DataRecordId);
                    
                    Repository.Remove(record);
                    
                }
                else if (redoItem.Type == UndoType.Removed)
                {
                    
                    var obj = InvertJsonExtensions.DeserializeObject(Type.GetType(redoItem.RecordType), JSON.Parse(redoItem.Data).AsObject) as IDataRecord;
                    Repository.Add(obj);
                }
                else
                {
                    var record = Repository.GetById<IDataRecord>(redoItem.DataRecordId);
                    // We don't want to signal any events on deserialization
                    record.Repository = null;
                    InvertJsonExtensions.DeserializeExistingObject(record, JSON.Parse(redoItem.Data).AsObject);
                    record.Changed = true;
                    record.Repository = Repository;

                }
                Repository.Remove(redoItem);
                Repository.Add(undoItem);
            }
            IsUndoRedo = false;

        }

        public void QueryToolbarCommands(ToolbarUI ui)
        {
            var repo = Container.Resolve<IRepository>();
            var undoItem = repo.All<UndoItem>().LastOrDefault();
            if (undoItem != null)
            {
                ui.AddCommand(new ToolbarItem()
                {
                    Title = "Undo " + undoItem.Name,
                    Command = new UndoCommand(),
                    Position = ToolbarPosition.BottomRight
                });
              
            }
            ui.AddCommand(new ToolbarItem()
            {
                Title = "Testy ",
                Command = new TestyCommand(),
                Position = ToolbarPosition.Right
            });

        }

        public void CommandExecuting(ICommand command)
        {
            if (command is UndoCommand) return;
            InvertApplication.Log(command.GetType().Name);
            CurrentUndoGroupId = DateTime.Now.Hour.ToString() + DateTime.Now.Minute + DateTime.Now.Second.ToString();
            CurrentName = command.Title;
            UndoItems.Clear();
        }

        public string CurrentName { get; set; }

        public override void Loaded(UFrameContainer container)
        {
            base.Loaded(container);
            Repository = container.Resolve<IRepository>();
        }

        public IRepository Repository { get; set; }
        public override void Initialize(UFrameContainer container)
        {
            base.Initialize(container); 
            InvertApplication.Log("Initialized Twice?");
        }

        public void CommandExecuted(ICommand command)
        {
            if (command is UndoCommand || command is RedoCommand) return;
            CurrentUndoGroupId = null;
            if (Repository != null)
            {
                foreach (var item in UndoItems)
                {
                    Repository.Add(item);
                }
                Repository.RemoveAll<RedoItem>();
                Repository.Commit();
            }
        }

        public UndoSystem()
        {
            Debug.Log(this.GetHashCode());
        }
        public void DrawExplorer()
        {
            if (Repository == null) return;
            GUILayout.Label("Start");
            foreach (var item in Repository.All<UndoItem>().GroupBy(p => p.Group))
            {
                if (GUIHelpers.DoToolbarEx(item.Key.ToString()))
                {
                    foreach (var x in item)
                    if (GUILayout.Button(x.Type.ToString()))
                    {
                    }
                }
              
            }
        }

        public void Execute(TestyCommand command)
        {
            var sb = new StringBuilder();
            foreach (var item in InvertApplication.Plugins.OrderBy(p=>p.Title))
            {
                sb.AppendLine(item.Title);
            }
            InvertApplication.Log(sb.ToString());
        }

        public bool KeyEvent(KeyCode keyCode, ModifierKeyState state)
        {
            if (state.Ctrl && keyCode == KeyCode.Z)
            {
                InvertApplication.Execute(new UndoCommand());
                return true;
            }
            if (state.Ctrl && keyCode == KeyCode.Y)
            {
                InvertApplication.Execute( new RedoCommand());
                return true;
            }
            return false;
        }
    }

    public class TransactionItem : IDataRecord
    {
        private string _dataRecordId;
        private UndoType _type;
        private string _recordType;
        private string _data;
        private string _name;
        private string _group;
        public IRepository Repository { get; set; }
        public string Identifier { get; set; }
        public bool Changed { get; set; }

        [JsonProperty]
        public DateTime Time { get; set; }

        [JsonProperty]
        public string Group
        {
            get { return _group; }
            set
            {
                _group = value;
                Changed = true;
            }
        }

        [JsonProperty]
        public string DataRecordId
        {
            get { return _dataRecordId; }
            set
            {
                _dataRecordId = value;
                Changed = true;
            }
        }

        [JsonProperty]
        public UndoType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                Changed = true;
            }
        }

        [JsonProperty]
        public string RecordType
        {
            get { return _recordType; }
            set
            {
                _recordType = value;
                Changed = true;
            }
        }

        [JsonProperty]
        public string Data
        {
            get { return _data; }
            set
            {
                _data = value;
                Changed = true;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                Changed = true;
            }
        }
    }

    public class UndoItem : TransactionItem
    {
      
    }

    public class RedoItem : TransactionItem
    {
        private string _undoData;
        [JsonProperty]
        public string UndoData
        {
            get { return _undoData; }
            set { _undoData = value;
                Changed = true;
            }
        }
    }
    public enum UndoType
    {
        Inserted,
        Removed,
        Changed
    }
}
