

// GokuBracken.Scripts.GokuController
using System;
using System.Collections.Generic;
using UnityEngine;
using LCSoundTool;

public class RustyController : MonoBehaviour
{
	private RadMechAI RadMechAI { get; set; }

	private GameObject BaseRustyObject { get; set; }
	private Material spotlightMat { get; set; }
	private Material defaultMat { get; set; }

	private GameObject SecondaryGokuObject { get; set; }

	private bool takingStep {get; set;}
	private SkinnedMeshRenderer[] skinnedMeshRenderers { get; set; }

	private bool IsAttacking { get; set; }

	private bool IsDead { get; set; }
	//private string bundleLocation;

	private List<string> baseMatNames = new List<string> { "ArmLeft", "ArmRight", "Booster", "Core", "EyeArmL", "EyeArmR" };

	Animator vanillaRadMechAnimator;
	Animator rustyAnimator;
	AudioSource rustyAudioSource;
	int loggedMisslesFired = 0;

	AudioClip audioMainSystem;
	AudioClip audioAllOrNothing;
	AudioClip audioBuddy;
	AudioClip audioDontDie;
	AudioClip audioHeyBuddy;
	AudioClip audioHopeOrDispair;
	AudioClip audioIWontMiss;
	AudioClip audioImComing1;
	AudioClip audioImComing2;
	AudioClip audioKeepYouWaiting;
	AudioClip audioLockOn;
	AudioClip audioNoChoice;
	AudioClip audioNoGraverThreat;
	AudioClip audioOnlyICanFly;
	AudioClip audioRubicon;
	AudioClip audioStayCool;
	AudioClip audioStepUpMyGame;
	AudioClip audioUnderPressure;
	AudioClip audioWatchOut;
	AudioClip audioWhatDrivesYou;
	AudioClip audioYouCanDoBetter;
	AudioClip audioYouThereBuddy;
	AudioClip audioYoureFast;
	AudioClip audioYoureGood;

	List<AudioClip> voiceLines;

	int voiceCooldown = 0;

	/* anims

	1 = step 1
	2 = step 2
	3 = grab start
	4 = grab kill
	5 = shoot
	6 = boost
	7 = fly/idle

	*/

	private void Start()
	{

		RadMechAI = GetComponent<RadMechAI>();

		UpdateScanNodeData();
		try{
			//RadMechAI.gunArm.GetComponent<Renderer>().enabled = false;
		}catch(Exception ex) {
			RustyModBase.mls.LogError("RustyMod ARM ERROR: " + ex);
		}
		HideRadMechModel();
		CreateRustyModels();
		LoadSounds();
		rustyAudioSource.loop = true;
		rustyAudioSource.PlayOneShot(audioMainSystem);
		//rustyAudioSource.volume = 10;
	}

	


	private void Update()
	{
		try{

			bool isAggro = RadMechAI.currentBehaviourState.name == "Fly";

			if(RadMechAI.spotlight.activeSelf){
			RadMechAI.spotlight.SetActive(false);
			}

			try{

				if(isAggro && !rustyAudioSource.isPlaying) {
					rustyAudioSource.Play();
				}
				if(!isAggro && rustyAudioSource.isPlaying) {
					rustyAudioSource.Pause();
				}
				//RustyModBase.mls.LogInfo("RAD MECH ALERT: " + RadMechAI.isAlerted);
				//RustyModBase.mls.LogInfo("RAD MECH ALERT 2: " + RadMechAI.currentBehaviourState.name);


				//RustyModBase.mls.LogInfo("Rusty Voice Timer: " + voiceCooldown);

				if(isAggro) {
					
					if(voiceCooldown > 0) --voiceCooldown;
					else
					{
						int randomVal = new System.Random().Next(1, RustyModBase.BoundConfig.fastVoiceCooldown.Value ? 21 : 301);  //21);
						//RustyModBase.mls.LogDebug("Random voiceline roll; " + randomVal);
						if(randomVal == 1) {

							int playedVoiceInt = new System.Random().Next(1, voiceLines.Count);

							rustyAudioSource.PlayOneShot(voiceLines[playedVoiceInt]);
							voiceCooldown = RustyModBase.BoundConfig.fastVoiceCooldown.Value ? 30 : 450; //30;
						}
					}
				}

				
				if(rustyAnimator.GetInteger("currentAnim") == 7 || 
				(!RadMechAI.attemptingGrab && rustyAnimator.GetInteger("currentnim") == 3)
				) {
					rustyAnimator.SetInteger("currentAnim", 0);
				}
				if(!RadMechAI.chargingForward) {
					rustyAnimator.SetInteger("currentAnim", 7);
				}

				//RustyModBase.mls.LogInfo("Is taking Step: " + vanillaRadMechAnimator.GetBool("leftFootForward"));

				if(vanillaRadMechAnimator.GetInteger("currentAnim") < 3) {
					if(vanillaRadMechAnimator.GetBool("leftFootForward")){
						rustyAnimator.SetInteger("currentAnim", 2);
					} else {
						rustyAnimator.SetInteger("currentAnim", 1);
					}
				}

				if(RadMechAI.aimingGun) {

					if(RadMechAI.missilesFired != loggedMisslesFired) {

						rustyAnimator.SetInteger("currentAnim", 5);

					}

				}

				if(RadMechAI.chargingForward) {
					rustyAnimator.SetInteger("currentAnim", 6);
				}

				if(RadMechAI.attemptingGrab) {

					rustyAnimator.SetInteger("currentAnim", 3);

				}
				
				

			}catch(Exception ex) {
				RustyModBase.mls.LogError("Step tracker error: " + ex);
			}

		}catch(Exception ex ) {
			RustyModBase.mls.LogError("CANNOT CHANGE MATERIAL: " + ex);
		}
	}
	


