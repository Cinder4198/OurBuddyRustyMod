using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

internal static class Assets
{
	public static AssetBundle AssetBundle { get; private set; }

	private static Dictionary<string, Object> AssetList { get; set; }

	private static string AssemblyName => Assembly.GetExecutingAssembly().FullName!.Split(new char[1] { ',' })[0];

	public static void PopulateAssets()
	{
		if ((Object)(object)AssetBundle != (Object)null)
		{
			RustyModBase.mls.LogWarning("Attempted to load the asset bundle but the bundle was not null!");
			return;
		}
		{
			AssetBundle = RustyModBase.RustyBundle;
		}
		if ((Object)(object)AssetBundle == (Object)null)
		{
			RustyModBase.mls.LogError("Asset bundle at " + RustyModBase.location + "\rusty failed to load!");
		}
		AssetList = new Dictionary<string, Object>();
		Object[] array = AssetBundle.LoadAllAssets();
        RustyModBase.mls.LogInfo("Assets from bundle: \n");
		foreach (Object val in array)
		{
			AssetList.Add(val.name, val);
            RustyModBase.mls.LogInfo(val.name);
		}
	}

	public static T GetAsset<T>(string name) where T : Object
	{
		if (!AssetList.TryGetValue(name, out var value))
		{
			RustyModBase.mls.LogError("Attempted to load asset of name " + name + " but no asset of that name exists!");
			return default(T);
		}
		T val = (T)(object)((value is T) ? value : null);
		if ((Object)(object)val == (Object)null)
		{
			RustyModBase.mls.LogError("Attempted to load an asset of type " + typeof(T).Name + " but asset of name " + name + " does not match this type!");
			return default(T);
		}
		return val;
	}
}