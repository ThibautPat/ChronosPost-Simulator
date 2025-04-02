using UnityEngine;
public static class LayerExtension
{
    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }

    /// <summary>
    /// Extension method that returns a layer mask including all layers except the specified one
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static LayerMask Exclude(this LayerMask mask, int layer)
    {
        return ~(1 << layer);
    }
}

public static class SpriteExtension
{
    /// <summary>
    /// Extension method to convert a Texture2D to a Sprite
    /// </summary>
    /// <param name="texture"></param>
    /// <returns></returns>
    public static Sprite ConvertToSprite(this Texture2D texture)
    {
        return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
    }
}
