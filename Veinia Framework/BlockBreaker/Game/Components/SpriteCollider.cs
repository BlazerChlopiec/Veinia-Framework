using Humper.Responses;
using Microsoft.Xna.Framework;

public class SpriteCollider : ColliderTemplate
{
	Sprite sprite;

	public SpriteCollider(CollisionResponses type) : base(type)
	{
	}

	public override void Initialize()
	{
		sprite = GetComponent<Sprite>();
		offset = new Vector2(-((sprite.screenRect.Width - Transform.PIXELS_PER_UNIT) / 2), -((sprite.screenRect.Height - Transform.PIXELS_PER_UNIT) / 2));
		//firstFrameBox = new RectangleF(sprite.screenRect.X + offset.X, sprite.screenRect.Y + offset.Y, sprite.screenRect.Width, sprite.screenRect.Height);
		firstFrameBox = sprite.worldRect;

		base.Initialize();
	}
}
