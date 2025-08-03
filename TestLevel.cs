using Godot;
using System;

public partial class TestLevel : Node2D
{
	[Export]
	public PackedScene PlayerScene { get; set; }

	[Export]
	public PlayerResource PlayerOneResource { get; set; }

	[Export]
	public PlayerResource PlayerTwoResource { get; set; }

	[Export]
	public PlayerResource PlayerThreeResource { get; set; }

	[Export]
	public PlayerResource PlayerFourResource { get; set; }

	public override void _Ready()
	{
		PlayerResource[] playerResources = [ PlayerOneResource, PlayerTwoResource, PlayerThreeResource, PlayerFourResource ];
		for (int i = 0; i < GameManager.Instance.NumberOfPlayers; i++)
		{
			TestCharacter player = PlayerScene.Instantiate<TestCharacter>();
			player.PlayerResource = playerResources[i];
			player.GlobalPosition = new Vector2(128 * (i + 1), 128 * (i + 1));
			AddChild(player);
		}
		
		Spawner otherScript = GetNode<Spawner>("SpawnControl");
		otherScript.RandomSpawner();
	}
}
