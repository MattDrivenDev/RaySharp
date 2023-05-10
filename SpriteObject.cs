using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public class SpriteObject
{
    public SpriteObject(
        Texture2D texture,
        Single x, Single y,
        Player player,
        ObjectRenderer objectRenderer)
    {
        _texture = texture;
        _objectRenderer = objectRenderer;
        _player = player;
        _x = x;
        _y = y;
    }

    private Int32 _width => _texture.Width;
    private Int32 _halfWidth => _width / 2;
    private Int32 _height => _texture.Height;
    private Single _ratio => (Single)_width / (Single)_height;
    private Single _x;
    private Single _y;
    private Int32 _screenX;
    private Int32 _screenY;
    private readonly ObjectRenderer _objectRenderer;
    private readonly Texture2D _texture;
    private readonly Player _player;
    private Single _distance;
    private Single _normalizedDistance;
    private Single _shiftHeight => 0.27f;

    public void Update(GameTime gameTime)
    {
        GetSprite();
    }

    public void GetSprite()
    {
        // Get the delta between the player and the sprite
        var dx = _x - _player.X;
        var dy = _y - _player.Y;

        // Calculate the theta
        var theta = Math.Atan2(dy, dx);

        // Calculate the delta between the player's rotation and the theta
        var delta = theta - _player.Rotation;
        if ((dx > 0 && _player.Rotation > Math.PI)
            || (dx < 0 && dy < 0))
        {
            delta += Math.Tau;
        }   
        if (delta > Math.PI)
        {
            delta -= Math.Tau;
        }

        var deltaRays = delta / Settings.DELTA_ANGLE;
        _screenX = (Int32)(Settings.HALF_RAY_COUNT + deltaRays) * Settings.SCALE;

        // Calculate the distance between the player and the sprite using
        // the hypotenuse
        _distance = MathF.Sqrt(MathF.Pow(dx, 2) + MathF.Pow(dy, 2));
        _normalizedDistance = (Single)(_distance * Math.Cos(delta));

        if (-_halfWidth < _screenX 
            && _screenX < (Settings.WIDTH + _halfWidth) 
            && _normalizedDistance > 0.5f)
        {
            GetSpriteProjection();
        }
    }

    private void GetSpriteProjection()
    {
        var projection = Settings.SCREEN_DISTANCE / _normalizedDistance * 0.7f; // half scale
        var projectionWidth = projection * _ratio;
        var projectionHeight = projection;
        var halfSpriteWidth = projectionWidth / 2;
        var heightShift = (Single)projectionHeight * (Single)_shiftHeight;
        var px = _screenX - halfSpriteWidth;
        var py = (Settings.HALF_HEIGHT) - (projectionHeight / 2) + heightShift;
        
        var renderable = new RenderableObject(
            _normalizedDistance,
            _texture,
            new Rectangle((Int32)px, (Int32)py, (Int32)projectionWidth, (Int32)projectionHeight));

        _objectRenderer.AddObjectToRender(renderable);
    }
}