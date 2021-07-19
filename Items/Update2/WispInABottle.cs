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
using System.Collections.ObjectModel;

using UnityEngine.Serialization;
using MonoMod.Utils;
using Brave.BulletScript;
using GungeonAPI;
using SaveAPI;
//Garbage Code Incoming
namespace Planetside
{
    public class WispInABottle : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Wisp In A Bottle";
            string resourceName = "Planetside/Resources/WispInABottle.png";
            GameObject obj = new GameObject(itemName);
            WispInABottle testActive = obj.AddComponent<WispInABottle>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Unmatched Power";
            string longDesc = "A bottle that contains a whole living star inside of it.\n\nUse with caution.";
            testActive.SetupItem(shortDesc, longDesc, "psog");
            testActive.SetCooldownType(ItemBuilder.CooldownType.Damage, 800f);
            testActive.consumable = false;
            testActive.quality = PickupObject.ItemQuality.B;
            testActive.SetupUnlockOnCustomFlag(CustomDungeonFlags.DEFEAT_OPHANAIM, true);

            GameObject gameObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/VFX/Sun/sunflare_chargeup_001", null, true);
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            GameObject gameObject2 = new GameObject("StrikeStuff");
            tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);

            //KineticStrike.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/Sun/sunflare_chargeup_001", tk2dSprite.Collection));

            for (int A = 1; A < 10; A++)
            {
                WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection($"Planetside/Resources/VFX/Sun/sunflare_chargeup_00{A}", tk2dSprite.Collection));
            }
            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection($"Planetside/Resources/VFX/Sun/sunflare_chargeup_010", tk2dSprite.Collection));
            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection($"Planetside/Resources/VFX/Sun/sunflare_chargeup_011", tk2dSprite.Collection));
            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection($"Planetside/Resources/VFX/Sun/sunflare_chargeup_012", tk2dSprite.Collection));


            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/Sun/sunflare_fire_001", tk2dSprite.Collection));
            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/Sun/sunflare_fire_002", tk2dSprite.Collection));
            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/Sun/sunflare_fire_003", tk2dSprite.Collection));
            WispInABottle.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/Sun/sunflare_fire_004", tk2dSprite.Collection));


            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            ForgiveMePlease.spriteIds.Add(tk2dSprite.spriteId);
            gameObject2.SetActive(false);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[0]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[1]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[2]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[3]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[4]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[5]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[6]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[7]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[8]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[9]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[10]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[11]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[12]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[13]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[14]);
            tk2dSprite.SetSprite(WispInABottle.spriteIds[15]);

            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            WispInABottle.SunPrefab = gameObject2;
        }
        public static GameObject SunPrefab;
        public static List<int> spriteIds = new List<int>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        protected override void DoEffect(PlayerController user)
        {
            AkSoundEngine.PostEvent("Play_BOSS_lichB_charge_01", base.gameObject);
            GameManager.Instance.StartCoroutine(this.HeatTime(user.CurrentRoom, user));
        }
        private IEnumerator HeatTime(RoomHandler room, PlayerController player)
        {
            if (player != null)
            {
                GameObject original;
                original = WispInABottle.SunPrefab;
                tk2dSprite component = GameObject.Instantiate(original, player.specRigidbody.UnitTopCenter, Quaternion.identity, player.transform).GetComponent<tk2dSprite>();
                component.transform.position.WithZ(transform.position.z + 99999);
                component.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(player.CenterPosition, tk2dBaseSprite.Anchor.MiddleCenter);
                player.sprite.AttachRenderer(component.GetComponent<tk2dBaseSprite>());


                component.PlaceAtPositionByAnchor(base.LastOwner.sprite.WorldTopCenter, tk2dBaseSprite.Anchor.LowerCenter);
                component.scale = Vector3.one;
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[0]);
                for (int A = 0; A < 11; A++)
                {
                    component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[A]);
                    yield return new WaitForSeconds(0.1f);
                }
                AkSoundEngine.PostEvent("Play_BOSS_doormimic_flame_01", player.gameObject);
                for (int e = 0; e < 5; e++)
                {
                    AkSoundEngine.PostEvent("Play_Burn", player.gameObject);
                    for (int w = 0; w < 4; w++)
                    {
                        for (int A = 11; A < 15; A++)
                        {
                            component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[A]);
                            Exploder.DoDistortionWave(component.sprite.WorldCenter, 0.1f, 0.25f, 6, 0.5f);

                            List<AIActor> activeEnemies = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                            Vector2 centerPosition = player.sprite.WorldCenter;
                            if (activeEnemies != null)
                            {
                                foreach (AIActor aiactor in activeEnemies)
                                {
                                    bool ae = Vector2.Distance(aiactor.CenterPosition, centerPosition) < 20 && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && player != null;
                                    if (ae)
                                    {
                                        var Frail = aiactor.transform.Find("heatStrokeVFX");
                                        bool fra = Frail == null;
                                        if (fra)
                                        {
                                            this.AffectEnemy(aiactor);
                                        }
                                    }
                                }
                            }
                            yield return new WaitForSeconds(0.125f);
                        }
                    }
                }
                AkSoundEngine.PostEvent("Play_BOSS_lichB_charge_01", base.gameObject);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[11]);
                yield return new WaitForSeconds(0.2f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[7]);
                yield return new WaitForSeconds(0.2f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[6]);
                yield return new WaitForSeconds(0.1f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[5]);
                yield return new WaitForSeconds(0.1f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[4]);
                yield return new WaitForSeconds(0.1f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[3]);
                yield return new WaitForSeconds(0.1f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[2]);
                yield return new WaitForSeconds(0.1f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[1]);
                yield return new WaitForSeconds(0.1f);
                component.GetComponent<tk2dBaseSprite>().SetSprite(WispInABottle.spriteIds[0]);
                yield return new WaitForSeconds(0.1f);
                Destroy(component);
                yield break;
            }  
        }
        private void AffectEnemy(AIActor target)
        {
            target.ApplyEffect(DebuffLibrary.HeatStroke, 1f, null);
        }
    }
}



