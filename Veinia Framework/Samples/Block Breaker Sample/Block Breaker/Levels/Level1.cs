using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.BlockBreaker
{
	public class Level1 : Level
	{
		public Level1(PrefabManager prefabManager) : base(prefabManager)
		{
		}

		public Level1(PrefabManager prefabManager, string editorLevelName) : base(prefabManager, editorLevelName)
		{
		}

		public override void LoadContents()
		{
			base.LoadContents();


			GameObject leftBorder = Instantiate(
				new Transform(-Vector2.UnitX * 9.7f),
				new List<Component>
				{
				new RectangleCollider(Vector2.Zero, new Vector2(.3f, 10))
				}, isStatic: true);
			GameObject rightBorder = Instantiate(
				new Transform(Vector2.UnitX * 9.7f),
				new List<Component>
				{
				new RectangleCollider(Vector2.Zero, new Vector2(.3f, 10))
				}, isStatic: true);
			GameObject topBorder = Instantiate(
				new Transform(Vector2.UnitY * 5.5f),
				new List<Component>
				{
				new RectangleCollider(Vector2.Zero, new Vector2(19f, .3f)),
				}, isStatic: true);
			GameObject bottomBorder = Instantiate(
				new Transform(Vector2.UnitY * -5.5f),
				new List<Component>
				{
				new GameOverBorder(),
				new RectangleCollider(Vector2.Zero, new Vector2(19f, .3f)),
				}, isStatic: true);



			//LEVEL
			GameObject paddle = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
				new Sprite("Block Breaker/Paddle", .1f, Color.Blue),
				new Paddle(),
				new RectangleCollider(Vector2.Zero, new Vector2(2.5f, .85f), trigger: true)
				}, isStatic: false);

			GameObject ball = Instantiate(
				new Transform(Vector2.UnitY),
				new List<Component>
				{
				new Sprite("Block Breaker/Ball", .1f, Color.Blue, new Vector2(.8f, .8f)),
				new Ball(),
				new CircleCollider(Vector2.Zero, .4f),
				new Physics(),
				}, isStatic: false);
		}
	}
}