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
    internal class EnergyShield : IounStoneOrbitalItem
    {
        public static void Init()
        {
            string name = "Energy-Plated Shield";
            string resourcePath = "Planetside/Resources/energyshield.png";
            GameObject gameObject = new GameObject();
            EnergyShield item = gameObject.AddComponent<EnergyShield>();
            ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
            string shortDesc = "Thunk";
            string longDesc = "4 Plates of protective shields.\n\nOriginally banned by the Guneva Convention for its use in torture methods, it was disguised as a defensive item when being smuggled around the Hegemony. In a hilarious twist, it proved to function better as a defensive shield than a torture device.";

            ItemBuilder.SetupItem(item, shortDesc, longDesc, "psog");

            item.quality = PickupObject.ItemQuality.B;
            EnergyShield.BuildPrefab();
            item.OrbitalPrefab = EnergyShield.orbitalPrefab;
            item.Identifier = IounStoneOrbitalItem.IounStoneIdentifier.GENERIC;
            item.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

            EnergyShield.EnergyPlatedShieldID = item.PickupObjectId;
            ItemIDs.AddToList(item.PickupObjectId);

        }
        public static int EnergyPlatedShieldID;

        public static void BuildPrefab()
        {
            string value = "AWESOME";
            string.IsNullOrEmpty(value);
            bool flag = EnergyShield.orbitalPrefab != null;
            if (!flag)
            {
                GameObject gameObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/Guons/EnergyPlatedGuon/energyshiledguon.png");
                gameObject.name = "Energy Shield Orbital";
                SpeculativeRigidbody speculativeRigidbody = gameObject.GetComponent<tk2dSprite>().SetUpSpeculativeRigidbody(IntVector2.Zero, new IntVector2(10, 10));
                speculativeRigidbody.CollideWithTileMap = false;
                speculativeRigidbody.CollideWithOthers = true;
                speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
                EnergyShield.orbitalPrefab = gameObject.AddComponent<PlayerOrbital>();
                EnergyShield.orbitalPrefab.motionStyle = PlayerOrbital.OrbitalMotionStyle.ORBIT_PLAYER_ALWAYS;
                EnergyShield.orbitalPrefab.shouldRotate = false;
                EnergyShield.orbitalPrefab.orbitRadius = 4f;
                EnergyShield.orbitalPrefab.SetOrbitalTier(0);
                EnergyShield.orbitalPrefab.orbitDegreesPerSecond = 0;
                EnergyShield.orbitalPrefab.perfectOrbitalFactor = 1000f;
                EnergyShield.orbitalPrefab.gameObject.AddComponent<EnergyGuonGlowManager>();
                UnityEngine.Object.DontDestroyOnLoad(gameObject);
                FakePrefab.MarkAsFakePrefab(gameObject);
                gameObject.SetActive(false);
                EnergyGuon = gameObject;

            }
        }
        private static GameObject EnergyGuon;
        public List<GameObject> EnergyOrbitals = new List<GameObject>();
        public override void Pickup(PlayerController player)
        {

                EnergyShield.guonHook = new Hook(typeof(PlayerOrbital).GetMethod("Initialize"), typeof(EnergyShield).GetMethod("GuonInit"));
           // player.gameObject.AddComponent<EnergyShield.ElectricGuonbehavior>();
            GameManager.Instance.OnNewLevelFullyLoaded += this.FixGuon;
            base.Pickup(player);
        }

        protected override void Update()
        {
            if (base.Owner != null)
            {
                foreach (GameObject obj in EnergyOrbitals)
                {
                    if (obj == null)
                    {
                        EnergyOrbitals.Remove(obj);
                    }
                }
                if (EnergyOrbitals.Count != 3 && EnergyOrbitals.Count <=4)
                {

                    GameObject guon = PlayerOrbitalItem.CreateOrbital(base.Owner, EnergyShield.EnergyGuon, false);
                    EnergyOrbitals.Add(guon);
                }
            }
        }



        private void FixGuon()
        {
            /*
            bool flag = base.Owner && base.Owner.GetComponent<EnergyShield.ElectricGuonbehavior>() != null;
            bool flag2 = flag;
            bool flag3 = flag2;
            if (flag3)
            {
                base.Owner.GetComponent<EnergyShield.ElectricGuonbehavior>().Destroy();
            }
            */
            PlayerController owner = base.Owner;
            //owner.gameObject.AddComponent<EnergyShield.ElectricGuonbehavior>();
        }

        public override DebrisObject Drop(PlayerController player)
        {
            foreach (GameObject guon in EnergyOrbitals)
            {
                if (guon != null)
                {
                    Destroy(guon.gameObject);
                }
            }
            EnergyOrbitals.Clear();
            //player.GetComponent<EnergyShield.ElectricGuonbehavior>().Destroy();
            EnergyShield.guonHook.Dispose();
            GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
            return base.Drop(player);
        }

        protected override void OnDestroy()
        {
            if (base.Owner != null)
            {
                foreach (GameObject guon in EnergyOrbitals)
                {
                    if (guon != null)
                    {
                        Destroy(guon.gameObject);
                    }
                }
                EnergyShield.guonHook.Dispose();
                EnergyOrbitals.Clear();

                GameManager.Instance.OnNewLevelFullyLoaded -= this.FixGuon;
                base.OnDestroy();
            }
        }




        public static void GuonInit(Action<PlayerOrbital, PlayerController> orig, PlayerOrbital self, PlayerController player)
        {
            orig(self, player);
        }
        public static Hook guonHook;
        public static PlayerOrbital orbitalPrefab;
       
    }
}

