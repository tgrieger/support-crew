using Godot;

namespace SupportCrew.Items;

[Tool]
public partial class Item : StaticBody2D
{
	private ItemResource _itemResource;

	[Export]
	public ItemResource ItemResource
	{
		get => _itemResource;
		set
		{
			_itemResource = value;
			if (Engine.IsEditorHint())
			{
				SetItemSprite(_itemResource?.ItemTexture);
			}
		}
	}

	public override void _Ready()
	{
		SetItemSprite(_itemResource?.ItemTexture);
	}

	private void SetItemSprite(Texture2D texture)
	{
		GetNode<Sprite2D>("ItemSprite").Texture = texture;
	}
}