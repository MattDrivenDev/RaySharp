using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public class Map
{
    public readonly Int32[,] World = 
    {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,0,0,1,0,0,0,0,0,0,0,0,0,1},
        {1,0,1,1,0,1,0,1,0,0,1,0,0,0,0,1},
        {1,0,1,1,0,1,1,1,0,0,1,0,0,0,0,1},
        {1,0,0,0,0,0,0,0,0,0,1,0,0,0,0,1},
        {1,0,1,1,0,1,1,1,0,1,1,1,1,0,1,1},
        {1,0,1,1,0,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,0,0,1,0,1,0,1,1,0,0,0,0,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
    };

    public void Draw(SpriteBatch spriteBatch)
    {
        var rows = World.GetLength(0);
        var cols = World.GetLength(1);

        for (var y = 0; y < rows; y++)
        {
            for (var x = 0; x < cols; x++)
            {
                if (World[y, x] == 1)
                {      
                    spriteBatch.DrawRectangle(new Rectangle(x * 100, y * 100, 100, 100), Color.DarkGray);
                }
            }
        }
    }
}