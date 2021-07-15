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
	public class PurplePain : GunBehaviour
	{
		public static void Add()
		{
			Gun gun = ETGMod.Databases.Items.NewGun("tets", "knightsword");
			Game.Items.Rename("outdated_gun_mods:tets", "psog:tets");
			gun.gameObject.AddComponent<PurplePain>();
			gun.SetShortDescription("Reaped By Death");
			gun.SetLongDescription("A simple, elegant and powerful revolver, capable of killing even behind cover.\n\nWielded by a particularly regretful undead Gungeoneer seeking an old friend...");
			GunExt.SetupSprite(gun, null, "knightsword_idle_001", 11);
			GunExt.SetAnimationFPS(gun, gun.shootAnimation, 8);
			GunExt.SetAnimationFPS(gun, gun.reloadAnimation, 7);
			GunExt.SetAnimationFPS(gun, gun.idleAnimation, 1);
			GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(56) as Gun, true, false);
			gun.gunSwitchGroup = (PickupObjectDatabase.GetById(387) as Gun).gunSwitchGroup;
			gun.DefaultModule.ammoCost = 1;
			gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
			gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
			gun.reloadTime = 1.7f;
			gun.DefaultModule.cooldownTime = .5f;
			gun.DefaultModule.numberOfShotsInClip = 5;
			gun.SetBaseMaxAmmo(50);
			gun.quality = PickupObject.ItemQuality.EXCLUDED;
			gun.DefaultModule.angleVariance = 6f;
			gun.DefaultModule.burstShotCount = 1;
			Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
			projectile.gameObject.SetActive(false);
			FakePrefab.MarkAsFakePrefab(projectile.gameObject);
			UnityEngine.Object.DontDestroyOnLoad(projectile);
			gun.DefaultModule.projectiles[0] = projectile;
			projectile.baseData.damage = 30f;
			projectile.baseData.speed *= 3f;
			projectile.AdditionalScaleMultiplier *= 0.75f;
			projectile.shouldRotate = true;
			projectile.pierceMinorBreakables = true;
			projectile.PenetratesInternalWalls = true;
			gun.IsHeroSword = true;
			gun.HeroSwordDoesntBlank = false;
			PierceProjModifier spook = projectile.gameObject.AddComponent<PierceProjModifier>();
			spook.penetration = 3;
			//projectile.baseData.range = 5.8f;
			gun.encounterTrackable.EncounterGuid = "ree";
			ETGMod.Databases.Items.Add(gun, null, "ANY");

		}
		public override void PostProcessProjectile(Projectile projectile)
		{
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
				AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
				HasReloaded = false;
				base.OnReloadPressed(player, gun, bSOMETHING);
				AkSoundEngine.PostEvent("Play_WPN_rpg_reload_01", gameObject);
			}
		}
	}

}


namespace TurboItems
{
    public class FinalCountdown : GunBehaviour
    {


