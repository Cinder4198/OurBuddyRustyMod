using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using RustyMod;
using UnityEngine;
using UnityEngine.Video;

namespace RustyModUtils;
internal static class Assets
	{
		public static AssetBundle AssetBundle { get; private set; }

		private static Dictionary<string, UnityEngine.Object> AssetList { get; set; }

		private static string AssemblyName => Assembly.GetExecutingAssembly().FullName.Split(new char[1] { ',' })[0];

		public static void PopulateAssets()
		{
			if ((UnityEngine.Object)(object)AssetBundle != (UnityEngine.Object)null)
			{
				RustyModBase.mls.LogWarning("Attempted to load the asset bundle but the bundle was not null!");
				return;
			}
			string name = AssemblyName + ".Bundle.gokubracken";
			using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(name))
			{
				AssetBundle = AssetBundle.LoadFromStream(stream);
			}
			if ((UnityEngine.Object)(object)AssetBundle == (UnityEngine.Object)null)
			{
				RustyModBase.mls.LogError("Asset bundle at " + AssemblyName + ".gokubracken failed to load!");
			}
			AssetList = new Dictionary<string, UnityEngine.Object>();
			UnityEngine.Object[] array = AssetBundle.LoadAllAssets();
			foreach (UnityEngine.Object val in array)
			{
				AssetList.Add(val.name, val);
			}
		}
    }
	