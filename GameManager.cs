using Godot;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	public int NumberOfPlayers { get; set; } = 1;

	public override void _Ready()
	{
		Instance = this;
	}
}
