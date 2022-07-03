using Microsoft.Xna.Framework.Input;

[ExecuteUpdateWhenCulled]
public class KillOnClick : Component
{
	private Keys key;

	public KillOnClick(Keys key)
	{
		this.key = key;
	}

	public override void Update()
	{
		if (Globals.input.GetKeyDown(key))
		{
			DestroyGameObject();
		}
	}
}
