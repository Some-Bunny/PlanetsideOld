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
	public class Mop : AdvancedGunBehavior
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Mop", "mopgun");
			Game.Items.Rename("outdated_gun_mods:mop", "psog:mop");
			gun.gameObject.AddComponent<Mop>();
			gun.SetShortDescription("Honest Work");
			gun.SetLongDescription("A mop thats been left inside of a chest. Surely the chemicals used to clean the floors of the Gungeon are harmful to the denizens of the Gungeon?");
			gun.SetupSprite(null, "mopgun_idle_001", 11);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 15);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 7);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 2);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(33) as Gun).gunSwitchGroup;



			for (int i = 0; i < 3; i++)
			{
				gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(33) as Gun, true, false);

			}

			gun.Volley.projectiles[0].ammoCost = 1;
			gun.Volley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.Volley.projectiles[0].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.Volley.projectiles[0].cooldownTime = 0.5f;
			gun.Volley.projectiles[0].angleVariance = 10f;
			gun.Volley.projectiles[0].numberOfShotsInClip = -1;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.Volley.projectiles[0].projectiles[0]);
			projectile.gameObject.SetActive(false);
			gun.Volley.projectiles[0].projectiles[0] = projectile;
			projectile.baseData.damage = 6f;
			projectile.AdditionalScaleMultiplier *= 1.5f;
			projectile.baseData.speed *= 1.25f;
			projectile.gameObject.AddComponent<MopProjectile>();
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
			gun.Volley.projectiles[1].shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.Volley.projectiles[1].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.Volley.projectiles[1].cooldownTime = 0.5f;
			gun.Volley.projectiles[1].angleVariance = 10f;
			gun.Volley.projectiles[1].numberOfShotsInClip = -1;

			Projectile projectile1 = UnityEngine.Object.Instantiate<Projectile>(gun.Volley.projectiles[1].projectiles[0]);
			projectile1.gameObject.SetActive(false);
			gun.Volley.projectiles[1].projectiles[0] = projectile1;
			projectile1.gameObject.AddComponent<MopProjectile>();

			projectile1.baseData.damage = 3f;
			FakePrefab.MarkAsFakePrefab(projectile1.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile1);
			bool aa = gun.Volley.projectiles[1] != gun.DefaultModule;
			if (aa)
			{
				gun.Volley.projectiles[1].ammoCost = 0;
			}
			projectile1.transform.parent = gun.barrelOffset;

			gun.Volley.projectiles[2].ammoCost = 1;
			gun.Volley.projectiles[2].shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.Volley.projectiles[2].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.Volley.projectiles[2].cooldownTime = 0.5f;
			gun.Volley.projectiles[2].angleVariance = 10f;
			gun.Volley.projectiles[2].numberOfShotsInClip = -1;

			Projectile projectile2 = UnityEngine.Object.Instantiate<Projectile>(gun.Volley.projectiles[2].projectiles[0]);
			projectile1.gameObject.SetActive(false);
			gun.Volley.projectiles[2].projectiles[0] = projectile1;

			projectile2.baseData.damage = 3f;
			projectile2.gameObject.AddComponent<MopProjectile>();
			FakePrefab.MarkAsFakePrefab(projectile2.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile2);
			bool ee = gun.Volley.projectiles[2] != gun.DefaultModule;
			if (ee)
			{
				gun.Volley.projectiles[2].ammoCost = 0;
			}
			projectile1.transform.parent = gun.barrelOffset;


			gun.barrelOffset.transform.localPosition = new Vector3(0.5f, 0.5f, 0f);
			gun.reloadTime = 2.5f;
			gun.SetBaseMaxAmmo(25);
			gun.muzzleFlashEffects = (PickupObjectDatabase.GetById(417) as Gun).muzzleFlashEffects;
			gun.CanReloadNoMatterAmmo = true;
			//gun.GoopReloadsFree = true;


			gun.quality = PickupObject.ItemQuality.D;
			gun.encounterTrackable.EncounterGuid = "mop";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			Mop.MopID = gun.PickupObjectId;

		}
		public static int MopID;
		public override void OnPostFired(PlayerController player, Gun bruhgun)
		{
			gun.PreventNormalFireAudio = true;
		}
		private bool HasReloaded;

		protected override void Update()
		{
			base.Update();
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
		protected override void OnPickup(PlayerController player)
		{
			IsBlob = false;
			IsCharm = false;
			IsCheese = false;
			Isfire = false;
			IsgreenFire = false;
			IsOil = false;
			IsPoison = false;
			IsWater = false;
			IsWeb = false;
			IsBlood = false;
			IsPoop = false;
			IsPossessive = false;
			IsFrail = false;
			var FireaAnim = gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation);
			FireaAnim.frames[0].eventInfo = "frame1";
			FireaAnim.frames[0].triggerEvent = true;
			FireaAnim.frames[1].eventInfo = "frame2";
			FireaAnim.frames[1].triggerEvent = true;
			FireaAnim.frames[2].eventInfo = "frame3";
			FireaAnim.frames[2].triggerEvent = true;
			FireaAnim.frames[3].eventInfo = "frame4";
			FireaAnim.frames[3].triggerEvent = true;
			FireaAnim.frames[4].eventInfo = "frame5";
			FireaAnim.frames[4].triggerEvent = true;
			FireaAnim.frames[5].eventInfo = "frame6";
			FireaAnim.frames[5].triggerEvent = true;

			var IdleAnim = gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.idleAnimation);

			IdleAnim.frames[0].eventInfo = "idle";
			IdleAnim.frames[0].triggerEvent = true;
			IdleAnim.frames[1].eventInfo = "idle";
			IdleAnim.frames[1].triggerEvent = true;
			IdleAnim.frames[2].eventInfo = "idle";
			IdleAnim.frames[2].triggerEvent = true;
			IdleAnim.frames[3].eventInfo = "idle";
			IdleAnim.frames[3].triggerEvent = true;

			var RelAAnim = gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation);
			RelAAnim.frames[0].eventInfo = "reload1";
			RelAAnim.frames[0].triggerEvent = true;
			RelAAnim.frames[1].eventInfo = "reload2";
			RelAAnim.frames[1].triggerEvent = true;
			RelAAnim.frames[2].eventInfo = "reload3";
			RelAAnim.frames[2].triggerEvent = true;
			RelAAnim.frames[3].eventInfo = "reload4";
			RelAAnim.frames[3].triggerEvent = true;

			RelAAnim.frames[13].eventInfo = "unreload1";
			RelAAnim.frames[13].triggerEvent = true;
			RelAAnim.frames[14].eventInfo = "unreload2";
			RelAAnim.frames[14].triggerEvent = true;
			RelAAnim.frames[15].eventInfo = "unreload3";
			RelAAnim.frames[15].triggerEvent = true;

			gun.spriteAnimator.AnimationEventTriggered += AnimationEventTriggered;
			base.OnPickup(player);
		}

		protected override void OnPostDrop(PlayerController player)
		{
			gun.spriteAnimator.AnimationEventTriggered -= AnimationEventTriggered;
			base.OnPostDrop(player);
		}

		private void AnimationEventTriggered(tk2dSpriteAnimator animator, tk2dSpriteAnimationClip clip, int frameIdx)
		{

			if (clip.GetFrame(frameIdx).eventInfo == "fard")
			{
				ETGModConsole.Log("fard");
			}

			if (clip.GetFrame(frameIdx).eventInfo == "reload1")
			{
				this.gun.carryPixelOffset = new IntVector2(1, 0);
			}

			if (clip.GetFrame(frameIdx).eventInfo == "reload2")
			{
				this.gun.carryPixelOffset = new IntVector2(1, -5);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "reload3")
			{
				this.gun.carryPixelOffset = new IntVector2(2, -7);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "reload4")
			{
				this.gun.carryPixelOffset = new IntVector2(3, -8);
			}

			if (clip.GetFrame(frameIdx).eventInfo == "reload1")
			{
				this.gun.carryPixelOffset = new IntVector2(1, 0);
			}

			if (clip.GetFrame(frameIdx).eventInfo == "unreload1")
			{
				this.gun.carryPixelOffset = new IntVector2(3, -8);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "unreload2")
			{
				this.gun.carryPixelOffset = new IntVector2(2, -7);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "unreload3")
			{
				this.gun.carryPixelOffset = new IntVector2(1, -5);
			}




			if (clip.GetFrame(frameIdx).eventInfo == "idle")
			{
				this.gun.carryPixelOffset = new IntVector2((int)2.5f, (int)-0.5f);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "frame1")
			{
				this.gun.carryPixelOffset = new IntVector2((int)2.5f, (int)2.5f);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "frame2")
			{
				this.gun.carryPixelOffset = new IntVector2(8, -30);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "frame3")
			{
				this.gun.carryPixelOffset = new IntVector2(10, -27);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "frame4")
			{
				this.gun.carryPixelOffset = new IntVector2(5, -22);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "frame5")
			{
				this.gun.carryPixelOffset = new IntVector2(4, -14);
			}
			if (clip.GetFrame(frameIdx).eventInfo == "frame6")
			{
				this.gun.carryPixelOffset = new IntVector2(2, -4);
			}
		}
		public override void OnReloadPressed(PlayerController player, Gun bruhgun, bool bSOMETHING)
		{
			
			if (gun.IsReloading && this.HasReloaded)
			{
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
			this.goop = player.CurrentGoop;
			IsBlob = false;
			IsCharm = false;
			IsCheese = false;
			Isfire = false;
			IsgreenFire = false;
			IsOil = false;
			IsPoison = false;
			IsWater = false;
			IsWeb = false;
			IsPossessive = false;
			IsPoop = false;
			IsFrail = false;
			IsBlood = false;

			if (this.goop != null)
            {
				gun.GainAmmo(Mathf.Max(0, gun.ClipCapacity - gun.ClipShotsRemaining));
				bool fire = this.goop == EasyGoopDefinitions.FireDef | this.goop == EasyGoopDefinitions.FireDef2 | this.goop.name == "HelicopterNapalmGoop" | this.goop.name == "Napalm Goop" | this.goop.name == "NapalmGoopShortLife" | this.goop.name == "BulletKingWineGoop" | this.goop.name == "DevilGoop" | this.goop.name == "FlameLineGoop" | this.goop.name == "DemonWallGoop" | this.goop.name == "DemonWallGoop"; 
				if (fire)
				{
					Isfire = true;
				}
				bool greenfire = this.goop == EasyGoopDefinitions.GreenFireDef | this.goop.name == "GreenNapalmGoopThatWorks";
				if (greenfire)
				{
					IsgreenFire = true;
				}
				bool blob = this.goop == EasyGoopDefinitions.BlobulonGoopDef | this.goop.name == "BlobulordGoop";
				if (blob)
				{
					IsBlob = true;
				}
				bool OIL = this.goop == EasyGoopDefinitions.OilDef | this.goop.name == "Oil Goop";
				if (OIL)
				{
					IsOil = true;
				}
				bool cheese = this.goop == EasyGoopDefinitions.CheeseDef;
				if (cheese)
				{
					IsCheese = true;
				}
				bool water = this.goop == EasyGoopDefinitions.WaterGoop | this.goop.name == "MimicSpitGoop";
				if (water)
				{
					IsWater = true;
				}
				bool charm = this.goop == EasyGoopDefinitions.CharmGoopDef;	
				if (charm)
				{
					IsCharm = true;
				}
				bool posion = this.goop == EasyGoopDefinitions.PoisonDef | this.goop.name == "ResourcefulRatPoisonGoop" | this.goop.name == "MeduziPoisonGoop";
				if (posion)
				{
					IsPoison = true;
				}
				bool web = this.goop == EasyGoopDefinitions.WebGoop;
				if (web)
				{
					IsWeb = true;
				}
				bool blood = this.goop.name == "PermanentBloodGoop" | this.goop.name == "BloodGoop" | this.goop.name == "BloodbulonGoop";
				if (blood)
				{
					IsBlood = true;
				}
				bool poop = this.goop.name == "PoopulonGoop";
				if (poop)
				{
					IsPoop = true;
				}
				bool possession = this.goop == DebuffLibrary.PossesedPuddle;
				if (possession)
				{

					IsPossessive = true;
				}
				bool frail = this.goop == DebuffLibrary.FrailPuddle;
				if (frail)
				{
					IsFrail = true;
				}
			}
			
			/*
			ETGModConsole.Log("================================================");
			ETGModConsole.Log("Blob:" + IsBlob.ToString());
			ETGModConsole.Log("Charm:" + IsCharm.ToString());
			ETGModConsole.Log("Cheese:" + IsCheese.ToString());
			ETGModConsole.Log("Fire:" + Isfire.ToString());
			ETGModConsole.Log("Green Fire:" + IsgreenFire.ToString());
			ETGModConsole.Log("Oil:" + IsOil.ToString());
			ETGModConsole.Log("Poison:" + IsPoison.ToString());
			ETGModConsole.Log("Water:" + IsWater.ToString());
			ETGModConsole.Log("Web:" + IsWeb.ToString());
			ETGModConsole.Log("================================================");
			*/
			DeadlyDeadlyGoopManager.DelayedClearGoopsInRadius(player.CenterPosition, 2f);
		}
		public static bool Isfire;
		public static bool IsOil;
		public static bool IsPoison;
		public static bool IsWeb;
		public static bool IsWater;
		public static bool IsCharm;
		public static bool IsgreenFire;
		public static bool IsCheese;
		public static bool IsBlob;
		public static bool IsBlood;
		public static bool IsPoop;
		public static bool IsPossessive;
		public static bool IsFrail;
		GoopDefinition goop;
	}
}