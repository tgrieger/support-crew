using Godot;
using System;

[Tool]
[GlobalClass]
public partial class ItemResource : Resource
{
	[Export]
	public string Name { get; set; }

	[Export]
	public Texture2D ItemTexture { get; set; }
}
