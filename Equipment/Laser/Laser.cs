using Godot;
using SupportCrew.Equipment;

public partial class Laser : StaticBody2D, IActivatable
{
	[Signal]
	public delegate void ObjectiveCompletedEventHandler();
	private LaserBatteryAttachment _laserBatteryAttachment;
	private AnimatedSprite2D _buttonAnimatedSprite;

	[Export] public double LaserCost { get; set; } = 50;

	public override void _Ready()
	{
		_buttonAnimatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_laserBatteryAttachment = GetNode<LaserBatteryAttachment>("LaserBatteryAttachment");
	}

	public bool CanActivate(TestCharacter character)
	{
		_buttonAnimatedSprite.Play("ButtonPressed");
		return _laserBatteryAttachment.ItemResource is not null && !(_laserBatteryAttachment.DurabilityPercentage < LaserCost);
	}

	public void Activate()
	{
		_laserBatteryAttachment.DurabilityPercentage -= LaserCost;
		EmitSignal(SignalName.ObjectiveCompleted);
	}
}
