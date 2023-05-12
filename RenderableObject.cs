using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public struct RenderableObject
{
    public RenderableObject(Single depth, Rectangle source, Rectangle destination, Int32 textureIndex)
    {
        Depth = depth;
        Source = source;
        Destination = destination;
        TextureIndex = textureIndex;
        Texture = null;
    }

    public RenderableObject(Single depth, Rectangle source, Rectangle destination, Texture2D texture)
    {
        Depth = depth;
        Texture = texture;
        Source = source;
        Destination = destination;
        TextureIndex = 0;
    }

    public Single Depth { get; init; }
    public Rectangle Source { get; init; }
    public Rectangle Destination { get; init; }
    public Int32 TextureIndex { get; init; }
    public Texture2D Texture { get; init; }
}