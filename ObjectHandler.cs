using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public class ObjectHandler
{
    public ObjectHandler(
        ContentManager contentManager,
        Player player,
        ObjectRenderer objectRenderer)
    {
        _contentManager = contentManager;
        _player = player;
        _objectRenderer = objectRenderer;

        var spriteTexture = _contentManager.Load<Texture2D>("sprites/static_sprites/candlebra");
        var animatedSpriteTexture = _contentManager.Load<Texture2D>("sprites/animated_sprites/green_light/all");

        AddSprite(new SpriteObject(
            spriteTexture, 
            5, Settings.PLAYER_STARTING_Y + 0.0f, 
            spriteTexture.Width, spriteTexture.Height,
            _player, 
            _objectRenderer, 
            0.7f, 0.27f));

        AddSprite(new SpriteObject(
            spriteTexture, 
            6.5f, Settings.PLAYER_STARTING_Y + 1.5f, 
            spriteTexture.Width, spriteTexture.Height,
            _player, 
            _objectRenderer, 
            0.7f, 0.27f));

        AddSprite(new SpriteObject(
            spriteTexture, 
            8, Settings.PLAYER_STARTING_Y + 3.0f, 
            spriteTexture.Width, spriteTexture.Height,
            _player, 
            _objectRenderer, 
            0.7f, 0.27f));
        
        
        AddSprite(new AnimatedSpriteObject(
            animatedSpriteTexture, 
            10, Settings.PLAYER_STARTING_Y + 4.5f, 
            animatedSpriteTexture.Width / 4, animatedSpriteTexture.Height,
            _player, 
            _objectRenderer, 
            0.8f, 0.15f, 
            0.120f));
    }

    private readonly ContentManager _contentManager;
    private readonly Player _player;
    private readonly ObjectRenderer _objectRenderer;
    private readonly List<SpriteObject> _spriteObjects = new List<SpriteObject>();

    public void Update(GameTime gameTime)
    {
        foreach (var spriteObject in _spriteObjects)
        {
            spriteObject.Update(gameTime);
        }
    }

    private void AddSprite(SpriteObject spriteObject)
    {
        _spriteObjects.Add(spriteObject);
    }
}