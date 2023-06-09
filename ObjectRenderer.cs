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
        _skyTexture = LoadTexture("textures/sky");
    }

    private readonly ContentManager _contentManager;
    private readonly Dictionary<Int32, Texture2D> _wallTextures;
    private readonly Texture2D _skyTexture;
    private readonly List<RenderableObject> _renderableObjects = new List<RenderableObject>();
    private Int32 _skyOffset = 0;
    private Int32 _playerRelativeMovement = 0;

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

    public void SetPlayerRelativeMovement(Int32 playerRelativeMovement)
    {
        _playerRelativeMovement = playerRelativeMovement;
    }

    public void ClearObjectsToRender()
    {
        _renderableObjects.Clear();
    }

    public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
    {
        RenderSky(spriteBatch);
        RenderFloor(spriteBatch);
        RenderWalls(spriteBatch);   
    }

    private void RenderFloor(SpriteBatch spriteBatch)
    {
        spriteBatch.DrawRectangle(
            new Rectangle(0, Settings.HALF_HEIGHT, Settings.WIDTH, Settings.HALF_HEIGHT),
            Settings.FLOOR_COLOR);
    }

    private void RenderSky(SpriteBatch spriteBatch)
    {
        _skyOffset = (Int32)(_skyOffset + 4.5f * _playerRelativeMovement) % Settings.WIDTH;
        spriteBatch.Draw(
            _skyTexture,
            new Rectangle(-_skyOffset - Settings.WIDTH, 0, Settings.WIDTH, Settings.HALF_HEIGHT),
            Color.White);
        spriteBatch.Draw(
            _skyTexture,
            new Rectangle(-_skyOffset, 0, Settings.WIDTH, Settings.HALF_HEIGHT),
            Color.White);
        spriteBatch.Draw(
            _skyTexture,
            new Rectangle(-_skyOffset + Settings.WIDTH, 0, Settings.WIDTH, Settings.HALF_HEIGHT),
            Color.White);
    }

    private void RenderWalls(SpriteBatch spriteBatch)
    {
        // Oh my days! This took me a while to figure out.
        var backToFront = _renderableObjects.OrderByDescending(x => x.Depth);

        foreach (var renderableObject in backToFront)
        {
            if (renderableObject.Texture != null)
            {
                DrawObject(spriteBatch, renderableObject);
            }
            else
            {
                DrawWall(spriteBatch, renderableObject);
            }
        }
    }

    private void DrawObject(SpriteBatch spriteBatch, RenderableObject renderable)
    {
        var rgb = 1 / (1 + MathF.Pow(renderable.Depth, 5) * 0.00002f);
        var color = new Color(rgb, rgb, rgb);

        spriteBatch.Draw(
            renderable.Texture,
            renderable.Destination,
            renderable.Source,
            color);

        // Console.WriteLine(renderable.Destination);
        // spriteBatch.DrawRectangle(
        //     renderable.Destination,
        //     Color.White);
    }

    private void DrawWall(SpriteBatch spriteBatch, RenderableObject renderable)
    {        
        var rgb = 1 / (1 + MathF.Pow(renderable.Depth, 5) * 0.00002f);
        var color = new Color(rgb, rgb, rgb);

        spriteBatch.Draw(
            _wallTextures[renderable.TextureIndex],
            renderable.Destination,
            renderable.Source,
            color);
    }
}