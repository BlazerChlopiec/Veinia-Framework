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

			sound.soundEffect.Play(sound.volume * MasterVolume,
								   sound.uniqueModifier ? sound.pitch + Globals.random.NextFloat(-.15f, .15f) : sound.pitch,
								   pan: 0);
		}

		public static void Play(Music music)
		{
			if (music == null) return;

			MediaPlayer.Volume = music.volume * MasterVolume;
			MediaPlayer.IsRepeating = music.loop;
			MediaPlayer.Play(music.song);
		}
	}

	public class Sound
	{
		public string path;

		public SoundEffect soundEffect => Globals.content.Load<SoundEffect>(path);

		public float volume = 1f;
		public float pitch;

		// adds a unique pitch offset to each sound - Random(-.15f, .15f)
		public bool uniqueModifier;
	}


	public class Music
	{
		public string path;

		public Song song => Globals.content.Load<Song>(path);

		public bool loop = true;

		public float volume = 1f;
	}
}