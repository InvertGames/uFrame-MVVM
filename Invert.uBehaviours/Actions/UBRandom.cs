using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Random = System.Random;

namespace UBehaviours.Actions
{
    public class UBRandom
    {
        public static Quaternion RandomRotation(Vector3 min, Vector3 max)
        {
            return Quaternion.Euler(RandomVector3(min, max));
        }

        public static Vector3 RandomVector2(Vector2 min, Vector2 max)
        {
            return new Vector2(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y)
                );
        }

        public static Vector3 RandomVector3(Vector3 min, Vector3 max)
        {
            return new Vector3(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z)
                );
        }

        public static Vector4 RandomVector4(Vector4 min, Vector4 max)
        {
            return new Vector4(
                UnityEngine.Random.Range(min.x, max.x),
                UnityEngine.Random.Range(min.y, max.y),
                UnityEngine.Random.Range(min.z, max.z),
                UnityEngine.Random.Range(min.z, max.w)
                );
        }
    }
}