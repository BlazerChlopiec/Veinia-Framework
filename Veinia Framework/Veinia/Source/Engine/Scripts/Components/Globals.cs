using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Collisions;
using MonoGame.Extended.Tweening;
using MonoGame.Extended.ViewportAdapters;
using Myra.Graphics2D.UI;

namespace VeiniaFramework
{
	public class Globals
	{
		public static Input input = new Input();
		public static Tweener tweener = new Tweener();
		public static Tweener unscaledTweener = new Tweener();
		public static Loader loader;
		public static GraphicsDeviceManager graphicsManager;
		public static GraphicsDevice graphicsDevice;
		public static ContentManager content;
		public static Screen screen;
		public static FPS fps;
		public static OrthographicCamera camera;
		public static CollisionComponent collisionComponent;
		public static GameWindow window;
		public static Desktop myraDesktop;
		public static ViewportAdapter viewportAdapter;
	}
}