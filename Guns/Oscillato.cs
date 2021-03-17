﻿using System;
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
	public class Oscillato : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Oscillator", "oscillato");
			Game.Items.Rename("outdated_gun_mods:oscillator", "psog:oscillator");
			gun.gameObject.AddComponent<Oscillato>();
			gun.SetShortDescription("With Slowing Grace");
			gun.SetLongDescription("A gun that was manufactured on an earthquake-heavy planet under the Hegemony Of Mans rule. The reverberation from these events are projected onto the bullets.");
			GunExt.SetupSprite(gun, null, "oscillato_idle_001", 11);	
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 15);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 7);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 5);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(56) as Gun, true, false);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(89) as Gun).gunSwitchGroup;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 2.9f;
			gun.DefaultModule.cooldownTime = .25f;
			gun.DefaultModule.numberOfShotsInClip = 60;
			gun.SetBaseMaxAmmo(300);
			gun.quality = PickupObject.ItemQuality.C;
			gun.DefaultModule.angleVariance = 8f;
			gun.DefaultModule.burstShotCount = 4;
			gun.DefaultModule.burstCooldownTime = 0.025f;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage = 3.5f;
			projectile.baseData.speed *= 0.7f;
			projectile.AdditionalScaleMultiplier *= 1f;
			projectile.shouldRotate = true;
			projectile.pierceMinorBreakables = true;
			projectile.PenetratesInternalWalls = true;

			projectile.AnimateProjectile(new List<string> {
				"oscillato_projectile_001",
				"oscillato_projectile_002",
				"oscillato_projectile_003",
				"oscillato_projectile_004",
				"oscillato_projectile_005",
				"oscillato_projectile_006",
				"oscillato_projectile_007"
			}, 13, true, new List<IntVector2> {
				new IntVector2(7, 5), //1
                new IntVector2(7, 5), //2            All frames are 13x16 except select ones that are 11-14
                new IntVector2(7, 5), //3
                new IntVector2(7, 5),//4
                new IntVector2(7, 5),//5
                new IntVector2(7, 5),//6
                new IntVector2(7, 5),
            }, AnimateBullet.ConstructListOfSameValues(false, 7), AnimateBullet.ConstructListOfSameValues(tk2dBaseSprite.Anchor.MiddleCenter, 7), AnimateBullet.ConstructListOfSameValues(true, 7), AnimateBullet.ConstructListOfSameValues(false, 7),
			AnimateBullet.ConstructListOfSameValues<Vector3?>(null, 7), AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 7), AnimateBullet.ConstructListOfSameValues<IntVector2?>(null, 7), AnimateBullet.ConstructListOfSameValues<Projectile>(null, 7));

			projectile.SetProjectileSpriteRight("oscillato_projectile_001", 7, 5, false, tk2dBaseSprite.Anchor.MiddleCenter, 7, 5);

			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 1;
			projectile.OverrideMotionModule = new OscillatingeMotionModule();
			spook.penetratesBreakables = true;
			//projectile.baseData.range = 5.8f;
			gun.encounterTrackable.EncounterGuid = "https://www.youtube.com/watch?v=P5ChKb_9JoY";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			List<string> AAA = new List<string>
			{
				"psog:oscillator",
				"psog:oscillating_bullets",
			};
			CustomSynergies.Add("Reverberation", AAA, null, true);
			Oscillato.AAID = gun.PickupObjectId;
		}
		public static int AAID;

		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.OverrideMotionModule = new OscillatingeMotionModule();
		}
		public override void OnReloadPressed(PlayerController player, Gun bruhgun, bool bSOMETHING)
		{
			
		}
	}

}