using Godot;

[Tool]
[GlobalClass]
public partial class PlayerResource : Resource
{
	[Export]
	public string Up { get; set; }

	[Export]
	public string Down { get; set; }

	[Export]
	public string Left { get; set; }

	[Export]
	public string Right { get; set; }

	[Export]
	public string Interact { get; set; }

	[Export]
	public string Activate { get; set; }

	[Export]
	public string Sprint { get; set; }
}
