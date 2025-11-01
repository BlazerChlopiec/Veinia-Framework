using Microsoft.Xna.Framework;
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

			var leftBorder = Instantiate(
				new Transform { position = -Vector2.UnitX * 9.7f },
				new List<Component>
				{
					new PhysicsRectangle(.3f, 10f, bodyType: BodyType.Static)
				}, isStatic: true);

			var rightBorder = Instantiate(
				new Transform { position = Vector2.UnitX * 9.7f },
				new List<Component>
				{
					new PhysicsRectangle(.3f, 10, bodyType: BodyType.Static)
				}, isStatic: true);

			var topBorder = Instantiate(
				new Transform { position = Vector2.UnitY * 5.5f },
				new List<Component>
				{
					new PhysicsRectangle(19f, .3f, bodyType: BodyType.Static)
				}, isStatic: true);

			var bottomBorder = Instantiate(
				new Transform { position = Vector2.UnitY * -5.5f },
				new List<Component>
				{
					new PhysicsRectangle(19f, .3f, bodyType: BodyType.Static)
				}, isStatic: true);

			Instantiate(new Transform { Z = 0 }, new List<Component>
				{
					new Sprite("Sprites/Square", Color.Green, pixelsPerUnit: 200),
					new MovingSquare(),
					new PhysicsRectangle(bodyType: BodyType.Dynamic)
				}, isStatic: false);
		}
	}
}