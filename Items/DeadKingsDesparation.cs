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
    public class DeadKingsDesparation : PlayerItem
    {
        public static void Init()
        {
            string itemName = "Dead Kings Desparation";
            string resourceName = "Planetside/Resources/deadkingsdesparation.png";
            GameObject obj = new GameObject(itemName);
            DeadKingsDesparation testActive = obj.AddComponent<DeadKingsDesparation>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Strengths Falsified";
            string longDesc = "Steals the strengths of all foes in the room. A simple battle-helmet worn by an old King, who in his final act of desparation stole the empowering trinkets the Challenger brought with him.";
            testActive.SetupItem(shortDesc, longDesc, "psog");
            testActive.SetCooldownType(ItemBuilder.CooldownType.Damage, 1250f);
            testActive.consumable = false;
            testActive.quality = PickupObject.ItemQuality.A;

            DeadKingsDesparation.FireVFXPrefab = SpriteBuilder.SpriteFromResource(DeadKingsDesparation.FalseFireString, null, false);
            DeadKingsDesparation.FireVFXPrefab.name = DeadKingsDesparation.vfxName1;
            UnityEngine.Object.DontDestroyOnLoad(DeadKingsDesparation.FireVFXPrefab);
            FakePrefab.MarkAsFakePrefab(DeadKingsDesparation.FireVFXPrefab);
            DeadKingsDesparation.FireVFXPrefab.SetActive(false);

            DeadKingsDesparation.PosionVFXPrefab = SpriteBuilder.SpriteFromResource(DeadKingsDesparation.FalsePoisonString, null, false);
            DeadKingsDesparation.PosionVFXPrefab.name = DeadKingsDesparation.vfxName;
            UnityEngine.Object.DontDestroyOnLoad(DeadKingsDesparation.PosionVFXPrefab);
            FakePrefab.MarkAsFakePrefab(DeadKingsDesparation.PosionVFXPrefab);
            DeadKingsDesparation.PosionVFXPrefab.SetActive(false);

            List<string> mandatoryConsoleIDs = new List<string>
            {
                "psog:dead_kings_desparation",
            };
            List<string> optionalConsoleIDs = new List<string>
            {
                "psog:executioners_crossbow",
                "shotgun_full_of_hate",
                "psog:death_warrant"
            };
            CustomSynergies.Add("Vermincide.", mandatoryConsoleIDs, optionalConsoleIDs, true);
        }
        public static void BuildPrefab()
        {
            GameObject gameObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/VFX/MithrixCalldown/mithrixfalling_001", null, true);
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            GameObject gameObject2 = new GameObject("Mithrix Calldown");
            tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);

            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixfalling_001", tk2dSprite.Collection));

            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixland_001", tk2dSprite.Collection));
            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixland_002", tk2dSprite.Collection));
            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixland_003", tk2dSprite.Collection));
            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixland_004", tk2dSprite.Collection));
            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixland_005", tk2dSprite.Collection));
            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixland_006", tk2dSprite.Collection));

            DeadKingsDesparation.spriteIds1.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/MithrixCalldown/mithrixleap_001", tk2dSprite.Collection));


            Material mat = tk2dSprite.GetCurrentSpriteDef().material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = tk2dSprite.sprite.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color32(104, 182, 255, 255));
            mat.SetFloat("_EmissiveColorPower", 1.55f);
            mat.SetFloat("_EmissivePower", 100);
            tk2dSprite.sprite.renderer.material = mat;

            DeadKingsDesparation.spriteIds1.Add(tk2dSprite.spriteId);
            gameObject2.SetActive(false);


            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[0]); //Mithrix Fall

            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[1]); //Mithrix Land
            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[2]); 
            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[3]); 
            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[4]); 
            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[5]); 
            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[6]); 
            
            tk2dSprite.SetSprite(DeadKingsDesparation.spriteIds1[7]); //Mithrix Leap

            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            DeadKingsDesparation.CalldownPrefab = gameObject2;
        }
        public static GameObject CalldownPrefab;
        public static List<int> spriteIds1 = new List<int>();
        public GameObject objectToSpawn;
        public GameObject spawnedPlayerObject;

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        public override bool CanBeUsed(PlayerController user)
        {
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            return activeEnemies != null && this.TrinketingAndBaubling != true;
        }



        protected override void DoEffect(PlayerController user)
        {
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Combine(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.AIActorMods));
            AkSoundEngine.PostEvent("Play_ENM_Grip_Master_Lockon_01", base.gameObject);
            this.TrinketingAndBaubling = true;
            this.m_PoisonImmunity = new DamageTypeModifier();
            this.m_PoisonImmunity.damageMultiplier = 0f;
            this.m_PoisonImmunity.damageType = CoreDamageTypes.Poison;
            user.healthHaver.damageTypeModifiers.Add(this.m_PoisonImmunity);
            this.m_FireImmunity = new DamageTypeModifier();
            this.m_FireImmunity.damageMultiplier = 0f;
            this.m_FireImmunity.damageType = CoreDamageTypes.Fire;
            user.healthHaver.damageTypeModifiers.Add(this.m_FireImmunity);

            user.OnRoomClearEvent += this.onRoomCleared;
            RoomHandler absoluteRoom = base.transform.position.GetAbsoluteRoom();
            List<AIActor> activeEnemies = absoluteRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool flag = activeEnemies != null;
            bool flag2 = flag;
            if (flag2)
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    this.AffectEnemy(activeEnemies[i], user);
                }
            }
            bool flagA = user.PlayerHasActiveSynergy("Vermincide.");
            if (flagA)
            {
                base.StartCoroutine(this.HandlePlaceDoll(user.CurrentRoom, user));
            }
        }
        private IEnumerator HandlePlaceDoll(RoomHandler room, PlayerController player)
        {
            List<AIActor> activeEnemies1 = player.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            bool ee = activeEnemies1 != null;
            if (ee)
            {
                RoomHandler absoluteRoom1 = base.transform.position.GetAbsoluteRoom();
                AIActor randomActiveEnemy1;
                randomActiveEnemy1 = player.CurrentRoom.GetRandomActiveEnemy(true);
                Vector2 targetPoint = randomActiveEnemy1.sprite.WorldBottomCenter;
                {

                    GameObject fuck;
                    fuck = UnityEngine.Object.Instantiate<GameObject>(DeadKingsDesparation.CalldownPrefab, targetPoint, Quaternion.identity);
                    fuck.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(targetPoint + new Vector2(0f, 30f), tk2dBaseSprite.Anchor.LowerCenter);

                    AkSoundEngine.PostEvent("Play_BOSS_doormimic_land_01", base.gameObject);
                    fuck.GetComponent<tk2dBaseSprite>().SetSprite(DeadKingsDesparation.spriteIds1[0]);
                  
                    for (int i = 0; i < 60; i++)
                    {
                        fuck.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(fuck.GetComponent<tk2dBaseSprite>().WorldBottomCenter + new Vector2(0f, -(0.5f)), tk2dBaseSprite.Anchor.LowerCenter);
                        yield return new WaitForSeconds(0.015f);
                    }
                    {
                        AkSoundEngine.PostEvent("Play_ENM_rock_blast_01", base.gameObject);
                        Exploder.DoDistortionWave(targetPoint, 6f, 0.6f, 0.2f, 0.1f);
                        if (randomActiveEnemy1 != null)
                        {
                            randomActiveEnemy1.healthHaver.ApplyDamage(250f, Vector2.zero, "Erasure", CoreDamageTypes.None, DamageCategory.Normal, false, null, false);

                        }
                        for (int q = 0; q < 5; q++)
                        {
                            fuck.GetComponent<tk2dBaseSprite>().SetSprite(DeadKingsDesparation.spriteIds1[q]);
                            yield return new WaitForSeconds(0.25f*q);
                        }
                        fuck.GetComponent<tk2dBaseSprite>().SetSprite(DeadKingsDesparation.spriteIds1[6]);
                        Exploder.DoDistortionWave(targetPoint, 6f, 0.6f, 0.2f, 0.1f);
                        AkSoundEngine.PostEvent("Play_BOSS_doormimic_land_01", base.gameObject);
                        fuck.GetComponent<tk2dBaseSprite>().SetSprite(DeadKingsDesparation.spriteIds1[7]);
                        for (int i = 0; i < 90; i++)
                        {
                            fuck.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(fuck.GetComponent<tk2dBaseSprite>().WorldBottomCenter + new Vector2(0f, +(0.75f)), tk2dBaseSprite.Anchor.LowerCenter);
                            yield return new WaitForSeconds(0.01f);
                        }
                    }
                    UnityEngine.Object.Destroy(fuck);
                }
            }
            yield break;
        }
        private void onRoomCleared(PlayerController player)
        {
            ETGMod.AIActor.OnPreStart = (Action<AIActor>)Delegate.Remove(ETGMod.AIActor.OnPreStart, new Action<AIActor>(this.AIActorMods));
            this.TrinketingAndBaubling = false;
            player.healthHaver.damageTypeModifiers.Remove(this.m_FireImmunity);
            player.healthHaver.damageTypeModifiers.Remove(this.m_PoisonImmunity);

        }
        public void AIActorMods(AIActor target)
        {
            bool shouldSlowThisRoom = this.TrinketingAndBaubling;
            if (shouldSlowThisRoom)
            {
                bool flag = target && target.aiActor && target.aiActor.EnemyGuid != null;
                if (flag)
                {
                    this.AffectEnemy(target, base.LastOwner);
                }
            }
        }

        protected void AffectEnemy(AIActor target, PlayerController user)
        {
            bool flag = target.IsNormalEnemy || !target.IsHarmlessEnemy;
            bool flag2 = flag;
            if (flag2)
            {
                if (target != null)
                {
                    target.healthHaver.SetHealthMaximum(target.healthHaver.GetMaxHealth() * 0.8f);
                    GameActor gameActor = target.gameActor;
                    gameActor.EffectResistances = new ActorEffectResistance[]
                    {
                    new ActorEffectResistance
                    {
                        resistAmount = 0f,
                        resistType = EffectResistanceType.Poison
                    },
                    new ActorEffectResistance
                    {
                        resistAmount = 0f,
                        resistType = EffectResistanceType.Fire
                    }
                    };
                    GameManager.Instance.Dungeon.StartCoroutine(this.HandleSuckStrengths(target, user.sprite.WorldCenter,UnityEngine.Random.Range(0.5f, 1.5f), true));
                    GameManager.Instance.Dungeon.StartCoroutine(this.HandleSuckStrengths(target, user.sprite.WorldCenter, UnityEngine.Random.Range(0.5f, 1.5f), false));
                }

            }
        }
        private IEnumerator HandleSuckStrengths(AIActor target, Vector2 table, float DuartionForSteal, bool UsesOneOrAnother)
        {

            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 2.5f));
            Exploder.DoDistortionWave(target.sprite.WorldCenter, 1f, 0.25f, 3, 0.066f);
            AkSoundEngine.PostEvent("Play_ENM_beholster_teleport_01", base.gameObject);
            GameObject original;
            if (UsesOneOrAnother == true)
            {
                original = DeadKingsDesparation.FireVFXPrefab;
            }
            else
            {
                original = DeadKingsDesparation.PosionVFXPrefab;
            }
            tk2dSprite component = UnityEngine.Object.Instantiate<GameObject>(original, target.transform).GetComponent<tk2dSprite>();

            original.transform.parent = SpawnManager.Instance.VFX;
            GameObject gameObject2 = new GameObject("image parent");
            gameObject2.transform.position = component.WorldCenter;
            component.transform.parent = gameObject2.transform;

            Transform copySprite = gameObject2.transform;

            Vector3 startPosition = target.transform.position;
            float elapsed = 0f;
            float duration = DuartionForSteal;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                bool flag3 = base.LastOwner && copySprite;
                if (flag3)
                {
                    Vector3 position = base.LastOwner.sprite.WorldCenter;
                    float t = elapsed / duration * (elapsed / duration);
                    copySprite.position = Vector3.Lerp(startPosition, position, t);
                    copySprite.rotation = Quaternion.Euler(0f, 0f, 360f * BraveTime.DeltaTime) * copySprite.rotation;
                    copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0.1f, 0.1f, 0.1f), t);
                    position = default(Vector3);
                }
                yield return null;
            }
            bool flag4 = copySprite;
            if (flag4)
            {
                UnityEngine.Object.Destroy(copySprite.gameObject);
            }
            yield break;
        }
       
        private static string FalsePoisonString = "Planetside/Resources/VFX/FalseStrengths/falsestrengthpoison";
        private static GameObject PosionVFXPrefab;
        private static string vfxName = "PoisonFalseVFX";

        private static string FalseFireString = "Planetside/Resources/VFX/FalseStrengths/falsestrengthfire";
        private static GameObject FireVFXPrefab;
        private static string vfxName1 = "FireFalseVFX";

        private DamageTypeModifier m_PoisonImmunity;
        private DamageTypeModifier m_FireImmunity;

        public bool TrinketingAndBaubling;
    }
}

