//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using Invert.Core.GraphDesigner;
//using Invert.uFrame.MVVM;
//using UnityEditor;
//using UnityEngine;


//[CustomEditor(typeof(ElementsGraph), true)]
//public class GraphDataInspector : Editor
//{
//    public override void OnInspectorGUI()
//    {

//        if (GUILayout.Button("Upgrade Format"))
//        {
//            DoConvert();
//        }
//    }

//    private void DoConvert()
//    {
//        var oldGraph = target as IGraphData;

//        IGraphData graphData = null;
//        var graph = ScriptableObject.CreateInstance<UnityGraphData>();
//        if (oldGraph is ExternalStateMachineGraph)
//        {
//            graph.Graph = new StateMachineGraph();
//        }
//        else if (oldGraph is ExternalSubsystemGraph)
//        {
//            graph.Graph = new StateMachineGraph();
//        }
//        else
//        {
//            graph.Graph = new MVVMGraph();
//        }


//        graphData = graph;
//        graph.name = oldGraph.Name;
//        graph.Identifier = oldGraph.Identifier;
//        graph.Graph.Identifier = oldGraph.Identifier;

//        // Convert all the nodes
//        Dictionary<DiagramNode, DiagramNode> converted = new Dictionary<DiagramNode, DiagramNode>();
//        List<ConnectionData> connections = new List<ConnectionData>();
//        foreach (var oldNode in oldGraph.NodeItems.OfType<SceneManagerData>())
//        {
//            var node = new SceneTypeNode
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            //foreach (var item in oldNode.Transitions)
//            //{
//            //    node.ChildItems.Add(new SceneTransitionsReference()
//            //    {
//            //        Node = node,
//            //        Identifier = item.Identifier,
//            //        SourceIdentifier = item.CommandIdentifier,
//            //    });
//            //    if (string.IsNullOrEmpty(item.ToIdentifier)) continue;
//            //    connections.Add(new ConnectionData(item.Identifier, item.ToIdentifier));
//            //}
//            converted.Add(oldNode, node);
//        }
//        foreach (var oldNode in oldGraph.NodeItems.OfType<SubSystemData>())
//        {
//            var node = new SubsystemNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            converted.Add(oldNode, node);
//        }
//        foreach (var oldNode in oldGraph.NodeItems.OfType<ComputedPropertyData>())
//        {
//            var node = new ComputedPropertyNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name,
//                PropertyType = oldNode.RelatedType
//            };

//            foreach (var item in oldNode.DependantProperties)
//            {
//                connections.Add(new ConnectionData(item.Identifier, node.Identifier));
//            }

//            foreach (var x in oldNode.DependantNodes)
//            {
//                foreach (var item in x.AllProperties)
//                {
//                    if (x[item.Identifier])
//                    {
//                        node.ChildItems.Add(new SubPropertiesReference()
//                        {
//                            SourceIdentifier = item.Identifier,
//                            Node = node,
//                        });
//                    }
//                }
//            }

//            converted.Add(oldNode, node);
//        }
//        foreach (var oldNode in oldGraph.NodeItems.OfType<ElementData>())
//        {
//            var node = new ElementNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name,
//            };
//            if (!string.IsNullOrEmpty(oldNode.BaseIdentifier))
//            {
//                connections.Add(new ConnectionData(oldNode.BaseIdentifier, oldNode.Identifier));
//            }
//            foreach (var item in oldNode.Properties)
//            {
//                node.ChildItems.Add(new PropertiesChildItem()
//                {
//                    Identifier = item.Identifier,
//                    Node = node,
//                    Name = item.Name,
//                    RelatedType = item.RelatedType
//                });
//            }
//            foreach (var item in oldNode.Collections)
//            {
//                node.ChildItems.Add(new CollectionsChildItem()
//                {
//                    Identifier = item.Identifier,
//                    Name = item.Name,
//                    Node = node,
//                    RelatedType = item.RelatedType
//                });
//            }
//            foreach (var item in oldNode.Commands)
//            {
//                node.ChildItems.Add(new CommandsChildItem()
//                {
//                    Identifier = item.Identifier,
//                    Name = item.Name,
//                    Node = node,
//                    RelatedType = item.RelatedType
//                });

//                if (!string.IsNullOrEmpty(item.TransitionToIdentifier))
//                    connections.Add(new ConnectionData(item.Identifier, item.TransitionToIdentifier));
//            }
//            converted.Add(oldNode, node);
//        }
//        foreach (var oldNode in oldGraph.NodeItems.OfType<ClassNodeData>())
//        {
//            var node = new ElementNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name,
//            };
//            if (!string.IsNullOrEmpty(oldNode.BaseIdentifier))
//            {
//                connections.Add(new ConnectionData(oldNode.BaseIdentifier, oldNode.Identifier));
//            }
//            foreach (var item in oldNode.Properties)
//            {
//                node.ChildItems.Add(new PropertiesChildItem()
//                {
//                    Identifier = item.Identifier,
//                    Node = node,
//                    Name = item.Name,
//                    RelatedType = item.RelatedType
//                });
//            }
//            foreach (var item in oldNode.Collections)
//            {
//                node.ChildItems.Add(new CollectionsChildItem()
//                {
//                    Identifier = item.Identifier,
//                    Name = item.Name,
//                    Node = node,
//                    RelatedType = item.RelatedType
//                });
//            }

