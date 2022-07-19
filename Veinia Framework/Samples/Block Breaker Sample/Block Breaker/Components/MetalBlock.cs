namespace Veinia.BlockBreaker
{
	public class MetalBlock : Tile
	{
		private int hitPoints = 2;


		public override void Hit()
		{
			hitPoints--;
			GetComponent<Sprite>().ChangeTexture("Block Breaker/Metal Tile Broken");
			if (hitPoints <= 0)
				base.Hit();
		}
	}
}
