using Apos.Tweens;
using Microsoft.Xna.Framework;

public class Block : Component
{
	ITween<Vector2> scaleTween;
	ITween<float> rotationTween;

	private bool hasBeenHit;

	public void Hit()
	{
		if (hasBeenHit) return;

		RemoveComponent(GetComponent<Physics>());

		hasBeenHit = true;
		scaleTween = new Vector2Tween(transform.scale, Vector2.Zero, 300, Easing.BackIn);
		rotationTween = new FloatTween(transform.xRotation, -10, 150, Easing.ExpoInOut)
			.Offset(20, 150, Easing.ExpoInOut);
	}

	public override void Update()
	{
		if (scaleTween == null || rotationTween == null) return;

		transform.scale = scaleTween.Value;
		transform.xRotation = rotationTween.Value;
		if (scaleTween.Value == Vector2.Zero)
		{
			DestroyGameObject();

			if (FindComponentsOfType<Block>().Count == 0)
			{
				Globals.loader.Reload();
			}
		}
	}
}
