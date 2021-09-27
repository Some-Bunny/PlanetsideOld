using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using GungeonAPI;
using SaveAPI;
using System.Text;

using Gungeon;

namespace Planetside
{
    public class FlowInjectionInitialiser : FlowDatabase
    {
		public static PrototypeDungeonRoom BrokenChamberRoomPrefab;
		public static SharedInjectionData ForgeData;
		public static SharedInjectionData BaseSharedInjectionData;
		public static ProceduralFlowModifierData BrokenChamberRoom;
		public static AssetBundle sharedAssets2;



		public static SharedInjectionData GungeonInjectionData;


		public static void InitialiseFlows()
        {
			sharedAssets2 = ResourceManager.LoadAssetBundle("shared_auto_002");
			BaseSharedInjectionData = sharedAssets2.LoadAsset<SharedInjectionData>("Base Shared Injection Data");

			Dungeon GungeonProper = DungeonDatabase.GetOrLoadByName("Base_Gungeon");
			GungeonInjectionData = GungeonProper.PatternSettings.flows[0].sharedInjectionData[1];


			AddVrokenChamberRoom(false);
			AddShellraxRoom(false);
		}

		public static void AddVrokenChamberRoom(bool refreshFlows = false)
		{
			float Weight = 0.4f;
			if (SaveAPIManager.GetFlag(CustomDungeonFlags.BROKEN_CHAMBER_RUN_COMPLETED) == true)
            {
				Weight = 0.05f;
            }	
			BrokenChamberRoomPrefab = RoomFactory.BuildFromResource("Planetside/Resources/ShrineRooms/BrokenChamberRoom.room").room;
			BrokenChamberRoom = new ProceduralFlowModifierData()
			{
				annotation = "Broken Chamber Spawn",
				DEBUG_FORCE_SPAWN = false,
				OncePerRun = true,
				placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
					ProceduralFlowModifierData.FlowModifierPlacementType.END_OF_CHAIN
				},
				roomTable = null,
				exactRoom = BrokenChamberRoomPrefab,
				IsWarpWing = false,
				RequiresMasteryToken = false,
				chanceToLock = 0,
				selectionWeight = 2,
				chanceToSpawn = Weight,
				RequiredValidPlaceable = null,
				prerequisites = new DungeonPrerequisite[] {
					new DungeonPrerequisite
					{
						prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO,
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
						requiredTileset = GlobalDungeonData.ValidTilesets.CASTLEGEON,
						requireTileset = true,
						comparisonValue = 1f,
						encounteredObjectGuid = string.Empty,
						maxToCheck = TrackedMaximums.MOST_KEYS_HELD,
						requireDemoMode = false,
						requireCharacter = false,
						requiredCharacter = PlayableCharacters.Pilot,
						requireFlag = false,
						useSessionStatValue = false,
						encounteredRoom = null,
						requiredNumberOfEncounters = -1,
						saveFlagToCheck = GungeonFlags.TUTORIAL_COMPLETED,
						statToCheck = TrackedStats.GUNBERS_MUNCHED
					}
				},
				CanBeForcedSecret = true,
				RandomNodeChildMinDistanceFromEntrance = 0,
				exactSecondaryRoom = null,
				framedCombatNodes = 0,

			};
			
			
			BaseSharedInjectionData.InjectionData.Add(BrokenChamberRoom);



		}

		public static void AddShellraxRoom(bool refreshFlows = false)
		{
			AssetBundle shared_auto_001 = ResourceManager.LoadAssetBundle("shared_auto_001");

			GameObject iconPrefab =(shared_auto_001.LoadAsset("assets/data/prefabs/room icons/minimap_boss_icon.prefab") as GameObject);

			ShellraxRoomPrefab = RoomFactory.BuildFromResource("Planetside/Resources/ShrineRooms/BigDumbIdiotBossRoom1.room").room;
			ShellraxRoomPrefab.associatedMinimapIcon = iconPrefab;
			ShellraxRoom = new ProceduralFlowModifierData()
			{
				annotation = "Shellrax Room",
				DEBUG_FORCE_SPAWN = false,
				OncePerRun = true,
				placementRules = new List<ProceduralFlowModifierData.FlowModifierPlacementType>() {
					ProceduralFlowModifierData.FlowModifierPlacementType.HUB_ADJACENT_CHAIN_START
				},
				roomTable = null,
				exactRoom = ShellraxRoomPrefab,
				IsWarpWing = false,
				RequiresMasteryToken = false,
				chanceToLock = 0,
				selectionWeight = 2,
				chanceToSpawn = 1,
				RequiredValidPlaceable = null,
				prerequisites = new DungeonPrerequisite[] {
					new DungeonPrerequisite
					{
						prerequisiteOperation = DungeonPrerequisite.PrerequisiteOperation.EQUAL_TO,
						prerequisiteType = DungeonPrerequisite.PrerequisiteType.TILESET,
						requiredTileset = GlobalDungeonData.ValidTilesets.HELLGEON,
						requireTileset = true,
						comparisonValue = 1f,
						encounteredObjectGuid = string.Empty,
						maxToCheck = TrackedMaximums.MOST_KEYS_HELD,
						requireDemoMode = false,
						requireCharacter = false,
						requiredCharacter = PlayableCharacters.Pilot,
						requireFlag = false,
						useSessionStatValue = false,
						encounteredRoom = null,
						requiredNumberOfEncounters = -1,
						saveFlagToCheck = GungeonFlags.TUTORIAL_COMPLETED,
						statToCheck = TrackedStats.GUNBERS_MUNCHED
					}
				},
				CanBeForcedSecret = false,
				RandomNodeChildMinDistanceFromEntrance = 0,
				exactSecondaryRoom = null,
				framedCombatNodes = 0,

			};
			BaseSharedInjectionData.InjectionData.Add(ShellraxRoom);
		}



		public static PrototypeDungeonRoom PrayerAmuletRoomPrefab;
		public static ProceduralFlowModifierData PrayerAmuletRoom;

		public static PrototypeDungeonRoom ShellraxRoomPrefab;
		public static ProceduralFlowModifierData ShellraxRoom;


	}
}
