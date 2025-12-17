using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace VeiniaFramework
{
	public class AnimationManager : Component
	{
		List<AnimData> animDatas;
		Sprite sprite;

		AnimData currentAnim;
		float time;

		string startAnim;
		int tileX;
		int tileY;

		public float defaultTimeBetweenFrames;
		public float timeBetweenFramesMultiplier = 1;


		public AnimationManager(List<AnimData> animDatas, string startAnim, int tileX = 16, int tileY = 16, float timeBetweenFrames = .1f)
		{
			this.animDatas = animDatas;
			this.startAnim = startAnim;
			this.tileX = tileX;
			this.tileY = tileY;
			this.defaultTimeBetweenFrames = timeBetweenFrames;
		}

		public override void EarlyInitialize()
		{
			sprite = GetComponent<Sprite>();
			if (sprite == null) Say.Line("Animation doesn't have Sprite Component!");

			for (int row = 0; row < sprite.texture.Height / tileY; row++)
			{
				var anim = animDatas.Find(x => x.atlasRow == row);
				if (anim == null) continue;

				for (int column = 0; column < sprite.texture.Width / tileX; column++)
				{
					var srcRect = new Rectangle(tileX * column, tileY * row, tileX, tileY);
					anim.sourceRectangles.Add(srcRect);
				}
			}

			Play(startAnim);
		}

		public void Play(string name)
		{
			if (currentAnim != null) // reset currentAnim Data
			{
				if (currentAnim.name == name)
				{
					currentAnim.stop = false;
					return;
				}

				currentAnim.frame = 0;
				currentAnim.stop = false;
			}

			currentAnim = GetAnim(name);
			sprite.ChangeTexture(sprite.texture, currentAnim.sourceRectangles[0]);
			time = 0;
		}

		public void Stop(int? frame = null)
		{
			if (currentAnim != null)
			{
				currentAnim.stop = true;
				if (frame.HasValue) currentAnim.frame = frame.Value;
			}

			sprite.ChangeTexture(sprite.texture, currentAnim.sourceRectangles[currentAnim.frame]);
			time = 0;
		}

		public override void Update()
		{
			time += Time.deltaTime;

			var timeBetweenFrames = currentAnim.timeBetweenFrames ?? defaultTimeBetweenFrames;
			timeBetweenFrames *= timeBetweenFramesMultiplier;

			if (time > timeBetweenFrames && !currentAnim.stop)
			{
				time = 0;

				if (currentAnim.sourceRectangles.Count == currentAnim.frame + 1) currentAnim.frame = 0;
				else currentAnim.frame++;

				sprite.ChangeTexture(sprite.texture, currentAnim.sourceRectangles[currentAnim.frame]);
			}
		}

		private AnimData GetAnim(string name) => animDatas.Find(x => x.name == name);
	}

	public class AnimData
	{
		public List<Rectangle> sourceRectangles = new List<Rectangle>();
		public string name;
		public bool stop;

		public int atlasRow;
		public int frame = 0;

		public float? timeBetweenFrames;
	}
}