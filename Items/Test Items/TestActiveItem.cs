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
using System.Collections.ObjectModel;

using UnityEngine.Serialization;
using MonoMod.Utils;
using Brave.BulletScript;
using GungeonAPI;
using SaveAPI;

using System.IO;
using Planetside;
using FullInspector.Internal;

//Garbage Code Incoming
namespace Planetside
{
    public class TestActiveItem : PlayerItem
    {
        public static void Init()
        {
            string itemName = "PSOGTest Active Item";
            string resourceName = "Planetside/Resources/blashshower.png";
            GameObject obj = new GameObject(itemName);
            TestActiveItem testActive = obj.AddComponent<TestActiveItem>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "Used for testing";
            string longDesc = "Test Active For Testing 'On Active' Items.";
            testActive.SetupItem(shortDesc, longDesc, "psog");
            testActive.SetCooldownType(ItemBuilder.CooldownType.Timed, 1f);
            testActive.consumable = false;
            testActive.quality = PickupObject.ItemQuality.EXCLUDED;
        }
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }
        // ShaderCache.Acquire("Brave/LitCutoutUberPhantom");
        public static Shader RainbowMat = ShaderCache.Acquire("Brave/Internal/StarNest_Derivative");
        public static Shader fuckyou = ShaderCache.Acquire("Brave/Internal/WorldDecay");

        // new Material(ShaderCache.Acquire("Brave/Effects/SimplicityDerivativeShader"));
        private static AssetBundle bundle = ResourceManager.LoadAssetBundle("enemies_base_001");
        private GameObject ShipPrefab = bundle.LoadAsset("assets/data/enemies/bosses/bossfinalrogue.prefab") as GameObject;
        //private BasicBeamController mean = LaserReticle.get

        //WORKS
        //            material.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");

        public GameObject spawnedPlayerObject;
        public GameObject objectToSpawn;
        //GoopDefinition goop;
        private Projectile beamProjectile = EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").GetComponent<BossFinalRogueGunController>().GetComponent<BossFinalRogueLaserGun>().beamProjectile;

