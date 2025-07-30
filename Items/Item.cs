using Godot;

public partial class Item : StaticBody2D
{
	[Export]
	public ItemResource ItemResource { get; set; }

	public override void _Ready()
	{
		GetNode<Sprite2D>("ItemSprite").Texture = ItemResource.ItemTexture;
	}
}
