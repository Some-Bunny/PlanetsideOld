using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GungeonAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;
using Dungeonator;


namespace Planetside
{
	// Token: 0x02000018 RID: 24
	public static class Hooks
	{
		// Token: 0x060000B7 RID: 183 RVA: 0x00008CE4 File Offset: 0x00006EE4
		public static void Init()
		{
			try
			{

				Hook breachshrinereloadhook = new Hook(typeof(Foyer).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic), typeof(PlanetsideModule).GetMethod("ReloadBreachShrinesPSOG"));

				Hook OuroborousTrappedChests = new Hook(typeof(Chest).GetMethod("Open", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Ouroborous).GetMethod("TrappedChest"));
				Hook OuroborousMimicGunScaler = new Hook(typeof(LootEngine).GetMethod("PostprocessGunSpawn", BindingFlags.Static | BindingFlags.NonPublic), typeof(Ouroborous).GetMethod("MimicGunScaler"));
				Hook OuroborousGunfairyScaler = new Hook(typeof(MinorBreakable).GetMethod("OnBreakAnimationComplete", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Ouroborous).GetMethod("DoFairy"));
				Hook AngerTheGods = new Hook(typeof(PlayerController).GetMethod("DoSpinfallSpawn", BindingFlags.Instance | BindingFlags.Public), typeof(PlanetsideModule).GetMethod("aNGERgODScOMPONENT"));

				Hook GalaxyChestReward = new Hook(typeof(RoomHandler).GetMethod("HandleRoomClearReward", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("GalaxyChestReward"));

				Hook GalaxyChestRoom = new Hook(typeof(Chest).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Hooks).GetMethod("GalaxyChestPls"));

				//Hook LOTJ = new Hook(typeof(SuperReaperController).GetMethod("SpawnProjectiles", BindingFlags.Instance | BindingFlags.NonPublic), typeof(PlanetsideModule).GetMethod("SpawnProjectilesLOTJ"));

				//Hook OuroborousReducedInvulnFrames = new Hook(typeof(HealthHaver).GetMethod("TriggerInvulnerabilityPeriod", BindingFlags.Instance | BindingFlags.Public), typeof(Ouroborous).GetMethod("ReducedIFrames"));



			}
			catch (Exception e)
			{
				ItemAPI.Tools.PrintException(e, "FF0000");
			}
		}
		public static void OnQuickRestart1(Action<GameManager, float, QuickRestartOptions> orig, GameManager self, float duration, QuickRestartOptions options = default(QuickRestartOptions))
		{
			orig(self, duration, options);
			//Bugun.ThisIsBasicallyCelsRNGUNButTakenToASillyLevel();
		}
		public static void GalaxyChestReward(Action<RoomHandler> orig, RoomHandler self)
		{
			orig(self);
            float rng;
            rng = UnityEngine.Random.Range(0.0000000f, 1.0000000f);
            if (rng <= 0.0000001f)
            {

                PlayerController player = GameManager.Instance.PrimaryPlayer;
                Chest rainbow_Chest = GameManager.Instance.RewardManager.Rainbow_Chest;
                Chest chest2 = Chest.Spawn(rainbow_Chest, player.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
                chest2.sprite.usesOverrideMaterial = true;

                var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\nebula_reducednoise.png");
                chest2.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
                chest2.sprite.renderer.material.SetTexture("_EeveeTex", texture);

                chest2.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
                chest2.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");

                chest2.lootTable.S_Chance = 0.2f;
                chest2.lootTable.A_Chance = 0.2f;
                chest2.lootTable.B_Chance = 0.22f;
                chest2.lootTable.C_Chance = 0.22f;
                chest2.lootTable.D_Chance = 0.16f;
                chest2.lootTable.Common_Chance = 0f;
                chest2.lootTable.canDropMultipleItems = true;
                chest2.lootTable.multipleItemDropChances = new WeightedIntCollection();
                chest2.lootTable.multipleItemDropChances.elements = new WeightedInt[1];
                chest2.lootTable.overrideItemLootTables = new List<GenericLootTable>();
                chest2.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                WeightedInt weightedInt = new WeightedInt();
                weightedInt.value = 24;
                weightedInt.weight = 1f;
                weightedInt.additionalPrerequisites = new DungeonPrerequisite[0];
                chest2.lootTable.multipleItemDropChances.elements[0] = weightedInt;
                chest2.lootTable.onlyOneGunCanDrop = false;
                chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());
            }
		}

        public static void GalaxyChestPls(Action<Chest> orig, Chest self)
		{
            orig(self);
            float rng;
            rng = UnityEngine.Random.Range(0.0000f, 1.0000f);

            if (!self.IsGlitched && !self.IsMimic && !self.IsRainbowChest && (rng <= 0.0001f))
            {

                
                self.sprite.usesOverrideMaterial = true;
                self.BecomeRainbowChest();


                self.IsLocked= false;
                var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\nebula_reducednoise.png");

                self.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
                self.sprite.renderer.material.SetTexture("_EeveeTex", texture);

                self.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
                self.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");

                self.lootTable.S_Chance = 0.2f;
                self.lootTable.A_Chance = 0.2f;
                self.lootTable.B_Chance = 0.22f;
                self.lootTable.C_Chance = 0.22f;
                self.lootTable.D_Chance = 0.16f;
                self.lootTable.Common_Chance = 0f;
                self.lootTable.canDropMultipleItems = true;
                self.lootTable.multipleItemDropChances = new WeightedIntCollection();
                self.lootTable.multipleItemDropChances.elements = new WeightedInt[1];
                self.lootTable.overrideItemLootTables = new List<GenericLootTable>();
                self.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
                self.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
                WeightedInt weightedInt = new WeightedInt();
                weightedInt.value = 24;
                weightedInt.weight = 1f;
                weightedInt.additionalPrerequisites = new DungeonPrerequisite[0];
                self.lootTable.multipleItemDropChances.elements[0] = weightedInt;
                self.lootTable.onlyOneGunCanDrop = false;                
            }
		}
	}
}

