using System;
using System.Collections.Generic;
using System.Linq;
using Invert.Common;
using Invert.uFrame.Editor.ElementDesigner;
using Invert.uFrame.Editor.ElementDesigner.Commands;
using UnityEditor;
using UnityEngine;

namespace Invert.uFrame.Editor
{
    public class ElementsDesigner : EditorWindow, ICommandHandler
    {
        private static float HEIGHT = 768;

        private static List<Matrix4x4> stack = new List<Matrix4x4>();

        private static float WIDTH = 1024;

        public ICommandUI Toolbar
        {
            get
            {
                if (_toolbar != null) return _toolbar;


                return _toolbar = uFrameEditor.CreateCommandUI<ToolbarUI>(this, typeof(IToolbarCommand));
            }
            set { _toolbar = value; }
        }

        [SerializeField]
        private ElementsDiagram _diagram;

        private Vector2 _scrollPosition;
        private ICommandUI _toolbar;


        public static IElementDesignerData SelectedElementDiagram
        {
            get { return Selection.activeObject as IElementDesignerData; }
        }

        public ElementsDiagram Diagram
        {
            get { return _diagram; }
            set
            {
                _diagram = value;
                _toolbar = null;
            }
        }

        public float InspectorWidth
        {
            get
            {
                return 0;
                //if (Diagram == null || Diagram.Selected == null)
                //    return 0;
                //return 240;
            }
        }

        public bool IsMiddleMouseDown { get; set; }

        public string LastLoadedDiagram
        {
            get
            {
                return EditorPrefs.GetString("LastLoadedDiagram", null);
            }
            set
            {
                EditorPrefs.SetString("LastLoadedDiagram", value);
            }
        }

        public Vector2 PanStartPosition { get; set; }

        static public void BeginGUI()
        {
            stack.Add(GUI.matrix);
            Matrix4x4 m = new Matrix4x4();
            var w = (float)Screen.width;
            var h = (float)Screen.height;
            var aspect = w / h;
            var scale = 1f;
            var offset = Vector3.zero;
            if (aspect < (WIDTH / HEIGHT))
            { //screen is taller
                scale = (Screen.width / WIDTH);
                offset.y += (Screen.height - (HEIGHT * scale)) * 0.5f;
            }
            else
            { // screen is wider
                scale = (Screen.height / HEIGHT);
                offset.x += (Screen.width - (WIDTH * scale)) * 0.5f;
            }
            m.SetTRS(offset, Quaternion.identity, Vector3.one * scale);
            GUI.matrix *= m;
        }

        static public void EndGUI()
        {
            GUI.matrix = stack[stack.Count - 1];
            stack.RemoveAt(stack.Count - 1);
        }

        [MenuItem("Window/Element Designer", false, 1)]
        public static void Init()
        {
            // Get existing open window or if none, make a new one:
            var window = (ElementsDesigner)GetWindow(typeof(ElementsDesigner));
            window.title = "Elements";

            //var repo = new ElementsDataRepository();
            //var diagram = new ElementsDiagram(repo);
            //diagram.Data.ViewModels.Add(repo.GetViewModel(typeof(FPSWeaponViewModel)));
            //diagram.Data.ViewModels.Add(repo.GetViewModel(typeof(FPSBulletViewModel)));

            // RemoveFromDiagram when switching to add all
            window.Show();
        }

        public void InfoBox(string message, MessageType type = MessageType.Info)
        {
            EditorGUI.HelpBox(new Rect(15, 30, 300, 30), message, type);
        }

