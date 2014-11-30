using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets;
using System;
using System.IO;
using MiniJSON;

namespace GameWorld.Players
{	
	public class Player {
		public int Resources;
		public int Peasants;
		public int ManPower;
        public List<IOwned> Owned = new List<IOwned>();

        static Player prototype = new Player();
        public static Player Prototype
        {
            get
            {
                return Player.prototype;
            }
        }
        public static void FromJson(string configName)
        {
            Player.prototype = new Player();
            Dictionary<string, object> config;
            using (StreamReader sr = new StreamReader(Path.Combine(Application.dataPath, configName)))
            {
                string line = "";
                while (!sr.EndOfStream)
                    line += sr.ReadLine();
                Debug.Log(line);
                config = Json.Deserialize(line) as Dictionary<string, object>;
            }
            Player.prototype.Resources = (int)(long)config["Resources"];
            Player.prototype.Peasants = (int)(long)config["Peasants"];
            Player.prototype.ManPower = (int)(long)config["ManPower"];
        }

        public Player Copy()
        {
            var copy = new Player();
            copy.Resources = this.Resources;
            copy.Peasants = this.Peasants;
            copy.ManPower = this.ManPower;
            copy.Owned = this.Owned.ToList();
            return copy;
        }
	}
	public class SquadType{
		public readonly int MaxStrength;
		public readonly int Attack;
		public readonly int Defence;
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
	public class Squad: IOwned{
		private int strength;
        public int Strength
        {
            get
            {
                return this.strength;
            }
            set
            {
                if (value > this.Type.MaxStrength)
                {
                    this.strength = this.Type.MaxStrength;
                }
                else if (value < 0)
                {
                    this.strength = 0;
                }
                else
                {
                    this.strength = value;
                }
            }
        }
		private string name;
        public string Name
        {
            get
            {
                return this.name;
            }
        }
		public readonly SquadType Type;
	}
}