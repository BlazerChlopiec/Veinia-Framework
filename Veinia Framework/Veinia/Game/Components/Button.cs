using System;

public class Button : Component
{
	public Action OnMouseDown;
	public Action OnMouseUp;

	Sprite sprite;


	public override void Initialize()
	{
		sprite = GetComponent<Sprite>();
	}
	public override void Update()
	{
		if (sprite.worldRect.Contains(Globals.input.GetMouseWorldPosition()))
		{
			sprite.color = Microsoft.Xna.Framework.Color.AliceBlue;
		}
		else
			sprite.color = Microsoft.Xna.Framework.Color.Black;
	}
}