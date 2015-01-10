using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Invert.Core.GraphDesigner;
using Invert.uFrame.Editor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(UnityGraphData),true)]
public class GraphDataInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Convert to Text Asset"))
        {
            var t = target as IGraphData;
            var property = serializedObject.FindProperty("_jsonData");
            var jsonData = InvertGraph.Serialize(t).ToString();
            var path = AssetDatabase.GetAssetPath(target);
            File.WriteAllText(path.Replace(".asset",".txt"),jsonData);
            AssetDatabase.Refresh();
        }
        if (GUILayout.Button("Upgrade Format"))
        {
            var oldGraph = target as IGraphData;
            //var newGraph = new Invert.uFrame.Editor.ElementsGraph()
            //{
            //    Name = oldGraph.Name,
                
            //};


            // Convert all the nodes
            Dictionary<DiagramNode,DiagramNode> converted = new Dictionary<DiagramNode, DiagramNode>();
            List<ConnectionData> connections = new List<ConnectionData>();
            foreach (var oldNode in oldGraph.NodeItems.OfType<SceneManagerData>())
            {
                var node = new SceneManagerNode
                {
                    Identifier = oldNode.Identifier, Name = oldNode.Name
                };

                converted.Add(oldNode,node);
    
            }
            foreach (var oldNode in oldGraph.NodeItems.OfType<SubSystemData>())
            {
                var node = new SubsystemNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };
                converted.Add(oldNode, node);
        
            }
            foreach (var oldNode in oldGraph.NodeItems.OfType<ElementData>())
            {
                var node = new ElementNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };

                converted.Add(oldNode, node);
     
            }
            foreach (var oldNode in oldGraph.NodeItems.OfType<ViewData>())
            {
                var node = new ElementViewNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };

                converted.Add(oldNode, node);
     
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<ViewComponentData>())
            {
                var node = new ElementViewComponentNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };

                converted.Add(oldNode, node);
    
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<EnumData>())
            {
                //var node = new ElementViewComponentNode()
                //{
                //    Identifier = oldNode.Identifier,
                //    Name = oldNode.Name
                //};

                //converted.Add(oldNode, node);
                //Debug.Log(string.Format("Converted {0}", oldNode.Name));
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<StateMachineNodeData>())
            {
                var node = new StateMachineNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };

                converted.Add(oldNode, node);
           
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<StateMachineStateData>())
            {
                var node = new StateNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };

                converted.Add(oldNode, node);
              
            }


            // Grab all the connections
            ConvertSubsystems(converted, connections);




            foreach (var item in connections)
            {
                Debug.Log(item.OutputIdentifier + " -> " + item.InputIdentifier);
            }



            // Reconstruct the filters


        }
    }

    private void ConvertSubsystems(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
    {
        foreach (var o in converted.Keys.OfType<SubSystemData>())
        {
            var n = converted[o] as SubsystemNode;
            // Convert Instances
            foreach (var instance in o.Instances)
            {
                var nInstance = new RegisteredInstanceReference()
                {
                    Node = n,
                    Name = o.Name,
                    SourceIdentifier = instance.RelatedType
                };

                n.ChildItems.Add(nInstance);
            }
            // Convert connections
            connections.AddRange(GetSubsystemConnections(converted, o, n));
        }

        // Grab all the connections
        foreach (var o in converted.Keys.OfType<SubSystemData>())
        {
            var n = converted[o] as SubsystemNode;
            // Convert Instances
            foreach (var instance in o.Instances)
            {
                var nInstance = new RegisteredInstanceReference()
                {
                    Node = n,
                    Name = o.Name,
                    SourceIdentifier = instance.RelatedType
                };

                n.ChildItems.Add(nInstance);
            }
            // Convert connections
            connections.AddRange(GetSubsystemConnections(converted, o, n));
        }
    }
    private void ConvertSceneManagers(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
    {
        foreach (var o in converted.Keys.OfType<SubSystemData>())
        {
            var n = converted[o] as SubsystemNode;
            // Convert Instances
            foreach (var instance in o.Instances)
            {
                var nInstance = new RegisteredInstanceReference()
                {
                    Node = n,
                    Name = o.Name,
                    SourceIdentifier = instance.RelatedType
                };

                n.ChildItems.Add(nInstance);
            }
            // Convert connections
            connections.AddRange(GetSubsystemConnections(converted, o, n));
        }

        // Grab all the connections
        foreach (var o in converted.Keys.OfType<SubSystemData>())
        {
            var n = converted[o] as SubsystemNode;
            // Convert Instances
            foreach (var instance in o.Instances)
            {
                var nInstance = new RegisteredInstanceReference()
                {
                    Node = n,
                    Name = o.Name,
                    SourceIdentifier = instance.RelatedType
                };

                n.ChildItems.Add(nInstance);
            }
            // Convert connections
            connections.AddRange(GetSubsystemConnections(converted, o, n));
        }
    }

    private IEnumerable<ConnectionData> GetSubsystemConnections(Dictionary<DiagramNode,DiagramNode> converted, SubSystemData o, SubsystemNode n)
    {
        foreach (var item in o.Imports)
        {
            var oldImport = converted.Keys.FirstOrDefault(p => p.Identifier == item);
            if (oldImport == null) continue;
            var newImport = converted[oldImport] as SubsystemNode;

            yield return new ConnectionData(newImport.ExportOutputSlot.Identifier,n.ImportInputSlot.Identifier)
            {
                Output = newImport.ExportOutputSlot,
                Input = n.ImportInputSlot
            };
        }
    }
}

