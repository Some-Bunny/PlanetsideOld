using System;
using System.Collections.Generic;
using System.Linq;
using Dungeonator;
using ItemAPI;
using UnityEngine;
using System.Collections;
using SaveAPI;
using Brave.BulletScript;
using Gungeon;

namespace Planetside
{
	public class UnstableTeslaCoil : PassiveItem
	{
		public static void Init()
		{
			string name = "Volatile Tesla-Pack";
			string resourcePath = "Planetside/Resources/plaetunstableteslacoil.png";
			GameObject gameObject = new GameObject(name);
			UnstableTeslaCoil warVase = gameObject.AddComponent<UnstableTeslaCoil>();
			ItemBuilder.AddSpriteToObject(name, resourcePath, gameObject);
			string shortDesc = "Hari-Raising Experience";
			string longDesc = "A very volatile tesla-pack that's been hidden away in a chest to prevent harm. The arcs connect to nearby things and can erupt powerfully enough to confuse enemies.";
			ItemBuilder.SetupItem(warVase, shortDesc, longDesc, "psog");
			warVase.quality = PickupObject.ItemQuality.B;
            List<string> mandatoryConsoleIDs = new List<string>
            {
                "psog:volatile_tesla-pack",
            };
            List<string> optionalConsoleIDs = new List<string>
            {
                "face_melter",
                "prototype_railgun",
                "heavy_bullets",
                "potion_of_lead_skin",
                "platinum_bullets"
            };
            CustomSynergies.Add("Heavy Metals", mandatoryConsoleIDs, optionalConsoleIDs, true);
        }

		public override void Pickup(PlayerController player)
		{
			base.Pickup(player);
		}

		public override DebrisObject Drop(PlayerController player)
		{
			DebrisObject result = base.Drop(player);
			return result;
		}


        protected override void Update()
		{
            //Theres like 6 checks to remove the lightning god kill me
			base.Update();
            float num2 = 6f;
            {
                if (this.LinkVFXPrefab == null)
                {
                    this.LinkVFXPrefab = FakePrefab.Clone(Game.Items["shock_rounds"].GetComponent<ComplexProjectileModifier>().ChainLightningVFX);
                }
                if (base.Owner && this.extantLink == null)
                {
                    tk2dTiledSprite component = SpawnManager.SpawnVFX(this.LinkVFXPrefab, false).GetComponent<tk2dTiledSprite>();
                    this.extantLink = component;
                }
                bool isInCombat = base.Owner.IsInCombat;
                if (isInCombat)
                {
                    List<AIActor> activeEnemies = base.Owner.CurrentRoom.GetActiveEnemies(RoomHandler.ActiveEnemyType.All);
                    bool flag5 = activeEnemies == null | activeEnemies.Count <= 0;
                    if (!flag5)
                    {
                        AIActor nearestEnemy = this.GetNearestEnemy(activeEnemies, base.Owner.sprite.WorldCenter, out num2, null);
                        bool flag8 = nearestEnemy && nearestEnemy != null;
                        if (flag8)
                        {
                            if (base.Owner && this.extantLink != null && nearestEnemy != null)
                            {
                                UpdateLink(base.Owner, this.extantLink, nearestEnemy);
                            }
                            else if (extantLink != null && nearestEnemy != null)
                            {
                                SpawnManager.Despawn(extantLink.gameObject);
                                extantLink = null;
                            }
                        }
                        else if (extantLink != null)
                        {
                            SpawnManager.Despawn(extantLink.gameObject);
                            extantLink = null;
                        }
                    }
                    else if (extantLink != null)
                    {
                        SpawnManager.Despawn(extantLink.gameObject);
                        extantLink = null;
                    }
                }
                else if (extantLink != null)
                {
                    SpawnManager.Despawn(extantLink.gameObject);
                    extantLink = null;
                }
            }


        }
        public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
        {
            AIActor aiactor = null;
            nearestDistance = 4.66f;
            bool flag = activeEnemies == null;
            bool flag2 = flag;
            bool flag3 = flag2;
            bool flag4 = flag3;
            bool flag5 = flag4;
            AIActor result;
            if (flag5)
            {
                result = null;
            }
            else
            {
                for (int i = 0; i < activeEnemies.Count; i++)
                {
                    AIActor aiactor2 = activeEnemies[i];
                    bool flag6 = !aiactor2.healthHaver.IsDead && aiactor2.healthHaver.IsVulnerable;
                    bool flag7 = flag6;
                    if (flag7)
                    {
                        bool flag8 = filter == null || !filter.Contains(aiactor2.EnemyGuid);
                        bool flag9 = flag8;
                        if (flag9)
                        {
                            float num = Vector2.Distance(position, aiactor2.CenterPosition);
                            bool flag10 = num < nearestDistance;
                            bool flag11 = flag10;
                            if (flag11)
                            {
                                nearestDistance = num;
                                aiactor = aiactor2;
                            }
                        }
                    }
                }
                result = aiactor;
            }
            return result;
        }

