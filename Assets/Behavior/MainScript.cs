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

public class MainScript : MonoBehaviour {
    List<Player> Players;
    int currentPlayer;
	public InformationPanelScript informationPanel;
	public SquadControlPanelScript squadControlPanel;
	public ProvinceControlPanelScript provinceControlPanel;

	public enum Phases {
		Economy,
		War,
	}
	private Phases _currentPhase;
	public event EventHandler EconomyPhaseStarted;
	public event EventHandler WarPhaseStarted;
	public Phases currentPhase {
		get {
			return _currentPhase;
		}
		set {
			_currentPhase = value;
			if (value == Phases.Economy) {
				provinceControlPanel.gameObject.SetActive (true);
				squadControlPanel.gameObject.SetActive (false);
				if (EconomyPhaseStarted != null)
					EconomyPhaseStarted (this, null);
			} else {
				provinceControlPanel.gameObject.SetActive (false);
				squadControlPanel.gameObject.SetActive (true);
				if (WarPhaseStarted != null)
					WarPhaseStarted (this, null);
			}
		}
	}
    void InitPlayers()
    {
        this.Players = new List<Player>();
        for (int i = 0; i < 4; i++)
        {
            this.Players.Add(Player.Prototype.Copy());
        }
        currentPlayer = 0;
    }
	// Use this for initialization
	void Start () {
		informationPanel.endPhaseButton.Clicked += (sender, e) => {
			this.EndPhase ();
		};
		//
        SquadType.FromJSON("squad_types.json");
        StartCoroutine (InitCoroutine());
		 
	}
	IEnumerator InitCoroutine () {
		yield return null;
		currentPhase = Phases.Economy;
	}
	void EndPhase () {
		if (currentPhase == Phases.Economy)
			currentPhase = Phases.War;
		else
			currentPhase = Phases.Economy;
	}
	// Update is called once per frame
	void Update () {
		
	}
}