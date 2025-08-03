using Godot;

[Tool]
public partial class Durability : ProgressBar
{
	private readonly StyleBoxFlat _styleBoxFlat = new();
	private double _durabilityPercentage;

	[Export]
	public double DurabilityPercentage
	{
		get => _durabilityPercentage;
		set
		{
			_durabilityPercentage = value;
			Value = value;
			_styleBoxFlat.BgColor = GetProgressBarColor();
		}
	}

	public override void _Ready()
	{
		Value = DurabilityPercentage;
		_styleBoxFlat.BgColor = GetProgressBarColor();
		AddThemeStyleboxOverride("fill", _styleBoxFlat);
	}

	private Color GetProgressBarColor()
	{
		float green = (float)_durabilityPercentage * .01f;
		float red = 1 - green;
		return new Color(red, green, b: 0);
	}
}
