using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Invert.uFrame.Editor
{
    public interface IJsonObject
    {
        void Serialize(JSONClass cls);
        void Deserialize(JSONClass cls);

    }

    public static class JsonExtensions
    {
        public static void AddObject(this JSONClass cls, string name, IJsonObject jsonObject) 
        {
            cls.Add(name, SerializeObject(jsonObject));
        }
        public static IEnumerable<T> DeserializePrimitiveArray<T>(this JSONNode array, Func<JSONNode, T> deserialize)
        {
            if (array == null) yield break;
            foreach (JSONNode item in array.AsArray)
            {
                yield return deserialize(item);
            }
        }
        public static IEnumerable<T> DeserializePrimitiveArray<T>(this JSONArray array,Func<JSONNode,T> deserialize)
        {
            if (array == null) yield break;
            foreach (JSONNode item in array)
            {
                yield return deserialize(item);
            }
        }
        public static void AddPrimitiveArray<T>(this JSONClass cls,string name, IEnumerable<T> arr, Func<T,JSONNode> serializeItem)
        {
            var jsonArray = new JSONArray();
            foreach (var item in arr)
            {
                jsonArray.Add(serializeItem(item));
            }
            cls.Add(name,jsonArray);
        }
        public static void AddObjectArray<T>(this JSONClass cls,string name, IEnumerable<T> array) where T : IJsonObject
        {
            if (array == null) return;
            cls.Add(name, SerializeObjectArray(array));
        }
        public static JSONArray SerializeObjectArray<T>(this IEnumerable<T> array) where T : IJsonObject
        {
            var jsonArray = new JSONArray();
            foreach (var item in array)
            {
                jsonArray.Add(item.SerializeObject());
            }
            return jsonArray;
        }
        public static JSONClass SerializeObject(this IJsonObject obj)
        {
            var cls = new JSONClass() { { "_CLRType", obj.GetType().Name } };
            obj.Serialize(cls);
            return cls;
        }
        public static IEnumerable<T> DeserializeObjectArray<T>(this JSONNode array)
        {
            return array.AsArray.DeserializeObjectArray<T>();
        } 
        public static IEnumerable<T> DeserializeObjectArray<T>(this JSONArray array)
        {
            foreach (JSONNode item in array)
            {
                var obj = DeserializeObject(item);
                if (obj != null)
                yield return (T)obj;
            }
        } 

        public static IJsonObject DeserializeObject(this JSONNode node)
        {
            if (node == null) return null;
            var clrTypeString = node["_CLRType"].Value;
            if (string.IsNullOrEmpty(clrTypeString))
            {
                return null;
            }
            var clrType = uFrameEditor.FindType(clrTypeString);
            if (clrType == null)
            throw new Exception("Could not find type " + clrTypeString);

            var obj = Activator.CreateInstance(clrType) as IJsonObject;
            if (obj != null)
            {
                obj.Deserialize(node as JSONClass);
                return obj;
            }
            
            throw new Exception("Type must be of type IJsonObject" + clrTypeString);

        }
    }
}
