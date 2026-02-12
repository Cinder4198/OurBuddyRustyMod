
using System;
using System.IO;
using HarmonyLib;
using static RustyModUtils.Assets;
using Mono.Cecil;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;
using UnityEngine.Video;

[HarmonyPatch]

public class RustyTerminal
{
    [HarmonyPatch(typeof(Terminal), "Start")]
    [HarmonyPostfix]
    public static void EditTerminal(Terminal __instance)
    {
	    //RustyModBase.mls.LogError("TERMINAL PATCH");
	
	    if (!RustyModBase.BoundConfig.logEntry.Value) return;
	    int index = __instance.enemyFiles.FindIndex((TerminalNode e) => ((UnityEngine.Object)e).name == "RadMechFile");
	    //int brackenindex = __instance.enemyFiles.FindIndex((TerminalNode e) => ((UnityEngine.Object)e).name == "BrackenFile");
	    //int num = __instance.terminalNodes.allKeywords.ToList()
	    //.FindIndex((TerminalKeyword e) => ((Object)e).name == "old birds");
		TerminalNode oldBirdNode = __instance.enemyFiles[index];
		//TerminalNode brackenNode = __instance.enemyFiles[brackenindex];
	    oldBirdNode.creatureName = "STEEL HAZE / V.IV Rusty";
	    oldBirdNode.displayText = "STEEL HAZE / V.IV Rusty\n\nHIGH AND DRYYYYYYYYYYYYY\nI FLY HIIIIIIIIIIIIIIIIIGH\nWATCH THE SUUUUUUUUUN RIIIIIIIIIISE\nIIIIIIIIINTOOOO DAAAAAAAWNNNN\n\nRUST AWAAAAAAAAAAYYYYYYYYYY\nLOST TO WIIIIIIIIIIIIIIIIIND\nWATCH THE SUUUUUUUUUUUN RIIIIIIIISE\nRUUUUUUUUUUSTED PRIIIIIIIIIIDE\n\n";
	    //oldBirdNode.displayText = "STEEL HAZE / V.IV Rusty\n\n\n\nHate to say it, but...\n\nRubicon still needs me.\n\nSo, buddy...\n\nwho needs you?";
	    //__instance.terminalNodes.allKeywords[num].word = "rusty";
		
		

		int randomVal = new System.Random().Next(0, 20);  //21);
	    
	    //RustyModBase.mls.LogError("Path: " + Path.Join(RustyModBase.location, "rustyVideo.mp4"));

		//RustyModBase.mls.LogDebug("Default File: " + clip.originalPath);

		//VideoClip vid = 

		if(randomVal == 0) {
	    oldBirdNode.displayVideo = Assets.GetAsset<VideoClip>("rustyVideo"); //brackenNode.displayVideo;
		oldBirdNode.loadImageSlowly = true;
		} else {
	    oldBirdNode.displayVideo = Assets.GetAsset<VideoClip>("rustyTerminal"); //brackenNode.displayVideo;
		oldBirdNode.loadImageSlowly = false;
		}
		
		TerminalKeyword keyword;
		try{
	    	keyword = TerminalApi.TerminalApi.GetKeyword("old birds");
	    	keyword.word = "rusty";
		}catch(NullReferenceException ex){
			keyword	 = TerminalApi.TerminalApi.GetKeyword("rusty");
		}
		
	    TerminalApi.TerminalApi.UpdateKeyword(keyword);

	    RustyModBase.mls.LogDebug("TERMINAL PATCHED (" + randomVal + ")");
    }

}