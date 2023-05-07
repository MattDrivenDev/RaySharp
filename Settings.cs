using System;

namespace RaySharp;

/// <summary>
/// Hardcoded (for now) settings for the game.
/// </summary>
public class Settings
{
    public static Int32 Width = 1600;
    public static Int32 HalfWidth = Width / 2;
    public static Int32 Height = 900;
    public static Int32 HalfHeight = Height / 2;
    public static Int32 TargetFPS = 60;
    public static Single PlayerStartingX = 1.5f;
    public static Single PlayerStartingY = 1.5f;
    public static Single PlayerStartingAngle = 0;
    public static Single PlayerSpeed = 2.5f;
    public static Single PlayerRotationSpeed = 0.1f;
    public static Int32 PlayerScale = 10;
    public static Single PlayerFOV = MathF.PI / 3;
    public static Single PlayerFOVOver2 = PlayerFOV / 2;
    public static Int32 RayCount = Width / 2;
    public static Int32 RayCountOver2 = RayCount / 2;
    public static Single DeltaFOV = PlayerFOV / RayCount;
    public static Int32 MaxDepth = 20;
    public static Boolean DrawPlayerDirection = false;
    public static Single ScreenDistance = HalfWidth / MathF.Tan(PlayerFOVOver2);
    public static Single TileScale = Width / RayCount;
}