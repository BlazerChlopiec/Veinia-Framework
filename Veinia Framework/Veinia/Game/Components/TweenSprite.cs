using Apos.Tweens;
using Microsoft.Xna.Framework;

public class TweenSprite : Component
{
	ITween<Vector2> tween;
	public override void Initialize()
	{

		tween = new Vector2Tween(transform.position, new Vector2(transform.position.X, 3), 500, Easing.BackInOut)
					.Wait(200)
					.Offset(new Vector2(0, -2), 500, Easing.BackInOut)
					.Wait(200)
					.Offset(new Vector2(-2, 0), 500, Easing.BackInOut)
					.Wait(200)
					.Offset(new Vector2(2, 0), 500, Easing.BackInOut)
					.Wait(200)
					.Loop();
	}

	public override void Update()
	{
		transform.position = tween.Value;
	}
}