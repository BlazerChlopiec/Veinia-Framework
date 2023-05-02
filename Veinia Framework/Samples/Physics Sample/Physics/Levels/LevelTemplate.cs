﻿using Microsoft.Xna.Framework;
using System.Collections.Generic;
using tainicom.Aether.Physics2D.Dynamics;

namespace VeiniaFramework.Samples.Physics
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
				}, isStatic: true);
			bottomBorder.body = Globals.physicsWorld.CreateRectangle(19f, .3f, 1f, bodyType: BodyType.Static);


			Instantiate(new Transform(Vector2.Zero), new List<Component>
			{
				new Sprite("Sprites/Square", 0, Color.Green, pixelsPerUnit: 200),
				new PhysicsMovement()
			}, isStatic: false);
		}
	}
}
