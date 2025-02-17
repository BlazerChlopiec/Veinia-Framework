using System;
using System.Collections.Generic;
using VeiniaFramework;

public class Timers
{
	private static List<Counter> timeCounters = new List<Counter>();

	public static void ProcessTimers()
	{
		foreach (var item in timeCounters.ToArray())
		{
			item.time = item.useUnscaled ? item.time - Time.unscaledDeltaTime : item.time - Time.deltaTime;

			if (item.time <= 0 && !item.stop)
			{
				item.onComplete?.Invoke();
				item.onComplete = null;
			}
		}
	}

	public static void New(string name, float time, Action onComplete, bool useUnscaled = false)
	{
		if (!AlreadyExists(name))
		{
			Counter counter = new Counter
			{
				name = name,
				time = time,
				startTime = time,
				onComplete = onComplete,
				useUnscaled = useUnscaled
			};

			timeCounters.Add(counter);
		}

		if (AlreadyExists(name))
		{
			//overriding the counter
			Counter target = timeCounters[timeCounters.IndexOf(timeCounters.Find(x => x.name == name))];
			target.stop = false;
			target.time = time;
			target.startTime = time;
			target.onComplete = onComplete;
		}
	}

	public static void New(string name, float time, bool useUnscaled = false)
	{
		if (!AlreadyExists(name))
		{
			Counter counter = new Counter
			{
				name = name,
				time = time,
				startTime = time,
				useUnscaled = useUnscaled
			};

			timeCounters.Add(counter);
		}

		if (AlreadyExists(name, out Counter target))
		{
			//overriding the counter
			target.stop = false;
			target.time = time;
			target.startTime = time;
		}
	}

	public static void Clear(string name)
	{
		AlreadyExists(name, out Counter target);
		if (target == null) return;
		target.time = -10f;
		target.stop = true;
	}

	public static bool IsUp(string name)
	{
		var result = timeCounters.Find(x => x.name == name);
		if (result == null) return true; // very convinient THO i would've lost an interview
		return result.time <= 0;
	}

	public static float GetTime(string name)
	{
		var result = timeCounters.Find(x => x.name == name);
		if (result != null) return result.time;
		else return 0f;
	}
	/// <summary>
	/// returns ratio of time / startTime
	/// </summary>
	public static float GetTimeRatio(string name)
	{
		var result = timeCounters.Find(x => x.name == name);
		if (result != null) return Math.Clamp(result.time, 0, float.MaxValue) / result.startTime;
		else return 0f;
	}

	public static bool AlreadyExists(string name)
	{
		List<Counter> temp = new List<Counter>();
		temp = timeCounters.FindAll(x => x.name == name);

		return temp.Count != 0;
	}
	public static bool AlreadyExists(string name, out Counter counter)
	{
		List<Counter> temp = new List<Counter>();
		temp = timeCounters.FindAll(x => x.name == name);

		var exisits = temp.Count != 0;
		counter = exisits ? temp[0] : default;
		return exisits;
	}
}

public class Counter
{
	public string name;
	public float time;
	public float startTime; // this is used for Get01FromStartValue()
	public bool stop;
	public bool useUnscaled;
	public Action onComplete;
}