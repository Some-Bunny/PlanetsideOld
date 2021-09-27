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
using SaveAPI;

namespace Planetside
{
    public class BloodIdol  : PlayerItem
    {

        public static void Init()
        {
            string itemName = "Blood Idol";
            string resourceName = "Planetside/Resources/bloodidol.png";
            GameObject obj = new GameObject(itemName);
            BloodIdol lockpicker = obj.AddComponent<BloodIdol>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "The Work Never Stops";
            string longDesc = "An 1dol dedicated to Kaliber, demanding the blood of slain Gundead.\n\nThough you may not complete her quest in this lifetime, she is faithful and will keep watch for many lifetimes.";
            lockpicker.SetupItem(shortDesc, longDesc, "psog");
            lockpicker.SetCooldownType(ItemBuilder.CooldownType.Timed, 1f);
            lockpicker.consumable = true;
            lockpicker.quality = PickupObject.ItemQuality.D;
            ItemBuilder.AddPassiveStatModifier(lockpicker, PlayerStats.StatType.Damage, 0.85f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(lockpicker, PlayerStats.StatType.MovementSpeed, 0.9f, StatModifier.ModifyMethod.MULTIPLICATIVE);
            ItemBuilder.AddPassiveStatModifier(lockpicker, PlayerStats.StatType.Curse, 1, StatModifier.ModifyMethod.ADDITIVE);
            ItemBuilder.AddPassiveStatModifier(lockpicker, PlayerStats.StatType.AdditionalItemCapacity, 1, StatModifier.ModifyMethod.ADDITIVE);
            BloodIdol.BloodIdolID = lockpicker.PickupObjectId;
            lockpicker.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

            ItemIDs.AddToList(lockpicker.PickupObjectId);
        }
        public static int BloodIdolID;
        public override bool CanBeUsed(PlayerController user)
        {
            return BllodCanBeUsed == true;
        }
        public override void Pickup(PlayerController player)
        {
            if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BLODD_IDOL_KILLS) >= 299)
            {
                BllodCanBeUsed = true;
            }
            else
            {
                BllodCanBeUsed = false;
            }
            player.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Combine(player.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            base.Pickup(player);
        }
        protected override void OnPreDrop(PlayerController user)
        {
            user.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(user.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
        }
        private void OnEnemyDamaged(float damage, bool fatal, HealthHaver enemy)
        {
           if (enemy != null&&fatal == true)
           {
                SaveAPIManager.RegisterStatChange(CustomTrackedStats.BLODD_IDOL_KILLS, 1);
                //ETGModConsole.Log("Current Kills: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BLODD_IDOL_KILLS).ToString());
            }
        }
        public override void Update()
        {
            if (SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BLODD_IDOL_KILLS) >= 299)
            {
                BllodCanBeUsed = true;
            }
            else
            {
                BllodCanBeUsed = false;
            }
            base.Update();
        }
        public bool BllodCanBeUsed;
        protected override void OnDestroy()
        {
            if (base.LastOwner != null)
            {
                base.LastOwner.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(base.LastOwner.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            }
            base.OnDestroy();
        }
        protected override void DoEffect(PlayerController user)
        {
            user.OnAnyEnemyReceivedDamage = (Action<float, bool, HealthHaver>)Delegate.Remove(user.OnAnyEnemyReceivedDamage, new Action<float, bool, HealthHaver>(this.OnEnemyDamaged));
            IntVector2 bestRewardLocation = user.CurrentRoom.GetBestRewardLocation(IntVector2.One * 3, RoomHandler.RewardLocationStyle.PlayerCenter, true);
            Chest chest2 = GameManager.Instance.RewardManager.SpawnRewardChestAt(bestRewardLocation, -1f, PickupObject.ItemQuality.EXCLUDED);
            chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());
            chest2.IsLocked = false;
            float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BLODD_IDOL_KILLS);
            SaveAPIManager.RegisterStatChange(CustomTrackedStats.BLODD_IDOL_KILLS, -300);
            //ETGModConsole.Log("Current Kills: " + SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.BLODD_IDOL_KILLS).ToString());

        }

    }
}



