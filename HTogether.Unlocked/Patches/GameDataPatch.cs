using HarmonyLib;
using System.Collections.Generic;

namespace HTogether.Patches;

[HarmonyPatch(typeof(GameData))]
public static class GameDataPatch
{
	public static Dictionary<int, string> ProductNames = [];

	[HarmonyPatch(nameof(GameData.Awake))]
	[HarmonyPostfix]
	public static void Awake(GameData __instance)
	{
		ProductNames.Clear();

		ProductListing listing = __instance.GetComponent<ProductListing>();

		for (int i = 0; i < listing.productPrefabs.Length; i++)
		{
			string name = listing.productPrefabs[i].name;
			name = name.Substring(name.IndexOf('_') + 1);

			ProductNames.Add(i, name);
		}
	}

}
