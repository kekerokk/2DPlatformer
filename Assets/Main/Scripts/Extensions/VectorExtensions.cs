
using UnityEngine;
public static class VectorExtensions {
    public static Vector3 WithX(this Vector3 vec, float val) => new(val,vec.y,vec.z);
    public static Vector3 WithY(this Vector3 vec, float val) => new(vec.x,val,vec.z);
    public static Vector3 WithZ(this Vector3 vec, float val) => new(vec.x,vec.y,val);
}
public static class ColorExtensions {
    public static Color WithA(this Color color, float val) => new Color(color.r, color.g, color.b, val);
}
