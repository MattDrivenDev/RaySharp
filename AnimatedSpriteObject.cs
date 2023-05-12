using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RaySharp;

public class AnimatedSpriteObject : SpriteObject
{
    public AnimatedSpriteObject(
        Texture2D texture, 
        Single x, Single y, 
        Int32 frameWidth, Int32 frameHeight,
        Player player, 
        ObjectRenderer objectRenderer,
        Single scale, Single shift,
        Single animationTime)
        : base(texture, x, y, frameWidth, frameHeight, player, objectRenderer, scale, shift)
    {
        _animationTime = animationTime;
        _frames = new LinkedList<Rectangle>();
        _frames.AddLast(new Rectangle(0 * frameWidth, 0, frameWidth, frameHeight));
        _frames.AddLast(new Rectangle(1 * frameWidth, 0, frameWidth, frameHeight));
        _frames.AddLast(new Rectangle(2 * frameWidth, 0, frameWidth, frameHeight));
        _frames.AddLast(new Rectangle(3 * frameWidth, 0, frameWidth, frameHeight));
    }

    private readonly Single _animationTime;
    private Single _prevAnimationTime;
    private Boolean _animate;
    private LinkedList<Rectangle> _frames;

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        CheckAnimationTime(gameTime);
        
        Animate();
    }

    private void Animate()
    {
        if (_animate)
        {
            var frame = _frames.First;
            _frames.RemoveFirst();
            _frames.AddLast(frame);

            SetSource(frame.Value);
        }
    }

    private void CheckAnimationTime(GameTime gameTime)
    {
        _animate = false;
        _prevAnimationTime += (Single)gameTime.ElapsedGameTime.TotalSeconds;

        if (_prevAnimationTime > _animationTime)
        {
            _animate = true;
            _prevAnimationTime = 0;
        }
    }
}