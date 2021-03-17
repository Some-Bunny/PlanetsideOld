﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gungeon;
using ItemAPI;
using UnityEngine;

namespace Planetside
{
    class HardlightNailgun : MultiActiveReloadController
    {
        public static void Add()
        {
            string shorthandName = "hardlight_nailgun";
            Gun gun = ETGMod.Databases.Items.NewGun("Hardlight Nailgun", "rake");
            Game.Items.Rename("outdated_gun_mods:" + shorthandName, "psog:" + shorthandName);
            var behav = gun.gameObject.AddComponent<HardlightNailgun>();
            gun.SetShortDescription("Hammer To A Gunfight");
            gun.SetLongDescription("On reloading, activating the active reload switches modes. \n\nA revolutionary nailgun that nails together materials with solid light! This one was weaponized.");
            gun.SetupSprite(null, "rake_idle_001", 6);
            gun.SetAnimationFPS(gun.shootAnimation, 24);

            GunExt.AddProjectileModuleFrom(gun, PickupObjectDatabase.GetById(26) as Gun, true, false);
            gun.SetBaseMaxAmmo(250);
            gun.DefaultModule.ammoCost = 1;

            //gun.gunSwitchGroup = (PickupObjectDatabase.GetById(577) as Gun).gunSwitchGroup;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].eventAudio = "Play_WPN_dl45heavylaser_shot_01";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.shootAnimation).frames[0].triggerEvent = true;
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].eventAudio = "Play_WPN_dl45heavylaser_reload";
            gun.GetComponent<tk2dSpriteAnimator>().GetClipByName(gun.reloadAnimation).frames[0].triggerEvent = true;
            Projectile component = ((Gun)ETGMod.Databases.Items[377]).DefaultModule.projectiles[0];
            Projectile replacementProjectile = component.projectile;
            gun.DefaultModule.usesOptionalFinalProjectile = true;
            gun.DefaultModule.numberOfFinalProjectiles = 0;
            gun.DefaultModule.finalProjectile = replacementProjectile;
            gun.DefaultModule.finalCustomAmmoType = gun.DefaultModule.customAmmoType;
            gun.DefaultModule.finalAmmoType = gun.DefaultModule.ammoType;

            gun.DefaultModule.shootStyle = ProjectileModule.ShootStyle.Automatic;
            gun.DefaultModule.sequenceStyle = ProjectileModule.ProjectileSequenceStyle.Random;
            gun.reloadTime = 1.6f;
            gun.DefaultModule.cooldownTime = 0.33f;
            //gun.gunClass = GunClass.SHITTY;
            gun.DefaultModule.numberOfShotsInClip = 10;
            gun.quality = PickupObject.ItemQuality.C;
            Gun gun2 = PickupObjectDatabase.GetById(223) as Gun;
            gun.muzzleFlashEffects = gun2.muzzleFlashEffects;
            //gun.gunSwitchGroup = gun2.gunSwitchGroup;
            //gun.gunHandedness = GunHandedness.TwoHanded;
            gun.barrelOffset.transform.localPosition = new Vector3(0.75f, .3125f, 0f);
            gun.DefaultModule.angleVariance = 4f;
            gun.encounterTrackable.EncounterGuid = "MUL-T GANG";
            gun.sprite.IsPerpendicular = true;
            Projectile projectile = UnityEngine.Object.Instantiate<Projectile>(gun.DefaultModule.projectiles[0]);
            projectile.gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(projectile.gameObject);
            UnityEngine.Object.DontDestroyOnLoad(projectile);
            gun.DefaultModule.projectiles[0] = projectile;
            projectile.baseData.damage = 6f;
            projectile.baseData.speed *= 1f;
            projectile.baseData.force *= 1f;
            projectile.baseData.range *= 4;
            projectile.HasDefaultTint = true;
            //Gun FUUUU = PickupObjectDatabase.GetById(577) as Gun;
            //gun.gunSwitchGroup = FUUUU.gunSwitchGroup;
            ETGMod.Databases.Items.Add(gun, null, "ANY");
            HardlightNailgun.HardAsNailsID = gun.PickupObjectId;
            projectile.transform.parent = gun.barrelOffset;
            behav.activeReloadEnabled = true;
            behav.reloads = new List<MultiActiveReloadData>
            {
                new MultiActiveReloadData(0.625f, 40, 60, 0, 5 * 5, false, true, new ActiveReloadData
                {
                    damageMultiply = 2f,

                }, true, "SwitchClip"),
            };
        }
        public static int HardAsNailsID;

