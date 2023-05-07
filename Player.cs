using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RaySharp;

public class Player
{
    public Player(Map map)
    {
        _map = map;
    }

    private readonly Map _map;
    private Single _x = Settings.PlayerStartingX;
    private Single _y = Settings.PlayerStartingY;
    private Double _rotation = Settings.PlayerStartingAngle;

    public Single X => _x;
    public Single Y => _y;
    public Int32 MapX => (Int32)X;
    public Int32 MapY => (Int32)Y;
    public Double Rotation => _rotation;

    private void Move(GameTime gameTime, KeyboardState ks)
    {
        var sin = Math.Sin(_rotation);
        var cos = Math.Cos(_rotation);
        var dx = 0f;
        var dy = 0f;
        var speed = Settings.PlayerSpeed * gameTime.ElapsedGameTime.TotalSeconds;
        sin = speed * sin;
        cos = speed * cos;

        if (ks.IsKeyDown(Keys.W))
        {
            dx += (Single)cos;
            dy += (Single)sin;
        }

        if (ks.IsKeyDown(Keys.S))
        {
            dx -= (Single)cos;
            dy -= (Single)sin;
        }

        if (ks.IsKeyDown(Keys.A))
        {
            dx += (Single)sin;
            dy -= (Single)cos;
        }

        if (ks.IsKeyDown(Keys.D))
        {
            dx -= (Single)sin;
            dy += (Single)cos;
        }

        CheckWallCollision(dx, dy);

        if (ks.IsKeyDown(Keys.Left))
        {
            _rotation -= Settings.PlayerRotationSpeed;
        }

        if (ks.IsKeyDown(Keys.Right))
        {
            _rotation += Settings.PlayerRotationSpeed;
        }

        _rotation %= Math.Tau;
    }

    private Boolean CheckWall(Int32 x, Int32 y) => _map.World[y, x] == 1;

    private void CheckWallCollision(Single dx, Single dy)
    {
        var x = (Int32)(_x + dx);
        var y = (Int32)(_y + dy);

        if (!CheckWall(x, y))
        {
            _x += dx;
            _y += dy;
        }
    }

    public void Update(GameTime gameTime)
    {
        Move(gameTime, Keyboard.GetState());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Settings.DrawPlayerDirection)
        {
            var x = (_x * 100);
            var y = (_y * 100);
            spriteBatch.DrawRectangle(
                new Rectangle(
                    (Int32)x - Settings.PlayerScale / 2, 
                    (Int32)y - Settings.PlayerScale / 2, 
                    Settings.PlayerScale, 
                    Settings.PlayerScale), 
                Color.Yellow);

            var start = new Vector2((Int32)x, (Int32)y);
            var end = new Vector2(
                (Int32)(x + Settings.Width * Math.Cos(_rotation)), 
                (Int32)(y + Settings.Height * Math.Sin(_rotation)));
            spriteBatch.DrawLine(start, end, Color.Yellow, 1f);
        }
    }
}