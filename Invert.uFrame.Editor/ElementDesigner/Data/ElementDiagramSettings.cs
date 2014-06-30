using System;
using Invert.uFrame.Editor;
using UnityEngine;

[Serializable]
public class ElementDiagramSettings : IJsonObject
{
    public void Serialize(JSONClass cls)
    {
        cls.Add("SnapSize", new JSONData(_snapSize));
        cls.Add("CodePathStrategyName", new JSONData(_codePathStrategyName));
        cls.Add("AssociationLinkColor",SerializeColor(_associationLinkColor));
        cls.Add("DefinitionLinkColor", SerializeColor(_definitionLinkColor));
        cls.Add("InheritanceLinkColor", SerializeColor(_inheritanceLinkColor));
        cls.Add("SceneManagerLinkColor", SerializeColor(_sceneManagerLinkColor));
        cls.Add("SubSystemLinkColor", SerializeColor(_subSystemLinkColor));
        cls.Add("TransitionLinkColor", SerializeColor(_transitionLinkColor));
        cls.Add("ViewLinkColor", SerializeColor(_viewLinkColor));
    }

    public JSONNode SerializeColor(Color color)
    {
        var cls = new JSONClass
        {
            {"r", new JSONData(color.r)},
            {"g", new JSONData(color.g)},
            {"b", new JSONData(color.b)},
            {"a", new JSONData(color.a)}
        };
        return cls;
    }
    public Color DeserializeColor(JSONNode color)
    {
        return new Color(color["r"].AsFloat, color["g"].AsFloat, color["b"].AsFloat, color["a"].AsFloat);
    }
    public void Deserialize(JSONClass cls)
    {
     
    }

    [SerializeField]
    private Color _associationLinkColor = Color.white;
    [SerializeField,HideInInspector]
    private Color _definitionLinkColor = Color.cyan; 
    [SerializeField]
    private Color _inheritanceLinkColor = Color.green;
    [SerializeField]
    private Color _sceneManagerLinkColor = Color.gray;
    [SerializeField]
    private Color _subSystemLinkColor = Color.grey;

    [SerializeField, HideInInspector]
    private string _codePathStrategyName = "Default";
    [SerializeField]
    private Color _transitionLinkColor = Color.yellow;
    [SerializeField]
    private Color _viewLinkColor = Color.blue;
    [SerializeField]
    private int _snapSize = 10;

    [NonSerialized]
    private ICodePathStrategy _codePathStrategy;

    public Color AssociationLinkColor
    {
        get { return _associationLinkColor; }
        set { _associationLinkColor = value; }
    }

    public Color DefinitionLinkColor
    {
        get { return _definitionLinkColor; }
        set { _definitionLinkColor = value; }
    }

    public Color InheritanceLinkColor
    {
        get { return _inheritanceLinkColor; }
        set { _inheritanceLinkColor = value; }
    }

    public Color SceneManagerLinkColor
    {
        get { return _sceneManagerLinkColor; }
        set { _sceneManagerLinkColor = value; }
    }

    public Color SubSystemLinkColor
    {
        get { return _subSystemLinkColor; }
        set { _subSystemLinkColor = value; }
    }

    public Color TransitionLinkColor
    {
        get { return _transitionLinkColor; }
        set { _transitionLinkColor = value; }
    }

    public Color ViewLinkColor
    {
        get { return _viewLinkColor; }
        set { _viewLinkColor = value; }
    }

    public int SnapSize
    {
        get { return _snapSize; }
        set { _snapSize = value; }
    }

    public string CodePathStrategyName
    {
        get { return string.IsNullOrEmpty(_codePathStrategyName) ? "Default" : _codePathStrategyName; }
        set { _codePathStrategyName = value; }
    }

    public ICodePathStrategy CodePathStrategy
    {
        get { return _codePathStrategy; }
        set { _codePathStrategy = value; }
    }

    public JSONNode Serialize()
    {
        return new JSONClass();
    }

    
}