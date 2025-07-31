using Godot;

[Tool]
public partial class ItemCrate : StaticBody2D
{
	[Export]
	public PackedScene Item { get; set; }

	[Export]
	public ItemResource ItemResource
	{
		get => GetNode<ItemSprite>("ItemSprite").ItemResource;
		set => GetNode<ItemSprite>("ItemSprite").ItemResource = value;
	}

	public Item GetItem()
	{
		Item item = Item.Instantiate<Item>();
		item.ItemResource = ItemResource;
		return item;
	}
}
