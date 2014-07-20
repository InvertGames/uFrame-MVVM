//using System;
//using System.Collections;
//using System.Collections.Generic;

//public class UFJsonSerializer : ISerializer
//{
//    private JSONClass _rootNode = new JSONClass();
//    private Stack<JSONNode> _nodeStack;

//    public JSONClass RootNode
//    {
//        get { return _rootNode; }
//        set { _rootNode = value; }
//    }

//    public UFJsonSerializer(string json)
//    {
//        RootNode = JSON.Parse(json).AsObject;
//    }

//    public UFJsonSerializer()
//    {

//    }

//    public void WriteArray(IUFSerializable obj)
//    {
//        var jsonNode = new JSONArray();

//    }

//    public Stack<JSONNode> NodeStack
//    {
//        get { return _nodeStack ?? (_nodeStack = new Stack<JSONNode>()); }
//        set { _nodeStack = value; }
//    }

//    public JSONNode CurrentNode
//    {
//        get
//        {
//            if (NodeStack.Count < 1)
//                return RootNode;
//            return NodeStack.Peek();
//        }
//    }

//    public void WriteObject(IUFSerializable obj)
//    {
//        NodeStack.Push(new JSONClass());
//        //jsonStream.SerializeString("CLRType",obj.GetType().AssemblyQualifiedName);
//        obj.Write(jsonStream);
//        RootNode.Add(obj.GetType().AssemblyQualifiedName, jsonNode);
//    }


//    public object ReadObject<T>(ISerializerStream stream)
//    {
        
//    }

//    public void SerializeField<T>(string name, T obj)
//    {
//        throw new NotImplementedException();
//    }

//    public object ReadField(string name)
//    {
//        throw new NotImplementedException();
//    }

//    public T ReadField<T>(string name)
//    {
//        throw new NotImplementedException();
//    }
//}