        public void OnGUI()
        {
         
            if ((Diagram == null || Diagram.Data == null) && !string.IsNullOrEmpty(LastLoadedDiagram))
            {
                Diagram = null;
                if (!LoadDiagram(LastLoadedDiagram))
                {
                    LastLoadedDiagram = null;
                    return;
                }
                if (Diagram == null)
                {
                    LastLoadedDiagram = null;
                    return;
                }
            }
            
          
            var style = ElementDesignerStyles.Background;
            style.border = new RectOffset(
                Mathf.RoundToInt(41),
                Mathf.RoundToInt(41),
                Mathf.RoundToInt(32),
                Mathf.RoundToInt(32));
            GUILayout.BeginHorizontal(EditorStyles.toolbar);
            //DoToolbar(diagramRect);
            DoToolbar();
            GUILayout.EndHorizontal();
            if (Diagram == null) return;
            var diagramRect = new Rect(0f, (EditorStyles.toolbar.fixedHeight) - 1, Screen.width - 3, Screen.height - (EditorStyles.toolbar.fixedHeight * 2) - EditorStyles.toolbar.fixedHeight - 2);
            Diagram.Rect = diagramRect;
            GUI.Box(diagramRect, string.Empty, style);

            if (Diagram == null)
            {
            }
            else
            {
                if (Event.current.control && Event.current.type == EventType.mouseDown)
                {
                }
                _scrollPosition = GUI.BeginScrollView(diagramRect, _scrollPosition, Diagram.DiagramSize);

                HandlePanning(diagramRect);
                if (Diagram.Data != null)
                {
                    var softColor = Diagram.Data.Settings.GridLinesColor;
                    var hardColor = Diagram.Data.Settings.GridLinesColorSecondary;
                    //var softColor = new Color(1f, 1f, 0f, 0.1f);
                    //var hardColor = new Color(1f, 1f, 1f, 0.3f);
                    var x = 0f;
                    var every10 = 0;

                    while (x < diagramRect.width + _scrollPosition.x)
                    {
                        Handles.color = softColor;
                        if (every10 == 10)
                        {
                            Handles.color = hardColor;
                            every10 = 0;
                        }
                        Handles.DrawLine(new Vector2(x, 0f), new Vector2(x, Screen.height + _scrollPosition.y));
                        x += Diagram.Data.Settings.SnapSize * ElementDesignerStyles.Scale;
                        every10++;
                    }
                    var y = 3f;
                    every10 = 0;
                    while (y < diagramRect.height + _scrollPosition.y)
                    {
                        Handles.color = softColor;
                        if (every10 == 10)
                        {
                            Handles.color = hardColor;
                            every10 = 0;
                        }
                        Handles.DrawLine(new Vector2(0, y), new Vector2(Screen.width + _scrollPosition.x, y));
                        y += Diagram.Data.Settings.SnapSize * ElementDesignerStyles.Scale;
                        every10++;
                    }
                }
                //BeginGUI();
                Diagram.Draw();

                //EndGUI();
                GUI.EndScrollView();
                GUILayout.Space(diagramRect.height);
                GUILayout.BeginHorizontal(EditorStyles.toolbar);
                //DoToolbar(diagramRect);
                Toolbar.GoBottom();
                GUILayout.EndHorizontal();
            }

            if (EditorApplication.isCompiling)
            {
                InfoBox("Compiling.. Please Wait!");
            }
            if (Diagram != null && Diagram.Data != null)
            {
                var refactors = Diagram.Data.RefactorCount;
                if (refactors > 0)
                {
                    InfoBox(string.Format("You have {0} refactors. Save before recompiling occurs.", refactors), MessageType.Warning);
                }
            }
            var evt = Event.current;
            if (evt != null && evt.isKey && evt.type == EventType.KeyUp && Diagram != null)
            {
              
                if (Diagram.SelectedData == null || !Diagram.SelectedData.IsEditing)
                {
                    
                    Diagram.HandleKeyEvent(evt);
                    evt.Use();
                }
            }
            if (Event.current.type == EventType.ValidateCommand &&
          Event.current.commandName == "UndoRedoPerformed")
            {
               
            }

            if (Diagram != null && Diagram.Dirty || EditorApplication.isCompiling)
            {
                Repaint();
            }
         
        }
   
        public void OnLostFocus()
        {
            if (Diagram == null) return;
            Diagram.Selected = null;
            Diagram.SelectedItem = null;
            Diagram.IsMouseDown = false;
            Diagram.DeselectAll();
        }

        //public virtual void OpenDiagramByAttribute(Type type)
        //{
        //    var attribute = type.GetCustomAttributes(typeof(DiagramInfoAttribute), true).FirstOrDefault() as DiagramInfoAttribute;
        //    if (attribute == null) return;
        //    LoadDiagramByName(attribute.DiagramName);
        //}
       
        public void Update()
        {
            if (Diagram == null) return;
           
            if (Diagram.IsMouseDown || Diagram.Dirty || EditorApplication.isCompiling)
            {
                Repaint();
                Diagram.Dirty = false;
            }
        }

