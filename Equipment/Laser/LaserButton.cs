using Godot;
using SupportCrew.Equipment;

public partial class LaserButton : StaticBody2D, IActivatable
{
	public delegate void LaserPushedHandler();
	public event LaserPushedHandler OnLaserPushed;

	private AnimatedSprite2D _buttonAnimatedSprite;

	public override void _Ready()
	{
		_buttonAnimatedSprite = GetNode<AnimatedSprite2D>("ButtonAnimatedSprite");
	}

	public void Activate()
	{
		_buttonAnimatedSprite.Play("ButtonPressed");
		OnLaserPushed?.Invoke();
	}
}
