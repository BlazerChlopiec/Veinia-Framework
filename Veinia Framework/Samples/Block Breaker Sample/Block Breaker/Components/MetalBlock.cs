using Microsoft.Xna.Framework.Graphics;

namespace VeiniaFramework.BlockBreaker
{
	public class MetalBlock : Tile
	{
		private int hitPoints = 2;


		public override void Hit()
		{
			hitPoints--;
			GetComponent<Sprite>().ChangeTexture("Sprites/Metal Tile Broken");
			if (hitPoints <= 0)
				base.Hit();
		}
	}
}
