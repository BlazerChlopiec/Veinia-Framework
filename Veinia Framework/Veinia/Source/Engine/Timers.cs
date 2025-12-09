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

				if (item.repeat) New(item.name, item.startTime, item.onComplete, item.useUnscaled, item.repeat);
				else item.onComplete = null;
			}
		}
	}

	public static void New(string name, float time, Action onComplete = null, bool useUnscaled = false, bool repeat = false)
	{
		var exists = AlreadyExists(name);

		if (!exists)
		{
			var counter = new Counter
			{
				name = name,
				time = time,
				startTime = time,
				onComplete = onComplete,
				useUnscaled = useUnscaled,
				repeat = repeat
			};

			timeCounters.Add(counter);
		}

		if (exists)
		{
			var target = timeCounters.Find(x => x.name == name);

			target.stop = false;
			target.time = time;
			target.startTime = time;
			target.onComplete = onComplete;
			target.repeat = repeat;
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

	public static bool AlreadyExists(string name) => timeCounters.Exists(x => x.name == name);
	public static bool AlreadyExists(string name, out Counter counter)
	{
		counter = timeCounters.Find(x => x.name == name);
		return counter != null;
	}
}

public class Counter
{
	public string name;
	public float time;
	public float startTime; // this is used for Get01FromStartValue()
	public bool stop;
	public bool useUnscaled;
	public bool repeat;
	public Action onComplete;
}