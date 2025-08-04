using Godot;
using System.Collections.Generic;

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
		List<Vector2> spawnCoordinates = [];

		int maxX = 1056;
		int maxY = 544;

		for (int x = 96; x < maxX; x=x+64)
		{
			for (int y = 96; y < maxY; y=y+64)
			{
				Vector2 v  = new(x, y);
				spawnCoordinates.Add(v);
			}
		}

		foreach (Node2D node in Nodes)
		{
			int randIndex = GD.RandRange(0, spawnCoordinates.Count - 1);
			Vector2 randomPoint = spawnCoordinates[randIndex];
			node.Position = randomPoint;
			spawnCoordinates.Remove(randomPoint);
		}
	}
}
