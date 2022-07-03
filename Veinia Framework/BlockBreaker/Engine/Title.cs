using Microsoft.Xna.Framework;
using System.Text;

public class Title
{
	string firstFrameName; // it takes the name of the game in the first frame before adding debug messages in the title
	StringBuilder finalText = new StringBuilder(); // final title after adding debug messages

	// since arrays can't be resized during runtime i set their size to 'titleItemLimit'
	static private string[] debugMessages;
	const int titleItemLimit = 10;
	//

	GameWindow gameWindow;// reference to the game window to later change its title


	public Title(GameWindow GAMEWINDOW)
	{
		gameWindow = GAMEWINDOW;
		firstFrameName = gameWindow.Title;
		debugMessages = new string[titleItemLimit]; // create the array to store debug messages of the max size 'titleItemLimit'
	}

	public static void Add<T1>(T1 input, string desc, int order) => debugMessages[order] = input.ToString() + desc;
	public static void Add<T1>(T1 input, int order) => debugMessages[order] = input.ToString();
	public static void Erase(int index) => debugMessages[index] = null;
	public virtual void Update()
	{
#if DEBUG
		finalText.Clear(); // reset the name each time 
		finalText.Append(firstFrameName); // add the title from the first frame

		for (int i = 0; i < debugMessages.Length; i++) // loop through all title debug messages 
		{
			if (debugMessages[i] != null) // avoid adding blank indexes in the title (unused indexes in the array)
			{
				finalText.Append("   " + "(" + debugMessages[i] + ")");
			}
		}

		gameWindow.Title = finalText.ToString();
#endif
	}
}