namespace Planetside
{
    public class EnergyGuonGlowManager : MonoBehaviour
    {
        public EnergyGuonGlowManager()
        {
            this.player = GameManager.Instance.PrimaryPlayer;
        }

        public void Awake()
        {
            this.actor = base.GetComponent<PlayerOrbital>();
        }

        public void Start()
        {

            PlayerOrbital playerOrbital2 = (PlayerOrbital)actor;
            SpeculativeRigidbody specRigidbody = playerOrbital2.specRigidbody;
            specRigidbody.OnPreRigidbodyCollision = (SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate)Delegate.Combine(specRigidbody.OnPreRigidbodyCollision, new SpeculativeRigidbody.OnPreRigidbodyCollisionDelegate(this.OnPreCollision));
            actor.sprite.usesOverrideMaterial = true;
            
            actor.sprite.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
            Material mat = actor.sprite.GetCurrentSpriteDef().material = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = actor.sprite.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color32(0, 242, 255, 255));
            mat.SetFloat("_EmissiveColorPower", 1.55f);
            mat.SetFloat("_EmissivePower", 70);
            actor.sprite.renderer.material = mat;
            GlowMat = mat;
        }
        private Material GlowMat;
        private void OnPreCollision(SpeculativeRigidbody myRigidbody, PixelCollider myCollider, SpeculativeRigidbody other, PixelCollider otherCollider)
        {
            Hits++;
            Projectile component = other.GetComponent<Projectile>();
            bool flag = component != null && !(component.Owner is PlayerController) && Hits == 2;
            if (flag)
            {
                actor.StartCoroutine(this.PhaseOut());
                
            }
        }

        private IEnumerator PhaseOut()
        {

            Material material1 = actor.sprite.renderer.material;
            material1.shader = ShaderCache.Acquire("Brave/Internal/HologramShader");
            material1.SetFloat("_IsGreen", 0f);
            actor.sprite.renderer.material = material1;
            AkSoundEngine.PostEvent("Play_WPN_melter_shot_01", player.gameObject);
            PlayerOrbital playerOrbital2 = (PlayerOrbital)actor;
            SpeculativeRigidbody specRigidbody = playerOrbital2.specRigidbody;
            specRigidbody.CollideWithTileMap = false;
            specRigidbody.CollideWithOthers = true;
            specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.Trap;
            actor.sprite.usesOverrideMaterial =false;
            yield return new WaitForSeconds(5);
            AkSoundEngine.PostEvent("Play_OBJ_mine_beep_01", player.gameObject);
            actor.sprite.renderer.material = GlowMat;
            Hits = 0;
            specRigidbody.CollideWithTileMap = false;
            specRigidbody.CollideWithOthers = true;
            specRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyBulletBlocker;
            yield break;
        }
        private int Hits;
        private PlayerOrbital actor;
        public PlayerController player;
    }
}