        private bool HasReloaded;
        private bool Switchclip = false;
        protected override void Update()
        {
            if (gun.CurrentOwner)
            {
                if (!gun.IsReloading && !HasReloaded)
                {
                    this.HasReloaded = true;
                }
            }
        }
        public override void OnReloadEndedSafe(PlayerController player, Gun gun)
        {
            base.OnReloadEnded(player, gun);
            if (Switchclip == true)
            {
                Switchclip = false;
            }
            //player.stats.RecalculateStats(player, false, false);
        }
        public override void PostProcessVolley(ProjectileVolleyData volley)
        {
            base.PostProcessVolley(volley);
        }
        public override void OnActiveReloadSuccess(MultiActiveReload reload)
        {
            base.OnActiveReloadSuccess(reload);
            PlayerController player = gun.CurrentOwner as PlayerController;
            if (reload.Name == "SwitchClip" && Switchclip == false)
            {
                Switchclip = true;
                this.AllClip(player);
            }
            else
            {
                this.NoClip(player);
            }
        }
        private void AllClip(PlayerController player)
        {
            float clip = 0f;
            clip = (player.stats.GetStatValue(PlayerStats.StatType.AdditionalClipCapacityMultiplier));
            int num = (int)(10 * clip);
            foreach (ProjectileModule projectileModule in this.gun.Volley.projectiles)
            {
                this.gun.DefaultModule.numberOfFinalProjectiles = num;
            }

        }
        public override void OnPostFired(PlayerController player, Gun flakcannon)
        {
            gun.PreventNormalFireAudio = true;
            //AkSoundEngine.PostEvent("Play_wpn_chargelaser_shot_01", gameObject);
        }
        private void NoClip(PlayerController player)
        {
            int num = 0;
            foreach (ProjectileModule projectileModule in this.gun.Volley.projectiles)
            {
                this.gun.DefaultModule.numberOfFinalProjectiles = num;
            }

        }
        public override void OnReloadPressed(PlayerController player, Gun gun, bool bSOMETHING)
        {
            if (gun.IsReloading && this.HasReloaded)
            {
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                this.NoClip(player);
                HasReloaded = false;
                Switchclip = false;
                AkSoundEngine.PostEvent("Stop_WPN_All", base.gameObject);
                base.OnReloadPressed(player, gun, bSOMETHING);
            }
        }
        public override void PostProcessProjectile(Projectile projectile)
        {
            projectile.DefaultTintColor = new Color(255f, 255f, 255f).WithAlpha(0.95f);
        }

        public HardlightNailgun()
        {
        }
        public void AddCurrentGunStatModifier(PlayerStats.StatType statType, float amount, StatModifier.ModifyMethod modifyMethod)
        {
            gun.currentGunStatModifiers = gun.currentGunStatModifiers.Concat(new StatModifier[] { new StatModifier { statToBoost = statType, amount = amount, modifyType = modifyMethod } }).ToArray();
        }

        public void RemoveCurrentGunStatModifier(PlayerStats.StatType statType)
        {
            List<StatModifier> list = new List<StatModifier>();
            foreach (StatModifier mod in gun.currentGunStatModifiers)
            {
                if (mod.statToBoost != statType)
                {
                    list.Add(mod);
                }
            }
            gun.currentGunStatModifiers = list.ToArray();
        }
    }
}