	private void HideRadMechModel()
	{
		Renderer[] componentsInChildren;
		try{
			componentsInChildren = ((Component)(object)RadMechAI).transform.Find("MeshContainer").GetComponentsInChildren<Renderer>();

		//RustyModBase.mls.LogInfo("RadMech model parts:\n");
		//foreach(Renderer obj in componentsInChildren[0].GetComponentsInChildren<Renderer>()) {

				
				//RustyModBase.mls.LogInfo(obj.name);

		//}
		
		componentsInChildren[0].enabled = false;

		
		}catch(Exception ex) {
			RustyModBase.mls.LogError("RUSTY MOD HIDE RADMECH ERROR: " + ex.Message);
			return;
		}

	}

	private void CreateRustyModels()
	{
		try{ 
		GameObject assets = Assets.GetAsset<GameObject>("rustyPrefab");

		BaseRustyObject = UnityEngine.Object.Instantiate<GameObject>(assets, ((Component)this).gameObject.transform);
		BaseRustyObject.name = "RustyModel";

		try {
			rustyAudioSource = BaseRustyObject.GetComponentInChildren<AudioSource>();
		}catch(Exception ex) {
			RustyModBase.mls.LogError("Audio source errer: " + ex);
		}
		//RustyModBase.mls.LogInfo("RUSYT AUDIO SOURCE: " + rustyAudioSource.name);

		try {
		Vector3 vector = new Vector3(
			gameObject.transform.position.x, 
			gameObject.transform.position.y,
			gameObject.transform.position.z// + 16
		);

		foreach(Animator obj in RadMechAI.transform.GetComponentsInChildren<Animator>()) {
			//RustyModBase.mls.LogInfo("Component: " + obj.name + " | " + obj.GetType());
			if(obj.name == "AnimContainer"){
				vanillaRadMechAnimator = obj;
			}
		}

		BaseRustyObject.transform.SetPositionAndRotation(vector, gameObject.transform.rotation);
		BaseRustyObject.transform.localScale = new Vector3( 0.6f, 0.6f, 0.6f);

		}catch(Exception ex) {
			RustyModBase.mls.LogError("Positioning error: " + ex);
		}

		try {
			rustyAnimator = BaseRustyObject.GetComponentInChildren<Animator>();
		}catch(Exception ex) {
			RustyModBase.mls.LogError("Animator errer: " + ex);
		}

		//RustyModBase.mls.LogInfo("ANIMATOR NAME: " + rustyAnimator.name);

		//RustyModBase.mls.LogInfo("Rusty Components:");
		try {
			rustyAnimator.SetInteger("currentAnim", 0);
		}catch(Exception ex) {
			RustyModBase.mls.LogError("ANIMATOR ERROR: " + ex);
		}
		
		}catch(Exception ex) {
			RustyModBase.mls.LogError("RUSTY MODEL ERROR: " + ex);
		}

	}

