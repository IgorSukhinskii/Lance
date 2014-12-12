using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets;
using UnityEngine;
using GameWorld.ProceduralGeneration;
using GameWorld.Players;

namespace GameWorld.Map
{

    public class GameMap
    {
        public List<Province> provinces {
        	get;
        	private set;
        }
        private Dictionary<Tuple<int, int>, bool> matrix;

        public GameMap()
        {
            provinces = new List<Province>();
            matrix = new Dictionary<Tuple<int, int>, bool>();
        }

        public bool Bordered(int province1, int province2)
        {
            return matrix.ContainsKey(new Tuple<int, int>(province1, province2));
        }

        public void AddProvince(Province province)
        {
            provinces.Add(province);
        }
        public void AddBorder(int province1, int province2)
        {
            matrix[new Tuple<int, int>(province1, province2)] = true;
            matrix[new Tuple<int, int>(province2, province1)] = true;
        }

        public static GameMap Generate()
        {
            var nameGen = new NameGenerator();
            GameMap gameRes = new GameMap();
            int n = 50;
            var points = new List<Vector2>();
            var colors = new List<uint>();
            var lines = new List<Tuple<Vector2, Vector2>>();
            for (int i = 0; i < n; i++)
            {
                points.Add(new Vector2(UnityEngine.Random.Range(-10.0F, 10.0F), UnityEngine.Random.Range(-10.0F, 10.0F)));
                colors.Add(0);
            }
            Delaunay.Voronoi v = new Delaunay.Voronoi(points, colors, new Rect(-10.0F, -10.0F, 20.0F, 20.0F));
            var regions = v.SiteCoords();
            foreach (var pts in v.Regions())
            {
                var prov = new Province();
                prov.Border = pts.Reverse<Vector2>().ToList();
                prov.Name = nameGen.getName();

                gameRes.AddProvince(prov);
            }
            return gameRes;
        }
    }

    public class Province: IOwned
    {
        public string Name { get; set; }
        public List<Vector2> Border { get; set; }
    }
}
