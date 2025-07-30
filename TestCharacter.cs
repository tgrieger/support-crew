using Godot;

public partial class TestCharacter : CharacterBody2D
{
	[Export] // Allows you to modify the speed in the editor
	public float Speed { get; set; } = 200.0f;

	private AnimatedSprite2D _playerAnimatedSprite;

	public override void _Ready()
	{
		_playerAnimatedSprite = GetNode<AnimatedSprite2D>("PlayerAnimatedSprite");
	}

	public override void _PhysicsProcess(double delta)
	{
		// Get input direction
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Set velocity based on input and speed
		Velocity = inputDirection * Speed;
		if (inputDirection.Y < 0)
		{
			_playerAnimatedSprite.Play("Up");
		}
		else if (inputDirection.Y > 0)
		{
			_playerAnimatedSprite.Play("Down");
		}
		else if (inputDirection.X == 1)
		{
			_playerAnimatedSprite.Play("Left");
		}
		else if (inputDirection.X == -1)
		{
			_playerAnimatedSprite.Play("Right");
		}

		// Move the character and handle collisions
		MoveAndSlide();
	}
}