        protected override void DoEffect(PlayerController user)
        {

            base.DoEffect(user);

            var enemy = EnemyDatabase.GetOrLoadByGuid("05b8afe0b6cc4fffa9dc6036fa24c8ec");
            if (!enemy)
            {
                ETGModConsole.Log("enemy null");
            }

            Projectile beam = null;
            foreach (Component item in enemy.GetComponentsInChildren(typeof(Component)))
            {
                ETGModConsole.Log("====");
                ETGModConsole.Log(item.name);
                ETGModConsole.Log(item.GetType().Name.ToString());
            }

            Fire(beam);

            /*
            float RNG = UnityEngine.Random.Range(5.5f, 15f);
            ETGModConsole.Log("As /= ");
            float As = RNG /= 5.5f;
            ETGModConsole.Log(As.ToString());
            ETGModConsole.Log("================");
            float Is = RNG / 5.5f;
            ETGModConsole.Log(Is.ToString());
            ETGModConsole.Log("As /");
            */
            /*
            float Loop = SaveAPIManager.GetPlayerStatValue(CustomTrackedStats.TIMES_LOOPED);
           // UIToolbox.TextBox(Color.white, "Ouroborous Level: " + Loop.ToString() ,user.gameObject,dfPivotPoint.TopCenter ,new Vector3(user.specRigidbody.UnitCenter.x - user.transform.position.x, 1.25f, 0f), 5, 1.5f);
            //List<AssetBundle> assetBundle = new List<AssetBundle> { ResourceManager.LoadAssetBundle("shared_auto_001"), ResourceManager.LoadAssetBundle("shared_auto_002"), ResourceManager.LoadAssetBundle("shared_auto_003"), ResourceManager.LoadAssetBundle("brave_resources_001"), ResourceManager.LoadAssetBundle("dungeon_scene_001"), ResourceManager.LoadAssetBundle("encounters_base_001"), ResourceManager.LoadAssetBundle("enemies_base_001"), ResourceManager.LoadAssetBundle("flows_base_001"), ResourceManager.LoadAssetBundle("foyer_001"), ResourceManager.LoadAssetBundle("foyer_002"), ResourceManager.LoadAssetBundle("foyer_003") };
            ETGModConsole.Log(user.CurrentRoom.GetRoomName());
            this.goop = user.CurrentGoop;
            if (this.goop != null)
            {
                ETGModConsole.Log("=================================");
                ETGModConsole.Log(goop.name);
                Tools.LogPropertiesAndFields(goop);
                //Object t = Resources.LoadAssetAtPath(localPath, typeof(T));
            }
            
            List<AIActor> activeEnemies = user.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            Vector2 centerPosition = user.sprite.WorldCenter;
            if (activeEnemies != null)
            {
                foreach (AIActor aiactor in activeEnemies)
                {
                    bool ae = Vector2.Distance(aiactor.CenterPosition, centerPosition) < 20 && aiactor.healthHaver.GetMaxHealth() > 0f && aiactor != null && aiactor.specRigidbody != null && user != null;
                    if (ae)
                    {

                        ETGModConsole.Log(aiactor.encounterTrackable.EncounterGuid);
                        ETGModConsole.Log(aiactor.healthHaver.GetMaxHealth().ToString());
                        ETGModConsole.Log("=========================================");

                    }
                }
            }
            */
            //AkSoundEngine.PostEvent("Play_BigSlam", base.gameObject);
            /*
            List<AIActor> activeEnemies = base.LastOwner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
            if (activeEnemies != null)
            {
                foreach (AIActor aiactor in activeEnemies)
                {
                    AddBeholsterBeamComponent yee = aiactor.gameObject.AddComponent<AddBeholsterBeamComponent>();
                    yee.AddsBaseBeamBehavior = true;
                }
            }
            */
            //assetBundle.LoadAsset(text);

            //for (int i = 0; i < GoopDefinition.Instance.transform.childCount; i++)
            //{
            //ETGModConsole.Log(Minimap.Instance.transform.GetChild(i).gameObject.name);
            //}

            //this.objectToSpawn = EnemyDatabase.GetOrLoadByGuid(LoaderPylonController.guid).gameObject;
            /*
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(LoaderPylonController.Turretprefab, user.specRigidbody.UnitCenter, Quaternion.identity);
            this.spawnedPlayerObject = gameObject;
            tk2dBaseSprite component2 = gameObject.GetComponent<tk2dBaseSprite>();
            if (component2 != null)
            {
                component2.PlaceAtPositionByAnchor(user.specRigidbody.UnitCenter.ToVector3ZUp(component2.transform.position.z), tk2dBaseSprite.Anchor.MiddleCenter);
                if (component2.specRigidbody != null)
                {
                    component2.specRigidbody.RegisterGhostCollisionException(user.specRigidbody);
                }
            }
            SpawnObjectItem componentInChildren2 = this.spawnedPlayerObject.GetComponentInChildren<SpawnObjectItem>();
            if (componentInChildren2)
            {
                componentInChildren2.SpawningPlayer = this.LastOwner;
            }
            LoaderPylonController yah = gameObject.AddComponent<LoaderPylonController>();
            if (yah != null)
            {
                yah.maxDuration = 120f;
            }
            */
            /*
            string guid;
            guid = "01972dee89fc4404a5c408d50007dad5";

            PlayerController owner = base.LastOwner;
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
            IntVector2? intVector = new IntVector2?(base.LastOwner.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Awaken, true);
            aiactor.CanTargetPlayers = true;
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
            aiactor.IgnoreForRoomClear = true;
            aiactor.IsHarmlessEnemy = false;
            aiactor.HandleReinforcementFallIntoRoom(0f);
            */
            //aiactor.gameObject.AddComponent<UmbraController>();

            /*
            Chest rainbow_Chest = GameManager.Instance.RewardManager.S_Chest;
            Chest chest2 = Chest.Spawn(rainbow_Chest, user.CurrentRoom.GetRandomVisibleClearSpot(1, 1));
            chest2.sprite.usesOverrideMaterial = true;

            var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\plating.png");

            chest2.sprite.renderer.material.shader = Shader.Find("Brave/PlayerShaderEevee");
            chest2.sprite.renderer.material.SetTexture("_EeveeTex", texture);
            //chest2.sprite.renderer.material.SetColor("_EmissiveColor", new Color32(255, 255, 255, 255));
            //chest2.sprite.renderer.material.SetFloat("_EmissiveColorPower", 1.55f);
            chest2.sprite.renderer.material.SetFloat("_StencilVal", 100000);
            chest2.sprite.renderer.material.SetFloat("_FlatColor", 0f);
            chest2.sprite.renderer.material.SetFloat("_Perpendicular", 0);


            chest2.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
            chest2.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");
            chest2.lootTable.S_Chance = 0.2f;
            chest2.lootTable.A_Chance = 0.2f;
            chest2.lootTable.B_Chance = 0.22f;
            chest2.lootTable.C_Chance = 0.22f;
            chest2.lootTable.D_Chance = 0.16f;
            chest2.lootTable.Common_Chance = 0f;
            chest2.lootTable.canDropMultipleItems = true;
            chest2.lootTable.multipleItemDropChances = new WeightedIntCollection();
            chest2.lootTable.multipleItemDropChances.elements = new WeightedInt[1];
            chest2.lootTable.overrideItemLootTables = new List<GenericLootTable>();
            chest2.lootTable.lootTable = GameManager.Instance.RewardManager.GunsLootTable;
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.GunsLootTable);
            chest2.lootTable.overrideItemLootTables.Add(GameManager.Instance.RewardManager.ItemsLootTable);
            WeightedInt weightedInt = new WeightedInt();
            weightedInt.value = 24;
            weightedInt.weight = 1f;
            weightedInt.additionalPrerequisites = new DungeonPrerequisite[0];
            chest2.lootTable.multipleItemDropChances.elements[0] = weightedInt;
            chest2.lootTable.onlyOneGunCanDrop = false;
            chest2.RegisterChestOnMinimap(chest2.GetAbsoluteParentRoom());
            */

            /*
            GameObject gameObject = new GameObject();
            gameObject.transform.position = user.sprite.WorldCenter;
            BulletScriptSource source = gameObject.GetOrAddComponent<BulletScriptSource>();
            gameObject.AddComponent<BulletSourceKiller>();
            var bulletScriptSelected = new CustomBulletScriptSelector(typeof(AngerGodsScript));
            AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
            AIBulletBank bulletBank = aIActor.GetComponent<AIBulletBank>();
            bulletBank.CollidesWithEnemies = true;
            source.BulletManager = bulletBank;
            source.BulletScript = bulletScriptSelected;
            source.Initialize();//to fire the script once
            */
            /*
            string guid;
            guid = "c4cf0620f71c4678bb8d77929fd4feff";
            PlayerController owner = user;
            AIActor orLoadByGuid = EnemyDatabase.GetOrLoadByGuid(guid);
            IntVector2? intVector = new IntVector2?(user.CurrentRoom.GetRandomVisibleClearSpot(2, 2));
            AIActor aiactor = AIActor.Spawn(orLoadByGuid.aiActor, intVector.Value, GameManager.Instance.Dungeon.data.GetAbsoluteRoomFromPosition(intVector.Value), true, AIActor.AwakenAnimationType.Default, true);
            //aiactor.gameObject.AddComponent<PlayerController>();
            //Chest aaa = aiactor.gameObject.AddComponent<Chest>();
            //var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\nebula_reducednoise.png");
            var texture = ItemAPI.ResourceExtractor.GetTextureFromResource("Planetside\\Resources\\nebula_reducednoise.png");

            aiactor.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/Internal/StarNest_Derivative");
            */
            //aiactor.sprite.renderer.material.SetTexture("_EeveeTex", texture);

            //     aiactor.sprite.renderer.material.DisableKeyword("BRIGHTNESS_CLAMP_ON");
            //   aiactor.sprite.renderer.material.EnableKeyword("BRIGHTNESS_CLAMP_OFF");

            /*
            aiactor.CanTargetEnemies = false;
            aiactor.CanTargetPlayers = true;
            PhysicsEngine.Instance.RegisterOverlappingGhostCollisionExceptions(aiactor.specRigidbody, null, false);
            aiactor.IsHarmlessEnemy = false;
            aiactor.IgnoreForRoomClear = true;
            aiactor.HandleReinforcementFallIntoRoom(-1f);
            */

        }
        private BasicBeamController m_laserBeam;
        public Transform beamTransform;
        private bool m_firingLaser;
        //private float LaserAngle = 90;

