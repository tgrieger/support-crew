using Godot;

public partial class HealthControl : Control
{
	private ProgressBar _healthBar;
	private Timer _healthTimer;
	private ItemSprite _objectiveItemSprite;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthTimer = GetNode<Timer>("HealthTimer");
		_objectiveItemSprite = GetNode<ItemSprite>("ObjectiveItemSprite");

		_healthTimer.Timeout += OnHealthTimerTimeout;

		// TODO don't hardcode objective
		_objectiveItemSprite.ItemResource = ResourceLoader.Load<ItemResource>("res://Items/Ammo/ammo.tres");
	}

	private void OnHealthTimerTimeout()
	{
		// If there's no objective, set one and don't decrease health.
		if (_objectiveItemSprite.ItemResource is null)
		{
			_objectiveItemSprite.ItemResource = ResourceLoader.Load<ItemResource>("res://Items/Ammo/ammo.tres");
			return;
		}

		_healthBar.Value -= 10;
	}
}
