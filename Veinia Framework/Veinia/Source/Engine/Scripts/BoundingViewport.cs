using Apos.Camera;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace VeiniaFramework
{
	public class BoundingViewport : IVirtualViewport
	{
		public BoundingViewport(GraphicsDevice graphicsDevice, GameWindow window, float targetWidth, float targetHeight)
		{
			_graphicsDevice = graphicsDevice;
			_window = window;

			TargetWidth = targetWidth;
			TargetHeight = targetHeight;

			window.ClientSizeChanged += OnClientSizeChanged;
			OnClientSizeChanged(this, EventArgs.Empty);
		}

		public void Dispose() => _window.ClientSizeChanged -= OnClientSizeChanged;

		public int X => _viewport.X;
		public int Y => _viewport.Y;
		public int Width => _viewport.Width;
		public int Height => _viewport.Height;

		public Vector2 XY => new Vector2(X, Y);

		public Vector2 Origin => _origin;

		public float TargetWidth { get; set; }
		public float TargetHeight { get; set; }

		public float VirtualWidth => _virtualWidth;
		public float VirtualHeight => _virtualHeight;
		public float aspectRatio { get; set; }

		public Matrix GetScaleMatrix()
		{
			return Matrix.CreateScale(
						  (float)(_graphicsDevice.Viewport.Width / TargetWidth),
						  (float)(_graphicsDevice.Viewport.Width / TargetWidth),
						  1f);
		}

		public void Set()
		{
			_oldViewport = _graphicsDevice.Viewport;
			_graphicsDevice.Viewport = _viewport;
		}
		public void Reset() => _graphicsDevice.Viewport = _oldViewport;

		public Matrix Transform(Matrix view)
		{
			//return view* GetScaleMatrix();
			return view * Matrix.CreateScale(_viewport.Width / TargetWidth, _viewport.Width / TargetWidth, 1);
		}

		public void OnClientSizeChanged(object sender, EventArgs e)
		{
			_virtualWidth = TargetWidth;
			_virtualHeight = TargetHeight;

			aspectRatio = (float)(_virtualWidth / _virtualHeight);


			// figure out the largest area that fits in this resolution at the desired aspect ratio
			int width = _graphicsDevice.PresentationParameters.BackBufferWidth;
			int height = (int)(width / aspectRatio + .5f);

			if (height > _graphicsDevice.PresentationParameters.BackBufferHeight)
			{
				height = _graphicsDevice.PresentationParameters.BackBufferHeight;
				width = (int)(height * aspectRatio + .5f);
			}

			_viewport.X = (_graphicsDevice.PresentationParameters.BackBufferWidth / 2) - (width / 2);
			_viewport.Y = (_graphicsDevice.PresentationParameters.BackBufferHeight / 2) - (height / 2);
			_viewport.Width = width;
			_viewport.Height = height;

			_origin = new Vector2(_virtualWidth / 2f, _virtualHeight / 2f);
		}

		private GraphicsDevice _graphicsDevice;
		private GameWindow _window;

		private float _virtualWidth;
		private float _virtualHeight;

		private Viewport _viewport;
		private Viewport _oldViewport;

		private Vector2 _origin;
	}
}