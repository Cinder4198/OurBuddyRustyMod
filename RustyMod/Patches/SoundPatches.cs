using System;
using LCSoundTool;
using UnityEngine;

internal static class SoundPatches {

    public static void Patch() {

        try{

            RustyModBase.mls.LogInfo("Sound Folder: " + RustyModBase.soundFolder);

            AudioClip none = SoundTool.GetAudioClip(RustyModBase.soundFolder, "none.mp3");
            AudioClip mainSystem = SoundTool.GetAudioClip(RustyModBase.soundFolder, "mainSystem.mp3");



            SoundTool.ReplaceAudioClip("robotTune", none, "3DLradAudio2");
            SoundTool.ReplaceAudioClip("RadMechAmbientSFX", none, "EngineSFX");
            SoundTool.ReplaceAudioClip("NeonLightFlicker", none, "VoiceSFX");
            SoundTool.ReplaceAudioClip("LRADAlarm3", none, "3DLradAudio");
            SoundTool.ReplaceAudioClip("ToWar", none, "3DLradAudio2");
            SoundTool.ReplaceAudioClip("LradBrainwashingSignal1", none, "3DLradAudio2");
            SoundTool.ReplaceAudioClip("LradBrainwashingSignal4", none, "3DLradAudio2");
            SoundTool.ReplaceAudioClip("LradBrainwashingSignal6", none, "3DLradAudio2");
            SoundTool.ReplaceAudioClip("LradBrainwashingSignal7", none, "3DLradAudio2");
            SoundTool.ReplaceAudioClip("LradBrainwashingSignal8", none, "3DLradAudio2");

            SoundTool.ReplaceAudioClip("RadMechWake", mainSystem, "CreatureSFX");


            RustyModBase.mls.LogInfo("AAAAAAAAAS");

        }catch(Exception ex) {
            RustyModBase.mls.LogError("SOUND ERROR: " + ex);
        }

    }


}