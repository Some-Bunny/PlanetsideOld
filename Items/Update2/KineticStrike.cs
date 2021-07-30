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
            lockpicker.AddToSubShop(ItemBuilder.ShopType.Trorc, 1f);

        }
        public static GameObject StrikePrefab;
        public static List<int> spriteIds = new List<int>();
        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);
        }

        public override bool CanBeUsed(PlayerController user)
        {
            if (HasTriggeredCrossHair != true)
            {
                return true;
            }
            else if (HasTriggeredCrossHair == true)
            {
                RoomHandler currentRoom = user.CurrentRoom;
                aimpointCanBeUsed = aimpoint;
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
            }
            else
            {
                return false;
            }
            return false;
        }

        protected override void DoEffect(PlayerController user)
        {
            if (HasTriggeredCrossHair != true)
            {
                GameManager.Instance.StartCoroutine(ClearCooldown());
                //base.ClearCooldowns();
                CrossHair = UnityEngine.Object.Instantiate<GameObject>(RandomPiecesOfStuffToInitialise.KineticStrikeTargetReticle, user.sprite.WorldCenter, Quaternion.identity);
                CrossHair.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(user.sprite.WorldCenter, tk2dBaseSprite.Anchor.LowerCenter);
                AkSoundEngine.PostEvent("Play_OBJ_supplydrop_activate_01", gameObject);
                this.m_currentAngle = BraveMathCollege.Atan2Degrees(user.unadjustedAimPoint.XY() - user.CenterPosition);
                this.m_currentDistance = 5f;
                this.UpdateReticlePosition();
            }
            else if(HasTriggeredCrossHair == true)
            {
                HasTriggeredCrossHair = false;
                GameManager.Instance.StartCoroutine(this.DoStrike(user.CurrentRoom, user));
                Destroy(CrossHair.gameObject);
            }
        }

        private IEnumerator ClearCooldown()
        {
            yield return new WaitForSeconds(0.1f);
            HasTriggeredCrossHair = true;
            base.ClearCooldowns();
            yield break;
        }
        public override void Update()
        {
            base.Update();
            if (CrossHair != null)
            {
                this.UpdateReticlePosition();
            }
        }
        private void UpdateReticlePosition()
        {
            tk2dBaseSprite sprite = CrossHair.GetComponent<tk2dBaseSprite>();
            if (BraveInput.GetInstanceForPlayer(base.LastOwner.PlayerIDX).IsKeyboardAndMouse(false))
            {
                Vector2 vector = base.LastOwner.unadjustedAimPoint.XY();
                Vector2 vector2 = vector - sprite.GetBounds().extents.XY();
                sprite.transform.position = vector2 + new Vector2(0.625f, -0.0625f);
                aimpoint = vector2 + new Vector2(0.6875f, 0);
            }
            else
            {
                BraveInput instanceForPlayer = BraveInput.GetInstanceForPlayer(base.LastOwner.PlayerIDX);
                Vector2 vector3 = base.LastOwner.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                vector3 += instanceForPlayer.ActiveActions.Aim.Vector * 8f * BraveTime.DeltaTime;
                this.m_currentAngle = BraveMathCollege.Atan2Degrees(vector3 - base.LastOwner.CenterPosition);
                this.m_currentDistance = Vector2.Distance(vector3, base.LastOwner.CenterPosition);
                this.m_currentDistance = Mathf.Min(this.m_currentDistance, this.maxDistance);
                vector3 = base.LastOwner.CenterPosition + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right).XY() * this.m_currentDistance;
                Vector2 vector4 = vector3 - sprite.GetBounds().extents.XY() + new Vector2(0.625f, -0.0625f);
                sprite.transform.position = vector4;
                aimpoint = vector4;
            }
        }

        private bool HasTriggeredCrossHair;
        private GameObject CrossHair;
        private IEnumerator DoStrike(RoomHandler room, PlayerController player)
        {
            HasTriggeredCrossHair = false;
            GameObject fuck;
            Vector2 LOL = aimpoint+ new Vector2(-0.125f ,1.25f);
            fuck = UnityEngine.Object.Instantiate<GameObject>(KineticStrike.StrikePrefab, LOL, Quaternion.identity);
            fuck.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(LOL, tk2dBaseSprite.Anchor.LowerCenter);
            tk2dSprite ahfuck = fuck.GetComponent<tk2dSprite>();
            fuck.GetComponent<tk2dBaseSprite>().SetSprite(KineticStrike.spriteIds[0]);
            AkSoundEngine.PostEvent("Play_WPN_dawnhammer_charge_01", fuck);


            Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
            mat.mainTexture = ahfuck.renderer.material.mainTexture;
            mat.SetColor("_EmissiveColor", new Color32(255, 0, 0, 255));
            mat.SetFloat("_EmissiveColorPower", 3);
            mat.SetFloat("_EmissivePower", 100);
            mat.EnableKeyword("BRIGHTNESS_CLAMP_ON");
            mat.DisableKeyword("BRIGHTNESS_CLAMP_OFF");

            ahfuck.renderer.material = mat;

            TextMaker text = player.gameObject.AddComponent<TextMaker>();
            text.TextSize = 4;
            text.Color = Color.red;
            text.ExistTime = 11;
            text.FadeInTime = 0f;
            text.FadeOutTime = 3f;
            text.Opacity = 1;
            text.anchor = dfPivotPoint.TopCenter;
            text.offset = new Vector3(-1.25f, 3f);
            text.GameObjectToAttachTo = player.gameObject;

            Transform copySprite = ahfuck.transform;
            bool Playsound = false;
            float ela = 0f;
            float dura = 10f;
            while (ela < dura)
            {
                float Timer = dura - ela;
                text.ChangeText("Kinetic Impact In:\n\n" + Timer.ToString() + " Seconds");
                //UIToolbox.TextBox(Color.red, "Kinetic Impact In:\n\n" + Timer.ToString() + " Seconds", base.LastOwner.gameObject, dfPivotPoint.TopCenter, new Vector3(0.625f, 1.5f), 0.01f, 0.5f, 0f, 0f, 0f);
                if (ela > 9 && Playsound == false)
                {
                    Playsound = true;
                    AkSoundEngine.PostEvent("Play_BOSS_RatMech_Whistle_01", fuck);
                }
                ela += BraveTime.DeltaTime;
                float t = ela / dura * (ela / dura);
                copySprite.localScale = Vector3.Lerp(Vector3.one, new Vector3(0f, 0f, 0f), t);
                copySprite.GetComponent<tk2dBaseSprite>().PlaceAtPositionByAnchor(LOL + new Vector2((ela / 12.08f), -1.3125f + (ela / 16)), tk2dBaseSprite.Anchor.LowerCenter);
                yield return null;
            }

            Destroy(fuck);
            GameObject fuck1;
            fuck1 = UnityEngine.Object.Instantiate<GameObject>(KineticStrike.StrikePrefab, LOL, Quaternion.identity);
            fuck1.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(LOL, tk2dBaseSprite.Anchor.LowerCenter);
            tk2dSprite troll = fuck1.GetComponent<tk2dSprite>();
            fuck1.GetComponent<tk2dBaseSprite>().SetSprite(KineticStrike.spriteIds[0]);
            
            //KineticStrike gameObject2 = base.gameObject.GetComponent<KineticStrike>();
            StarNuke = EnemyDatabase.GetOrLoadByGuid("b98b10fca77d469e80fb45f3c5badec5").GetComponent<BossFinalRogueDeathController>().DeathStarExplosionVFX;
            GameObject epicwin = UnityEngine.Object.Instantiate<GameObject>(StarNuke);

            epicwin.GetComponent<tk2dBaseSprite>().PlaceAtLocalPositionByAnchor(LOL, tk2dBaseSprite.Anchor.LowerCenter);
            epicwin.transform.position = LOL.Quantize(0.0625f);
            epicwin.GetComponent<tk2dBaseSprite>().UpdateZDepth();
            fuck1.GetComponent<tk2dBaseSprite>().SetSprite(KineticStrike.spriteIds[1]);
            this.Boom(LOL);
            AkSoundEngine.PostEvent("Play_OBJ_nuke_blast_01", fuck);
            text.ChangeText("Kinetic Impact Made Contact.\n\n     Reloading Payload.");
            text.ChangeOffset(new Vector3(-2, 3f));

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

        protected override void OnPreDrop(PlayerController user)
        {
            HasTriggeredCrossHair = false;
            base.OnPreDrop(user);
            if (CrossHair != null)
            {
                Destroy(CrossHair.gameObject);
            }
        }
        protected override void OnDestroy()
        {
            if (CrossHair != null)
            {
                Destroy(CrossHair.gameObject);
            }
            OnDestroy();
        }
        public static GameObject StarNuke;

        
        private Vector2 aimpoint;
        private Vector2 aimpointCanBeUsed;
        private float maxDistance = 15;
        private float m_currentAngle;
        private float m_currentDistance;


    }
}


