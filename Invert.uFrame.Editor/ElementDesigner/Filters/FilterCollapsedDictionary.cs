using System;
using System.Collections.Generic;
using Invert.uFrame.Editor;

[Serializable]
public class FilterCollapsedDictionary : FilterDictionary<bool>
{
    protected override JSONNode SerializeValue(bool value)
    {
        return new JSONData(value);
    }

    protected override bool DeserializeValue(JSONNode value)
    {
        return value.AsBool;
    }
}


public class FlagsDictionary : Dictionary<string,bool>, IJsonObject
{


    public void Serialize(JSONClass cls)
    {
        foreach (var item in this)
        {
            cls.Add(item.Key,new JSONData(item.Value));
        }
    }

    public void Deserialize(JSONClass cls)
    {
        this.Clear();
        foreach (KeyValuePair<string,JSONNode> jsonNode in cls)
        {
            this.Add(jsonNode.Key,jsonNode.Value.AsBool);
        }
    }
}