//            converted.Add(oldNode, node);
//        }
//        foreach (var oldNode in oldGraph.NodeItems.OfType<ViewData>())
//        {
//            var node = new ViewNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            // TODO CONVERT INHERITANCE
//            // Connect the scene property
//            foreach (var sceneProperty in oldNode.SceneProperties)
//            {
//                connections.Add(new ConnectionData(sceneProperty.Identifier, node.ScenePropertiesInputSlot.Identifier));
//            }

//            // TODO CONVERT BINDINGS

//            converted.Add(oldNode, node);
//        }

//        foreach (var oldNode in oldGraph.NodeItems.OfType<ViewComponentData>())
//        {
//            var node = new ViewComponentNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            // TODO CONVERT INHERITANCE
//            converted.Add(oldNode, node);
//        }

//        foreach (var oldNode in oldGraph.NodeItems.OfType<EnumData>())
//        {
//            var node = new EnumNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            foreach (var item in oldNode.EnumItems)
//            {
//                node.ChildItems.Add(new EnumChildItem()
//                {
//                    Identifier = item.Identifier,
//                    Node = node,
//                    Name = item.Name
//                });
//            }
//            converted.Add(oldNode, node);
//            //Debug.Log(string.Format("Converted {0}", oldNode.Name));
//        }

//        foreach (var oldNode in oldGraph.NodeItems.OfType<StateMachineNodeData>())
//        {
//            var node = new StateMachineNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            if (oldNode.StartState != null)
//            {
//                connections.Add(new ConnectionData(node.StartStateOutputSlot.Identifier, oldNode.StartState.Identifier));
//            }


//            foreach (var transition in oldNode.Transitions)
//            {
//                node.ChildItems.Add(new TransitionsChildItem()
//                {
//                    Name = transition.Name,
//                    Identifier = transition.Identifier,
//                    Node = node,
//                });
//                connections.Add(new ConnectionData(transition.PropertyIdentifier, transition.Identifier));
//                //connections.Add();
//            }
//            connections.Add(new ConnectionData(oldNode.StatePropertyIdentifier, oldNode.Identifier));
//            converted.Add(oldNode, node);
//        }

//        foreach (var oldNode in oldGraph.NodeItems.OfType<StateMachineStateData>())
//        {
//            var node = new StateNode()
//            {
//                Identifier = oldNode.Identifier,
//                Name = oldNode.Name
//            };
//            foreach (var transition in oldNode.Transitions)
//            {
//                node.ChildItems.Add(new StateTransitionsReference()
//                {
//                    Name = transition.Name,
//                    Identifier = transition.Identifier,
//                    Node = node,
//                });
//                connections.Add(new ConnectionData(transition.Identifier, transition.TransitionToIdentifier));
//            }

//            converted.Add(oldNode, node);
//        }


//        // Grab all the connections
//        ConvertSubsystems(converted, connections);
//        ConvertSceneManagers(converted, connections);
//        ConvertElements(converted, connections);
//        ConvertStateMachines(converted, connections);
//        ConvertViews(converted, connections);
//        foreach (var item in converted.Values)
//        {
//            graphData.AddNode(item);
//        }


//        foreach (var item in connections)
//        {
//            if (item == null) continue;
//            if (item.OutputIdentifier == item.InputIdentifier)
//            {
//                continue;
//            }
//            graphData.AddConnection(item.OutputIdentifier, item.InputIdentifier);
//            Debug.Log(string.Format("Added connection {0} - {1}", item.OutputIdentifier, item.InputIdentifier));
//        }
//        // Reconstruct the filters
//        var oldElementGraph = oldGraph as IGraphData;
//        if (oldElementGraph != null)
//        {
//            foreach (var node in converted.Keys)
//            {
//                var newNOde = converted[node];

//                if (oldGraph.PositionData.HasPosition(oldGraph.RootFilter, node))
//                {
//                    graph.SetItemLocation(newNOde, oldGraph.GetItemLocation(node));
//                }
//            }


//            foreach (var item in oldElementGraph.PositionData.Positions)
//            {
//                graph.PositionData.Positions.Add(item.Key, item.Value);
//            }
//        }


//        AssetDatabase.CreateAsset(graph, AssetDatabase.GetAssetPath(Selection.activeObject).Replace(".asset", "-new.asset"));
//        AssetDatabase.SaveAssets();
//    }

