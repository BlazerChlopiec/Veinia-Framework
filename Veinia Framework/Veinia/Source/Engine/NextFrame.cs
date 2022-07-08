using System;
using System.Collections.Generic;

public class NextFrame
{
	public static List<Action> actions = new List<Action>();

	public static void Update()
	{
		for (int i = 0; i < actions.Count; i++)
		{
			if (actions[i] != null) actions[i].Invoke();
		}
		actions.Clear();
	}
}