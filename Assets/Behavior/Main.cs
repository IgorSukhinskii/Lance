using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using Assets;
using System;
using Delaunay;
using Delaunay.Geo;
using GameWorld;
using GameWorld.Players;
using GameWorld.Map;

public class Main : MonoBehaviour {
    List<Player> Players;
    int currentP;
	


    void InitPlayers()
    {
        this.Players = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            this.Players.Add(Player.Prototype.Copy());
        }
        currentP = 0;
    }
	// Use this for initialization
	void Start () {

		//
        SquadType.FromJSON("squad_types.json");

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}