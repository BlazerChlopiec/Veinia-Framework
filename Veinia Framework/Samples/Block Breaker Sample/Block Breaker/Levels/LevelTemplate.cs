using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VeiniaFramework.BlockBreaker
{
	public class LevelTemplate : Level
	{
		public LevelTemplate(string levelPath) : base(levelPath)
		{
		}

		public LevelTemplate() : base()
		{
		}

		public override void CreateScene(bool loadObjectsFromPath = true)
		{
			base.CreateScene();


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
			GameObject ball = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
					new Sprite("Sprites/Ball", .1f, Color.Blue, pixelsPerUnit: 120),
					new Ball(),
					new CircleCollider(Vector2.Zero, .4f),
					new Physics(gravity: 0, removeVelocityBasedOnCollision: false),
				}, isStatic: false);
			GameObject paddle = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
					new Sprite("Sprites/Paddle", .1f, Color.Blue, pixelsPerUnit: 100),
					new Paddle(),
					new RectangleCollider(Vector2.Zero, new Vector2(2.5f, .85f))
				}, isStatic: false);

			GameObject UI = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
					new UI()
				}, isStatic: true);
		}

		public override void Unload()
		{
			base.Unload();

			Time.stop = false;
		}
	}
}