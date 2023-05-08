using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace RaySharp;

public class ObjectRenderer
{
    public ObjectRenderer(ContentManager contentManager)
    {
        _contentManager = contentManager;
        _wallTextures = LoadWallTextures();
    }

    private readonly ContentManager _contentManager;
    private readonly Dictionary<Int32, Texture2D> _wallTextures;
    private readonly List<RenderableObject> _renderableObjects = new List<RenderableObject>();

    private Texture2D LoadTexture(String name) => _contentManager.Load<Texture2D>(name);

    private Dictionary<Int32, Texture2D> LoadWallTextures() =>
        new Dictionary<Int32, Texture2D>
        {
            { 1, LoadTexture("textures/1") },
            { 2, LoadTexture("textures/2") },
            { 3, LoadTexture("textures/3") },
            { 4, LoadTexture("textures/4") },
            { 5, LoadTexture("textures/5") }
        };

    public void AddObjectToRender(RenderableObject renderableObject)
    {
        _renderableObjects.Add(renderableObject);
    }

    public void ClearObjectsToRender()
    {
        _renderableObjects.Clear();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        // Oh my days! This took me a while to figure out.
        var backToFront = _renderableObjects.OrderBy(x => x.Depth).Reverse();

        foreach (var renderableObject in backToFront)
        {
            var texture = _wallTextures[renderableObject.TextureIndex];
            var rgb = 1 / (1 + MathF.Pow(renderableObject.Depth, 5) * 0.00002f);
            var color = new Color(rgb, rgb, rgb);

            spriteBatch.Draw(
                texture,
                renderableObject.Destination,
                renderableObject.Source,
                color);
        }
    }
}

public struct RenderableObject
{
    public RenderableObject(Single depth, Rectangle source, Rectangle destination, Int32 textureIndex)
    {
        Depth = depth;
        Source = source;
        Destination = destination;
        TextureIndex = textureIndex;
    }

    public Single Depth { get; init; }
    public Rectangle Source { get; init; }
    public Rectangle Destination { get; init; }
    public Int32 TextureIndex { get; init; }
}