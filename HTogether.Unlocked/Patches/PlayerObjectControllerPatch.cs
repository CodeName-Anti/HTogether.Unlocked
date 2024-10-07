using HarmonyLib;
using System.Collections.Generic;

namespace HTogether.Patches;

[HarmonyPatch(typeof(PlayerObjectController))]
public static class PlayerObjectControllerPatch
{
	public static Dictionary<string, List<string>> MessageLogs = [];

	[HarmonyPatch(nameof(PlayerObjectController.UserCode_RpcReceiveChatMsg__String__String))]
	[HarmonyPrefix]
	public static void UserCode_RpcReceiveChatMsg__String__String(string playerName, string message)
	{
		if (!MessageLogs.TryGetValue(playerName, out List<string> messages))
		{
			messages = [];
			MessageLogs.Add(playerName, messages);
		}

		messages.Add(message);
	}

}