	private void LoadSounds() {
		string voiceFolder = RustyModBase.soundFolder;

		voiceLines  = new List<AudioClip>{};

		audioMainSystem = SoundTool.GetAudioClip(RustyModBase.soundFolder, "mainSystem.mp3");
		audioAllOrNothing = SoundTool.GetAudioClip(voiceFolder, "allOrNothing.mp3");
		voiceLines.Add(audioAllOrNothing);
		audioBuddy = SoundTool.GetAudioClip(voiceFolder, "buddy.mp3");
		voiceLines.Add(audioBuddy);
		audioDontDie = SoundTool.GetAudioClip(voiceFolder, "dontDie.mp3");
		voiceLines.Add(audioDontDie);
		audioHeyBuddy = SoundTool.GetAudioClip(voiceFolder, "heyBuddy.mp3");
		voiceLines.Add(audioHeyBuddy);
		audioHopeOrDispair = SoundTool.GetAudioClip(voiceFolder, "hopeOrDispair.mp3");
		voiceLines.Add(audioHopeOrDispair);
		audioIWontMiss = SoundTool.GetAudioClip(voiceFolder, "iWontMiss.mp3");
		voiceLines.Add(audioIWontMiss);
		audioImComing1 = SoundTool.GetAudioClip(voiceFolder, "imComing1.mp3");
		voiceLines.Add(audioImComing1);
		audioImComing2 = SoundTool.GetAudioClip(voiceFolder, "imComing2.mp3");
		voiceLines.Add(audioImComing2);
		audioKeepYouWaiting = SoundTool.GetAudioClip(voiceFolder, "keepYouWaiting.mp3");
		voiceLines.Add(audioKeepYouWaiting);
		audioLockOn = SoundTool.GetAudioClip(voiceFolder, "lockOn.mp3");
		voiceLines.Add(audioLockOn);
		audioNoChoice = SoundTool.GetAudioClip(voiceFolder, "noChoice.mp3");
		voiceLines.Add(audioNoChoice);
		audioNoGraverThreat = SoundTool.GetAudioClip(voiceFolder, "noGraverThreat.mp3");
		voiceLines.Add(audioNoGraverThreat);
		audioOnlyICanFly = SoundTool.GetAudioClip(voiceFolder, "onlyICanFly.mp3");
		voiceLines.Add(audioOnlyICanFly);
		audioRubicon = SoundTool.GetAudioClip(voiceFolder, "rubicon.mp3");
		voiceLines.Add(audioRubicon);
		audioStayCool = SoundTool.GetAudioClip(voiceFolder, "stayCool.mp3");
		voiceLines.Add(audioStayCool);
		audioStepUpMyGame = SoundTool.GetAudioClip(voiceFolder, "stepUpMyGame.mp3");
		voiceLines.Add(audioStepUpMyGame);
		audioUnderPressure = SoundTool.GetAudioClip(voiceFolder, "underPressure.mp3");
		voiceLines.Add(audioUnderPressure);
		audioWatchOut = SoundTool.GetAudioClip(voiceFolder, "watchOut.mp3");
		voiceLines.Add(audioWatchOut);
		audioWhatDrivesYou = SoundTool.GetAudioClip(voiceFolder, "whatDrivesYou.mp3");
		voiceLines.Add(audioWhatDrivesYou);
		audioYouCanDoBetter = SoundTool.GetAudioClip(voiceFolder, "youCanDoBetter.mp3");
		voiceLines.Add(audioYouCanDoBetter);
		audioYouThereBuddy = SoundTool.GetAudioClip(voiceFolder, "youThereBuddy.mp3");
		voiceLines.Add(audioYouThereBuddy);
		audioYoureFast = SoundTool.GetAudioClip(voiceFolder, "youreFast.mp3");
		voiceLines.Add(audioYoureFast);
		audioYoureGood = SoundTool.GetAudioClip(voiceFolder, "youreGood.mp3");
		voiceLines.Add(audioYoureGood);
		
	}

	public void TakeStep1()
	{
		//RustyModBase.mls.LogInfo("RIGHT FOOT LETS STOMP");
		rustyAnimator.SetBool("step2", false);
		rustyAnimator.SetBool("step1", true);
	}
	public void TakeStep2()
	{
		//RustyModBase.mls.LogInfo("LEFT FOOT LETS STOMP");
		rustyAnimator.SetBool("step1", false);
		rustyAnimator.SetBool("step2", true);
	}

	private void UpdateScanNodeData()
	{
		((Component)(object)RadMechAI).GetComponentInChildren<ScanNodeProperties>().headerText = "STEEL HAZE /\nV.IV Rusty";
	}
}
