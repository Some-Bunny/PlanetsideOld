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
using SaveAPI;

namespace Planetside
{
	public class LaserChainsaw : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Laser Chainsaw", "laserchainsaw");
			Game.Items.Rename("outdated_gun_mods:laser_chainsaw", "psog:laser_chainsaw");
			gun.gameObject.AddComponent<LaserChainsaw>();
			gun.SetShortDescription("KILL KILL KILL");
			gun.SetLongDescription("SHRED EVERYTHING IN SIGHT.\n\nLEAVE NO WITNESSES.\n\nHOLD THE TRIGGER UNTIL ALL THAT'S LEFT IS BLOOD.");
			GunExt.SetupSprite(gun, null, "laserchainsaw_idle_001", 11);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 30);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 1);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 1);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(56) as Gun, true, false);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(444) as Gun).gunSwitchGroup;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Burst;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = .05f;
			gun.DefaultModule.numberOfShotsInClip = 100;
			gun.SetBaseMaxAmmo(100);
			gun.quality = PickupObject.ItemQuality.S;
			gun.DefaultModule.angleVariance = 0f;
			gun.DefaultModule.burstShotCount = 10000000;
			gun.DefaultModule.burstCooldownTime = 0.05f;
			gun.DefaultModule.ammoType = GameUIAmmoType.AmmoType.BEAM;
			gun.barrelOffset.transform.localPosition = new Vector3(1.0f, 0.25f, 0f);
			gun.carryPixelOffset += new IntVector2((int)2.5f, (int)-0.5f);
			gun.AddCurrentGunStatModifier(PlayerStats.StatType.MovementSpeed, 1.5f, StatModifier.ModifyMethod.MULTIPLICATIVE);
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(358) as Gun).DefaultModule.chargeProjectiles[1].Projectile);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage = 25f;
			projectile.baseData.speed *= 1f;
			projectile.AdditionalScaleMultiplier *= 0.66f;
			projectile.shouldRotate = true;
			projectile.pierceMinorBreakables = true;
			projectile.baseData.range = 0.7f;
			projectile.baseData.force = 0f;
			projectile.ignoreDamageCaps = true;
			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 1000;

			spook.penetratesBreakables = true;
			gun.encounterTrackable.EncounterGuid = "https://enterthegungeon.gamepedia.com/Modding/Some_Bunny%27s_Content_Pack";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.BEAT_A_BOSS_UNDER_A_SECOND, true);

		}
		public override void PostProcessProjectile(Projectile projectile)
		{
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHit));
			projectile.OnWillKillEnemy = (Action<Projectile, SpeculativeRigidbody>)Delegate.Combine(projectile.OnWillKillEnemy, new Action<Projectile, SpeculativeRigidbody>(this.OnKill));

		}

		private void HandleHit(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			bool flag = arg2.aiActor != null && !arg2.healthHaver.IsBoss && !arg2.healthHaver.IsDead && arg2.aiActor.behaviorSpeculator && !arg2.aiActor.IsHarmlessEnemy && arg2.aiActor != null;
			if (flag)
			{
				this.gun.ammo += 1;
				this.teleporter = PickupObjectDatabase.GetById(449).GetComponent<TeleporterPrototypeItem>();
				UnityEngine.Object.Instantiate<GameObject>(this.teleporter.TelefragVFXPrefab, arg2.specRigidbody.UnitCenter, Quaternion.identity);
			}
		}
		private void OnKill(Projectile arg1, SpeculativeRigidbody arg2)
		{
			bool flag = !arg2.aiActor.healthHaver.IsDead;
			if (flag)
			{
				int ammo = this.gun.CurrentAmmo;
				this.gun.ammo += 100- ammo;
				PlayerController player = this.gun.CurrentOwner as PlayerController;
			}
		}

		private bool m_usedOverrideMaterial;
		private TeleporterPrototypeItem teleporter;
		private bool HasReloaded;

		public override void OnReloadPressed(PlayerController player, Gun bruhgun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
		}
		public override void OnPostFired(PlayerController player, Gun bruhgun)
		{
			TriggerRipAndTear = true;
			this.gun.ClipShotsRemaining += 2;

		}
		protected void Update()
		{
			PlayerController player = this.gun.CurrentOwner as PlayerController;
			if (gun.CurrentOwner)
			{
				if (this.gun.IsFiring)
                {
					TriggerRipAndTear = true;
				}
				else
                {
					TriggerRipAndTear = false;
				}
				if (TriggerRipAndTear == true && this.gun.CurrentAmmo != 0)
                {
					player.inventory.GunLocked.SetOverride("RIP", true, null);
					this.m_usedOverrideMaterial = player.sprite.usesOverrideMaterial;
					player.sprite.usesOverrideMaterial = true;
					player.SetOverrideShader(ShaderCache.Acquire("Brave/LitCutoutUberPhantom"));
					player.healthHaver.IsVulnerable = false;
				}
				else if (this.gun.CurrentAmmo == 0 || !this.gun.IsFiring)
                {
					TriggerRipAndTear = false;
					player.ClearOverrideShader();
					player.inventory.GunLocked.SetOverride("RIP", false, null);
					player.healthHaver.IsVulnerable = true;
				}
			}
		}
		private bool TriggerRipAndTear;
	}
}