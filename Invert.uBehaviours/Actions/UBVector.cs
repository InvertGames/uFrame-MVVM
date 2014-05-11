using UnityEngine;

namespace UBehaviours.Actions
{
    public static class UBVector
    {
        public static float GetVector2X(Vector2 vector)
        {
            return vector.x;
        }
        public static float GetVector2Y(Vector2 vector)
        {
            return vector.y;
        }
        public static float GetVector3X(Vector2 vector)
        {
            return vector.x;
        }
        public static float GetVector3Y(Vector2 vector)
        {
            return vector.y;
        }
        public static float GetVector3Z(Vector3 vector)
        {
            return vector.z;
        }

        public static Vector3 CreateVector3(float x, float y, float z)
        {
            return new Vector3(x,y,z);
        }
        public static Vector2 CreateVector2(float x, float y)
        {
            return new Vector2(x, y);
        }
    }
}