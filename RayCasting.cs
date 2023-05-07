using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public struct Ray
{
    public Ray(Double sin, Double cos, Double depth)
    {
        Sin = sin;
        Cos = cos;
        Depth = depth;
    }

    public Double Sin { get; init; }
    public Double Cos { get; init; }
    public Double Depth { get; init; }
}

public class RayCasting
{
    public RayCasting(Map map, Player player)
    {
        _map = map;
        _player = player;
    }

    private readonly Map _map;
    private readonly Player _player;
    private readonly Ray[] _rays = new Ray[Settings.RayCount];

    private void RayCast()
    {
        var originX = _player.X;
        var originY = _player.Y;
        var originMapX = _player.MapX;
        var originMapY = _player.MapY;        
        var rayAngle = _player.Rotation - Settings.PlayerFOVOver2 + 0.0001f;
        
        for (var i = 0; i < Settings.RayCount; i++)
        {
            var sin = Math.Sin(rayAngle);
            var cos = Math.Cos(rayAngle);
            var horzDepth = 0d;
            var vertDepth = 0d;
            var depth = 0d;

            {   // Horizontal Calculations                
                var (horzY, dy) = sin > 0 ? (originMapY + 1, 1) : (originMapY - 0.000001f, -1);
                horzDepth = (horzY - originY) / sin;
                var horzX = originX + horzDepth * cos;
                var deltaDepth = dy / sin;
                var dx = deltaDepth * cos;
                for (var j = 0; j < Settings.MaxDepth; j++)
                {
                    var horzTileX = Math.Clamp((Int32)horzX, 0, _map.World.GetLength(1) - 1);
                    var horzTileY = Math.Clamp((Int32)horzY, 0, _map.World.GetLength(0) - 1);

                    if (_map.World[horzTileY, horzTileX] == 1) // Hit a wall
                    {
                        break;
                    }

                    horzX += dx;
                    horzY += dy;
                    horzDepth += deltaDepth;
                }
            }

            {   // Vertical Calculations                
                var (vertX, dx) = cos > 0 ? (originMapX + 1, 1) : (originMapX - 0.000001f, -1);
                vertDepth = (vertX - originX) / cos;
                var vertY = originY + vertDepth * sin;
                var deltaDepth = dx / cos;
                var dy = deltaDepth * sin;
                for (var j = 0; j < Settings.MaxDepth; j++)
                {
                    
                    var vertTileX = Math.Clamp((Int32)vertX, 0, _map.World.GetLength(1) - 1);
                    var vertTileY = Math.Clamp((Int32)vertY, 0, _map.World.GetLength(0) - 1);
                    if (_map.World[vertTileY, vertTileX] == 1) // Hit a wall
                    {
                        break;
                    }
                    
                    vertX += dx;
                    vertY += dy;
                    vertDepth += deltaDepth;
                }
            }
            
            depth = vertDepth < horzDepth ? vertDepth : horzDepth;
            _rays[i] = new Ray(sin, cos, depth);

            rayAngle += Settings.DeltaFOV;
        }
    }

    public void Update(GameTime gameTime)
    {
        RayCast();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        var originX = _player.X;
        var originY = _player.Y;

        for (var i = 0; i < Settings.RayCount; i++)
        {
            var ray = _rays[i];
            var projectionHeight = Settings.ScreenDistance / (ray.Depth + 0.0001f);
            var color = Color.White * (Single)(1 - ray.Depth / Settings.MaxDepth);

            spriteBatch.DrawRectangle(
                new Rectangle(
                    i * (Int32)Settings.TileScale, 
                    (Int32)(Settings.HalfHeight - projectionHeight / 2), 
                    (Int32)Settings.TileScale, 
                    (Int32)projectionHeight), 
                color);
        }
    }
}