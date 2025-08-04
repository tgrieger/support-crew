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
			CollisionShape2D shape = node.GetNode<CollisionShape2D>("CollisionShape2D");
			int randIndex = GD.RandRange(0, spawnCoordinates.Count - 1);
			Vector2 randomPoint = spawnCoordinates[randIndex];
			node.Position = randomPoint;
			//this is some SHIIIIIIIT
			Vector2 ZeroPos = new(0, 0);
			Vector2 OffsetPos = new(32, 0);
			if (shape.Position != ZeroPos)
			{
				node.Position = node.Position + OffsetPos;
				Vector2 randomPoint2 = spawnCoordinates[randIndex+1];
				spawnCoordinates.Remove(randomPoint2);
			}
			spawnCoordinates.Remove(randomPoint);
		}
	}
}
