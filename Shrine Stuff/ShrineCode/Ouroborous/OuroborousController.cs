﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dungeonator;
using Gungeon;
using GungeonAPI;
using ItemAPI;
using UnityEngine;
using SaveAPI;
using Brave.BulletScript;

namespace Planetside
{

	public class Ouroborous : ETGModule
	{

		public static bool LoopingOn;
		bool disabled = false;


		public override void Exit()
		{
		}

		public override void Start()
		{
			try
			{
				this.CreateOrLoadConfiguration();
			}
			catch (Exception e)
			{
				Tools.PrintException(e, "FF0000");
			}
		}

		private void CreateOrLoadConfiguration()
		{
			bool flag = !File.Exists(SaveFilePath);
			if (flag)
			{
				global::ETGModConsole.Log("", false);
				Directory.CreateDirectory(ConfigDirectory);
				File.Create(SaveFilePath).Close();
				UpdateConfiguration();
			}
			else
			{
				string text = File.ReadAllText(SaveFilePath);
				bool flag2 = !string.IsNullOrEmpty(text);
				if (flag2)
				{
					File.WriteAllText(SaveFilePath, EnableLooping);
				}
				else
				{
					this.UpdateConfiguration();
				}
			}
		}
		public void UpdateConfiguration()
		{
			bool flag = !File.Exists(SaveFilePath);
			if (flag)
			{
				global::ETGModConsole.Log("", false);
				Directory.CreateDirectory(ConfigDirectory);
				File.Create(SaveFilePath).Close();
			}
			File.WriteAllText(SaveFilePath, EnableLooping);
		}
		private static string ConfigDirectory = Path.Combine(global::ETGMod.ResourcesDirectory, "psogconfig");
		public static string SaveFilePath = Path.Combine(ConfigDirectory, "loopingenabled.json");
		private static string EnableLooping = LoopingOn ? "true" : "false";
		public override void Init()
		{
			if (File.Exists(SaveFilePath))
			{
				string[] lines = File.ReadAllLines(SaveFilePath);
				if (lines.Contains("true"))
				{

					LoopingOn = true;

				}
				else
				{
					LoopingOn = false;
				}
			}
			else
			{
				LoopingOn = false;

			}
			global::ETGModConsole.Commands.AddGroup("psog", delegate (string[] args)
			{
				global::ETGModConsole.Log("Please specify a command.", false);
			});
			global::ETGModConsole.Commands.GetGroup("psog").AddUnit("toggleloops", delegate (string[] args)
			{
				if (LoopingOn == true)
				{
					File.WriteAllText(SaveFilePath, "false");
					LoopingOn = false;
					global::ETGModConsole.Log("Ouroborous Disabled.", false);
				}
				else
				{
					LoopingOn = true;
					File.WriteAllText(SaveFilePath, "true");
					float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
					global::ETGModConsole.Log("Ouroborous set to: " + Loop, false);
				}
			});
			global::ETGModConsole.Commands.GetGroup("psog").AddUnit("unlock_all", delegate (string[] args)
			{
				ETGModConsole.Log("<size=100><color=#ff0000ff>*Hits locks with oversized hammer*</color></size>", false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.JAMMED_GUARD_DEFEATED, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.SHELLRAX_DEFEATED, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BULLETBANK_DEFEATED, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_LOOP_1, true);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND, true);
			});
			global::ETGModConsole.Commands.GetGroup("psog").AddUnit("lock_all", delegate (string[] args)
			{
				ETGModConsole.Log("<size=100><color=#ff0000ff>Refitting the locks... don't hit them that hard next time, okay?</color></size>", false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.JAMMED_GUARD_DEFEATED, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.SHELLRAX_DEFEATED, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BULLETBANK_DEFEATED, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_LOOP_1, false);
				AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND, false);
			});
			ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPostStart, new Action<AIActor>(LoopScale));
			ETGMod.AIActor.OnPostStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPostStart, new Action<AIActor>(AssignUnlocks));

			ETGModConsole.Commands.GetGroup("psog").AddUnit("reset_loop", delegate (string[] args)
			{
				float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
				SaveAPIManager.RegisterStatChange(CustomTrackedStats.TIMES_LOOPED, Loop - (Loop*2));
				ETGModConsole.Log("Current Loop: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED).ToString());
			});
			
			ETGModConsole.Commands.GetGroup("psog").AddUnit("set_loop", delegate (string[] args)
			{
				float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
				SaveAPIManager.RegisterStatChange(CustomTrackedStats.TIMES_LOOPED, float.Parse(args[0]) - Loop);
				ETGModConsole.Log("Current Loop: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED).ToString());
			});
			ETGModConsole.Commands.GetGroup("psog").AddUnit("current_loop", delegate (string[] args)
			{
				ETGModConsole.Log("Current Loop: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED).ToString());
			});


			global::ETGModConsole.Commands.GetGroup("psog").AddUnit("to_do_list", delegate (string[] args)
			{

				string a = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED) ? " Done!\n" : " -Defeat The Dragun At A Higher Curse.\n";
				string b = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.JAMMED_GUARD_DEFEATED) ? " Done!\n" : " -Defeat The Guardian Of The Holy Chamber.\n";
				string c = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.SHELLRAX_DEFEATED) ? " Done!\n" : " -Defeat The Failed Demi-Lich\n";
				string d = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BULLETBANK_DEFEATED) ? " Done!\n" : " -Defeat The Banker Of Bullets.\n";
				string e = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED) ? " Done!\n" : " -Defeat The Lich With A Broken Remnant In Hand.\n";
				string f = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BEAT_LOOP_1) ? " Done!\n" : " -Beat The Game On Ouroborous Level 0.\n";
				string g = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND) ? " Done!\n" : " -Kill A Boss After Dealing 500 Damage Or More At Once.\n";
				string color1 = "9006FF";
				OtherTools.PrintNoID("Unlock List:\n" + a + b + c + d + e + f + g, color1);
			});

			global::ETGModConsole.Commands.GetGroup("psog").AddUnit("toggleui", delegate (string[] args)
			{
				if (!disabled)
				{
					ETGModConsole.Log("Ui is disabled");
					//GameUIRoot.Instance.SetAmmoCountColor(Color.red, GameManager.Instance.PrimaryPlayer);
					GameUIRoot.Instance.HideCoreUI("disabled");
					GameUIRoot.Instance.ForceHideGunPanel = true;
					GameUIRoot.Instance.ForceHideItemPanel = true;
					disabled = true;
				}
				else if (disabled)
				{
					ETGModConsole.Log("Ui is enabled");
					GameUIRoot.Instance.ShowCoreUI("disabled");
					GameUIRoot.Instance.ForceHideGunPanel = false;
					GameUIRoot.Instance.ForceHideItemPanel = false;
					disabled = false;
				}
			});

		}
		private void AssignUnlocks(AIActor target)
		{
			//Lich Kill unlocks
			if (target.EnemyGuid == "7c5d5f09911e49b78ae644d2b50ff3bf")
			{
				target.healthHaver.OnDeath += (obj) =>
				{
					if (LoopingOn == true)
					{
						float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
						if (Loop == 0 || Loop <= 0)
						{
							AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_LOOP_1, true);
							SaveAPIManager.SetStat(CustomTrackedStats.TIMES_LOOPED, 1);
						}
						else
						{
							if (Loop == 1)
                            {
								AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_LOOP_1, true);
							}
							SaveAPIManager.RegisterStatChange(CustomTrackedStats.TIMES_LOOPED, 1);
						}
					}
					//Beat Lich With Broken Chamber
					if ((GameManager.Instance.PrimaryPlayer.HasPickupID(ETGMod.Databases.Items["Broken Chamber"].PickupObjectId)))
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED, true);
					}
				};
			}
			//Dragun Kill Unlocks
			if (target.EnemyGuid == "465da2bb086a4a88a803f79fe3a27677")
			{
				target.healthHaver.OnDeath += (obj) =>
				{
					float num = 0f;
					num = (GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse));
					if (num == 15 || num >= 14)
					{
						AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.HIGHER_CURSE_DRAGUN_KILLED, true);
					}
				};
			}
		}
		public float AddedMasterRoundChance;
		public Color magenta = Color.magenta;
		public Color yellow = Color.yellow;
		public Color cyan = Color.cyan;
		public static List<AIActor> LoopEffects = new List<AIActor>();
		private void LoopScale(AIActor target)
		{
			float value = UnityEngine.Random.value;
			if (LoopingOn && !target.CompanionOwner && !BannedEnemies.Contains(target.EnemyGuid))
			{
				if (!LoopingOn)
				{

				}
				else
				{
					LoopEffectsEnemy(target);
					LoopEffects.Add(target);
				}
			}
		}

		private void LoopEffectsEnemy(AIActor target)
		{
			if (!Ouroborous.BannedEnemies.Contains(target.EnemyGuid))
            {
				float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
				float DownScaler = Loop / 30f;
				float InitialScale = 0.4f;

				if (LoopingOn == true)
                {
					Projectile.BaseEnemyBulletSpeedMultiplier = 1 + ((Loop / 30f));
				}
				else
                {
					Projectile.BaseEnemyBulletSpeedMultiplier = 1;
				}
				if (Loop == 50 || Loop >= 50)
                {
					target.MovementSpeed *= 3f + ((Loop / 50f)+InitialScale) - DownScaler;
				}
				else
                {
					target.MovementSpeed *= 1 + ((Loop / 25f)+InitialScale)-DownScaler;
				}
				target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * ((1 + Loop/8.75f)+InitialScale)- DownScaler);
				target.knockbackDoer.weight *= 1 + ((Loop / 1.66f)+InitialScale)- -DownScaler;
				target.behaviorSpeculator.CooldownScale *= ((1f+(Loop/1.75f)) + InitialScale)-DownScaler;
				//target.behaviorSpeculator.CooldownScale *= 0;
				float random = UnityEngine.Random.Range(0.0f, 1.0f);
				if (random <= Loop/25)
				{
					target.healthHaver.spawnBulletScript = true;
					target.healthHaver.chanceToSpawnBulletScript = 1f;
					target.healthHaver.bulletScriptType = HealthHaver.BulletScriptType.OnPreDeath;
					target.healthHaver.bulletScript = new CustomBulletScriptSelector(typeof(ExplosiveDeath));
				}
			}
		}

		public static void TrappedChest(Action<Chest, PlayerController> orig, Chest self, PlayerController player)
		{
			orig(self, player);
            {
				if (LoopingOn == true)
                {
					float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
					if (Loop == 10 || Loop >= 10)
                    {
						int num3 = UnityEngine.Random.Range(0, 15);
						bool ItsATrap = num3 == 1;
						if (ItsATrap)
						{
							GameObject gameObject = new GameObject();
							gameObject.transform.position = self.transform.position + new Vector3(0, -1f, 0f);
							BulletScriptSource source = gameObject.GetOrAddComponent<BulletScriptSource>();
							gameObject.AddComponent<BulletSourceKiller>();
							var bulletScriptSelected = new CustomBulletScriptSelector(typeof(GrenadeYahyeet));
							AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
							AIBulletBank bulletBank = aIActor.GetComponent<AIBulletBank>();
							bulletBank.CollidesWithEnemies = true;
							source.BulletManager = bulletBank;
							source.BulletScript = bulletScriptSelected;
							source.Initialize();//to fire the script once
						}
					}
					else
                    {
						int num3 = (int)UnityEngine.Random.Range(0, 25 - (Loop));
						bool ItsATrap = num3 == 1;
						if (ItsATrap)
						{
							
							GameObject gameObject = new GameObject();
							gameObject.transform.position = self.transform.position + new Vector3(0, -1f, 0f);
							BulletScriptSource source = gameObject.GetOrAddComponent<BulletScriptSource>();
							gameObject.AddComponent<BulletSourceKiller>();
							var bulletScriptSelected = new CustomBulletScriptSelector(typeof(GrenadeYahyeet));
							AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
							AIBulletBank bulletBank = aIActor.GetComponent<AIBulletBank>();
							bulletBank.CollidesWithEnemies = true;
							source.BulletManager = bulletBank;
							source.BulletScript = bulletScriptSelected;
							source.Initialize();//to fire the script once

							
						}
					}
				}
            }
		}

		public static void MimicGunScaler(Action<Gun> orig, Gun spawnedGun)
		{
			if (LoopingOn == true)
            {
				float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
				spawnedGun.gameObject.SetActive(true);
				if (GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_HAS_BEEN_PEDESTAL_MIMICKED) && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && UnityEngine.Random.value < 0.001f+(Loop/750))
				{
					spawnedGun.gameObject.AddComponent<MimicGunMimicModifier>();
				}
			}
			else
            {
				spawnedGun.gameObject.SetActive(true);
				if (GameStatsManager.Instance.GetFlag(GungeonFlags.ITEMSPECIFIC_HAS_BEEN_PEDESTAL_MIMICKED) && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE && UnityEngine.Random.value < 0.001)
				{
					spawnedGun.gameObject.AddComponent<MimicGunMimicModifier>();
				}
			}
		}

		public static void DoFairy(Action<MinorBreakable> orig, MinorBreakable self)
		{
			orig(self);
			if (LoopingOn == true)
            {
				bool flag = !self.name.ToLower().Contains("pot");
				if (!flag)
				{
					float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
					if (Loop == 75 | Loop >= 75)
					{
						int FairyScaler;
						FairyScaler = UnityEngine.Random.Range(0, 150);
						if (FairyScaler == 1)
						{
							PotFairyEngageDoer.InstantSpawn = true;
							PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
							AIActor prefabActor = Game.Enemies["pot_fairy"];
							AIActor.Spawn(prefabActor, self.sprite.WorldCenter, primaryPlayer.CurrentRoom, false, AIActor.AwakenAnimationType.Default, true);
						}
					}
					else
					{
						int FairyScaler;
						FairyScaler = (int)UnityEngine.Random.Range(0, 200 - (Loop / 5));
						if (FairyScaler == 1)
						{
							PotFairyEngageDoer.InstantSpawn = true;
							PlayerController primaryPlayer = GameManager.Instance.PrimaryPlayer;
							AIActor prefabActor = Game.Enemies["pot_fairy"];
							AIActor.Spawn(prefabActor, self.sprite.WorldCenter, primaryPlayer.CurrentRoom, false, AIActor.AwakenAnimationType.Default, true);
						}
					}
				}
			}
		}
		private void DoPoisonGoop(Vector2 v)
		{
			float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/poison goop.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
			goopManagerForGoopType.TimedAddGoopCircle(v, 1f + (Loop / 10)+InitialScaling, 0.35f, false);
			goopDef.damagesEnemies = false;
		}

		private void DoFireGoop(Vector2 v)
		{
			float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
			AssetBundle assetBundle = ResourceManager.LoadAssetBundle("shared_auto_001");
			GoopDefinition goopDef = assetBundle.LoadAsset<GoopDefinition>("assets/data/goops/napalmgoopquickignite.asset");
			DeadlyDeadlyGoopManager goopManagerForGoopType = DeadlyDeadlyGoopManager.GetGoopManagerForGoopType(goopDef);
			goopManagerForGoopType.TimedAddGoopCircle(v, 1f+(Loop/10) + InitialScaling, 0.35f, false);
			goopDef.damagesEnemies = false;
		}
		public static string[] BannedEnemies = new string[]
		{
			"fodder",
		};
		public static string[] NoNo = new string[]
		{
			 "c0260c286c8d4538a697c5bf24976ccf",
			 "4d37ce3d666b4ddda8039929225b7ede",
			 "3cadf10c489b461f9fb8814abc1a09c1",
		};
		public static float InitialScaling = 0.33f;
	}
}

