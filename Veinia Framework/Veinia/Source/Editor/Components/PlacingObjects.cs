using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Veinia.Editor
{
	public class PlacingObjects : Component
	{
		PrefabManager prefabs;

		GameObject preview;


		public PlacingObjects(PrefabManager prefabs)
		{
			this.prefabs = prefabs;
		}

		public override void Initialize()
		{
			preview = Instantiate(Transform.Empty, new List<Component>
			{
				new Sprite("Block Breaker/Grass Tile",.99f, Color.White * .5f, Vector2.One)
			}, isStatic: false);
		}

		public override void Update()
		{
			ShowPreview();

			if (Globals.input.GetMouseButtonDown(0) && !Globals.input.GetKey(Keys.LeftAlt))
			{
				PlaceObject();
			}
		}

		private void ShowPreview()
		{
			var mousePos = Globals.input.GetMouseWorldPosition();

			if (!Globals.input.GetKey(Keys.LeftControl))
				mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			preview.transform.position = mousePos;
		}

		private void PlaceObject()
		{
			Title.Add("Object count " + parent.world.scene.Count, 7);

			var mousePos = Globals.input.GetMouseWorldPosition();

			if (!Globals.input.GetKey(Keys.LeftControl))
				mousePos = new Vector2(MathF.Round(mousePos.X), MathF.Round(mousePos.Y));

			var objectToSpawn = prefabs.Find("Block");
			var onlySprite = new GameObject(new Transform(mousePos), new List<Component>
			{
				objectToSpawn.GetComponent<Sprite>()
			}, isStatic: true);


			Instantiate(onlySprite);
		}
	}
}
