using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Invert.Core.GraphDesigner;

namespace uFrame.MVVM.Templates
{

    public partial class SimpleClassTemplate
    {
        //Types accepted for the serialization and their serialized identifiers
        public static Dictionary<Type, string> AcceptableTypes = new Dictionary<Type, string>
        {
            {typeof (int), "Int"},
            {typeof (Vector3), "Vector3"},
            {typeof (Vector2), "Vector2"},
            {typeof (string), "String"},
            {typeof (bool), "Bool"},
            {typeof (float), "Float"},
            {typeof (double), "Double"},
            {typeof (Quaternion), "Quaternion"},
        };

        [GenerateMethod] //Generate method based on the content
        public string Serialize() //The name and return type will be copied to the result method
        {
            //Simple output the class. Semicolon (;) is added automatically
            Ctx._("var jsonObject = new JSONClass()");

            //For each is not a pure output. It is actually invoked during generating!!!
            //Here we iterate over properties of the simple class
            foreach (var viewModelPropertyData in Ctx.Data.Properties)
            {
                //Get node of the property if any
                var relatedNode = viewModelPropertyData.RelatedTypeNode;
                
                //If enum node
                if (relatedNode is EnumNode)
                {
                    //Formatted output. Check the generated code to see how it looks like
                    Ctx._("jsonObject.Add(\"{0}\", new JSONData((int)this.{0}));", viewModelPropertyData.Name);
                }
                else 
                {
                    if (viewModelPropertyData.Type == null) continue;
                    if (!AcceptableTypes.ContainsKey(viewModelPropertyData.Type)) continue;
                    Ctx._("jsonObject.Add(\"{0}\", new JSONData(this.{0}))",
                        viewModelPropertyData.Name);
                }

            }
            Ctx._("return jsonObject.ToString()");
            return null;
        }

        [GenerateMethod]
        public void Deserialize(string json)
        {
            Ctx._("var node = JSON.Parse(json)");
            foreach (var simpleClassPropertyData in Ctx.Data.Properties)
            {

                var relatedNode = simpleClassPropertyData.RelatedTypeNode;
                if (relatedNode is EnumNode)
                {
                    Ctx._("this.{0} = ({1})node[\"{0}\"].AsInt", simpleClassPropertyData.Name,
                        simpleClassPropertyData.RelatedTypeName);
                }
                else
                {
                    if (simpleClassPropertyData.Type == null) continue;
                    if (!AcceptableTypes.ContainsKey(simpleClassPropertyData.Type)) continue;
                    Ctx.PushStatements(Ctx._if("node[\"{0}\"] != null", simpleClassPropertyData.Name).TrueStatements);
                    Ctx._("this.{0} = node[\"{0}\"].As{1}", simpleClassPropertyData.Name,
                        AcceptableTypes[simpleClassPropertyData.Type]);
                    Ctx.PopStatements();
                }
            }
        }


    }
}