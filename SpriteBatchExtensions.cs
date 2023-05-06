using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public static class SpriteBatchExtensions
{
    private static Texture2D _texture;

    private static void EnsureTexture(SpriteBatch spriteBatch)
    {
        if (_texture == null) 
        {
            _texture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            _texture.SetData(new[] {Color.White});
        }
    }

    public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, Single thickness)
    {
        var edge = end - start;
        var rotation = (Single)Math.Atan2(edge.Y, edge.X);
        var lineScale = new Vector2(edge.Length(), thickness);

        spriteBatch.Draw(_texture, start, null, color, rotation, Vector2.Zero, lineScale, SpriteEffects.None, 0);
    }

    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        EnsureTexture(spriteBatch);
        spriteBatch.Draw(_texture, rectangle, color);
    }
}