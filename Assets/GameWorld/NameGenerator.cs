using System;
using System.IO;
using UnityEngine;
using MiniJSON;
using System.Collections.Generic;

namespace GameWorld.ProceduralGeneration
{
	public class NameGenerator
	{
		const string config_name = "names.json";
		Dictionary<string, object> config;
		public NameGenerator ()
		{
			try 
			{
				using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, config_name)))
				{
					string line = "";
					while (!sr.EndOfStream)
						line += sr.ReadLine();
					Debug.Log(line);
					config = Json.Deserialize(line) as Dictionary<string, object>;
				}
			}
			catch (IOException e)
			{
				Debug.Log(e.ToString());
			}
		}
		public string getName() {
			var firstBound = ((List<object>)config ["first"]).Count - 1;
			var secondBound = ((List<object>)config ["second"]).Count - 1;
			var str = ((string)((List<object>)config ["first"]) [UnityEngine.Random.Range (0, firstBound)]) +
			          ((string)((List<object>)config ["second"]) [UnityEngine.Random.Range (0, secondBound)]);
			return char.ToUpper(str [0]) + str.Substring (1);
		}
	}
}