        public static void Add()
        {
            Gun gun = ETGMod.Databases.Items.NewGun("Final Countdown", "final_countdown");
            Game.Items.Rename("outdated_gun_mods:final_countdown", "turbo:final_countdown");
            gun.gameObject.AddComponent<FinalCountdown>();
            gun.SetShortDescription("*sick guitar riff*");
            gun.SetLongDescription("Fires many final projectiles from whatever guns it feels like in no particular order.\n\nIT'S THE FI-NAL COUNT-DOWN");
            gun.SetupSprite(null, "final_countdown_idle_001", 24);
            gun.SetAnimationFPS(gun.shootAnimation, 24);
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 0f;
            gun.DefaultModule.cooldownTime = 0.2f;
            gun.DefaultModule.numberOfShotsInClip = -1;
            gun.quality = PickupObject.ItemQuality.S;
            gun.InfiniteAmmo = true;
            //gun.SetBaseMaxAmmo(350);
            gun.encounterTrackable.EncounterGuid = "*sick guitar riff* IT'S THE FINAL COUNTDOWN";


            for (int i = 0; i < 8; i++)
            {
                gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(88) as Gun, true, false);

            }
            //ice breaker
            Projectile IceBreakerFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(225) as Gun).DefaultModule.finalProjectile);
            IceBreakerFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(IceBreakerFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(IceBreakerFinalProjectile);

            //zorgun
            Projectile ZorgunFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(6) as Gun).DefaultModule.finalProjectile);
            ZorgunFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(ZorgunFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(ZorgunFinalProjectile);

            //judge
            Projectile JudgeFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(184) as Gun).DefaultModule.finalProjectile);
            JudgeFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(JudgeFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(JudgeFinalProjectile);

            //teapot
            Projectile TeapotFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(596) as Gun).DefaultModule.finalProjectile);
            TeapotFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(TeapotFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(TeapotFinalProjectile);

            //luxin cannon
            Projectile LuxinCannonFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(199) as Gun).DefaultModule.finalProjectile);
            LuxinCannonFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(LuxinCannonFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(LuxinCannonFinalProjectile);

            //barrel (like shooting fish's synergy form)
            Projectile BarrelFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(709) as Gun).DefaultModule.finalProjectile);
            BarrelFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(BarrelFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BarrelFinalProjectile);

            //eye of the beholster
            Projectile BeholsterFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(90) as Gun).DefaultModule.finalProjectile);
            BeholsterFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(BeholsterFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BeholsterFinalProjectile);

            //finished gun
            Projectile FinishedFinalProjectile = UnityEngine.Object.Instantiate<Projectile>((PickupObjectDatabase.GetById(762) as Gun).DefaultModule.finalProjectile);
            FinishedFinalProjectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(FinishedFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(FinishedFinalProjectile);

            gun.Volley.projectiles[0].ammoCost = 1;
            gun.Volley.projectiles[0].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[0].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[0].cooldownTime = 0.2f;
            gun.Volley.projectiles[0].angleVariance = 9f;
            gun.Volley.projectiles[0].numberOfShotsInClip = -1;
            IceBreakerFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[0].projectiles[0] = IceBreakerFinalProjectile;
            FakePrefab.MarkAsFakePrefab(IceBreakerFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(IceBreakerFinalProjectile);
            gun.DefaultModule.projectiles[0] = IceBreakerFinalProjectile;
            bool flag = gun.Volley.projectiles[0] != gun.DefaultModule;
            if (flag)
            {
                gun.Volley.projectiles[0].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[1].ammoCost = 1;
            gun.Volley.projectiles[1].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[1].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[1].cooldownTime = 0.2f;
            gun.Volley.projectiles[1].angleVariance = 9f;
            gun.Volley.projectiles[1].numberOfShotsInClip = -1;
            ZorgunFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[1].projectiles[0] = ZorgunFinalProjectile;
            FakePrefab.MarkAsFakePrefab(ZorgunFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(ZorgunFinalProjectile);
            gun.DefaultModule.projectiles[1] = ZorgunFinalProjectile;
            bool flag1 = gun.Volley.projectiles[1] != gun.DefaultModule;
            if (flag1)
            {
                gun.Volley.projectiles[1].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[2].ammoCost = 1;
            gun.Volley.projectiles[2].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[2].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[2].cooldownTime = 0.2f;
            gun.Volley.projectiles[2].angleVariance = 9f;
            gun.Volley.projectiles[2].numberOfShotsInClip = -1;
            JudgeFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[2].projectiles[0] = JudgeFinalProjectile;
            FakePrefab.MarkAsFakePrefab(JudgeFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(JudgeFinalProjectile);
            gun.DefaultModule.projectiles[2] = JudgeFinalProjectile;
            bool flag2 = gun.Volley.projectiles[2] != gun.DefaultModule;
            if (flag2)
            {
                gun.Volley.projectiles[2].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[3].ammoCost = 1;
            gun.Volley.projectiles[3].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[3].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[3].cooldownTime = 0.2f;
            gun.Volley.projectiles[3].angleVariance = 9f;
            gun.Volley.projectiles[3].numberOfShotsInClip = -1;
            JudgeFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[3].projectiles[0] = TeapotFinalProjectile;
            FakePrefab.MarkAsFakePrefab(TeapotFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(TeapotFinalProjectile);
            gun.DefaultModule.projectiles[3] = TeapotFinalProjectile;
            bool flag3 = gun.Volley.projectiles[3] != gun.DefaultModule;
            if (flag3)
            {
                gun.Volley.projectiles[3].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[4].ammoCost = 1;
            gun.Volley.projectiles[4].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[4].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[4].cooldownTime = 0.2f;
            gun.Volley.projectiles[4].angleVariance = 9f;
            gun.Volley.projectiles[4].numberOfShotsInClip = -1;
            JudgeFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[4].projectiles[0] = LuxinCannonFinalProjectile;
            FakePrefab.MarkAsFakePrefab(LuxinCannonFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(LuxinCannonFinalProjectile);
            gun.DefaultModule.projectiles[4] = LuxinCannonFinalProjectile;
            bool flag4 = gun.Volley.projectiles[4] != gun.DefaultModule;
            if (flag4)
            {
                gun.Volley.projectiles[4].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[5].ammoCost = 1;
            gun.Volley.projectiles[5].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[5].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[5].cooldownTime = 0.2f;
            gun.Volley.projectiles[5].angleVariance = 9f;
            gun.Volley.projectiles[5].numberOfShotsInClip = -1;
            JudgeFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[5].projectiles[0] = BarrelFinalProjectile;
            FakePrefab.MarkAsFakePrefab(BarrelFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BarrelFinalProjectile);
            gun.DefaultModule.projectiles[5] = BarrelFinalProjectile;
            bool flag5 = gun.Volley.projectiles[5] != gun.DefaultModule;
            if (flag5)
            {
                gun.Volley.projectiles[5].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[6].ammoCost = 1;
            gun.Volley.projectiles[6].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[6].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[6].cooldownTime = 0.2f;
            gun.Volley.projectiles[6].angleVariance = 9f;
            gun.Volley.projectiles[6].numberOfShotsInClip = -1;
            JudgeFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[6].projectiles[0] = BeholsterFinalProjectile;
            FakePrefab.MarkAsFakePrefab(BeholsterFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(BeholsterFinalProjectile);
            gun.DefaultModule.projectiles[6] = BeholsterFinalProjectile;
            bool flag6 = gun.Volley.projectiles[6] != gun.DefaultModule;
            if (flag6)
            {
                gun.Volley.projectiles[6].ammoCost = 0;
            }
            //=================================================================
            gun.Volley.projectiles[7].ammoCost = 1;
            gun.Volley.projectiles[7].shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.Volley.projectiles[7].sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.Volley.projectiles[7].cooldownTime = 0.2f;
            gun.Volley.projectiles[7].angleVariance = 9f;
            gun.Volley.projectiles[7].numberOfShotsInClip = -1;
            JudgeFinalProjectile.gameObject.SetActive(false);
            gun.Volley.projectiles[7].projectiles[0] = FinishedFinalProjectile;
            FakePrefab.MarkAsFakePrefab(FinishedFinalProjectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(FinishedFinalProjectile);
            gun.DefaultModule.projectiles[7] = FinishedFinalProjectile;
            bool flag7 = gun.Volley.projectiles[7] != gun.DefaultModule;
            if (flag7)
            {
                gun.Volley.projectiles[7].ammoCost = 0;
            }
            //God kill me.
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            FinalCountdownID = gun.PickupObjectId;
        }
        public static int FinalCountdownID;
        public override void OnPostFired(PlayerController player, Gun gun)
        {
            gun.PreventNormalFireAudio = true;
            AkSoundEngine.PostEvent("Play_WPN_smileyrevolver_shot_01", gameObject);
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
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_WPN_SAA_reload_01", base.gameObject);
            }
        }
    }
}



/*


using System;
using System.Collections;
using Gungeon;
using MonoMod;
using UnityEngine;
using ItemAPI;
using MultiplayerBasicExample;

namespace Knives
{

    public class Baba : AdvancedGunBehaviour
    {
        public static void Add()
        {
            // Get yourself a new gun "base" first.
            // Let's just call it "Basic Gun", and use "jpxfrd" for all sprites and as "codename" All sprites must begin with the same word as the codename. For example, your firing sprite would be named "jpxfrd_fire_001".
            Gun gun = ETGMod.Databases.Items.NewGun("Baba Yaga", "pbpp");
            // "kp:basic_gun determines how you spawn in your gun through the console. You can change this command to whatever you want, as long as it follows the "name:itemname" template.
            Game.Items.Rename("outdated_gun_mods:baba_yaga", "ski:baba_yaga");
            gun.gameObject.AddComponent<Baba>();
            //These two lines determines the description of your gun, ".SetShortDescription" being the description that appears when you pick up the gun and ".SetLongDescription" being the description in the Ammonomicon entry. 
            gun.SetShortDescription("Para Bellum");
            gun.SetLongDescription("A pocket pistol bloodied by years of contract killing. When it's owner quit the trade this gun was lost to the gungeon. When reloading hitting an enemy will steal ammo from them. Luckily, the ammo is universal." +
                "\n\n\n - Knife_to_a_Gunfight");
            // This is required, unless you want to use the sprites of the base gun.
            // That, by default, is the pea shooter.
            // SetupSprite sets up the default gun sprite for the ammonomicon and the "gun get" popup.
            // WARNING: Add a copy of your default sprite to Ammonomicon Encounter Icon Collection!
            // That means, "sprites/Ammonomicon Encounter Icon Collection/defaultsprite.png" in your mod .zip. You can see an example of this with inside the mod folder.
            gun.SetupSprite(null, "pbpp_idle_001", 8);
            // ETGMod automatically checks which animations are available.
            // The numbers next to "shootAnimation" determine the animation fps. You can also tweak the animation fps of the reload animation and idle animation using this method.
            gun.SetAnimationFPS(gun.shootAnimation, 36);
            gun.SetAnimationFPS(gun.reloadAnimation, 8);
            // Every modded gun has base projectile it works with that is borrowed from other guns in the game. 
            // The gun names are the names from the JSON dump! While most are the same, some guns named completely different things. If you need help finding gun names, ask a modder on the Gungeon discord.
            gun.AddProjectileModuleFrom(PickupObjectDatabase.GetById(79) as Gun, true, false);
            // Here we just take the default projectile module and change its settings how we want it to be.
            gun.DefaultModule.ammoCost = 1;
            gun.DefaultModule.angleVariance = 0f;
            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.SemiAutomatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1f;
            gun.DefaultModule.cooldownTime = .1f;
            gun.DefaultModule.numberOfShotsInClip = 9;
            gun.SetBaseMaxAmmo(36);
            // Here we just set the quality of the gun and the "EncounterGuid", which is used by Gungeon to identify the gun.
            gun.quality = PickupObject.ItemQuality.B;
            gun.encounterTrackable.EncounterGuid = "I guess I'm back";
            //This block of code helps clone our projectile. Basically it makes it so things like Shadow Clone and Hip Holster keep the stats/sprite of your custom gun's projectiles.
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            //projectile.baseData allows you to modify the base properties of your projectile module.
            //In our case, our gun uses modified projectiles from the ak-47.
            //Setting static values for a custom gun's projectile stats prevents them from scaling with player stats and bullet modifiers (damage, shotspeed, knockback)
            //You have to multiply the value of the original projectile you're using instead so they scale accordingly. For example if the projectile you're using as a base has 10 damage and you want it to be 6 you use this
            //In our case, our projectile has a base damage of 5.5, so we multiply it by 1.1 so it does 10% more damage from the ak-47.
            projectile.baseData.damage *= 1f;
            projectile.baseData.speed *= 1f;
            projectile.transform.parent = gun.barrelOffset;

            ETGMod.Databases.Items.Add(gun, null, "ANY");

        }


        public override void OnPostFired(PlayerController player, Gun gun)
        {
            AkSoundEngine.PostEvent("Play_WPN_beretta_shot_01", base.gameObject);

        }
        private bool HasReloaded;
        //This block of code allows us to change the reload sounds.
        protected override void Update()
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

        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                HasReloaded = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
                AkSoundEngine.PostEvent("Play_cata_reload_sfx", base.gameObject);


            }

            if (gun.ClipShotsRemaining < gun.ClipCapacity)
            {
                Gun swipeFlash = (Gun)PickupObjectDatabase.GetByEncounterName("Silencer");
                Projectile projectile1 = ((Gun)ETGMod.Databases.Items[3]).DefaultModule.projectiles[0];
                GameObject gameObject = SpawnManager.SpawnProjectile(projectile1.gameObject, player.CurrentGun.sprite.WorldCenter, Quaternion.Euler(0f, 0f, (player.CurrentGun == null) ? 0f : player.CurrentGun.CurrentAngle), true);
                Projectile component = gameObject.GetComponent<Projectile>();
                component.baseData.damage = 3f;
                component.Owner = player;
                ProjectileSlashingBehaviour slasher = component.gameObject.GetOrAddComponent<ProjectileSlashingBehaviour>();
                slasher.SlashRange = 4f;
                slasher.SlashDimensions = 5;
                slasher.SlashVFX = swipeFlash.muzzleFlashEffects;
                slasher.DoSound = false;

                routinedoer(player);
            }
        }
        public void routinedoer(PlayerController player)
        {
            Vector2 vector = player.unadjustedAimPoint.XY() - player.CenterPosition;

            GameManager.Instance.StartCoroutine(this.HandleSwing(player, vector, .001f, 4f));
        }

        private IEnumerator HandleSwing(PlayerController user, Vector2 aimVec, float rayDamage, float rayLength)
        {
            float elapsed = 0f;
            yield return new WaitForSecondsRealtime(.01f);
            while (elapsed < 1)
            {

                elapsed += BraveTime.DeltaTime;
                SpeculativeRigidbody hitRigidbody = this.IterativeRaycast(user.CenterPosition, aimVec, rayLength, int.MaxValue, user.specRigidbody);
                if (hitRigidbody != null)
                {
                    AiactorSpecialStates state = hitRigidbody.aiActor.gameObject.GetOrAddComponent<AiactorSpecialStates>();
                    if (hitRigidbody && hitRigidbody.aiActor && hitRigidbody.aiActor.IsNormalEnemy && state.LootedByBaba == false)
                    {
                        LootEngine.SpawnItem(PickupObjectDatabase.GetById(78).gameObject, hitRigidbody.sprite.WorldCenter, aimVec * -1, 5f, false, false, false);
                        state.LootedByBaba = true;
                        hitRigidbody.aiActor.behaviorSpeculator.Stun(1f, true);
                        yield break;
                    }

                }


            }
            yield break;
        }

        protected SpeculativeRigidbody IterativeRaycast(Vector2 rayOrigin, Vector2 rayDirection, float rayDistance, int collisionMask, SpeculativeRigidbody ignoreRigidbody)
        {
            int num = 0;
            RaycastResult raycastResult;
            while (PhysicsEngine.Instance.Raycast(rayOrigin, rayDirection, rayDistance, out raycastResult, true, true, collisionMask, new CollisionLayer?(CollisionLayer.BulletBlocker), false, null, ignoreRigidbody))
            {
                num++;
                SpeculativeRigidbody speculativeRigidbody = raycastResult.SpeculativeRigidbody;
                if (num < 3 && speculativeRigidbody != null)
                {
                    MinorBreakable component = speculativeRigidbody.GetComponent<MinorBreakable>();
                    if (component != null)
                    {

                        component.Break(rayDirection.normalized * 3f);
                        RaycastResult.Pool.Free(ref raycastResult);
                        continue;
                    }
                }
                RaycastResult.Pool.Free(ref raycastResult);
                return speculativeRigidbody;
            }
            return null;
        }
    }
}
*/