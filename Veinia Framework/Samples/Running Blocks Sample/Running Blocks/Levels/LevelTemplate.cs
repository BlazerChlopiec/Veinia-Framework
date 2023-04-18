using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VeiniaFramework.RunningBlocks
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

			Collider.showHitboxes = true;

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
				}, isStatic: true);


			Instantiate(new Transform(Vector2.Zero), new List<Component>
			{
				new TopDownMovement(),
				new RectangleCollider(Vector2.One, Vector2.One),
				new RectangleCollider(Vector2.Zero, Vector2.One),
				new RectangleCollider(Vector2.One * -1, Vector2.One),
				new Physics(gravity: 0),
			}, isStatic: false);

			Instantiate(new Transform(Vector2.Zero), new List<Component>
			{
				new FollowMouse(),
				new CircleCollider(Vector2.Zero, .5f),
				new CircleCollider(Vector2.One, .5f),
				new CircleCollider(Vector2.One*-1, .5f),
			}, isStatic: false);
		}
	}
}