//    private void ConvertElements(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
//    {

//    }
//    private void ConvertStateMachines(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
//    {

//    }
//    private void ConvertViews(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
//    {
//        var dictionary = new Dictionary<string, string>()
//        {
//            {"StateMachinePropertyBindingGenerator", "BindStateProperty"},
//            {"StandardPropertyBindingGenerator", "BindProperty"},
//            {"DefaultCollectionBindingGenerator", "BindCollection"},
//            {"ViewCollectionBindingGenerator", "BindToViewCollection"},
//            {"CommandExecutedBindingGenerator", "BindCommandExecuted"},
//            {"ComputedPropertyBindingGenerator", "BindProperty"},

//        };
//        foreach (var old in converted.Keys.OfType<ViewData>())
//        {
//            var newItem = converted[old] as ViewNode;
//            if (newItem == null) continue;
//            connections.Add(new ConnectionData(old.ViewForElement.Identifier, newItem.ElementInputSlot.Identifier)
//            {
//                Output = converted.Values.FirstOrDefault(p => p.Identifier == old.ViewForElement.Identifier),
//                Input = newItem,
//            });
//            foreach (var item in old.SceneProperties)
//            {
//                connections.Add(new ConnectionData(item.Identifier, newItem.ScenePropertiesInputSlot.Identifier));
//            }
//            foreach (var binding in old.Bindings)
//            {
//                if (!dictionary.ContainsKey(binding.GeneratorType))
//                {
//                    Debug.Log(string.Format("Couldn't convert binding {0}", binding.GeneratorType));
//                    continue;
//                }
//                var bindingType = dictionary[binding.GeneratorType];

//                var newBinding = new BindingsReference()
//                {
//                    Node = newItem,
//                    Identifier = binding.Identifier,
//                    SourceIdentifier = binding.PropertyIdentifier,
//                    BindingName = bindingType
//                };

//                newItem.ChildItems.Add(newBinding);
//            }

//        }
//    }
//    private void ConvertSubsystems(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
//    {
//        foreach (var o in converted.Keys.OfType<SubSystemData>())
//        {
//            var n = converted[o] as SubsystemNode;
//            // Convert Instances
//            foreach (var instance in o.Instances)
//            {
//                var nInstance = new InstancesReference()
//                {
//                    Node = n,
//                    Name = instance.Name,
//                    SourceIdentifier = instance.RelatedType
//                };

//                n.ChildItems.Add(nInstance);
//            }
//            // Convert connections
//            //connections.AddRange(GetSubsystemConnections(converted, o, n));
//        }

//    }
//    private void ConvertSceneManagers(Dictionary<DiagramNode, DiagramNode> converted, List<ConnectionData> connections)
//    {
//        //foreach (var o in converted.Keys.OfType<SceneManagerData>())
//        //{
//        //    var n = converted[o] as SceneManagerNode;
//        //    var subsystemIdentifier = o.SubSystemIdentifier;
//        //    //var subsystem = converted.Values.OfType<SubsystemNode>().FirstOrDefault(p => p.Identifier == subsystemIdentifier);
//        //    //if (subsystem != null)
//        //    //{
//        //    //    connections.Add(
//        //    //        new ConnectionData(subsystem.ExportOutputSlot.Identifier, n.SubsystemInputSlot.Identifier)
//        //    //        {
//        //    //            Input = n.SubsystemInputSlot,
//        //    //            Output = subsystem.ExportOutputSlot
//        //    //        });
//        //    //}
//        //    //foreach (var item in o.Transitions)
//        //    //{
//        //    //    var newTransition = new SceneTransitionsReference
//        //    //    {
//        //    //        Node = n,
//        //    //        Identifier = item.Identifier,
//        //    //        SourceIdentifier = item.CommandIdentifier
//        //    //    };
//        //    //    n.ChildItems.Add(newTransition);

//        //    //    connections.Add(new ConnectionData(item.Identifier, item.ToIdentifier)
//        //    //    {
//        //    //        Output = newTransition,

//        //    //    });
//        //    //}

//        //}



//    }

//    //private IEnumerable<ConnectionData> GetSubsystemConnections(Dictionary<DiagramNode, DiagramNode> converted, SubSystemData o, SubsystemNode n)
//    //{
//    //    //foreach (var item in o.Imports)
//    //    //{
//    //    //    var oldImport = converted.Keys.FirstOrDefault(p => p.Identifier == item);
//    //    //    if (oldImport == null) continue;
//    //    //    var newImport = converted[oldImport] as SubsystemNode;

//    //    //    //yield return new ConnectionData(newImport.ExportOutputSlot.Identifier, n.ImportInputSlot.Identifier)
//    //    //    //{
//    //    //    //    Output = newImport.ExportOutputSlot,
//    //    //    //    Input = n.ImportInputSlot
//    //    //    //};
//    //    //}
//    //}
//}

