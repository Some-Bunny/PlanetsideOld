﻿using Gungeon;
using ItemAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FakePrefab = ItemAPI.FakePrefab;
using Object = UnityEngine.Object;

namespace Planetside
{
	// Token: 0x0200002D RID: 45
	public class ShockChain : GunBehaviour
	{
		// Token: 0x0600013F RID: 319 RVA: 0x0000D468 File Offset: 0x0000B668
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Shock-Chain", "shocklaser");
			Game.Items.Rename("outdated_gun_mods:shock-chain", "psog:shock-chain");
			gun.gameObject.AddComponent<ShockChain>();
			GunExt.SetShortDescription(gun, "Proc Coefficient: 0.2");
			GunExt.SetLongDescription(gun, "A reformed taser weapon that fires electric arcs in a wide area. Gungeonologists speculate whether the similarities between this weapon and an existing weapon are intentional, or just coincidence.");
			GunExt.SetupSprite(gun, null, "shocklaser_idle_001", 8);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 24);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 9);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 2);
			gun.SetBaseMaxAmmo(150);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(13) as Gun, true, false);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(13) as Gun, true, false);

			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_WPN_plasmacell_shot_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].eventAudio = "Play_WPN_plasmacell_reload_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].triggerEvent = true;

			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(153) as Gun).gunSwitchGroup;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.damageModifier = 1;
			gun.reloadTime = 2.7f;
			gun.DefaultModule.cooldownTime = 0.33f;
			gun.DefaultModule.numberOfShotsInClip = 10;
			gun.DefaultModule.angleVariance = 0f;

			gun.Volley.projectiles[0].ammoCost = 1;
			gun.Volley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.Volley.projectiles[0].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.Volley.projectiles[0].cooldownTime = 0.33f;
			gun.Volley.projectiles[0].angleVariance = 0f;
			gun.Volley.projectiles[0].numberOfShotsInClip = 10;
			gun.Volley.projectiles[0].angleFromAim = 30;
			gun.Volley.projectiles[0].angleFromAim = 30;

			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.Volley.projectiles[0].projectiles[0]);
			projectile.gameObject.SetActive(false);
			gun.Volley.projectiles[0].projectiles[0] = projectile;
			projectile.baseData.damage = 2f;
			projectile.baseData.speed *= 3f;
			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 20;
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			bool flag = gun.Volley.projectiles[0] != gun.DefaultModule;
			if (flag)
			{
				gun.Volley.projectiles[0].ammoCost = 0;
			}
			projectile.transform.parent = gun.barrelOffset;


			gun.Volley.projectiles[1].ammoCost = 1;
			gun.Volley.projectiles[1].shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.Volley.projectiles[1].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.Volley.projectiles[1].cooldownTime = 0.33f;
			gun.Volley.projectiles[1].angleVariance = 0f;
			gun.Volley.projectiles[1].numberOfShotsInClip = 10;
			gun.Volley.projectiles[1].angleFromAim = -30;
			Projectile projectile1 = UnityEngine.Object.Instantiate<Projectile>(gun.Volley.projectiles[1].projectiles[0]);
			projectile1.gameObject.SetActive(false);
			gun.Volley.projectiles[1].projectiles[0] = projectile1;
			projectile1.baseData.damage = 2f;
			projectile1.baseData.speed *= 3f;
			PierceProjModifier spooky = projectile1.gameObject.AddComponent<PierceProjModifier>();
			spooky.penetration = 20;
			FakePrefab.MarkAsFakePrefab(projectile1.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile1);
			bool aa = gun.Volley.projectiles[1] != gun.DefaultModule;
			if (aa)
			{
				gun.Volley.projectiles[1].ammoCost = 0;
			}
			projectile1.transform.parent = gun.barrelOffset;
			gun.encounterTrackable.EncounterGuid = "):";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.barrelOffset.transform.localPosition = new Vector3(1.25f, 0.3125f, 0f);
			gun.carryPixelOffset = new IntVector2((int)4f, (int)-0.5f);
			Gun gun4 = PickupObjectDatabase.GetById(13) as Gun;
			gun.muzzleFlashEffects = gun4.muzzleFlashEffects;
			gun.quality = PickupObject.ItemQuality.C;
			List<string> mandatoryConsoleIDs1 = new List<string>
			{
				"psog:shock-chain"
			};
			List<string> optionalConsoleIDs = new List<string>
			{
				"shock_rifle",
				"laser_lotus",
				"the_emperor",
				"relodestone"
			};
			CustomSynergies.Add("POWER SURGE", mandatoryConsoleIDs1, optionalConsoleIDs, true);
			List<string> AAA = new List<string>
			{
				"psog:shock-chain",
				"shock_rounds"
			};
			CustomSynergies.Add("Single A", AAA, null, true);
			ShockChain.ElectricMusicID = gun.PickupObjectId;

		}
		public static int ElectricMusicID;

		public override void PostProcessProjectile(Projectile projectile)
		{
			PlayerController player = this.gun.CurrentOwner as PlayerController;
			float ElectricDamage = 6;
			bool flagA = player.PlayerHasActiveSynergy("Single A");
			if (flagA)
            {
				ElectricDamage *= 2;
            }
			ComplexProjectileModifier complexProjectileModifier = PickupObjectDatabase.GetById(298) as ComplexProjectileModifier;
			ChainLightningModifier orAddComponent = projectile.gameObject.GetOrAddComponent<ChainLightningModifier>();
			orAddComponent.LinkVFXPrefab = complexProjectileModifier.ChainLightningVFX;
			orAddComponent.damageTypes = complexProjectileModifier.ChainLightningDamageTypes;
			orAddComponent.maximumLinkDistance = 100;
			orAddComponent.damagePerHit = ElectricDamage;
			orAddComponent.damageCooldown = complexProjectileModifier.ChainLightningDamageCooldown;
			orAddComponent.UsesDispersalParticles = true;
			orAddComponent.DispersalParticleSystemPrefab = complexProjectileModifier.ChainLightningDispersalParticles;
			orAddComponent.DispersalDensity = complexProjectileModifier.ChainLightningDispersalDensity;
			orAddComponent.DispersalMinCoherency = complexProjectileModifier.ChainLightningDispersalMinCoherence;
			orAddComponent.DispersalMaxCoherency = complexProjectileModifier.ChainLightningDispersalMaxCoherence;
			orAddComponent.UsesDispersalParticles = true;
			orAddComponent.CanChainToAnyProjectile = false;
		}

		private bool HasReloaded;
		protected void Update()
		{
			if (gun.CurrentOwner)
			{

				if (!gun.PreventNormalFireAudio)
				{
					this.gun.PreventNormalFireAudio = true;
				}
				if (!gun.IsReloading && !HasReloaded)
				{
					this.HasReloaded = true;
				}
			}
		}
		public override void OnReloadPressed(PlayerController player, Gun bruhgun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				bool flag = this.HasReloaded && gun.ClipShotsRemaining == 0;
				if (flag)
				{
					bool flagA = player.PlayerHasActiveSynergy("POWER SURGE");
					if (flagA)
					{
						for (int counter = 0; counter < 8; counter++)
						{
							Projectile projectile = ((Gun)ETGMod.Databases.Items[16]).DefaultModule.projectiles[0];
							Vector3 vector = player.unadjustedAimPoint - player.LockedApproximateSpriteCenter;
							Vector3 vector2 = player.specRigidbody.UnitCenter;
							GameObject gameObject = SpawnManager.SpawnProjectile(projectile.gameObject, player.sprite.WorldCenter, Quaternion.Euler(0f, 0f, ((player.CurrentGun == null) ? 1.2f : player.CurrentGun.CurrentAngle) + UnityEngine.Random.Range(-30, 30)), true);
							Projectile component = gameObject.GetComponent<Projectile>();
							if (flag)
							{
								component.Owner = player;
								component.Shooter = player.specRigidbody;
							}

						}
					}
				}
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
		}
		public Vector3 projectilePos;		
	}
}
