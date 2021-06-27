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
    public class KineticStrike : PlayerItem
    {

        public static void Init()
        {
            string itemName = "Kinetic Bombardment";
            string resourceName = "Planetside/Resources/kinetisstrikeitem.png";
            GameObject obj = new GameObject(itemName);
            KineticStrike lockpicker = obj.AddComponent<KineticStrike>();
            ItemBuilder.AddSpriteToObject(itemName, resourceName, obj);
            string shortDesc = "KA-BEWWWWMMM!";
            string longDesc = "Calls in an incredibly powerful, yet delayed kinetic strike on your cursor.\n\nHow did one of these end up inside the Gungeon? No one knows.\n\nHow does one of these even *land* inside the Gungeon? No one knows either.";
            lockpicker.SetupItem(shortDesc, longDesc, "psog");
            lockpicker.SetCooldownType(ItemBuilder.CooldownType.Damage, 1500f);
            lockpicker.consumable = false;
            lockpicker.quality = PickupObject.ItemQuality.S;
            lockpicker.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);


            GameObject gameObject = SpriteBuilder.SpriteFromResource("Planetside/Resources/VFX/KineticStrike/redmarksthespot", null, true);
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            GameObject gameObject2 = new GameObject("Kinetic Strike Stuff");
            tk2dSprite tk2dSprite = gameObject2.AddComponent<tk2dSprite>();
            tk2dSprite.SetSprite(gameObject.GetComponent<tk2dBaseSprite>().Collection, gameObject.GetComponent<tk2dBaseSprite>().spriteId);

            KineticStrike.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/KineticStrike/redmarksthespot", tk2dSprite.Collection));
            KineticStrike.spriteIds.Add(SpriteBuilder.AddSpriteToCollection("Planetside/Resources/VFX/KineticStrike/kineticstrike", tk2dSprite.Collection));

            tk2dSprite.GetCurrentSpriteDef().material.shader = ShaderCache.Acquire("Brave/PlayerShader");
            ForgiveMePlease.spriteIds.Add(tk2dSprite.spriteId);
            gameObject2.SetActive(false);

            tk2dSprite.SetSprite(KineticStrike.spriteIds[0]); //Marker
            tk2dSprite.SetSprite(KineticStrike.spriteIds[1]); //The Actual Strike

            FakePrefab.MarkAsFakePrefab(gameObject2);
            UnityEngine.Object.DontDestroyOnLoad(gameObject2);
            KineticStrike.StrikePrefab = gameObject2;

        }
        public static GameObject StrikePrefab;
        public static List<int> spriteIds = new List<int>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            RoomHandler currentRoom = user.CurrentRoom;
            bool flag = BraveInput.GetInstanceForPlayer(user.PlayerIDX).IsKeyboardAndMouse(false);
            if (flag)
            {
                this.aimpointCanBeUsed = user.unadjustedAimPoint.XY();
            }
            else
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(user.PlayerIDX);
                Vector2 a2 = user.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                a2 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
                this.m_currentAngle = BraveMathCollege.Atan2Degrees(a2 - user.CenterPosition);
                this.m_currentDistance = Vector2.Distance(a2, user.CenterPosition);
                this.m_currentDistance = Mathf.Min(this.m_currentDistance, 15);
                a2 = user.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                Vector2 v2 = a2; //- this.m_extantReticleQuad.GetBounds().extents.XY();
                this.aimpointCanBeUsed = v2;
            }
            IntVector2? vector = (user as PlayerController).CurrentRoom.GetRandomAvailableCell(new IntVector2?(IntVector2.One * 2), CellTypes.FLOOR | CellTypes.PIT, false, null);
            if (vector != null)
            {
                CellData cellAim = currentRoom.GetNearestCellToPosition(aimpointCanBeUsed);
                CellData cellAimLeft = currentRoom.GetNearestCellToPosition(aimpointCanBeUsed + Vector2.left);
                CellData cellAimRight = currentRoom.GetNearestCellToPosition(aimpointCanBeUsed + Vector2.right);
                CellData cellAimUp = currentRoom.GetNearestCellToPosition(aimpointCanBeUsed + Vector2.up);
                CellData cellAimDown = currentRoom.GetNearestCellToPosition(aimpointCanBeUsed + Vector2.down);
                if (!cellAim.isNextToWall && !cellAimLeft.isNextToWall && !cellAimRight.isNextToWall && !cellAimUp.isNextToWall && !cellAimDown.isNextToWall)
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            base.StartCoroutine(this.DoStrike(user.CurrentRoom, user));
        }
        private IEnumerator DoStrike(RoomHandler room, PlayerController player)
        {
            base.CanBeDropped = false;
            bool flag = BraveInput.GetInstanceForPlayer(player.PlayerIDX).IsKeyboardAndMouse(false);
            if (flag)
            {
                this.aimpoint = player.unadjustedAimPoint.XY();
            }
            else
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(player.PlayerIDX);
                Vector2 a = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                a += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
                this.m_currentAngle = BraveMathCollege.Atan2Degrees(a - player.CenterPosition);
                this.m_currentDistance = Vector2.Distance(a, player.CenterPosition);
                this.m_currentDistance = Mathf.Min(this.m_currentDistance, 15f);
                this.aimpoint = player.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
            }
            AkSoundEngine.PostEvent("Play_WPN_dawnhammer_charge_01", base.gameObject);
            GameObject fuck;
            Vector2 LOL = aimpoint;
            fuck = UnityEngine.Object.Instantiate<GameObject>(KineticStrike.StrikePrefab, LOL, Quaternion.identity);
            fuck.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(LOL, tk2dBaseSprite.Anchor.LowerCenter);
            tk2dSprite ahfuck = fuck.GetComponent<tk2dSprite>();
            fuck.GetComponent<tk2dBaseSprite>().SetSprite(KineticStrike.spriteIds[0]);


            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = ahfuck.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color32(255, 0, 0, 255));
            mat.SetFloat("_EmissiveColorPower", 3);
            mat.SetFloat("_EmissivePower", 100);
            mat.EnableKeyword("BRIGHTNESS_CLAMP_ON");
            mat.DisableKeyword("BRIGHTNESS_CLAMP_OFF");

            ahfuck.renderer.material = mat;

            Transform copySprite = ahfuck.transform;
            bool Playsound = false;
            float ela = 0f;
            float dura = 10f;            
            while (ela < dura)
            {
                float Timer = dura - ela;
                UIToolbox.TextBox(Color.red, "Kinetic Impact In:\n\n" + Timer.ToString() + " Seconds", base.LastOwner.gameObject, dfPivotPoint.TopCenter, new Vector3(0.625f, 1.5f), 0.01f, 0.5f, 0f, 0f, 0f);
                if (ela > 9 && Playsound == false)
                {
                    Playsound = true;
                    AkSoundEngine.PostEvent("Play_BOSS_RatMech_Whistle_01", base.gameObject);
                }
                ela += BraveTime.DeltaTime;
                float t = ela / dura * (ela / dura);
                copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0f, 0f, 0f), t);
                copySprite.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(LOL + new Vector2((ela/12.08f), -1.3125f + (ela / 16)), tk2dBaseSprite.Anchor.LowerCenter);
                yield return null;
            }

            Destroy(fuck);
            GameObject fuck1;
            fuck1 = UnityEngine.Object.Instantiate<GameObject>(KineticStrike.StrikePrefab, LOL, Quaternion.identity);
            fuck1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(LOL + new Vector2(0f, 0f), tk2dBaseSprite.Anchor.LowerCenter);
            tk2dSprite troll = fuck1.GetComponent<tk2dSprite>();
            fuck1.GetComponent<tk2dBaseSprite>().SetSprite(KineticStrike.spriteIds[0]);
            // EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").GetComponent<BossFinalRogueDeathController>()
            KineticStrike gameObject2 = base.gameObject.GetComponent<KineticStrike>();

            gameObject2.Nuke = EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").GetComponent<BossFinalRogueDeathController>().DeathStarExplosionVFX;
            GameObject epicwin = UnityEngine.Object.Instantiate<GameObject>(gameObject2.Nuke);

            epicwin.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(LOL, tk2dBaseSprite.Anchor.LowerCenter);
            epicwin.transform.position = LOL.Quantize(0.0625f);
            epicwin.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            fuck1.GetComponent<tk2dBaseSprite>().SetSprite(KineticStrike.spriteIds[1]);
            this.Boom(LOL);
            AkSoundEngine.PostEvent("Play_OBJ_nuke_blast_01", base.gameObject);
            UIToolbox.TextBox(Color.red, "Kinetic Impact Made Contact.\n\n     Reloading Payload.", base.LastOwner.gameObject, dfPivotPoint.TopCenter, new Vector3(0.625f, 1.5f), 1f, 0.5f, 0f, 2f, 0f);
            yield return new WaitForSeconds(5f);

            troll.usesOverrideMaterial = true;
            troll.OverrideMaterialMode = tk2dBaseSprite.SpriteMaterialOverrideMode.OVERRIDE_MATERIAL_COMPLEX;

            troll.sprite.usesOverrideMaterial = true;
            troll.renderer.material.EnableKeyword("_BurnAmount");

            troll.renderer.material.shader = ShaderCache.Acquire("Brave/LitCutoutUber");
            Material targetMaterial = troll.renderer.material;
            Destroy(epicwin);
            ela = 0f;
            dura = 5f;
            while (ela < dura)
            {
                ela += BraveTime.DeltaTime;
                float t = ela / dura;
                targetMaterial.SetFloat("_BurnAmount", t);
                yield return null;
            }
            base.CanBeDropped = true;
            Destroy(fuck1);
            yield break;
        }
        public void Boom(Vector3 position)
        {
            ExplosionData defaultSmallExplosionData = GameManager.Instance.Dungeon.sharedSettingsPrefab.DefaultSmallExplosionData;
            this.smallPlayerSafeExplosion.effect = defaultSmallExplosionData.effect;
            this.smallPlayerSafeExplosion.ignoreList = defaultSmallExplosionData.ignoreList;
            this.smallPlayerSafeExplosion.ss = defaultSmallExplosionData.ss;
            Exploder.Explode(position, this.smallPlayerSafeExplosion, Vector2.zero, null, false, CoreDamageTypes.None, false);
        }
        private ExplosionData smallPlayerSafeExplosion = new ExplosionData
        {
            damageRadius = 3.75f,  
            damageToPlayer = 2f,
            doDamage = true,
            damage = 5000f,
            doExplosionRing = false,
            doDestroyProjectiles = true,
            doForce = true,
            debrisForce = 100f,
            preventPlayerForce = false,
            explosionDelay = 0f,
            usesComprehensiveDelay = false,
            doScreenShake = true,
            playDefaultSFX = false
        };
        public GameObject Nuke;

        private float m_currentAngle;
        private float m_currentDistance;
        private Vector2 aimpoint;
        private Vector2 aimpointCanBeUsed;

    }
}



