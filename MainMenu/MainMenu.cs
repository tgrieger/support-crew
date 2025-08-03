using Godot;

public partial class MainMenu : Control
{
	[Export]
	public PackedScene TestLevel { get; set; }

	private void OnePlayerPressed()
	{
		GameManager.Instance.NumberOfPlayers = 1;
		GetTree().ChangeSceneToPacked(TestLevel);
	}

	private void TwoPlayersPressed()
	{
		GameManager.Instance.NumberOfPlayers = 2;
		GetTree().ChangeSceneToPacked(TestLevel);
	}

	private void ThreePlayersPressed()
	{
		GameManager.Instance.NumberOfPlayers = 3;
		GetTree().ChangeSceneToPacked(TestLevel);
	}

	private void FourPlayersPressed()
	{
		GameManager.Instance.NumberOfPlayers = 4;
		GetTree().ChangeSceneToPacked(TestLevel);
	}
}
