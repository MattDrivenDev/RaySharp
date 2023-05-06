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

    public RaySharp()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
        _graphics.PreferredBackBufferWidth = Settings.Width;
        _graphics.PreferredBackBufferHeight = Settings.Height;
        _graphics.IsFullScreen = true;
        IsFixedTimeStep = true;
        TargetElapsedTime = TimeSpan.FromSeconds(1d / Settings.TargetFPS);
        _graphics.ApplyChanges();
    }

    protected override void Initialize()
    {
        _map = new Map();
        _player = new Player();
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
        
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        var fps = 1 / gameTime.ElapsedGameTime.TotalSeconds;
        
        Window.Title = $"RaySharp - FPS: {fps:0.00}";
        
        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin();

        _map.Draw(_spriteBatch);

        _player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
