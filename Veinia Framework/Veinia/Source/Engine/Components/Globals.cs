using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tweening;
using MonoGame.Extended.ViewportAdapters;
using Myra.Graphics2D.UI;

namespace Veinia
{
	public class Globals
	{
		public static Input input = new Input();
		public static Loader loader = new Loader();
		public static Tweener tweener = new Tweener();
		public static GraphicsDeviceManager graphicsManager;
		public static GraphicsDevice graphicsDevice;
		public static ContentManager content;
		public static Screen screen;
		public static FPS fps;
		public static OrthographicCamera camera;
		public static CollisionComponent collisionComponent;
		public static GameWindow window;
		public static Desktop desktop;
		public static ViewportAdapter viewportAdapter;
	}
}