using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using HarmonyLib;
using RustyMod;
using UnityEngine;

[HarmonyPatch]
internal class SpawnObjectPatch
{
	[HarmonyPatch(typeof(RoundManager), "SpawnNestObjectForOutsideEnemy")]
	[HarmonyPostfix]
	public static void GetMapObject(RoundManager __instance, EnemyType enemyType){

		if(enemyType.enemyName != "RadMech") return;

        try{

			RustyModBase.mls.LogInfo("Patching RadMech spawn");

			List<EnemyAINestSpawnObject> spawnObjects = __instance.enemyNestSpawnObjects;

			foreach(EnemyAINestSpawnObject obj in spawnObjects) {
				
				RustyModBase.mls.LogInfo("Outside Object: " + obj.enemyType);

			}

            EnemyAINestSpawnObject gameObject = __instance.enemyNestSpawnObjects.Last();

            Renderer[] componentsInChildren;
		    try{
                //HideRadMechModel
		    	componentsInChildren = __instance.enemyNestSpawnObjects.Last().transform.Find("MeshContainer").GetComponentsInChildren<Renderer>();

		        //RustyModBase.mls.LogInfo("RadMech model parts:\n");
		        //foreach(Renderer obj in componentsInChildren[0].GetComponentsInChildren<Renderer>()) {

				
				//RustyModBase.mls.LogInfo(obj.name);

		        //}
		
		        componentsInChildren[0].enabled = false;
				__instance.enemyNestSpawnObjects.Last().GetComponentInChildren<ScanNodeProperties>().headerText = "STEEL HAZE /\nV.IV Rusty";

		
		        }catch(Exception ex) {
			    RustyModBase.mls.LogError("RUSTY MOD HIDE RADMECH ERROR: " + ex.Message);
		    }
            

            //CreateRustyModel
			RustyModBase.mls.LogInfo("Adding rusty to inactive mechs");
            GameObject assets = Assets.GetAsset<GameObject>("rustyPrefab");

		    GameObject BaseRustyObject = UnityEngine.Object.Instantiate<GameObject>(assets, gameObject.transform);
		    BaseRustyObject.name = "RustyModel";
			BaseRustyObject.transform.localScale = new Vector3( 0.6f, 0.6f, 0.6f);

            }catch(Exception ex) {
                RustyModBase.mls.LogError("Spawn Object Error: " + ex);
            }

			RustyModBase.mls.LogInfo("RadMech spawn patched");

        }
}