using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;

public class Globals
{
	public static GraphicsDeviceManager graphicsManager;
	public static GraphicsDevice graphicsDevice;
	public static ContentManager content;
	public static Screen screen;
	public static Input input;
	public static Loader loader;
	public static FPS fps;
	public static OrthographicCamera camera;
	public static CollisionComponent collisionComponent;
}