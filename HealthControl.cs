using Godot;

public partial class HealthControl : Control
{
	private ProgressBar _healthBar;
	private Button _resumeButton;
	private Button _retryButton;
	private Button _quitButton;
	private int _playerScore;
	private int _objectiveIndex = 0;
	private readonly string[] _objectives = ["res://Items/Ammo/ammo.tres", "res://Items/LaserBeam/laser_beam.tres"];
	private Label _scoreLabel;
	private int _goalScore;
	private TestCharacter _playerCharacter;

	public delegate void PauseEventHandler();
	public event PauseEventHandler OnPause;

	public delegate void ResumeEventHandler();
	public event ResumeEventHandler OnResume;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_resumeButton = GetNode<Button>("ResumeButton");
		_retryButton = GetNode<Button>("RetryButton");
		_quitButton = GetNode<Button>("QuitButton");
		_scoreLabel = GetNode<Label>("ScoreLabel");
		_resumeButton.Visible = false;
		_retryButton.Visible = false;
		_quitButton.Visible = false;
		_goalScore = 3;
		_resumeButton.Pressed += ResumeGame;
		_retryButton.Pressed += RetryGame;
		_quitButton.Pressed += QuitGame;

		_scoreLabel.Text = $"Score: {_playerScore} / {_goalScore}";

		Laser laser = GetNode<Laser>("/root/Node2D/Laser");
		Launcher launcher = GetNode<Launcher>("/root/Node2D/Launcher");
		launcher.ObjectiveCompleted += ObjectiveSuccess;
		laser.ObjectiveCompleted += ObjectiveSuccess;
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			DeathEvent();
			_resumeButton.Visible = true;
		}
	}

	public void ObjectiveSuccess()
	{
		_playerScore++;
		_scoreLabel.Text = $"Score: {_playerScore} / {_goalScore}";

		if (_playerScore >= _goalScore)
		{
			WinEvent();
		}
	}

	public void HandleFailureEvent()
	{
		_healthBar.Value -= 10;
		if (_healthBar.Value <= 0)
		{
			_healthBar.Value = 0;
			DeathEvent();
		}
	}

	private void WinEvent()
	{
		DeathEvent();
		_retryButton.Text = "Next Level";
	}

	private void DeathEvent()
	{
		_retryButton.Visible = true;
		_quitButton.Visible = true;
		GetPlayerCharacter().CanMove = false;
		OnPause?.Invoke();
	}

	private void ResumeGame()
	{
		_resumeButton.Visible = false;
		_retryButton.Visible = false;
		_quitButton.Visible = false;

		GetPlayerCharacter().CanMove = true;
		OnResume?.Invoke();
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

	private TestCharacter GetPlayerCharacter()
	{
		return _playerCharacter ??= GetNode<TestCharacter>("/root/Node2D/PlayerCharacter");
	}
}
