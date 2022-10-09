using Veinia;

public class TestUp : Component
{
	public override void Update()
	{
		transform.position += transform.up * 5 * Globals.input.vertical * Time.deltaTime;
		transform.xRotation += Globals.input.horizontal * 50 * Time.deltaTime;

		Say.Line(transform.up);
	}
}