        public void Fire(Projectile projectile)
        {
            m_firingLaser = true;
            //this.m_fireTimer = this.fireTime;
            //this.LaserAngle = -90f;
            //if (this.LightToTrigger)
            //{
            //    this.LightToTrigger.ManuallyDoBulletSpawnedFade();
            //}
            beamTransform = GameManager.Instance.PrimaryPlayer.transform;
            base.StartCoroutine(this.FireBeam(projectile));
            //base.StartCoroutine(this.DoGunMotionCR());
            //if (this.doScreenShake)
            //{
            //    GameManager.Instance.MainCameraController.DoContinuousScreenShake(this.screenShake, this, false);
            //}
        }

        public void LateUpdate()
        {
            if (m_firingLaser && this.m_laserBeam)
            {
                this.m_laserBeam.LateUpdatePosition(this.beamTransform.position);
            }
            else if (this.m_laserBeam && this.m_laserBeam.State == BasicBeamController.BeamState.Dissipating)
            {
                this.m_laserBeam.LateUpdatePosition(this.beamTransform.position);
            }
        }
        public IntVector2 colliderOffset;

        public IntVector2 colliderSize;
        protected IEnumerator FireBeam(Projectile projectile)
        {
            GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(projectile.gameObject);
            this.m_laserBeam = beamObject.GetComponent<BasicBeamController>();
            this.m_laserBeam.Owner = LastOwner;
            this.m_laserBeam.HitsPlayers = false;
            this.m_laserBeam.HitsEnemies = true;
            this.m_laserBeam.collisionLength = 100;
            this.m_laserBeam.collisionWidth = 6;
            this.m_laserBeam.collisionRadius = 6;
            this.m_laserBeam.collisionType = BasicBeamController.BeamCollisionType.Default;

            //this.m_laserBeam.projectile.collidesWithEnemies = true;



            this.m_laserBeam.OverrideHitChecks = delegate (SpeculativeRigidbody hitRigidbody, Vector2 dirVec)
            {
                HealthHaver healthHaver = (!hitRigidbody) ? null : hitRigidbody.healthHaver;
                if (hitRigidbody && hitRigidbody.projectile && hitRigidbody.GetComponent<BeholsterBounceRocket>())
                {
                    BounceProjModifier component = hitRigidbody.GetComponent<BounceProjModifier>();
                    if (component)
                    {
                        component.numberOfBounces = 0;
                    }
                    hitRigidbody.projectile.DieInAir(false, true, true, false);
                }
                if (healthHaver != null)
                {
                    if (healthHaver.aiActor)
                    {
                        Projectile currentProjectile = projectile;
                        healthHaver.ApplyDamage(ProjectileData.FixedFallbackDamageToEnemies, dirVec, "Death", currentProjectile.damageTypes, DamageCategory.Normal, false, null, false);
                    }
                    else
                    {
                        Projectile currentProjectile = projectile;
                        healthHaver.ApplyDamage(ProjectileData.FixedFallbackDamageToEnemies, dirVec, "Death", currentProjectile.damageTypes, DamageCategory.Normal, false, null, false);
                    }
                }
                if (hitRigidbody.majorBreakable)
                {
                    hitRigidbody.majorBreakable.ApplyDamage(26f * BraveTime.DeltaTime, dirVec, false, false, false);
                }
            };

            this.m_laserBeam.ContinueBeamArtToWall = true;
            this.m_laserBeam.projectile.baseData.damage = 200;
            this.m_laserBeam.projectile.collidesWithEnemies = true;


            bool firstFrame = true;
            while (this.m_laserBeam != null && this.m_firingLaser)
            {
                float clampedAngle = BraveMathCollege.ClampAngle360(LastOwner.CurrentGun.CurrentAngle);
                Vector2 dirVec = new Vector3(Mathf.Cos(clampedAngle * 0.0174532924f), Mathf.Sin(clampedAngle * 0.0174532924f)) * 10f;
                this.m_laserBeam.Origin = this.beamTransform.position;
                this.m_laserBeam.Direction = dirVec;
                if (firstFrame)
                {
                    yield return null;
                    firstFrame = false;
                }
                else
                {
                    yield return null;
                    while (Time.timeScale == 0f)
                    {
                        yield return null;
                    }
                }
            }
            if (!this.m_firingLaser && this.m_laserBeam != null)
            {
                this.m_laserBeam.CeaseAttack();
            }
            if (this.m_laserBeam)
            {
                this.m_laserBeam.SelfUpdate = false;
                while (this.m_laserBeam)
                {
                    this.m_laserBeam.Origin = this.beamTransform.position;
                    yield return null;
                }
            }
            this.m_laserBeam = null;
            yield break;
        }

