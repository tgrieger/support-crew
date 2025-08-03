using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

public struct Point
{
	public int X { get; set; }
	public int Y { get; set; }
	
	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}
}

public partial class Spawner : Control
{
	[Export]
	public Node2D[] Nodes { get; set; }
	
	public override void _Ready()
	{
		RandomSpawner();
	}
	
	public void RandomSpawner()
	{
		List<Point> SpawnCoordinates = new List<Point>();
		
		int maxX = 1056;
		int maxY = 544;
		
		for (int x = 96; x < maxX; x=x+64)
		{
			for (int y = 96; y < maxY; y=y+64)
			{
				Point p  = new Point(x, y);
				SpawnCoordinates.Add(p);
			}
		}
		foreach (Node2D x in Nodes)
		{
			int randIndex = (int)GD.RandRange(0, SpawnCoordinates.Count);
			Point RandomPoint = SpawnCoordinates[randIndex];
			x.Position = new Vector2(RandomPoint.X, RandomPoint.Y);
			SpawnCoordinates.Remove(RandomPoint);
		}
	}
}
