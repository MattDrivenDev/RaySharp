using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public struct Ray
{
    public Ray(Double sin, Double cos, Double depth, Double rotation,
        Int32 texture, Double offset, Double projectionHeight)
    {
        Sin = sin;
        Cos = cos;
        Depth = depth;
        Rotation = rotation;
        Texture = texture;
        Offset = offset;
        ProjectionHeight = projectionHeight;
    }

    public Double Sin { get; init; }
    public Double Cos { get; init; }
    public Double Depth { get; init; }
    public Double Rotation { get; init; }
    public Int32 Texture { get; init; }
    public Double Offset { get; init; }
    public Double ProjectionHeight { get; init; }
}

public class RayCasting
{
    public RayCasting(Map map, Player player, ObjectRenderer objectRenderer)
    {
        _map = map;
        _player = player;
        _objectRenderer = objectRenderer;
    }

    private readonly Map _map;
    private readonly Player _player;
    private readonly ObjectRenderer _objectRenderer;
    private readonly Ray[] _rays = new Ray[Settings.RAY_COUNT];
    
    public void CreateObjectsToRender()
    {
        _objectRenderer.ClearObjectsToRender();

        for (var i = 0; i < _rays.Length; i++)
        {
            var ray = _rays[i];
            Rectangle source, destination;
            {
                var x = (Int32)(ray.Offset * (Settings.TEXTURE_SIZE - Settings.SCALE));
                var y = 0;
                var width = (Int32)Settings.SCALE;
                var height = (Int32)Settings.TEXTURE_SIZE;
                source = new Rectangle(x, y, width, height);
            }

            {
                var x = i * Settings.SCALE;
                var y = (Int32)(Settings.HALF_HEIGHT - ray.ProjectionHeight / 2);
                var width = (Int32)Settings.SCALE;
                var height = (Int32)ray.ProjectionHeight;
                destination = new Rectangle(x, y, width, height);
            }

            var renderableObject = new RenderableObject(
                (Single)ray.Depth,
                source,
                destination,
                ray.Texture);

            _objectRenderer.AddObjectToRender(renderableObject);
        }
    }

    private void RayCast()
    {
        var originX = _player.X;
        var originY = _player.Y;
        var originMapX = _player.MapX;
        var originMapY = _player.MapY;        
        var rayAngle = _player.Rotation - Settings.HALF_FOV + 0.0001f;
        
        for (var i = 0; i < Settings.RAY_COUNT; i++)
        {
            var sin = Math.Sin(rayAngle);
            var cos = Math.Cos(rayAngle);
            var horzDepth = 0d;
            var vertDepth = 0d;
            var depth = 0d;
            var vertTexture = 1;
            var horzTexture = 1;
            Double vy, hx = 0f;

            {   // Horizontal Calculations                
                var (horzY, dy) = sin > 0 ? (originMapY + 1, 1) : (originMapY - 0.000001f, -1);
                horzDepth = (horzY - originY) / sin;
                var horzX = originX + horzDepth * cos;
                var deltaDepth = dy / sin;
                var dx = deltaDepth * cos;
                for (var j = 0; j < Settings.MAX_DEPTH; j++)
                {
                    var horzTileX = Math.Clamp((Int32)horzX, 0, _map.World.GetLength(1) - 1);
                    var horzTileY = Math.Clamp((Int32)horzY, 0, _map.World.GetLength(0) - 1);

                    if (_map.World[horzTileY, horzTileX] >= 1) // Hit a wall
                    {
                        horzTexture = _map.World[horzTileY, horzTileX];
                        break;
                    }

                    horzX += dx;
                    horzY += dy;
                    horzDepth += deltaDepth;
                }
                hx = horzX;
            }

            {   // Vertical Calculations                
                var (vertX, dx) = cos > 0 ? (originMapX + 1, 1) : (originMapX - 0.000001f, -1);
                vertDepth = (vertX - originX) / cos;
                var vertY = originY + vertDepth * sin;
                var deltaDepth = dx / cos;
                var dy = deltaDepth * sin;
                for (var j = 0; j < Settings.MAX_DEPTH; j++)
                {
                    
                    var vertTileX = Math.Clamp((Int32)vertX, 0, _map.World.GetLength(1) - 1);
                    var vertTileY = Math.Clamp((Int32)vertY, 0, _map.World.GetLength(0) - 1);
                    if (_map.World[vertTileY, vertTileX] >= 1) // Hit a wall
                    {
                        vertTexture = _map.World[vertTileY, vertTileX];
                        break;
                    }
                    
                    vertX += dx;
                    vertY += dy;
                    vertDepth += deltaDepth;
                }
                vy = vertY;
            }
            
            // Calculate depth and texture offset
            var offset = 0d;
            var texture = 1;
            if (vertDepth < horzDepth)
            {
                depth = vertDepth;
                texture = vertTexture;
                vy %= 1;
                offset = cos > 0 ? vy : 1 - vy;
            }
            else
            {
                depth = horzDepth;
                texture = horzTexture;
                hx %= 1;
                offset = sin > 0 ? 1 - hx : hx;
            }

            // Fix fish eye effect
            depth *= Math.Cos(_player.Rotation - rayAngle);

            // Calculate Projection Height
            var projectionHeight = Settings.SCREEN_DISTANCE / (depth + 0.0001f);

            _rays[i] = new Ray(
                sin, 
                cos, 
                depth, 
                rayAngle,
                texture, 
                offset, 
                projectionHeight);

            rayAngle += Settings.DELTA_ANGLE;
        }
    }

    public void Update(GameTime gameTime)
    {
        RayCast();
        CreateObjectsToRender();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        // var originX = _player.X;
        // var originY = _player.Y;

        // for (var i = 0; i < Settings.RayCount; i++)
        // {
        //     var ray = _rays[i];
        //     var projectionHeight = Settings.ScreenDistance / (ray.Depth + 0.0001f);
        //     var color = Color.White * (Single)(1 - ray.Depth / Settings.MaxDepth);

        //     spriteBatch.DrawRectangle(
        //         new Rectangle(
        //             i * (Int32)Settings.TileScale, 
        //             (Int32)(Settings.HalfHeight - projectionHeight / 2), 
        //             (Int32)Settings.TileScale, 
        //             (Int32)projectionHeight), 
        //         color);
        // }
    }
}