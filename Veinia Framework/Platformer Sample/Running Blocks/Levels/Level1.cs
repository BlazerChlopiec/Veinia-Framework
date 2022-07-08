using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Veinia.RunningBlocks
{
	public class Level1 : Level
	{
		public Level1(string name, PrefabManager prefabManager) : base(name, prefabManager)
		{
		}

		public override void LoadContents()
		{
			base.LoadContents();

			GameObject leftBorder = Instantiate(
				new Transform(-Vector2.UnitX * 9.7f),
				new List<Component>
				{
				new RectanglePhysics(Vector2.Zero, new Vector2(.3f, 10))
				}, isStatic: true);
			GameObject rightBorder = Instantiate(
				new Transform(Vector2.UnitX * 9.7f),
				new List<Component>
				{
				new RectanglePhysics(Vector2.Zero, new Vector2(.3f, 10))
				}, isStatic: true);
			GameObject topBorder = Instantiate(
				new Transform(Vector2.UnitY * 5.5f),
				new List<Component>
				{
				new RectanglePhysics(Vector2.Zero, new Vector2(19f, .3f))
				}, isStatic: true);
			GameObject bottomBorder = Instantiate(
				new Transform(Vector2.UnitY * -5.5f),
				new List<Component>
				{
				new RectanglePhysics(Vector2.Zero, new Vector2(19f, .3f)),
				}, isStatic: true);


			Instantiate(new Transform(Vector2.Zero), prefabManager.Find("Block"));
			Instantiate(new Transform(Vector2.UnitY), prefabManager.Find("Block"));
			Instantiate(new Transform(Vector2.UnitY * 2), prefabManager.Find("Block"));
			Instantiate(new Transform(Vector2.UnitY * 3), prefabManager.Find("Block"));

			// DOESNT SUPPORT MULTIPLE RECTANGLES FOR NOW
			Instantiate(new Transform(Vector2.Zero), new List<Component>
			{
				new FollowMouse(),
				new RectanglePhysics(Vector2.Zero, Vector2.One),
				//new RectanglePhysics(Vector2.One, Vector2.One),
				//new RectanglePhysics(Vector2.One*-1, Vector2.One),
			}, isStatic: false);
		}
	}
}
