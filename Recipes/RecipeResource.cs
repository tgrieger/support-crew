using Godot;
using Godot.Collections;

[Tool]
[GlobalClass]
public partial class RecipeResource : Resource
{
	[Export]
	public Array<ItemResource> InputResources { get; set; }

	[Export]
	public ItemResource OutputResource { get; set; }
}