public class GrenadeYahyeet : Script
{
	protected override IEnumerator Top()
	{
		PlayerController player = GameManager.Instance.PrimaryPlayer;
		DraGunController dragunController = EnemyDatabase.GetOrLoadByGuid("05b8afe0b6cc4fffa9dc6036fa24c8ec").GetComponent<DraGunController>();
		AkSoundEngine.PostEvent("Play_BOSS_wall_slam_01", base.BulletBank.aiActor.gameObject);
		yield return base.Wait(75);
		this.FireRocket(dragunController.skyRocket, player.sprite.WorldCenter);

		yield break;
	}
	private void FireRocket(GameObject skyRocket, Vector2 target)
	{
		SkyRocket component = SpawnManager.SpawnProjectile(skyRocket, base.Position, Quaternion.identity, true).GetComponent<SkyRocket>();
		component.TargetVector2 = target;
		tk2dSprite componentInChildren = component.GetComponentInChildren<tk2dSprite>();
		component.transform.position = component.transform.position.WithY(component.transform.position.y - componentInChildren.transform.localPosition.y);
		component.ExplosionData.ignoreList.Add(base.BulletBank.specRigidbody);
	}
}

public class ExplosiveDeath : Script
{
	protected override IEnumerator Top()
	{
		base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("880bbe4ce1014740ba6b4e2ea521e49d").bulletBank.GetBullet("grenade"));
		float airTime = base.BulletBank.GetBullet("grenade").BulletObject.GetComponent<ArcProjectile>().GetTimeInFlight();
		Vector2 vector = this.BulletManager.PlayerPosition();
		Bullet bullet2 = new Bullet("grenade", false, false, false);
		float direction2 = (vector - base.Position).ToAngle();
		base.Fire(new Direction(direction2, DirectionType.Absolute, -1f), new Speed(1f, SpeedType.Absolute), bullet2);
		(bullet2.Projectile as ArcProjectile).AdjustSpeedToHit(vector);
		bullet2.Projectile.ImmuneToSustainedBlanks = true;
		yield break;
	}

}
