using Godot;

[Tool]
public partial class ItemSprite : Sprite2D
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
				SetItemSprite();
			}
		}
	}

	public override void _Ready()
	{
		SetItemSprite();
	}

	private void SetItemSprite()
	{
		Texture = _itemResource?.ItemTexture;
	}
}
