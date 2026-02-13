

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
using RustyModUtils;
using UnityEngine.Video;


[BepInPlugin(modGUID, modName, modVersion)]
[BepInDependency("LCSoundTool", BepInDependency.DependencyFlags.HardDependency)]
[BepInDependency("CustomSounds")]
[BepInDependency("TerminalApi")]
public class RustyModBase : BaseUnityPlugin
{
    private const string modGUID = "Eyasu.OurBuddyRusty";
    private const string modName = "Our Buddy Rusty";
    private const string modVersion = "1.6";
	//private static RustyModBase _instance;

	private readonly Harmony Harmony = new Harmony(modGUID);
    public static ManualLogSource mls;
    public static AssetBundle RustyBundle;
    public static Mesh shovelMesh;
    public static VideoClip rustyVideo;
    public static Material shovelMat;
    public static string location;
    public static string soundFolder;
	
    private static RustyModBase Instance;
    internal static RustyConfig BoundConfig { get; private set; } = null;

	private void Awake()
	{
        mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
        mls.LogDebug("AMOGUS SUS");
        if (Instance == null) {
			Instance = this;
        }

        //define paths
        location = Path.Join(Paths.PluginPath, "Eyasu-OurBuddyRusty");
        soundFolder = location; //Path.Join(location, "sounds");
        mls.LogDebug("Plugin path: " + location);
        string bundlePath = Path.Join(location, "rusty");
        
        //apply config
        BoundConfig = new RustyConfig(base.Config);
        //var configVoiceCooldown = base.Config.Bind("General", "FastVoiceCooldown", false, "Switch to using the bugged cooldown timings");
        
        
        //load bundle
        mls.LogDebug("Bundle path: " + bundlePath);
	    RustyBundle = AssetBundle.LoadFromFile(bundlePath);
	    
	    //load assets from bundle
        mls.LogDebug("Loading asset bundle...");
		Assets.PopulateAssets();
        if(RustyBundle == null) {
            mls.LogError("RUSTY BUNDLE FAILED TO LOAD");
        }

        if(RustyBundle.Contains("Assets/rusty.fbx")) {mls.LogDebug("Contains: Assets/rusty.fbx");}
        if(RustyBundle.Contains("Assets/rusty")) {mls.LogDebug("Contains: Assets/rusty");}
        if(RustyBundle.Contains("rusty.fbx")) {mls.LogDebug("Contains: rusty.fbx");}
        if(RustyBundle.Contains("rusty")) {mls.LogDebug("Contains: rusty");}
        if(RustyBundle.Contains("")) {mls.LogDebug("Contains: ");}
        if(RustyBundle.Contains("rustyVideo.webm")) {mls.LogDebug("Contains: rustyVideo.webm");}
        if(RustyBundle.Contains("rustyVideo")) {mls.LogDebug("Contains: rustyVideo");}
        if(RustyBundle.Contains("rustyTerminal.webm")) {mls.LogDebug("Contains: rustyTerminal.webm");}
        if(RustyBundle.Contains("rustyTerminal")) {mls.LogDebug("Contains: rustyTerminal");}
        shovelMesh = RustyBundle.LoadAsset<Mesh>("rusty.fbx");
        mls.LogDebug("Rusty BUndle meshes: ");
        foreach(Mesh mesh in RustyBundle.LoadAllAssets<Mesh>()){
            mls.LogDebug(mesh.name);
        }
        rustyVideo = Assets.GetAsset<VideoClip>("rustyVideo");

        //add patches
		Harmony.PatchAll(typeof(RustyModBase));
		Harmony.PatchAll(typeof(EnemyPatches));
		Harmony.PatchAll(typeof(RustyTerminal));
        
        SoundPatches.Patch();

        try{
		    Harmony.PatchAll(typeof(SpawnObjectPatch));
			RustyModBase.mls.LogDebug("Patch applied");
        }catch(Exception ex) {
            mls.LogError("Spawn Object Patching Error: " + ex);
        }



            RustyModBase.mls.LogDebug("Sound Folder: " + RustyModBase.soundFolder);

            AudioClip none = SoundTool.GetAudioClip(RustyModBase.soundFolder, "none.mp3");
            AudioClip mainSystem = SoundTool.GetAudioClip(RustyModBase.soundFolder, "mainSystem.mp3");


            RustyModBase.mls.LogDebug("AAAAAAAAAS");
	}
}