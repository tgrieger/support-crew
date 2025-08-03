using Godot;

public partial class HealthControl : Control
{
	private ProgressBar _healthBar;
	private Timer _healthTimer;
	private ItemSprite _objectiveItemSprite;
	private TextureProgressBar _deliveryTimerBar;
	private Button _retryButton;
	private Button _quitButton;
	public int _playerScore;
	private int _objectiveIndex = 0;
	private readonly string[] _objectives = ["res://Items/Ammo/ammo.tres", "res://Items/LaserBeam/laser_beam.tres"];
	private Label _scoreLabel;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthTimer = GetNode<Timer>("HealthTimer");
		_objectiveItemSprite = GetNode<ItemSprite>("ObjectiveItemSprite");
		_deliveryTimerBar = GetNode<TextureProgressBar>("DeliveryTimerBar");
		_retryButton = GetNode<Button>("RetryButton");
		_quitButton = GetNode<Button>("QuitButton");
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_deliveryTimerBar.MaxValue = _healthTimer.WaitTime;

		_retryButton.Visible = false;
		_quitButton.Visible = false;

		_retryButton.Pressed += RetryGame;
		_quitButton.Pressed += QuitGame;

		_healthTimer.Timeout += OnHealthTimerTimeout;
		
		var Laser = GetNode<Laser>("/root/Node2D/Laser");
		var Launcher = GetNode<Launcher>("/root/Node2D/Launcher");
		Launcher.ObjectiveCompleted += ObjectiveSuccess;
		Laser.ObjectiveCompleted += ObjectiveSuccess;

		// TODO don't hardcode objective
		_objectiveItemSprite.ItemResource = ResourceLoader.Load<ItemResource>(GetCurrentObjective());
	}

	public override void _Process(double delta)
	{
		_deliveryTimerBar.Value = _healthTimer.TimeLeft;

		if (_healthBar.Value < 5)
		{
			DeathEvent();
		}
	}
	
	public void ObjectiveSuccess()
	{
		_objectiveItemSprite.ItemResource = null;
		_playerScore++;
		_scoreLabel.Text = $"Score: {_playerScore}";
	}
	
	private void OnHealthTimerTimeout()
	{
		// If there's no objective, set one and don't decrease health.
		if (_objectiveItemSprite.ItemResource is null)
		{
			_objectiveItemSprite.ItemResource = ResourceLoader.Load<ItemResource>(GetCurrentObjective());
			return;
		}

		_healthBar.Value -= 10;
	}

	private void DeathEvent()
	{
		_healthTimer.Stop();
		_retryButton.Visible = true;
		_quitButton.Visible = true;
	}

	private void RetryGame()
	{
		GetTree().ReloadCurrentScene();
	}

	private void QuitGame()
	{
		GetTree().Quit();
	}

	private string GetCurrentObjective()
	{
		int currentIndex = _objectiveIndex;
		_objectiveIndex = (_objectiveIndex + 1) % _objectives.Length;

		return _objectives[currentIndex];
	}
}
