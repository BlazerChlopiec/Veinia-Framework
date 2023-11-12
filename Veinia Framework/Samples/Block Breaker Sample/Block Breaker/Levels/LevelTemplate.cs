using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework.Samples.BlockBreaker
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
				}, isStatic: true);
			leftBorder.body = Globals.physicsWorld.CreateRectangle(.3f, 10, 1f, bodyType: BodyType.Static);

			GameObject rightBorder = Instantiate(
				new Transform(Vector2.UnitX * 9.7f),
				new List<Component>
				{
				}, isStatic: true);
			rightBorder.body = Globals.physicsWorld.CreateRectangle(.3f, 10, 1f, bodyType: BodyType.Static);

			GameObject topBorder = Instantiate(
				new Transform(Vector2.UnitY * 5.5f),
				new List<Component>
				{
				}, isStatic: true);
			topBorder.body = Globals.physicsWorld.CreateRectangle(19f, .3f, 1f, bodyType: BodyType.Static);

			GameObject bottomBorder = Instantiate(
				new Transform(Vector2.UnitY * -5.5f),
				new List<Component>
				{
					new GameOverBorder(),
				}, isStatic: true);
			bottomBorder.body = Globals.physicsWorld.CreateRectangle(19f, .3f, 1f, bodyType: BodyType.Static);



			//LEVEL
			GameObject ball = Instantiate(
				new Transform(-Vector2.UnitY * 10),
				new List<Component>
				{
					new Sprite("Sprites/Ball", .1f, Color.Blue, pixelsPerUnit: 120),
					new Ball(),
				}, isStatic: false);
			GameObject paddle = Instantiate(
				new Transform(Vector2.Zero),
				new List<Component>
				{
					new Sprite("Sprites/Paddle", .1f, Color.Blue, pixelsPerUnit: 100),
					new Paddle(),
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