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

    public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
    {
        EnsureTexture(spriteBatch);
        spriteBatch.Draw(_texture, rectangle, color);
    }
}