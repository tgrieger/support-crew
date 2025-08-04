using Godot;

[Tool]
[GlobalClass]
public partial class AbilityResource : Resource
{
	[Export]
	public string AbilityName { get; set; }

	[Export]
	public double CooldownTime { get; set; }

	[Export]
	public double ExpirationTime { get; set; }

	[Export]
	public Texture2D AbilityTexture { get; set; }
}
