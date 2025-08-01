using System;
using System.Collections.Generic;
using System.Linq;
using Godot;

public class RecipeManager
{
	private static readonly Dictionary<string, ItemResource> _recipeMap = new();

	public static RecipeManager Instance { get; private set; }

	static RecipeManager()
	{
		string resourceDir = "res://Recipes/";
		DirAccess dir = DirAccess.Open(resourceDir);
		if (dir is null)
		{
			throw new Exception("Could not open recipes");
		}

		dir.ListDirBegin();
		for (string fileName = dir.GetNext(); !string.IsNullOrWhiteSpace(fileName); fileName = dir.GetNext())
		{
			if (!fileName.EndsWith(".tres"))
			{
				continue;
			}

			try
			{
				RecipeResource recipe = ResourceLoader.Load<RecipeResource>(resourceDir.PathJoin(fileName));
				string key = CreateRecipeKey(recipe.InputResources);
				if (_recipeMap.ContainsKey(key))
				{
					throw new Exception("Duplicate recipe key");
				}

				_recipeMap.Add(key, recipe.OutputResource);
			}
			catch (InvalidCastException) {}
		}

		dir.ListDirEnd();
	}

	public static ItemResource GetRecipeOutput(params ItemResource[] items)
	{
		string key = CreateRecipeKey(items);
		return _recipeMap.GetValueOrDefault(key);
	}

	private static string CreateRecipeKey(IEnumerable<ItemResource> items)
	{
		return string.Join(string.Empty, items.Select(i => i.Name).Order());
	}
}
