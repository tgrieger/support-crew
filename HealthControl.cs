using Godot;

public partial class HealthControl : Control
{
	private ProgressBar _healthBar;
	private Timer _healthTimer;
	private ItemSprite _objectiveItemSprite;
	private TextureProgressBar _deliveryTimerBar;

	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("HealthBar");
		_healthTimer = GetNode<Timer>("HealthTimer");
		_objectiveItemSprite = GetNode<ItemSprite>("ObjectiveItemSprite");
		_deliveryTimerBar = GetNode<TextureProgressBar>("DeliveryTimerBar");

		_healthTimer.Timeout += OnHealthTimerTimeout;

		// TODO don't hardcode objective
		_objectiveItemSprite.ItemResource = ResourceLoader.Load<ItemResource>("res://Items/Ammo/ammo.tres");
	}
	
	public override void _Process(double delta)
	{
		_deliveryTimerBar.Value = _healthTimer.TimeLeft;
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
