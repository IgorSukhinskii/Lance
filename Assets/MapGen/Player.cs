using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets;
using System;
using Delaunay;
using Delaunay.Geo;
using ProceduralGeneration;

namespace Players{
	
	public class Player  {
		
		public int resources;
		public int peasants;
		public int army;
	}
	public class SquadType{
		public int max_strength;
		public int attack;
		public int protection;
	}
	public class Squad{
		public int strength;
		public string name;
		public SquardType type;
	}
	
	
	
}