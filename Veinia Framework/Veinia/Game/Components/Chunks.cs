using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System.Collections.Generic;

public class Chunks : Component, IDrawn
{
	Chunk currentChunk;

	Transform targetSpot;

	public void SetTarget(Transform target) => targetSpot = target;

	public override void Initialize()
	{
		SpawnNewChunk(new Vector2(0, 0));
	}

	private void SpawnNewChunk(Vector2 pos)
	{
		currentChunk = new Chunk(pos);
		Iterate();
	}

	public override void Update()
	{
		if (targetSpot == null) return;

		if (!currentChunk.colliderRect.Contains(Utils.Vector2ToPoint(Transform.WorldToScreenPos(targetSpot.position))))
		{
			SpawnNewChunk(targetSpot.position);
		}
	}

	public void Iterate()
	{
		//foreach (var collider in FindComponentsOfType<ColliderTemplate>())
		//{
		//	if (!collider.isStatic) continue;

		//	if (currentChunk.rect.Intersects(collider.rect) && !collider.parent.isEnabled)
		//	{
		//		collider.parent.isEnabled = true;
		//	}
		//	if (!currentChunk.rect.Intersects(collider.rect) && collider.parent.isEnabled)
		//	{
		//		collider.parent.isEnabled = false;
		//	}
		//}
	}

	public void Draw(SpriteBatch sb)
	{
		//drawn bounds
		sb.DrawRectangle(new RectangleF(currentChunk.rect.X, currentChunk.rect.Y, currentChunk.rect.Width, currentChunk.rect.Height),
					  Color.Purple, thickness: 10, layerDepth: 1);

		//collider bounds
		sb.DrawRectangle(new RectangleF(currentChunk.colliderRect.X, currentChunk.colliderRect.Y, currentChunk.colliderRect.Width, currentChunk.colliderRect.Height),
			  Color.Green, thickness: 10, layerDepth: 1);
	}

	public class Chunk
	{
		Vector2 pos;
		Vector2 colliderPos;
		Vector2 size = new Vector2(Globals.camera.BoundingRectangle.Width, Globals.camera.BoundingRectangle.Height) * 3f;
		public Chunk(Vector2 pos)
		{
			colliderPos = Transform.WorldToScreenPos(pos) - colliderSize / 2;
			this.pos = Transform.WorldToScreenPos(pos) - size / 2;
		}
		public RectangleF rect => new RectangleF(pos.X, pos.Y, size.X, size.Y);
		public RectangleF colliderRect => new RectangleF(colliderPos.X, colliderPos.Y, colliderSize.X, colliderSize.Y);
		public Vector2 colliderSize => size / 2.5f;
	}
}