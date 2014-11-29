using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets;
using System;
using Delaunay;
using Delaunay.Geo;
using ProceduralGeneration;
using System.IO;
using MiniJSON;

namespace Players{
	
	public class Player  {
		public int Resources;
		public int Peasants;
		public int ManPower;
	}
	public class SquadType{
		public int MaxStrength;
		public int Attack;
		public int Defence;
        private static Dictionary<string, SquadType> types;
        private SquadType(int maxStrength, int attack, int defence)
        {
            this.MaxStrength = maxStrength;
            this.Attack = attack;
            this.Defence = defence;
        }
        public static void FromJSON(string configName)
        {
            types = new Dictionary<string, SquadType>();
            Dictionary<string, object> config;
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, configName)))
            {
                string line = "";
                while (!sr.EndOfStream)
                    line += sr.ReadLine();
                Debug.Log(line);
                config = Json.Deserialize(line) as Dictionary<string, object>;
            }
            foreach(var squadType in config)
            {
                var fields = (Dictionary<string, object>)squadType.Value;
                int maxStr = (int)(long)(fields["MaxStrength"]);
                int attack = (int)(long)fields["Attack"];
                int defence = (int)(long)fields["Defence"];
                SquadType.types[squadType.Key] = new SquadType(maxStr, attack, defence);
            }
        }
        public static SquadType ByName(string name)
        {
            return SquadType.types[name];
        }
	}
	public class Squad{
		public int Strength;
		public string Name;
		public SquadType Type;
	}
	
	
	
}