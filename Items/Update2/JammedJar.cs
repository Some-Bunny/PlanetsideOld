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
    public class JammedJar  : PlayerItem
    {

        public static void Init()
        {
            string itemName = "Jammed Jar";
            string resourceName = "Planetside/Resources/JammedJarIDK/cursejar_001.png";
            GameObject obj = new GameObject(itemName);
            JammedJar lockpicker = obj.AddComponent<JammedJar>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Jar Full Of The Jammed";
            string longDesc = "A jar full of Jammed protective relics. Do you dare sink your hand into it?";
            lockpicker.SetupItem(shortDesc, longDesc, "psog");
            lockpicker.SetCooldownType(ItemBuilder.CooldownType.Timed, 1f);
            lockpicker.consumable = true;
            lockpicker.numberOfUses = 3;
            lockpicker.quality = PickupObject.ItemQuality.C;
            JammedJar.spriteIDs = new int[JammedJar.spritePaths.Length];
            JammedJar.spriteIDs[3] = ItemAPI.SpriteBuilder.AddSpriteToCollection(JammedJar.spritePaths[0], lockpicker.sprite.Collection);
            JammedJar.spriteIDs[2] = SpriteBuilder.AddSpriteToCollection(JammedJar.spritePaths[1], lockpicker.sprite.Collection);
            JammedJar.spriteIDs[1] = SpriteBuilder.AddSpriteToCollection(JammedJar.spritePaths[2], lockpicker.sprite.Collection);
            JammedJar.spriteIDs[0] = SpriteBuilder.AddSpriteToCollection(JammedJar.spritePaths[3], lockpicker.sprite.Collection);
            Uses = 3;
            lockpicker.AddToSubShop(ItemBuilder.ShopType.Cursula, 1f);

        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {
           Uses--; 
           base.sprite.SetSprite(JammedJar.spriteIDs[Uses]);
           AkSoundEngine.PostEvent("Play_OBJ_cursepot_shatter_01", base.gameObject);
            PickupObject pickupObject = Game.Items["psog:damned_guon_stone"];
            user.AcquirePassiveItemPrefabDirectly(pickupObject as PassiveItem);
        }
        private static int[] spriteIDs;
        private static readonly string[] spritePaths = new string[]
        {
            "Planetside/Resources/JammedJarIDK/cursejar_001",
            "Planetside/Resources/JammedJarIDK/cursejar_002",
            "Planetside/Resources/JammedJarIDK/cursejar_003",
            "Planetside/Resources/JammedJarIDK/cursejar_004"
        };
        public static int Uses;
    }
}



