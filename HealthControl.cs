using Godot;

public partial class HealthControl : Control
{
	private ProgressBar _healthBar;
	private Timer _healthTimer;
	private ItemSprite _objectiveItemSprite;
	private TextureProgressBar _deliveryTimerBar;
	private Button _resumeButton;
	private Button _retryButton;
	private Button _quitButton;
	public int _playerScore;
	private int _objectiveIndex = 0;
	private readonly string[] _objectives = ["res://Items/Ammo/ammo.tres", "res://Items/LaserBeam/laser_beam.tres"];
	private Label _scoreLabel;
	private int _goalScore;
	public CharacterBody2D _playerCharacter;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthTimer = GetNode<Timer>("HealthTimer");
		_objectiveItemSprite = GetNode<ItemSprite>("ObjectiveItemSprite");
		_deliveryTimerBar = GetNode<TextureProgressBar>("DeliveryTimerBar");
		_resumeButton = GetNode<Button>("ResumeButton");
		_retryButton = GetNode<Button>("RetryButton");
		_quitButton = GetNode<Button>("QuitButton");
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_deliveryTimerBar.MaxValue = _healthTimer.WaitTime;
		_resumeButton.Visible = false;
		_retryButton.Visible = false;
		_quitButton.Visible = false;
		_goalScore = 2;
		_resumeButton.Pressed += ResumeGame;
		_retryButton.Pressed += RetryGame;
		_quitButton.Pressed += QuitGame;

		_healthTimer.Timeout += OnHealthTimerTimeout;
		
		//If I try to do this here, Godot can't find it. It can find it if I do it later.
		//_playerCharacter = GetNode<CharacterBody2D>("/root/Node2D/PlayerCharacter");
		
		var Laser = GetNode<Laser>("/root/Node2D/Laser");
		var Launcher = GetNode<Launcher>("/root/Node2D/Launcher");
		Launcher.ObjectiveCompleted += ObjectiveSuccess;
		Laser.ObjectiveCompleted += ObjectiveSuccess;

		// TODO don't hardcode objective
		_objectiveItemSprite.ItemResource = ResourceLoader.Load<ItemResource>(GetCurrentObjective());
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			DeathEvent();
			_resumeButton.Visible = true;
		}
	}

	public override void _Process(double delta)
	{
		_deliveryTimerBar.Value = _healthTimer.TimeLeft;

		if (_healthBar.Value < 5)
		{
			DeathEvent();
		}

		if (_playerScore > _goalScore)
		{
			//this doesn't work yet because when it reloads the scene it resets the _goalScore during the _Ready() process
			_goalScore++;
			WinEvent();
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

	private void WinEvent()
	{
		//you win bitch
		DeathEvent();
		_retryButton.Text = "Next Level";
	}
	private void DeathEvent()
	{
		_retryButton.Visible = true;
		_quitButton.Visible = true;
		//I don't know why the below works
		_playerCharacter = GetNode<CharacterBody2D>("/root/Node2D/PlayerCharacter");
		if (_playerCharacter is TestCharacter myScriptInstance)
		{
			myScriptInstance._canMove = false;
		}
		_healthTimer.Paused = true;
		
	}

	private void ResumeGame()
	{
		_healthTimer.Paused = false;
		_resumeButton.Visible = false;
		_retryButton.Visible = false;
		_quitButton.Visible = false;
		//yeah im doing this twice; I don't know where else to assign this because it isn't working in _Ready()
		_playerCharacter = GetNode<CharacterBody2D>("/root/Node2D/PlayerCharacter");
		if (_playerCharacter is TestCharacter myScriptInstance)
		{
			myScriptInstance._canMove = true;
		}
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
