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
	public class SoulLantern : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("Soul Lantern", "lantern");
			Game.Items.Rename("outdated_gun_mods:soul_lantern", "psog:soul_lantern");
			gun.gameObject.AddComponent<SoulLantern>();

			GunExt.SetShortDescription(gun, "Light The Way");
			GunExt.SetLongDescription(gun, "A lantern filled with the souls that came before and after its time. Who knows how old it is, and if it even was manufactured by human hands...");
			GunExt.SetupSprite(gun, null, "lantern_idle_001", 11);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 15);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 10);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 4);
			gun.AddPassiveStatModifier(PlayerStats.StatType.Curse, 0.5f, StatModifier.ModifyMethod.ADDITIVE);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(378) as Gun, true, false);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(52) as Gun).gunSwitchGroup;
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_WPN_Life_Orb_Fade_01";
			gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 0f;
			gun.DefaultModule.cooldownTime = .2f;
			gun.DefaultModule.numberOfShotsInClip = -1;
			gun.SetBaseMaxAmmo(500);
			gun.quality = PickupObject.ItemQuality.B;
			gun.DefaultModule.angleVariance = 30f;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage = 4f;
			projectile.baseData.speed = 1f;
			projectile.sprite.renderer.enabled = false;
			projectile.AdditionalScaleMultiplier *= 0.5f;
			projectile.shouldRotate = true;
			projectile.pierceMinorBreakables = true;
			//projectile.baseData.range = 5.8f;
			gun.carryPixelOffset = new IntVector2((int)2f, (int)2f);
			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 1;


			HomingModifier homing = projectile.gameObject.AddComponent<HomingModifier>();
			homing.HomingRadius = 250f;
			homing.AngularVelocity = 120f;

			gun.encounterTrackable.EncounterGuid = "The Jar";
			ETGMod.Databases.Items.Add(gun, null, "ANY");
			gun.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);
			gun.PreventNormalFireAudio = true;
			gun.SetupUnlockOnCustomFlag(CustomDungeonFlags.BULLETBANK_DEFEATED, true);

		}

		private bool HasReloaded;
		public override void PostProcessProjectile(Projectile projectile)
		{
			TrailRenderer tr;
			var tro = projectile.gameObject.AddChild("trail object");
			tro.transform.position = projectile.transform.position;
			tro.transform.localPosition = new Vector3(0f, 0f, 0f);
			tr = tro.AddComponent<TrailRenderer>();
			tr.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			tr.receiveShadows = false;
			var mat = new Material(Shader.Find("Sprites/Default"));
			mat.mainTexture = _gradTexture;
			mat.SetColor("_Color", new Color(3f, 2f, 0f, 0.7f));
			tr.material = mat;
			tr.time = 0.3f;
			tr.minVertexDistance = 0.1f;
			tr.startWidth = 0.1f;
			tr.endWidth = 0f;
			tr.startColor = Color.white;
			tr.endColor = new Color(3f, 2f, 0f, 0f);
			projectile.OnHitEnemy = (Action<Projectile, SpeculativeRigidbody, bool>)Delegate.Combine(projectile.OnHitEnemy, new Action<Projectile, SpeculativeRigidbody, bool>(this.HandleHit));
			base.StartCoroutine(this.Speed(projectile));
		}
		public Texture _gradTexture;
		public IEnumerator Speed(Projectile projectile)
		{
			bool flag = this.gun.CurrentOwner != null;
			bool flag3 = flag;
			if (flag3)
			{
				for (int i = 0; i < 15; i++)
				{
					projectile.baseData.speed += 1f;
					projectile.UpdateSpeed();
					yield return new WaitForSeconds(0.1f);
				}
			}
			yield break;
		}
		private void HandleHit(Projectile arg1, SpeculativeRigidbody arg2, bool arg3)
		{
			PlayerController player = this.gun.CurrentOwner as PlayerController;

			bool flag = arg2.aiActor != null && !arg2.healthHaver.IsDead && arg2.aiActor.behaviorSpeculator && !arg2.aiActor.IsHarmlessEnemy && arg2.aiActor != null;
			if (flag)
			{
				arg1.AppliesPoison = true;
				arg1.PoisonApplyChance = 1f;
				arg1.healthEffect = DebuffLibrary.Possessed;
			}


		}

		public override void OnPostFired(PlayerController player, Gun bruhgun)
		{
			bruhgun.PreventNormalFireAudio = true;
		}
		public override void OnReloadPressed(PlayerController player, Gun bruhgun, bool bSOMETHING)
		{
			if (gun.IsReloading && this.HasReloaded)
			{
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
			}
		}
	}
}