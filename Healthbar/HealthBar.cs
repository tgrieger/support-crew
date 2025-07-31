using Godot;

public partial class HealthBar : ProgressBar
{
	private ProgressBar _healthBar;
	private Timer HealthTimer;
	
	public override void _Ready()
	{
		_healthBar = GetNode<ProgressBar>("/root/Node2D/HealthControl/HealthBar");
		_healthBar.Value = 100.0f;
	}
	
	public void _on_timer_timeout()
	{
		_healthBar.Value-=10;
	}
}
