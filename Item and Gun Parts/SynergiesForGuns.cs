using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ItemAPI;
using Dungeonator;
using System.Reflection;
using Random = System.Random;
using FullSerializer;
using System.Collections;
using Gungeon;
using MonoMod.RuntimeDetour;
using MonoMod;

namespace Planetside
{
	
	internal class InitialiseSynergies
	{
		public static void DoInitialisation()
		{
			List<string> mandatoryConsoleIDs2 = new List<string>
		    {
			"psog:hardlight_nailgun",
			"nail_gun"
			};
			CustomSynergies.Add("Stop!", mandatoryConsoleIDs2, null, true);
			List<string> ee = new List<string>
			{
			"psog:shock-chain",
			"thunderclap"
			};

			CustomSynergies.Add("UNLIMITED POWER!!!", ee, null, true);

		List<string> OOHT = new List<string>
		{
				"heart_holster",
				"mimic_tooth_necklace",
				"bloodied_scarf",
				"backpack",
				"ammo_belt",
				"utility_belt",
				"resourceful_sack",
				"hip_holster"
		};
			Random r1 = new Random();
			int index1 = r1.Next(OOHT.Count);
			string OneToHoldIt = OOHT[index1];
		List<string> OOFT = new List<string>
		{
				"lichy_trigger_finger",
				"ring_of_triggers",
				"backup_gun",
				"bullet_idol",
				"hip_holster",
				"turkey",
				"sunglasses"

		};
			Random r2 = new Random();
			int index2 = r2.Next(OOFT.Count);
			string OneToFireIt = OOFT[index2];

		List<string> OOPT = new List<string>
		{
				"yellow_chamber",
				"drum_clip",
				"oiled_cylinder",
				"bionic_leg",
				"cog_of_battle",
				"bloody_9mm",
				"crisis_stone",
				"ballistic_boots"
				
		};
			Random r3 = new Random();
			int index3 = r3.Next(OOPT.Count);
			string OneToPrimeIt = OOPT[index3];

		List<string> OOTS = new List<string>
		{
			"barrel",
			"makarov",
			"derringer",
			"m1911",
			"saa",
			"mailbox",
			"origuni",
			"snowballer",
			"cactus",
			"bullet",
			"elimentaler"
		};
			Random r4 = new Random();
			int index4 = r4.Next(OOTS.Count);
			string OneToShootIt = OOTS[index4];



		List<string> eeeee = new List<string>
		{
			OneToHoldIt,
			OneToFireIt,
			OneToPrimeIt,
			OneToShootIt
		};
			CustomSynergies.Add("End Of Everything", eeeee, null, true);
			Gun gun = PickupObjectDatabase.GetByEncounterName(OneToShootIt) as Gun;
			int StoredGunID = gun.PickupObjectId;
			InitialiseSynergies.GunIDForEOE = OneToShootIt;
			AdvancedTransformGunSynergyProcessor advancedTransformGunSynergyProcessor = (PickupObjectDatabase.GetById(StoredGunID) as Gun).gameObject.AddComponent<AdvancedTransformGunSynergyProcessor>();

			EndOfEverything aaaa = (PickupObjectDatabase.GetById(StoredGunID) as Gun).gameObject.GetOrAddComponent<EndOfEverything>();

			advancedTransformGunSynergyProcessor.NonSynergyGunId = StoredGunID;
			advancedTransformGunSynergyProcessor.SynergyGunId = GTEE.FuckinGhELL;
			advancedTransformGunSynergyProcessor.SynergyToCheck = "End Of Everything";
			ETGModConsole.Commands.AddUnit("gteecomponents", (args) =>
			{

				PlanetsideModule.Log(OneToHoldIt, TEXT_COLOR_GTEE);
				PlanetsideModule.Log(OneToFireIt, TEXT_COLOR_GTEE);
				PlanetsideModule.Log(OneToPrimeIt, TEXT_COLOR_GTEE);
				PlanetsideModule.Log(OneToShootIt, TEXT_COLOR_GTEE);
			});

		}
		public static string GunIDForEOE;
	
		public static readonly string TEXT_COLOR_GTEE = "#FFe400";
	}
}