        /*
        protected IEnumerator FireBeam()
        {
            ETGModConsole.Log("1");
            //this.m_firingLaser = true;
            //GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(projectile.gameObject);
            ETGModConsole.Log("2");

            //BasicBeamController hinge = ShipPrefab.GetComponent("BasicBeamController") as BasicBeamController;
            //List<Transform> proj = EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").GetComponents<Transform>();


            //BasicBeamController beam = ShipPrefab.GetComponentInChildren<BasicBeamController>();

            BasicBeamController beam = m_laserBeam;
            Transform beamtrans = base.LastOwner.transform;
            //ETGModConsole.Log("2.1");
            var enemy = EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5");
            foreach (Component item in enemy.GetComponentsInChildren(typeof(Component)))
            {
                //ETGModConsole.Log("2.2");
                if (item is BossFinalRogueLaserGun laser)
                {
                    //ETGModConsole.Log("2.3");
                    if (laser.beamProjectile != null)
                    {
                        //ETGModConsole.Log("2.4");
                        GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(laser.beamProjectile.gameObject);
                        beam = beamObject.GetOrAddComponent<BasicBeamController>();
                        //GameObject beamObject = UnityEngine.Object.Instantiate<GameObject>(laser.projectile.gameObject);
                        //this.m_laserBeam = beamObject.GetComponent<BasicBeamController>();
                    }
                    if (laser.beamTransform != null)
                    {
                        beamtrans = laser.beamTransform;
                    }
                }
            }
            
            //ETGModConsole.Log("3");

            if (beam != null)
            {
                //ETGModConsole.Log("4");
                beam.Owner = base.LastOwner;
                beam.HitsPlayers = true;//projectile.collidesWithPlayer;
                beam.HitsEnemies = true;//projectile.collidesWithEnemies;
                beam.ContinueBeamArtToWall = false;
                beam.chargeDelay = 1;
                beam.usesTelegraph = true;
                beam.renderer.enabled = true;
                bool firstFrame = true;
                while (beam != null)// && this.m_firingLaser)
                {
                    tk2dSprite sproot1 = beam.GetComponent<tk2dSprite>();
                    if (sproot1 != null)
                    {
                        sproot1.HeightOffGround = -2;
                    }
                    //ETGModConsole.Log(beam.Origin.ToString());
                    //ETGModConsole.Log(base.LastOwner.transform.position.ToString());
                    float clampedAngle = BraveMathCollege.ClampAngle360(base.LastOwner.CurrentGun.CurrentAngle);
                    Vector2 dirVec = new Vector3(Mathf.Cos(clampedAngle * 0.0174532924f), Mathf.Sin(clampedAngle * 0.0174532924f)) * 10f;

                    beam.Origin = base.LastOwner.sprite.transform.position + beamtrans.position;// + new Vector3(-15, -5);
                    beam.Direction = dirVec;
                    beam.transform.position.WithZ(beam.transform.position.z + 99999);

                    //beam.gameObject.transform.position = GameManager.Instance.PrimaryPlayer.transform.position;
                    beam.sprite.UpdateZDepth();
                    if (firstFrame)
                    {
                        yield return null;
                        firstFrame = false;
                    }
                    else
                    {
                        yield return null;
                        while (Time.timeScale == 0f)
                        {
                            yield return null;
                        }
                    }
                }
                //if (!this.m_firingLaser && beam != null)
                //{
                    //this.m_firingLaser = false;
                    //beam.CeaseAttack();
                //}
                /*
                if (beam)
                {
                    beam.SelfUpdate = false;
                    while (beam)
                    {
                        beam.Origin = base.LastOwner.sprite.transform.position + beamtrans.position;
                        yield return null;
                    }
                }
                //beam = null;
                */


    }
}