        private void DoToolbar()
        {
            try
            {
                if (
                    GUILayout.Button(
                        new GUIContent(Diagram == null || Diagram.Data == null ? "--Select Diagram--" : Diagram.Data.Name),
                        EditorStyles.toolbarPopup))
                {
                    SelectDiagram();
                }
            }
            catch (Exception ex)
            {
                Diagram = null;
            
                Repaint();
                return;

            }
            Toolbar.Go();
        }

        private void HandlePanning(Rect diagramRect)
        {
            if (Event.current.button == 2 && Event.current.type == EventType.MouseDown)
            {
                IsMiddleMouseDown = true;
                PanStartPosition = Event.current.mousePosition;
            }
            if (Event.current.button == 2 && Event.current.rawType == EventType.MouseUp && IsMiddleMouseDown)
            {
                IsMiddleMouseDown = false;
            }
            if (IsMiddleMouseDown)
            {
                var delta = PanStartPosition - Event.current.mousePosition;
                _scrollPosition += delta;
                if (_scrollPosition.x < 0)
                    _scrollPosition.x = 0;
                if (_scrollPosition.y < 0)
                    _scrollPosition.y = 0;
                if (_scrollPosition.x > diagramRect.width - diagramRect.x)
                {
                    _scrollPosition.x = diagramRect.width - diagramRect.x;
                }
                if (_scrollPosition.y > diagramRect.height - diagramRect.y)
                {
                    _scrollPosition.y = diagramRect.height - diagramRect.y;
                }
                Repaint();
            }

        }

        public bool LoadDiagram(string path)
        {
            try
            {
                //Undo.undoRedoPerformed = UndoRedoPerformed;
                Undo.undoRedoPerformed += UndoRedoPerformed;
                //Diagram = uFrameEditor.Container.Resolve<ElementsDiagram>();
                Diagram = new ElementsDiagram(path);
                //Diagram.SelectionChanged += DiagramOnSelectionChanged;
                LastLoadedDiagram = path;
                Diagram.Dirty = true;
                Diagram.Data.ApplyFilter();
                Diagram.Refresh(true);
                this.Repaint();
            }
            catch (Exception ex)
            {
#if DEBUG
                UnityEngine.Debug.LogException(ex);
#endif
                Debug.Log("Either a plugin isn't installed or the file could no longer be found.");
                LastLoadedDiagram = null;
                return false;
            }
            return true;
            // var newScrollPosition = new Vector2(Diagram.DiagramSize.width, Diagram.DiagramSize.height).normalized / 2;
            //_scrollPosition = new Vector2(250,250);
        }

        private void UndoRedoPerformed()
        {
            Diagram = null;
            Repaint();
        }

        public void LoadDiagramByName(string diagramName)
        {
            var repos = uFrameEditor.Container.ResolveAll<IElementsDataRepository>();
            var diagrams = repos.SelectMany(p => p.GetProjectDiagrams()).ToDictionary(p => p.Key, p => p.Value);
            if (diagrams.ContainsKey(diagramName))
                LoadDiagram(diagrams[diagramName]);

            //var diagram = UFrameAssetManager.Diagrams.FirstOrDefault(p => p.Name == diagramName);
            //if (diagram == null) return;
            //LoadDiagram(diagram);
        }

        private void SelectDiagram()
        {
            var repositories = uFrameEditor.Container.ResolveAll<IElementsDataRepository>();
            var diagramNames = repositories.SelectMany(p => p.GetProjectDiagrams().Keys).ToArray();
            var diagramPaths = repositories.SelectMany(p => p.GetProjectDiagrams().Values).ToArray();


            var menu = new GenericMenu();
            for (int index = 0; index < diagramNames.Length; index++)
            {
                var diagramName = diagramNames[index];
                var diagram = diagramPaths[index];

                menu.AddItem(new GUIContent(diagramName), Diagram != null && diagram == LastLoadedDiagram, () =>
                {
                    LastLoadedDiagram = diagramName;
                    LoadDiagram(diagram);

                });
            }
            menu.ShowAsContext();
        }

        public IEnumerable<object> ContextObjects
        {
            get
            {
                yield return this;
                if (Diagram != null)
                    yield return Diagram;
                if (Diagram != null && Diagram.Data != null)
                    yield return Diagram.Data;
            }
        }

        public void CommandExecuted(IEditorCommand command)
        {
            if (Diagram != null)
            {
                Diagram.Refresh();
            }
            Repaint();
        }

        public void CommandExecuting(IEditorCommand command)
        {

        }
    }
}