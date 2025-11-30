using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace VeiniaFramework
{
	public class AudioManager
	{
		public static float MasterVolume
		{
			get { return masterVolume; }
			set { masterVolume = MathHelper.Clamp(value, 0, 1); }
		}
		private static float masterVolume = 1f;


		public static void Play(Sound sound)
		{
			if (sound == null) return;

			var pitch = sound.uniqueModifier ?
						MathHelper.Clamp(sound.pitch + Globals.random.NextFloat(-.15f, .15f), -1, 1) :
						MathHelper.Clamp(sound.pitch, -1, 1);

			sound.soundEffect.Play(sound.volume * MasterVolume, pitch, pan: 0);
		}

		public static void Play(Music music)
		{
			if (music == null) return;

			MediaPlayer.Volume = music.volume * MasterVolume;
			MediaPlayer.IsRepeating = music.loop;
			MediaPlayer.Play(music.song);
		}

		public static void StopMusic() => MediaPlayer.Stop();
	}

	public class Sound
	{
		public SoundEffect soundEffect;

		public float volume = 1f;
		public float pitch;

		// adds a unique pitch offset to each sound - Random(-.15f, .15f)
		public bool uniqueModifier;

		public Sound(string path, float volume = 1f, float pitch = 0f, bool uniqueModifier = false)
		{
			this.volume = volume;
			this.pitch = pitch;
			this.uniqueModifier = uniqueModifier;

			if (path != null && path != string.Empty)
				soundEffect = Globals.content.Load<SoundEffect>(path);
		}
	}


	public class Music
	{
		public Song song;

		public float volume = 1f;
		public bool loop;

		public Music(string path, float volume = 1f, bool loop = true)
		{
			this.volume = volume;
			this.loop = loop;

			if (path != null && path != string.Empty)
				song = Globals.content.Load<Song>(path);
		}
	}
}