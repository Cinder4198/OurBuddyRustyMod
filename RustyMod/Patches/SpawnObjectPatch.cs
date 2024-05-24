using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using BepInEx.Logging;
using HarmonyLib;
using RustyMod;
using UnityEngine;
using UnityEngine.SceneManagement;

[HarmonyPatch]
internal class SpawnObjectPatch
{
	[HarmonyPatch(typeof(RoundManager), "SyncNestSpawnPositionsClientRpc")]
	[HarmonyPostfix]
	public static void GetMapObject(RoundManager __instance){

		//if(enemyType.enemyName != "RadMech") return;

        try{

			RustyModBase.mls.LogInfo("Patching RadMech spawn");

			RustyModBase.mls.LogInfo("Testing testing: " + __instance.enemyNestSpawnObjects.Count);

			List<EnemyAINestSpawnObject> gameObjects = __instance.enemyNestSpawnObjects;
			

			foreach(EnemyAINestSpawnObject obj in gameObjects) {

				RustyModBase.mls.LogInfo("Game Object: " + obj.name);

			}

			/*foreach(GameObject obj in gameObjects) {
				RustyModBase.mls.LogInfo("Object Name: " + obj.name);
				if(obj.transform.GetComponent<EnemyAINestSpawnObject>()) spawnObjects.Add(obj.transform.GetComponent<EnemyAINestSpawnObject>());
			}*/

			foreach(EnemyAINestSpawnObject obj in gameObjects) {
				
				RustyModBase.mls.LogInfo("Outside Object: " + obj.enemyType);

				if(obj.enemyType.enemyName != "RadMech") continue;



            EnemyAINestSpawnObject gameObject = obj;

            Renderer[] componentsInChildren;

		    try{
                //HideRadMechModel
		    	componentsInChildren = obj.transform.Find("MeshContainer").GetComponentsInChildren<Renderer>();

		        //RustyModBase.mls.LogInfo("RadMech model parts:\n");
		        //foreach(Renderer obj in componentsInChildren[0].GetComponentsInChildren<Renderer>()) {

				
				//RustyModBase.mls.LogInfo(obj.name);

		        //}
		
		        componentsInChildren[0].enabled = false;
				obj.GetComponentInChildren<ScanNodeProperties>().headerText = "STEEL HAZE /\nV.IV Rusty";

		
		        }catch(Exception ex) {
			    RustyModBase.mls.LogError("RUSTY MOD HIDE RADMECH ERROR: " + ex.Message);
		    }
            

            //CreateRustyModel
			RustyModBase.mls.LogInfo("Adding rusty to inactive mechs");
            GameObject assets = Assets.GetAsset<GameObject>("rustyPrefab");

		    GameObject BaseRustyObject = UnityEngine.Object.Instantiate<GameObject>(assets, gameObject.transform);
		    BaseRustyObject.name = "RustyModel";

			RustyModBase.mls.LogInfo("Rusty Position: " + BaseRustyObject.transform.position);
			RustyModBase.mls.LogInfo("Old Bird Position: " + gameObject.transform.position);

			BaseRustyObject.transform.localScale = new Vector3( 0.6f, 0.6f, 0.6f);
			//BaseRustyObject.transform.localPosition = gameObject.transform.localPosition;
			RustyModBase.mls.LogInfo("Rusty Position 2: " + BaseRustyObject.transform.localPosition);
			RustyModBase.mls.LogInfo("Old Bird Position 2: " + gameObject.transform.localPosition);
			//BaseRustyObject.transform.rotation = gameObject.transform.rotation;

			}

            }catch(Exception ex) {
                RustyModBase.mls.LogError("Spawn Object Error: " + ex);
            }

			RustyModBase.mls.LogInfo("RadMech spawn patched");

        }
}