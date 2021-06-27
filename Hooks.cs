﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GungeonAPI;
using MonoMod.RuntimeDetour;
using UnityEngine;
using Dungeonator;
using SaveAPI;

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

                Hook uhohgun = new Hook(typeof(GameManager).GetMethod("DelayedQuickRestart", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("OnQuickRestart1"));


                Hook breachshrinereloadhook = new Hook(typeof(Foyer).GetMethod("Awake", BindingFlags.Instance | BindingFlags.NonPublic), typeof(PlanetsideModule).GetMethod("ReloadBreachShrinesPSOG"));

                Hook OuroborousTrappedChests = new Hook(typeof(Chest).GetMethod("Open", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Ouroborous).GetMethod("TrappedChest"));
                Hook OuroborousMimicGunScaler = new Hook(typeof(LootEngine).GetMethod("PostprocessGunSpawn", BindingFlags.Static | BindingFlags.NonPublic), typeof(Ouroborous).GetMethod("MimicGunScaler"));
                Hook OuroborousGunfairyScaler = new Hook(typeof(MinorBreakable).GetMethod("OnBreakAnimationComplete", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Ouroborous).GetMethod("DoFairy"));
                Hook AngerTheGods = new Hook(typeof(PlayerController).GetMethod("DoSpinfallSpawn", BindingFlags.Instance | BindingFlags.Public), typeof(PlanetsideModule).GetMethod("RunStartHook"));

                //Hook GalaxyChestReward = new Hook(typeof(RoomHandler).GetMethod("HandleRoomClearReward", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("GalaxyChestReward"));

                //Hook GalaxyChestRoom = new Hook(typeof(Chest).GetMethod("Initialize", BindingFlags.Instance | BindingFlags.NonPublic), typeof(Hooks).GetMethod("GalaxyChestPls"));

                Hook MACHOOOOOO = new Hook(typeof(PlayerController).GetMethod("OnDidDamage", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("DamageHook"));

                //Hook LOTJ = new Hook(typeof(SuperReaperController).GetMethod("SpawnProjectiles", BindingFlags.Instance | BindingFlags.NonPublic), typeof(PlanetsideModule).GetMethod("SpawnProjectilesLOTJ"));

                //Hook OuroborousReducedInvulnFrames = new Hook(typeof(HealthHaver).GetMethod("TriggerInvulnerabilityPeriod", BindingFlags.Instance | BindingFlags.Public), typeof(Ouroborous).GetMethod("ReducedIFrames"));
                //Hook customEnemyChangesHook = new Hook(typeof(AIActor).GetMethod("Update", BindingFlags.Instance | BindingFlags.Public),typeof(Hooks).GetMethod("HandleCustomEnemyChanges"));
                //Hook customEnemyChangesHook = new Hook(typeof(AIActor).GetMethod("Awake", BindingFlags.Instance | BindingFlags.Public),typeof(Hooks).GetMethod("HandleCustomEnemyChanges"));
                Hook Reard = new Hook(typeof(RoomHandler).GetMethod("HandleRoomClearReward", BindingFlags.Instance | BindingFlags.Public), typeof(Hooks).GetMethod("OuroborousRoomDrop"));


            }
            catch (Exception e)
            {
                ItemAPI.Tools.PrintException(e, "FF0000");
            }
        }


        public static void OuroborousRoomDrop(Action<RoomHandler> orig, RoomHandler self)
        {
            bool LoopOn = AdvancedGameStatsManager.Instance.GetFlag(CustomDungeonFlags.LOOPING_ON);
            if (LoopOn == false)
            {
                orig(self);
            }
            else
            {
                float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
                //PlayerController player = GameManager.Instance.BestActivePlayer;
                if (GameManager.Instance.IsFoyer || GameManager.Instance.InTutorial)
                {
                    return;
                }
                if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
                {
                    return;
                }
                Type type = typeof(RoomHandler); FieldInfo _property = type.GetField("m_hasGivenReward", BindingFlags.NonPublic | BindingFlags.Instance); _property.GetValue(self);
                bool Has = (bool)_property.GetValue(self);
                if (Has == true)
                {
                    return;
                }
                else
                {
                    Has = true;
                }
                if (self.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.REWARD)
                {
                    return;
                }
                if (self.area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.BOSS && self.area.PrototypeRoomBossSubcategory == PrototypeDungeonRoom.RoomBossSubCategory.FLOOR_BOSS)
                {
                    HandleBossClearReward(self);
                }
                if (self.PreventStandardRoomReward)
                {
                    return;
                }
                FloorRewardData currentRewardData = GameManager.Instance.RewardManager.CurrentRewardData;
                LootEngine.AmmoDropType ammoDropType = LootEngine.AmmoDropType.DEFAULT_AMMO;
                bool flag = LootEngine.DoAmmoClipCheck(currentRewardData, out ammoDropType);
                string path = (ammoDropType != LootEngine.AmmoDropType.SPREAD_AMMO) ? "Ammo_Pickup" : "Ammo_Pickup_Spread";
                float value = UnityEngine.Random.value;
                float num = currentRewardData.ChestSystem_ChestChanceLowerBound;
                float num2 = GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness) / 100f - Loop;
                float num3 = -(GameManager.Instance.PrimaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse) / 100f - Loop);
                if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER)
                {
                    num2 += GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.Coolness) / 100f - Loop;
                    num3 -= GameManager.Instance.SecondaryPlayer.stats.GetStatValue(PlayerStats.StatType.Curse) / 100f - Loop;
                }
                if (PassiveItem.IsFlagSetAtAll(typeof(ChamberOfEvilItem)))
                {
                    num3 *= -2f;
                }
                num = Mathf.Clamp(num + GameManager.Instance.PrimaryPlayer.AdditionalChestSpawnChance, currentRewardData.ChestSystem_ChestChanceLowerBound, currentRewardData.ChestSystem_ChestChanceUpperBound) + num2 + num3;
                bool flag2 = currentRewardData.SingleItemRewardTable != null;
                bool flag3 = false;
                float num4 = 0.1f - Loop/1000;
                if (!RoomHandler.HasGivenRoomChestRewardThisRun && MetaInjectionData.ForceEarlyChest)
                {
                    flag3 = true;
                }
                if (flag3)
                {
                    if (!RoomHandler.HasGivenRoomChestRewardThisRun && (GameManager.Instance.CurrentFloor == 1 || GameManager.Instance.CurrentFloor == -1))
                    {
                        flag2 = false;
                        num += num4;
                        if (GameManager.Instance.PrimaryPlayer && GameManager.Instance.PrimaryPlayer.NumRoomsCleared > 4)
                        {
                            num = 1f;
                        }
                    }
                    if (!RoomHandler.HasGivenRoomChestRewardThisRun && self.distanceFromEntrance < RoomHandler.NumberOfRoomsToPreventChestSpawning)
                    {
                        GameManager.Instance.Dungeon.InformRoomCleared(false, false);
                        return;
                    }
                }
                BraveUtility.Log("Current chest spawn chance: " + num, Color.yellow, BraveUtility.LogVerbosity.IMPORTANT);
                if (value > num)
                {
                    if (flag)
                    {
                        IntVector2 bestRewardLocation = self.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
                        LootEngine.SpawnItem((GameObject)BraveResources.Load(path, ".prefab"), bestRewardLocation.ToVector3(), Vector2.up, 1f, true, true, false);
                    }
                    GameManager.Instance.Dungeon.InformRoomCleared(false, false);
                    return;
                }
                if (flag2)
                {
                    float num5 = currentRewardData.PercentOfRoomClearRewardsThatAreChests;
                    if (PassiveItem.IsFlagSetAtAll(typeof(AmazingChestAheadItem)))
                    {
                        num5 *= 2f;
                        num5 = Mathf.Max(0.5f, num5);
                    }
                    flag2 = (UnityEngine.Random.value > num5);
                }
                if (flag2)
                {
                    float num6 = (GameManager.Instance.CurrentGameType != GameManager.GameType.COOP_2_PLAYER) ? GameManager.Instance.RewardManager.SinglePlayerPickupIncrementModifier : GameManager.Instance.RewardManager.CoopPickupIncrementModifier;
                    GameObject gameObject;
                    if (UnityEngine.Random.value < 1f / num6)
                    {
                        gameObject = currentRewardData.SingleItemRewardTable.SelectByWeight(false);
                    }
                    else
                    {
                        gameObject = ((UnityEngine.Random.value >= 0.9f) ? GameManager.Instance.RewardManager.FullHeartPrefab.gameObject : GameManager.Instance.RewardManager.HalfHeartPrefab.gameObject);
                    }
                    //UnityEngine.Debug.Log(gameObject.name + "SPAWNED");
                    DebrisObject debrisObject = LootEngine.SpawnItem(gameObject, self.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true).ToVector3() + new Vector3(0.25f, 0f, 0f), Vector2.up, 1f, true, true, false);
                    Exploder.DoRadialPush(debrisObject.sprite.WorldCenter.ToVector3ZUp(debrisObject.sprite.WorldCenter.y), 8f, 3f);
                    AkSoundEngine.PostEvent("Play_OBJ_item_spawn_01", debrisObject.gameObject);
                    GameManager.Instance.Dungeon.InformRoomCleared(true, false);
                }
                else
                {
                    IntVector2 bestRewardLocation = self.GetBestRewardLocation(new IntVector2(2, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
                    bool isRainbowRun = GameStatsManager.Instance.IsRainbowRun;
                    if (isRainbowRun)
                    {
                        LootEngine.SpawnBowlerNote(GameManager.Instance.RewardManager.BowlerNoteChest, bestRewardLocation.ToCenterVector2(), self, true);
                        RoomHandler.HasGivenRoomChestRewardThisRun = true;
                    }
                    else
                    {
                        Chest exists = self.SpawnRoomRewardChest(null, bestRewardLocation);
                        if (exists)
                        {
                            RoomHandler.HasGivenRoomChestRewardThisRun = true;
                        }
                    }
                    GameManager.Instance.Dungeon.InformRoomCleared(true, true);
                }
                if (flag)
                {
                    IntVector2 bestRewardLocation = self.GetBestRewardLocation(new IntVector2(1, 1), RoomHandler.RewardLocationStyle.CameraCenter, true);
                    LootEngine.DelayedSpawnItem(1f, (GameObject)BraveResources.Load(path, ".prefab"), bestRewardLocation.ToVector3() + new Vector3(0.25f, 0f, 0f), Vector2.up, 1f, true, true, false);
                }
                
            }
        }

        public static void HandleBossClearReward(RoomHandler room)
        {
            if (GameManager.Instance.CurrentGameMode == GameManager.GameMode.SHORTCUT)
            {
                GameStatsManager.Instance.CurrentResRatShopSeed = UnityEngine.Random.Range(1, 1000000);
            }
            GlobalDungeonData.ValidTilesets tilesetId = GameManager.Instance.Dungeon.tileIndices.tilesetId;
            if (!room.PlayerHasTakenDamageInThisRoom && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.NONE)
            {
                if (tilesetId != GlobalDungeonData.ValidTilesets.GUNGEON)
                {
                    if (tilesetId != GlobalDungeonData.ValidTilesets.CASTLEGEON)
                    {
                        if (tilesetId != GlobalDungeonData.ValidTilesets.MINEGEON)
                        {
                            if (tilesetId != GlobalDungeonData.ValidTilesets.CATACOMBGEON)
                            {
                                if (tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON)
                                {
                                    GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, true);
                                }
                            }
                            else
                            {
                                GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_HOLLOW, true);
                            }
                        }
                        else
                        {
                            GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_MINES, true);
                        }
                    }
                    else
                    {
                        GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_CASTLE, true);
                    }
                }
                else
                {
                    GameStatsManager.Instance.SetFlag(GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_GUNGEON, true);
                }
            }
            if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.CHARACTER_PAST)
            {
                return;
            }
            if (tilesetId == GlobalDungeonData.ValidTilesets.HELLGEON)
            {
                return;
            }
            if (tilesetId == GlobalDungeonData.ValidTilesets.RATGEON)
            {
                return;
            }
            for (int i = 0; i < room.connectedRooms.Count; i++)
            {
                if (room.connectedRooms[i].area.PrototypeRoomCategory == PrototypeDungeonRoom.RoomCategory.EXIT)
                {
                    room.connectedRooms[i].OnBecameVisible(GameManager.Instance.BestActivePlayer);
                }
            }
            IntVector2 intVector = IntVector2.Zero;
            if (room.OverrideBossPedestalLocation != null)
            {
                intVector = room.OverrideBossPedestalLocation.Value;
            }
            else if (!room.area.IsProceduralRoom && room.area.runtimePrototypeData.rewardChestSpawnPosition != IntVector2.NegOne)
            {
                intVector = room.area.basePosition + room.area.runtimePrototypeData.rewardChestSpawnPosition;
            }
            else
            {
                UnityEngine.Debug.LogWarning("tollred");
                intVector = room.GetCenteredVisibleClearSpot(2, 2);
            }
            GameObject gameObject = GameManager.Instance.Dungeon.sharedSettingsPrefab.ChestsForBosses.SelectByWeight();
            Chest chest = gameObject.GetComponent<Chest>();
            bool isRainbowRun = GameStatsManager.Instance.IsRainbowRun;
            if (isRainbowRun)
            {
                chest = null;
            }
            if (chest != null)
            {
                Chest chest2 = Chest.Spawn(chest, intVector, room, false);
                chest2.RegisterChestOnMinimap(room);
            }
            else
            {
                DungeonData data = GameManager.Instance.Dungeon.data;
                RewardPedestal component = gameObject.GetComponent<RewardPedestal>();
                if (component)
                {
                    bool flag = tilesetId != GlobalDungeonData.ValidTilesets.FORGEGEON;
                    bool flag2 = !room.PlayerHasTakenDamageInThisRoom && GameManager.Instance.Dungeon.BossMasteryTokenItemId >= 0 && !GameManager.Instance.Dungeon.HasGivenMasteryToken;
                    if (flag && flag2)
                    {
                        intVector += IntVector2.Left;
                    }
                    if (flag)
                    {
                        RewardPedestal rewardPedestal = RewardPedestal.Spawn(component, intVector, room);
                        rewardPedestal.IsBossRewardPedestal = true;
                        rewardPedestal.lootTable.lootTable = room.OverrideBossRewardTable;
                        rewardPedestal.RegisterChestOnMinimap(room);
                        data[intVector].isOccupied = true;
                        data[intVector + IntVector2.Right].isOccupied = true;
                        data[intVector + IntVector2.Up].isOccupied = true;
                        data[intVector + IntVector2.One].isOccupied = true;
                        if (flag2)
                        {
                            rewardPedestal.OffsetTertiarySet = true;
                        }
                        if (GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.NumberOfLivingPlayers == 1)
                        {
                            rewardPedestal.ReturnCoopPlayerOnLand = true;
                        }
                        if (room.area.PrototypeRoomName == "DoubleBeholsterRoom01")
                        {
                            for (int j = 0; j < 8; j++)
                            {
                                IntVector2 centeredVisibleClearSpot = room.GetCenteredVisibleClearSpot(2, 2);
                                RewardPedestal rewardPedestal2 = RewardPedestal.Spawn(component, centeredVisibleClearSpot, room);
                                rewardPedestal2.IsBossRewardPedestal = true;
                                rewardPedestal2.lootTable.lootTable = room.OverrideBossRewardTable;
                                data[centeredVisibleClearSpot].isOccupied = true;
                                data[centeredVisibleClearSpot + IntVector2.Right].isOccupied = true;
                                data[centeredVisibleClearSpot + IntVector2.Up].isOccupied = true;
                                data[centeredVisibleClearSpot + IntVector2.One].isOccupied = true;
                            }
                        }
                    }
                    else if (tilesetId == GlobalDungeonData.ValidTilesets.FORGEGEON && GameManager.Instance.CurrentGameType == GameManager.GameType.COOP_2_PLAYER && GameManager.Instance.NumberOfLivingPlayers == 1)
                    {
                        PlayerController playerController = (!GameManager.Instance.PrimaryPlayer.healthHaver.IsDead) ? GameManager.Instance.SecondaryPlayer : GameManager.Instance.PrimaryPlayer;
                        playerController.specRigidbody.enabled = true;
                        playerController.gameObject.SetActive(true);
                        playerController.ResurrectFromBossKill();
                    }
                    if (flag2)
                    {
                        GameStatsManager.Instance.RegisterStatChange(TrackedStats.MASTERY_TOKENS_RECEIVED, 1f);
                        GameManager.Instance.PrimaryPlayer.MasteryTokensCollectedThisRun++;
                        if (flag)
                        {
                            intVector += new IntVector2(2, 0);
                        }
                        RewardPedestal rewardPedestal3 = RewardPedestal.Spawn(component, intVector, room);
                        data[intVector].isOccupied = true;
                        data[intVector + IntVector2.Right].isOccupied = true;
                        data[intVector + IntVector2.Up].isOccupied = true;
                        data[intVector + IntVector2.One].isOccupied = true;
                        GameManager.Instance.Dungeon.HasGivenMasteryToken = true;
                        rewardPedestal3.SpawnsTertiarySet = false;
                        rewardPedestal3.contents = PickupObjectDatabase.GetById(GameManager.Instance.Dungeon.BossMasteryTokenItemId);
                        rewardPedestal3.MimicGuid = null;
                    }
                }
                if (GameManager.Instance.Dungeon.tileIndices.tilesetId == GlobalDungeonData.ValidTilesets.CATHEDRALGEON && GameManager.Options.CurrentGameLootProfile == GameOptions.GameLootProfile.CURRENT)
                {
                    IntVector2? randomAvailableCell = room.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 4), new CellTypes?(CellTypes.FLOOR), false, null);
                    IntVector2? intVector2 = (randomAvailableCell == null) ? null : new IntVector2?(randomAvailableCell.GetValueOrDefault() + IntVector2.One);
                    if (intVector2 != null)
                    {
                        Chest chest3 = Chest.Spawn(GameManager.Instance.RewardManager.Synergy_Chest, intVector2.Value);
                        if (chest3)
                        {
                            chest3.RegisterChestOnMinimap(room);
                        }
                    }
                }
            }
        }


        public static void DamageHook(Action<PlayerController, float, bool, HealthHaver> orig, PlayerController self, float damagedone, bool fatal, HealthHaver target)
        {
            orig(self, damagedone, fatal, target);
            if (target.IsBoss && damagedone >= 500 && fatal == true)
            {
                AdvancedGameStatsManager.Instance.SetFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND, true);
            }

        }

        public static void OnQuickRestart1(Action<GameManager, float, QuickRestartOptions> orig, GameManager self, float duration, QuickRestartOptions options = default(QuickRestartOptions))
        {
            orig(self, duration, options);
            Resault.ResetMaxAmmo();
        }
        public static void HandleCustomEnemyChanges(Action<AIActor> orig, AIActor self)
        {

            orig(self);
            try
            {
                self.gameObject.AddComponent<PingPongComponent>();
                //AddBeholsterBeamComponent yee = self.gameObject.GetOrAddComponent<AddBeholsterBeamComponent>();
                //yee.AddsBaseBeamBehavior = true;
                //yee.CustomAngleValue = 72;
                //yee.UsesCustomAngle = true;
                //self.gameObject.GetOrAddComponent<AddDashComponent>();
            }
            catch
            {
                ETGModConsole.Log("epic fail");
            }
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

