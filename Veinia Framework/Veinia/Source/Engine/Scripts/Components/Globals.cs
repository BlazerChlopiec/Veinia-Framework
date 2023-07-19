using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tweening;
using MonoGame.Extended.ViewportAdapters;
using Myra.Graphics2D.UI;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework
{
	public class Globals
	{
		public static bool debugDraw;

		public static Input input = new Input();
		public static Tweener tweener = new Tweener();
		public static Tweener unscaledTweener = new Tweener();
		public static Loader loader;
		public static GraphicsDeviceManager graphicsManager;
		public static GraphicsDevice graphicsDevice;
		public static ContentManager content;
		public static Screen screen;
		public static FPS fps;
		public static Camera camera;
		public static World physicsWorld;
		public static GameWindow window;
		public static Desktop myraDesktop;
		public static ViewportAdapter viewportAdapter;
	}
}