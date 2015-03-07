using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Invert.Core.GraphDesigner;
using Invert.uFrame.MVVM;
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
//            var property = serializedObject.FindProperty("_jsonData");
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
                foreach (var item in oldNode.Transitions)
                {
                    node.ChildItems.Add(new SceneTransitionsReference()
                    {
                        Node = node,
                        Identifier = item.Identifier,
                        SourceIdentifier = item.CommandIdentifier,
                    });
                    if (string.IsNullOrEmpty(item.ToIdentifier)) continue;
                    connections.Add(new ConnectionData(item.Identifier,item.ToIdentifier));
                }
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
            foreach (var oldNode in oldGraph.NodeItems.OfType<ComputedPropertyData>())
            {
                var node = new ComputedPropertyNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name,
                    Type = oldNode.RelatedType
                };
                // TODO output to transition?
                foreach (var item in oldNode.DependantProperties)
                {
                    connections.Add(new ConnectionData(item.Identifier,node.Identifier));
                }
                // TODO sub properties
                converted.Add(oldNode, node);

            }
            foreach (var oldNode in oldGraph.NodeItems.OfType<ElementData>())
            {
                var node = new ElementNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name,
                };
                if (!string.IsNullOrEmpty(oldNode.BaseIdentifier))
                {
                    connections.Add(new ConnectionData(oldNode.BaseIdentifier, oldNode.Identifier));
                }
                foreach (var item in oldNode.Properties)
                {
                    node.ChildItems.Add(new PropertiesChildItem()
                    {
                        Identifier = item.Identifier,
                        Node = node,
                        Name = item.Name,
                        RelatedType = item.RelatedType
                    });
                    
                }
                foreach (var item in oldNode.Collections)
                {
                    node.ChildItems.Add(new CollectionsChildItem()
                    {
                        Identifier = item.Identifier,
                        Name = item.Name,
                        Node = node,
                        RelatedType = item.RelatedType
                    });

                }
                foreach (var item in oldNode.Commands)
                {
                    node.ChildItems.Add(new CommandsChildItem()
                    {
                        Identifier = item.Identifier,
                        Name = item.Name,
                        Node = node,
                        RelatedType = item.RelatedType
                    });
                    if (!string.IsNullOrEmpty(item.TransitionToIdentifier))
                    connections.Add(new ConnectionData(item.Identifier,item.TransitionToIdentifier));
                }
                converted.Add(oldNode, node);
     
            }
            foreach (var oldNode in oldGraph.NodeItems.OfType<ViewData>())
            {
                var node = new ViewNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };
                // TODO CONVERT INHERITANCE
                // Connect the scene property
                foreach (var sceneProperty in oldNode.SceneProperties)
                {
                    connections.Add(new ConnectionData(sceneProperty.Identifier,node.ScenePropertiesInputSlot.Identifier));
                }

                // TODO CONVERT BINDINGS

                converted.Add(oldNode, node);
     
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<ViewComponentData>())
            {
                var node = new ViewComponentNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };
                // TODO CONVERT INHERITANCE
                converted.Add(oldNode, node);
    
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<EnumData>())
            {
                var node = new EnumNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };
                foreach (var item in oldNode.EnumItems)
                {
                    node.ChildItems.Add(new EnumChildItem()
                    {
                        Identifier = item.Identifier,
                        Node = node,
                        Name = item.Name
                    });
                }
                converted.Add(oldNode, node);
                //Debug.Log(string.Format("Converted {0}", oldNode.Name));
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<StateMachineNodeData>())
            {
                var node = new StateMachineNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };
                foreach (var transition in oldNode.Transitions)
                {
                    node.ChildItems.Add(new TransitionsChildItem()
                    {
                        Name = transition.Name,
                        Identifier =  transition.Identifier,
                        Node = node,
                    });
                    connections.Add(new ConnectionData(transition.PropertyIdentifier,transition.Identifier));
                    //connections.Add();
                }
                converted.Add(oldNode, node);
            }

            foreach (var oldNode in oldGraph.NodeItems.OfType<StateMachineStateData>())
            {
                var node = new StateNode()
                {
                    Identifier = oldNode.Identifier,
                    Name = oldNode.Name
                };
                foreach (var transition in oldNode.Transitions)
                {
                    node.ChildItems.Add(new StateTransitionsReference()
                    {
                        Name = transition.Name,
                        Identifier = transition.Identifier,
                        Node = node,
                    });
                    connections.Add(new ConnectionData(transition.Identifier, transition.TransitionToIdentifier));
                }
                converted.Add(oldNode, node);
            }


            // Grab all the connections
            ConvertSubsystems(converted, connections);
            ConvertSceneManagers(converted, connections);
            ConvertElements(converted, connections);
            ConvertStateMachines(converted, connections);
            ConvertViews(converted, connections);
            foreach (var item in connections)
            {
                Debug.Log(item.OutputIdentifier + " -> " + item.InputIdentifier);
            }
            // Reconstruct the filters


        }
    }

    private void ConvertElements(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
    {
        
    }
    private void ConvertStateMachines(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
    {

    }
    private void ConvertViews(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
    {

    }
    private void ConvertSubsystems(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
    {
        foreach (var o in converted.Keys.OfType<SubSystemData>())
        {
            var n = converted[o] as SubsystemNode;
            // Convert Instances
            foreach (var instance in o.Instances)
            {
                var nInstance = new InstancesReference()
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
                var nInstance = new InstancesReference()
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
                var nInstance = new InstancesReference()
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
                var nInstance = new InstancesReference()
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

