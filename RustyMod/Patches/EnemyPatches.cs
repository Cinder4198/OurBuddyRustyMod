using System;
using BepInEx.Logging;
using HarmonyLib;
using RustyMod;
using UnityEngine;

[HarmonyPatch]
internal class EnemyPatches
{
	[HarmonyPatch(typeof(EnemyAI), "Start")]
	[HarmonyPostfix]
	public static void CreateRustyModel(EnemyAI __instance)
	{
		if (!(__instance is RadMechAI))
			{
				return;
			}

			//if ((Component)(object)__instance.GetComponent<RustyController>() == null) {

				((Component)(object)__instance).gameObject.AddComponent<RustyController>();

			//}
	}
}