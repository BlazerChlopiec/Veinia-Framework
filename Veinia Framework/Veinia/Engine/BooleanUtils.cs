using System.Collections.Generic;

public class BooleanUtils
{
	static List<Boolean> booleans = new List<Boolean>();

	public static bool WheneverFalse(bool currentValue, string booleanIdentifier)
	{
		bool temp = false;

		if (booleans.Find(x => x.booleanIdentifier == booleanIdentifier) == null && currentValue == true)
		{
			booleans.Add(new Boolean(booleanIdentifier));
		}

		if (currentValue == false && booleans.Find(x => x.booleanIdentifier == booleanIdentifier) != null)
		{
			booleans.Remove(booleans.Find(x => x.booleanIdentifier == booleanIdentifier));
			temp = true;
		}

		return temp;
	}
	public static bool WheneverTrue(bool currentValue, string booleanIdentifier)
	{
		bool temp = false;

		if (booleans.Find(x => x.booleanIdentifier == booleanIdentifier) == null && currentValue == false)
		{
			booleans.Add(new Boolean(booleanIdentifier));
		}

		if (currentValue == true && booleans.Find(x => x.booleanIdentifier == booleanIdentifier) != null)
		{
			booleans.Remove(booleans.Find(x => x.booleanIdentifier == booleanIdentifier));
			temp = true;
		}

		return temp;
	}

	public class Boolean
	{
		public string booleanIdentifier;

		public Boolean(string booleanIdentifier)
		{
			this.booleanIdentifier = booleanIdentifier;
		}
	}
}
