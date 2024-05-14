

// GokuBracken.Core.GokuBrackenBase
using System.IO;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using System.Runtime.CompilerServices;
using LCSoundTool;
using System.Reflection;
using System;
using System.Linq;


[BepInPlugin(modGUID, modName, modVersion)]
[BepInDependency("LCSoundTool", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("CustomSounds")]
public class RustyModBase : BaseUnityPlugin
{
    private const string modGUID = "Eyasu.OurBuddyRusty";
    private const string modName = "Our Buddy Rusty";
    private const string modVersion = "1.0";
	//private static RustyModBase _instance;

	private readonly Harmony Harmony = new Harmony(modGUID);
    public static ManualLogSource mls;
    public static AssetBundle RustyBundle;
    public static Mesh shovelMesh;
    public static Material shovelMat;
    public static string location;
    public static string soundFolder;
	
    private static RustyModBase Instance;

	private void Awake()
	{
        mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        mls.LogInfo("AMOGUS SUS");
        if (Instance == null) {
        Instance = this;
        }

        
        location = Path.Join(Paths.PluginPath, "RustyModTEst");
        soundFolder = Path.Join(location, "sounds");
        mls.LogInfo("Plugin path: " + location);
        string bundlePath = Path.Join(location, "rusty");
        mls.LogInfo("Bundle path: " + bundlePath);
        RustyBundle = AssetBundle.LoadFromFile(bundlePath);
		mls.LogInfo("Loading asset bundle...");
		Assets.PopulateAssets();
        if(RustyBundle == null) {
            mls.LogError("RUSTY BUNDLE FAILED TO LOAD");
        }

        if(RustyBundle.Contains("Assets/rusty.fbx")) {mls.LogMessage("Contains: Assets/rusty.fbx");}
        if(RustyBundle.Contains("Assets/rusty")) {mls.LogMessage("Contains: Assets/rusty");}
        if(RustyBundle.Contains("rusty.fbx")) {mls.LogMessage("Contains: rusty.fbx");}
        if(RustyBundle.Contains("rusty")) {mls.LogMessage("Contains: rusty");}
        if(RustyBundle.Contains("")) {mls.LogMessage("Contains: ");}
        shovelMesh = RustyBundle.LoadAsset<Mesh>("rusty.fbx");
        mls.LogWarning("Rusty BUndle meshes: ");
        foreach(Mesh mesh in RustyBundle.LoadAllAssets<Mesh>()){
            mls.LogMessage(mesh.name);
        }

		Harmony.PatchAll(typeof(RustyModBase));
		Harmony.PatchAll(typeof(EnemyPatches));
        
        SoundPatches.Patch();

        try{
		    Harmony.PatchAll(typeof(SpawnObjectPatch));
			RustyModBase.mls.LogInfo("Patch applied");
        }catch(Exception ex) {
            mls.LogError("Spawn Object Patching Error: " + ex);
        }



            RustyModBase.mls.LogInfo("Sound Folder: " + RustyModBase.soundFolder);

            AudioClip none = SoundTool.GetAudioClip(RustyModBase.soundFolder, "none.mp3");
            AudioClip mainSystem = SoundTool.GetAudioClip(RustyModBase.soundFolder, "mainSystem.mp3");


            RustyModBase.mls.LogInfo("AAAAAAAAAS");
	}
}