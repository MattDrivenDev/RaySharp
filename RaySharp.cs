using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace RaySharp;

public class RaySharp : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Map _map;
    private Player _player;
    private RayCasting _rayCasting;
    private ObjectRenderer _objectRenderer;
    private SpriteObject[] _spriteObjects = new SpriteObject[3];

    public RaySharp()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = false;
        _graphics.PreferredBackBufferWidth = Settings.WIDTH;
        _graphics.PreferredBackBufferHeight = Settings.HEIGHT;
        _graphics.IsFullScreen = false;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / Settings.TARGET_FPS);
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        var spriteTexture = Content.Load<Texture2D>("sprites/static_sprites/candlebra");

        _map = new Map();
        _objectRenderer = new ObjectRenderer(Content);
        _player = new Player(_map, _objectRenderer);
        _rayCasting = new RayCasting(_map, _player, _objectRenderer);
        
        _spriteObjects[0] = new SpriteObject(spriteTexture, 5, Settings.PLAYER_STARTING_Y + 0.0f, _player, _objectRenderer);
        _spriteObjects[1] = new SpriteObject(spriteTexture, 6.5f, Settings.PLAYER_STARTING_Y + 1.5f, _player, _objectRenderer);
        _spriteObjects[2] = new SpriteObject(spriteTexture, 8, Settings.PLAYER_STARTING_Y + 3.0f, _player, _objectRenderer);
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
    }
    
    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed 
            || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        _player.Update(gameTime);
        _rayCasting.Update(gameTime);
        
        foreach (var spriteObject in _spriteObjects)
        {
            spriteObject.Update(gameTime);
        }
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        
        Window.Title = $"RaySharp - FPS: {fps:0.00}";
        
        //GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _map.Draw(_spriteBatch);

        _player.Draw(_spriteBatch);

        _rayCasting.Draw(_spriteBatch);

        _objectRenderer.Draw(_spriteBatch, gameTime);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
