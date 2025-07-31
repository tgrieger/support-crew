using Godot;

[Tool]
public partial class Item : StaticBody2D
{
	[Export]
	public ItemResource ItemResource
	{
		get => GetNode<ItemSprite>("ItemSprite").ItemResource;
		set => GetNode<ItemSprite>("ItemSprite").ItemResource = value;
	}
}