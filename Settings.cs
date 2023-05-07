using System;

namespace RaySharp;

/// <summary>
/// Hardcoded (for now) settings for the game.
/// </summary>
public class Settings
{
    public const Int32 Width = 1600;
    public const Int32 Height = 900;
    public const Int32 TargetFPS = 60;
    public const Single PlayerStartingX = 1.5f;
    public const Single PlayerStartingY = 1.5f;
    public const Single PlayerStartingAngle = 0;
    public const Single PlayerSpeed = 4f;
    public const Single PlayerRotationSpeed = 0.1f;
    public const Int32 PlayerScale = 10;
    public const Single PlayerFOV = MathF.PI / 3;
    public const Single PlayerFOVOver2 = PlayerFOV / 2;
    public const Int32 RayCount = Width / 2;
    public const Int32 RayCountOver2 = RayCount / 2;
    public const Single DeltaFOV = PlayerFOV / RayCount;
    public const Int32 MaxDepth = 20;
    public const Boolean DrawPlayerDirection = false;
}