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
    public class OffWorldMedicine  : PlayerItem
    {

        public static void Init()
        {
            string itemName = "Off-World Medicine";
            string resourceName = "Planetside/Resources/offworldmedicine.png";
            GameObject obj = new GameObject(itemName);
            OffWorldMedicine lockpicker = obj.AddComponent<OffWorldMedicine>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "I'll Show You Off-World";
            string longDesc = "An off-world medicine from a hi-tech civilzation, capable of curing even the most sturdiest addictions and festering wounds.";
            lockpicker.SetupItem(shortDesc, longDesc, "psog");
            lockpicker.SetCooldownType(ItemBuilder.CooldownType.Timed, 5f);
            lockpicker.consumable = true;
            lockpicker.quality = PickupObject.ItemQuality.A;

            lockpicker.AddToSubShop(ItemBuilder.ShopType.Goopton, 1f);
            lockpicker.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);


        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_OBJ_dice_bless_01", base.gameObject);
            user.PlayEffectOnActor(ResourceCache.Acquire("Global VFX/VFX_Healing_Sparkles_001") as GameObject, Vector3.zero, true, false, false);
            user.healthHaver.FullHeal();
            int SpiceToDown = user.spiceCount;
            user.spiceCount = -SpiceToDown;
            ChangeSpiceWeight();
        }
        public static float ChangeSpiceWeight()
        {
            return 0f;
        }
    }
}



