using Humper.Responses;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

public class CustomCollider : ColliderTemplate
{
	private Vector2 size;
	private Vector2 unitOffset;

	public CustomCollider(CollisionResponses type, Vector2 offset, Vector2 size) : base(type)
	{
		//this.size = Transform.ToScreenUnits(size);
		//unitOffset = Transform.ToScreenUnits(offset);
		this.size = Transform.ToScreenUnits(size);
		unitOffset = Transform.ToScreenUnits(offset);
	}

	public override void Initialize()
	{
		offset = new Vector2(-((size.X - Transform.PIXELS_PER_UNIT) / 2) + unitOffset.X, -((size.Y - Transform.PIXELS_PER_UNIT) / 2) + unitOffset.Y);
		firstFrameBox = new RectangleF(transform.screenPos.X + offset.X, transform.screenPos.Y + offset.Y, size.X, size.Y);
		base.Initialize();
	}
}
