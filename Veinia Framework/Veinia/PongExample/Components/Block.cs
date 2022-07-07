using Apos.Tweens;
using Microsoft.Xna.Framework;

public class Block : Component
{
	ITween<Vector2> tween;

	private bool hasBeenHit;

	public void Hit()
	{
		if (hasBeenHit) return;

		hasBeenHit = true;
		tween = new Vector2Tween(transform.scale, Vector2.Zero, 100, Easing.BackIn);
	}

	public override void Update()
	{
		if (tween == null) return;

		transform.scale = tween.Value;
		if (tween.Value == Vector2.Zero) DestroyGameObject();
	}
}