        private void UpdateLink(PlayerController target, tk2dTiledSprite m_extantLink, AIActor actor)
        {
            Vector2 unitCenter = actor.specRigidbody.HitboxPixelCollider.UnitCenter;
            Vector2 unitCenter2 = target.specRigidbody.HitboxPixelCollider.UnitCenter;
            m_extantLink.transform.position = unitCenter;
            Vector2 vector = unitCenter2 - unitCenter;
            float num = BraveMathCollege.Atan2Degrees(vector.normalized);
            int num2 = Mathf.RoundToInt(vector.magnitude / 0.0625f);
            m_extantLink.dimensions = new Vector2((float)num2, m_extantLink.dimensions.y);
            m_extantLink.transform.rotation = Quaternion.Euler(0f, 0f, num);
            m_extantLink.UpdateZDepth();
            this.ApplyLinearDamage(unitCenter, unitCenter2);

        }
        private void ApplyLinearDamage(Vector2 p1, Vector2 p2)
        {
            PlayerController player = (GameManager.Instance.PrimaryPlayer);
            float num = 3.75f;
            for (int i = 0; i < StaticReferenceManager.AllEnemies.Count; i++)
            {
                AIActor aiactor = StaticReferenceManager.AllEnemies[i];
                if (!this.m_damagedEnemies.Contains(aiactor))
                {
                    if (aiactor && aiactor.HasBeenEngaged && aiactor.IsNormalEnemy && aiactor.specRigidbody)
                    {
                        Vector2 zero = Vector2.zero;
                        if (BraveUtility.LineIntersectsAABB(p1, p2, aiactor.specRigidbody.HitboxPixelCollider.UnitBottomLeft, aiactor.specRigidbody.HitboxPixelCollider.UnitDimensions, out zero))
                        {
                            if (5 >= aiactor.healthHaver.GetCurrentHealth() || 5 == aiactor.healthHaver.GetCurrentHealth())
                            {
                                float StunRadius = 2.5f;
                                bool flagA = base.Owner.PlayerHasActiveSynergy("Heavy Metals");
                                if (flagA)
                                {
                                    StunRadius = 4f;
                                }
                                Vector2 bector2 = aiactor.sprite.WorldCenter;
                                float death = aiactor.healthHaver.GetCurrentHealth();
                                player.CurrentRoom.ApplyActionToNearbyEnemies(bector2, StunRadius, new Action<AIActor, float>(this.ProcessEnemy));
                                aiactor.healthHaver.ApplyDamage(death, Vector2.zero, "Chain Lightning", CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
                                GameManager.Instance.StartCoroutine(this.HandleDamageCooldown(aiactor));
                                Exploder.DoDistortionWave(bector2, 1f, 0.25f, 3, 0.066f);


                            }
                            else
                            {
                                aiactor.healthHaver.ApplyDamage(num, Vector2.zero, "Chain Lightning", CoreDamageTypes.Electric, DamageCategory.Normal, false, null, false);
                                GameManager.Instance.StartCoroutine(this.HandleDamageCooldown(aiactor));
                            }
                        }
                    }
                }
            }
        }

        private void ProcessEnemy(AIActor target, float distance)
        {
            float StunTime = 3f;
            bool flagA = base.Owner.PlayerHasActiveSynergy("Heavy Metals");
            if (flagA)
            {
                StunTime = 5f;
            }
            target.behaviorSpeculator.Stun(StunTime, true);
        }
        private IEnumerator HandleDamageCooldown(AIActor damagedTarget)
        {
            this.m_damagedEnemies.Add(damagedTarget);
            yield return new WaitForSeconds(0.25f);
            this.m_damagedEnemies.Remove(damagedTarget);
            yield break;
        }
        private HashSet<AIActor> m_damagedEnemies = new HashSet<AIActor>();
        private GameObject LinkVFXPrefab;
        private tk2dTiledSprite extantLink;
    }
}
