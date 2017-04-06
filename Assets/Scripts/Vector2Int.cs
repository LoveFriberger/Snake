using UnityEngine;

public struct Vector2Int
{
    public int x;
    public int y;

    public Vector2Int(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public static Vector2Int operator + (Vector2Int vector1, Vector2Int vector2)
    {
        return new Vector2Int(vector1.x + vector2.x, vector1.y + vector2.y);
    }

    public static Vector2Int operator - (Vector2Int vector1, Vector2Int vector2)
    {
        return new Vector2Int(vector1.x - vector2.x, vector1.y - vector2.y);
    }

    static public explicit operator Vector2(Vector2Int intVector)
    {
        return new Vector2(intVector.x, intVector.y);
    }
}
