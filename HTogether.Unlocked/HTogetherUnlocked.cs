using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using HTogether.Utils;
using ImGuiNET;
using System;
using System.Reflection;

namespace HTogether;

[BepInPlugin(UnlockedPluginInfo.PLUGIN_GUID, UnlockedPluginInfo.PLUGIN_NAME, UnlockedPluginInfo.PLUGIN_VERSION)]
[BepInDependency("HTogether", "1.0.4")]
public class HTogetherUnlocked : BaseUnityPlugin
{
	public static new ManualLogSource Logger;

	public Harmony HarmonyInstance { get; private set; }

	private readonly HTogether hTogether = HTogether.Instance;
	private bool updateAvailable;

	private static void CreateTab(string name, out int tabId)
	{
		tabId = HTogether.Instance.Renderer.CreateTab(name);
	}

	private void Awake()
	{
		// Plugin startup logic
		Logger = base.Logger;
		HTogether.LockdownFeatures = false;

		try
		{
			HarmonyInstance = new Harmony(MyPluginInfo.PLUGIN_GUID);

			HarmonyInstance.PatchAll();
		}
		catch (Exception ex)
		{
			Logger.LogError("Harmony patching error: " + ex.ToString());
		}

		try
		{
			updateAvailable = UpdateChecker.IsUpdateAvailable("CodeName-Anti", "HTogether.Unlocked", UnlockedPluginInfo.PLUGIN_VERSION);
		}
		catch (Exception ex)
		{
			HTogether.Logger.LogError($"Error fetching GitHub releases: {ex}");
		}

		CreateTab("Exploits", out Tabs.Exploits);
		CreateTab("Playerlist", out Tabs.Playerlist);

		hTogether.ModuleManager.RegisterModules(Assembly.GetExecutingAssembly());

		hTogether.Renderer.OnDrawIntro += DrawIntro;
	}

	private void DrawIntro()
	{
		ImGui.BulletText($"HTogether.Unlocked {UnlockedPluginInfo.PLUGIN_VERSION} loaded.");

		if (updateAvailable)
		{
			hTogether.Renderer.DrawUpdateAvailable("HTogether.Unlocked", "https://github.com/CodeName-Anti/HTogether.Unlocked");
		}
	}
}
