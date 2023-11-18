using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tweening;

namespace VeiniaFramework.Samples.BlockBreaker
{
	public class Transition : Component
	{
		Tween tween;
		Texture2D texture;

		public Transition(string path) => this.texture = Globals.content.Load<Texture2D>(path);

		public override void Initialize()
		{
			Image transition = new Image();
			transition.Texture = texture;
			transition.Anchor = Anchor.Center;
			transition.Scale = 0;
			transition.FillColor = Color.Black;
			transition.DontDestroyOnLoad = true;
			UserInterface.Active.AddEntity(transition);

			tween = Globals.unscaledTweener.TweenTo(target: transition, expression: image => image.Scale,
											toValue: 1, .5f)
			.Easing(EasingFunctions.CircleOut)
			.OnEnd((x) =>
			{
				Globals.loader.Reload();
				NextFrame.actions.Add(() => { transition.BringToFront(); }); // when the UI gets reset our transition isn't on top

				tween = Globals.unscaledTweener.TweenTo(target: transition, expression: image => image.Scale,
								toValue: 0, .5f, delay: .1f)
				.Easing(EasingFunctions.SineIn)
				.OnEnd((x) => { UserInterface.Active.RemoveEntity(transition); });
			});
		}
	}
}
