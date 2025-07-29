using Godot;

public partial class TestCharacter : CharacterBody2D
{
	[Export] // Allows you to modify the speed in the editor
	public float Speed { get; set; } = 200.0f;

	public override void _PhysicsProcess(double delta)
	{
		// Get input direction
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

		// Set velocity based on input and speed
		Velocity = inputDirection * Speed;

		// Move the character and handle collisions
		MoveAndSlide();
	}
}
