using Godot;

[Tool]
public partial class ItemCrate : StaticBody2D
{
	[Export]
	public ItemResource ItemResource
	{
		get => GetNode<ItemSprite>("ItemSprite").ItemResource;
		set => GetNode<ItemSprite>("ItemSprite").ItemResource = value;
	}
}
