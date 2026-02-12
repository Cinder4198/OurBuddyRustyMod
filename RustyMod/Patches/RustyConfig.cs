using System.Collections.Generic;
using System.Reflection;
using BepInEx.Configuration;
using HarmonyLib;

namespace RustyModUtils;


public class RustyConfig
{
    public readonly ConfigEntry<bool> fastVoiceCooldown;
    public readonly ConfigEntry<bool> logEntry;

    public RustyConfig(ConfigFile config)
    {
        //disable saving the config on each individual bind
        config.SaveOnConfigSet = false;
        
        //reads the config file
        fastVoiceCooldown = config.Bind("General", "FastVoiceCooldown", false, "Switch to using the bugged voiceline cooldown timings");
        logEntry = config.Bind("General", "LogEntry", true, "Replaces the Old Bird bestiary entry. This may conflict with other mods, so try disabling it if there's issues with the terminal.");

        //deletes unused config
        ClearOrphanedEntries(config);

        config.Save();
        
        //reenables saving on config bind
        config.SaveOnConfigSet = true;
    }
    
    static void ClearOrphanedEntries(ConfigFile cfg) 
    { 
        // Find the private property `OrphanedEntries` from the type `ConfigFile` //
        PropertyInfo orphanedEntriesProp = AccessTools.Property(typeof(ConfigFile), "OrphanedEntries"); 
        // And get the value of that property from our ConfigFile instance //
        var orphanedEntries = (Dictionary<ConfigDefinition, string>)orphanedEntriesProp.GetValue(cfg); 
        // And finally, clear the `OrphanedEntries` dictionary //
        orphanedEntries.Clear(); 
    } 
    
}