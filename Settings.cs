using System;
using Microsoft.Xna.Framework;

namespace RaySharp;

/// <summary>
/// Hardcoded (for now) settings for the game.
/// </summary>
public class Settings
{
    public static Int32 WIDTH = 1600;
    public static Int32 HALF_WIDTH = WIDTH / 2;
    public static Int32 HEIGHT = 900;
    public static Int32 HALF_HEIGHT = HEIGHT / 2;
    public static Int32 TARGET_FPS = 60;
    public static Single PLAYER_STARTING_X = 1.5f;
    public static Single PLAYER_STARTING_Y = 1.5f;
    public static Single PLAYER_STARTING_ROTATION = 0;
    public static Single PLAYER_SPEED = 2.5f;
    public static Single PLAYER_ROTATION_SPEED = 0.1f;
    public static Int32 PLAYER_SCALE = 10;
    public static Single FOV = MathF.PI / 3;
    public static Single HALF_FOV = FOV / 2;
    public static Int32 RAY_COUNT = WIDTH / 2;
    public static Int32 HALF_RAY_COUNT = RAY_COUNT / 2;
    public static Single DELTA_ANGLE = FOV / RAY_COUNT;
    public static Int32 MAX_DEPTH = 20;
    public static Boolean DrawPlayerDirection = false;
    public static Single SCREEN_DISTANCE = HALF_WIDTH / MathF.Tan(HALF_FOV);
    public static Int32 SCALE = WIDTH / RAY_COUNT;
    public static Int32 TEXTURE_SIZE = 256;
    public static Int32 HALF_TEXTURE_SIZE = TEXTURE_SIZE / 2;
    public static Single MOUSE_SENSITIVITY = 0.3f;
    public static Int32 MOUSE_MAX_RELATIVE_X = 40;
    public static Int32 MOUSE_BORDER_LEFT = 100;
    public static Int32 MOUSE_BORDER_RIGHT = WIDTH - MOUSE_BORDER_LEFT;
    public static Color FLOOR_COLOR = new Color(30, 30, 30);
}