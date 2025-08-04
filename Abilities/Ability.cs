using Godot;

public partial class Ability : Node2D
{
	private Sprite2D _abilitySprite;
	private AbilityResource _abilityResource;
	private Timer _timer;
	private TextureProgressBar _deliveryTimerBar;
	private bool _onCooldown = false;

	public delegate void FailureEventHandler();
	public event FailureEventHandler OnFailure;

	[Export]
	public AbilityResource AbilityResource
	{
		get => _abilityResource;
		set
		{
			_abilityResource = value;
			UpdateTimer();
			UpdateDeliveryTimerBar();
			SetAbilitySprite();
		}
	}

	public override void _Ready()
	{
		_timer = GetNode<Timer>("Timer");
		_deliveryTimerBar = GetNode<TextureProgressBar>("DeliveryTimerBar");

		SetAbilitySprite();
	}

	public override void _Process(double delta)
	{
		_deliveryTimerBar.Value = _timer.TimeLeft;
	}

	public bool CanCompleteEvent(string name)
	{
		if (name != _abilityResource.AbilityName)
		{
			return false;
		}

		return !_onCooldown;
	}

	public void CompleteEvent()
	{
		_onCooldown = true;
		_timer.WaitTime = _abilityResource.CooldownTime;
		_timer.Timeout += OnCooldown;
		_timer.Timeout -= OnExpiration;
		_timer.Start();

		GetAbilitySprite().Modulate = new Color(.3f, .3f, .3f);

		UpdateDeliveryTimerBar();
	}

	public void OnPause()
	{
		_timer.Paused = true;
	}

	public void OnResume()
	{
		_timer.Paused = false;
	}

	private void OnCooldown()
	{
		_onCooldown = false;
		_timer.WaitTime = _abilityResource.ExpirationTime;
		_timer.Timeout += OnExpiration;
		_timer.Timeout -= OnCooldown;
		_timer.Start();

		GetAbilitySprite().Modulate = new Color(1, 1, 1);

		UpdateDeliveryTimerBar();
	}

	private void OnExpiration()
	{
		_timer.Start();
		OnFailure?.Invoke();
	}

	private void UpdateTimer()
	{
		_timer.WaitTime = _abilityResource?.ExpirationTime ?? 0;
		_timer.Timeout += OnExpiration;
		_timer.Start();
	}

	private void UpdateDeliveryTimerBar()
	{
		_deliveryTimerBar.MaxValue = _timer.WaitTime;
		_deliveryTimerBar.Value = _timer.WaitTime;
	}

	private void SetAbilitySprite()
	{
		Sprite2D abilitySprite = GetAbilitySprite();
		if (abilitySprite is null)
		{
			return;
		}

		abilitySprite.Texture = _abilityResource?.AbilityTexture;
	}

	private Sprite2D GetAbilitySprite()
	{
		return _abilitySprite ??= GetNodeOrNull<Sprite2D>("AbilitySprite");
	}
}
