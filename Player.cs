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
    private Single _x = Settings.PLAYER_STARTING_X;
    private Single _y = Settings.PLAYER_STARTING_Y;
    private Double _rotation = Settings.PLAYER_STARTING_ROTATION;

    public Single X => _x;
    public Single Y => _y;
    public Int32 MapX => (Int32)X;
    public Int32 MapY => (Int32)Y;
    public Double Rotation => _rotation;

    private void Move(GameTime gameTime, KeyboardState ks, MouseState ms)
    {
        var sin = Math.Sin(_rotation);
        var cos = Math.Cos(_rotation);
        var dx = 0f;
        var dy = 0f;
        var speed = Settings.PLAYER_SPEED * gameTime.ElapsedGameTime.TotalSeconds;
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
        MouseRotation(ms, gameTime);
    }

    private void MouseRotation(MouseState ms, GameTime gameTime)
    {
        var x = ms.X;
        if (x < Settings.MOUSE_BORDER_LEFT || X > Settings.MOUSE_BORDER_RIGHT)
        {
            Mouse.SetPosition(Settings.HALF_WIDTH, Settings.HALF_HEIGHT);
        }

        // Don't forget to flip the input because as the x lowers (i.e. moves closer
        // to the x=0) the rotation should increase (i.e. rotate counter-clockwise)
        var mouseDelta = -(Settings.HALF_WIDTH - x);

        // Ensure that the delta is within the bounds of the maximum per update.
        mouseDelta = Math.Max(-Settings.MOUSE_MAX_RELATIVE_X, Math.Min(Settings.MOUSE_MAX_RELATIVE_X, mouseDelta));
        
        // Get the delta and apply to the player's rotation.
        var rotationDelta = mouseDelta * Settings.MOUSE_SENSITIVITY * gameTime.ElapsedGameTime.TotalSeconds;
        _rotation += rotationDelta;
        
        // Set the mouse position back to the center of the screen ready for another update.
        Mouse.SetPosition(Settings.HALF_WIDTH, Settings.HALF_HEIGHT);
    }

    private Boolean CheckWall(Int32 x, Int32 y) => _map.World[y, x] >= 1;

    private void CheckWallCollision(Single dx, Single dy)
    {
        var x = (Int32)(_x + dx * Settings.PLAYER_SCALE);
        var y = (Int32)(_y + dy * Settings.PLAYER_SCALE);

        if (!CheckWall(x, y))
        {
            _x += dx;
            _y += dy;
        }
    }

    public void Update(GameTime gameTime)
    {
        Move(gameTime, Keyboard.GetState(), Mouse.GetState());
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (Settings.DrawPlayerDirection)
        {
            var x = (_x * 100);
            var y = (_y * 100);
            spriteBatch.DrawRectangle(
                new Rectangle(
                    (Int32)x - Settings.PLAYER_SCALE / 2, 
                    (Int32)y - Settings.PLAYER_SCALE / 2, 
                    Settings.PLAYER_SCALE, 
                    Settings.PLAYER_SCALE), 
                Color.Yellow);

            var start = new Vector2((Int32)x, (Int32)y);
            var end = new Vector2(
                (Int32)(x + Settings.WIDTH * Math.Cos(_rotation)), 
                (Int32)(y + Settings.HEIGHT * Math.Sin(_rotation)));
            spriteBatch.DrawLine(start, end, Color.Yellow, 1f);
        }
    }
}