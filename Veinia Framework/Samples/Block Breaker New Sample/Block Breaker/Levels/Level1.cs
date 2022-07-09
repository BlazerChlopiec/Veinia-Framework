using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.BlockBreaker
{
	public class Level1 : Level
	{
		public Level1(PrefabManager prefabManager) : base(prefabManager)
		{
		}

		public override void LoadContents()
		{
			base.LoadContents();

			GameObject background = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
				new Sprite("Block Breaker/Background", 0, Color.White, new Vector2(16*1.2f,9*1.2f)),
				}, isStatic: true);

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
				new RectangleCollider(Vector2.Zero, new Vector2(19f, .3f))
				}, isStatic: true);
			GameObject bottomBorder = Instantiate(
				new Transform(Vector2.UnitY * -5.5f),
				new List<Component>
				{
				new RectangleCollider(Vector2.Zero, new Vector2(19f, .3f)),
				new GameOverBorder()
				}, isStatic: true);



			//LEVEL
			GameObject paddle = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
				new Sprite("Block Breaker/Paddle", 0, Color.Blue),
				new Paddle(),
				new RectangleCollider(Vector2.Zero, new Vector2(2.5f, .85f), trigger: true)
				}, isStatic: false);

			GameObject ball = Instantiate(
				new Transform(Vector2.UnitY),
				new List<Component>
				{
				new Sprite("Block Breaker/Ball", 0, Color.Blue, new Vector2(.8f, .8f)),
				new Ball(),
				new Physics(),
				new CircleCollider(Vector2.Zero, .4f),
				}, isStatic: false);


			//block rows
			for (int i = 0; i < 11; i++)
			{
				Instantiate(new Transform(i - 5, 0), prefabManager.Find("Block"));
			}
			for (int i = 0; i < 11; i++)
			{
				Instantiate(new Transform(i - 5, 1), prefabManager.Find("Block"));
			}
			for (int i = 0; i < 11; i++)
			{
				Instantiate(new Transform(i - 5, 2), prefabManager.Find("Block"));
			}
		}
	}
}