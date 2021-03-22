using Dungeonator;
using System;
using System.Collections.Generic;
using System.Linq;
using tk2dRuntime.TileMap;
using UnityEngine;
using ItemAPI;
using FullInspector;

using Gungeon;

//using DirectionType = DirectionalAnimation.DirectionType;
using AnimationType = ItemAPI.EnemyBuilder.AnimationType;
using System.Collections;

using Brave.BulletScript;
using GungeonAPI;

using System.Text;
using System.IO;
using System.Reflection;


using MonoMod.RuntimeDetour;

namespace Planetside
{

    public static class OtherTools
    {
        public static bool verbose = true;
        private static string defaultLog = Path.Combine(ETGMod.ResourcesDirectory, "PSOG.txt");
        public static string modID = "PSOG";

        public static void Init()
        {
            if (File.Exists(defaultLog)) File.Delete(defaultLog);
        }
        public static bool OwnerHasSynergy(this Gun gun, string synergyName)
        {
            return gun.CurrentOwner is PlayerController && (gun.CurrentOwner as PlayerController).PlayerHasActiveSynergy(synergyName);
        }

        public static void PrintNoID<T>(T obj, string color = "FFFFFF", bool force = false)
        {
            if (verbose || force)
            {
                string[] lines = obj.ToString().Split('\n');
                foreach (var line in lines)
                    LogToConsole($"<color=#{color}> {line}</color>");
            }

            Log(obj.ToString());
        }
        public static void LogToConsole(string message)
        {
            message.Replace("\t", "    ");
            ETGModConsole.Log(message);
        }
        public static void Log<T>(T obj)
        {
            using (StreamWriter writer = new StreamWriter(Path.Combine(ETGMod.ResourcesDirectory, defaultLog), true))
            {
                writer.WriteLine(obj.ToString());
            }
        }

        public static void Notify(string header, string text, string spriteID, UINotificationController.NotificationColor color = UINotificationController.NotificationColor.SILVER)
        {
            tk2dSpriteCollectionData encounterIconCollection = AmmonomiconController.Instance.EncounterIconCollection;
            int spriteIdByName = encounterIconCollection.GetSpriteIdByName(spriteID);
            GameUIRoot.Instance.notificationController.DoCustomNotification(header, text, encounterIconCollection, spriteIdByName, color, false, false);
        }
        public static void ApplyStat(PlayerController player, PlayerStats.StatType statType, float amountToApply, StatModifier.ModifyMethod modifyMethod)
        {
            player.stats.RecalculateStats(player, false, false);
            StatModifier statModifier = new StatModifier()
            {
                statToBoost = statType,
                amount = amountToApply,
                modifyType = modifyMethod
            };
            player.ownerlessStatModifiers.Add(statModifier);
            player.stats.RecalculateStats(player, false, false);
        }
        public static bool Randomizer(float value)
        {
            return UnityEngine.Random.value > value;
        }


        public static void AnimateProjectile(this Projectile proj, List<string> names, int fps, bool loops, List<IntVector2> pixelSizes, List<bool> lighteneds, List<tk2dBaseSprite.Anchor> anchors, List<bool> anchorsChangeColliders,
            List<bool> fixesScales, List<Vector3?> manualOffsets, List<IntVector2?> overrideColliderPixelSizes, List<IntVector2?> overrideColliderOffsets, List<Projectile> overrideProjectilesToCopyFrom)
        {
            tk2dSpriteAnimationClip clip = new tk2dSpriteAnimationClip();
            clip.name = "idle";
            clip.fps = fps;
            List<tk2dSpriteAnimationFrame> frames = new List<tk2dSpriteAnimationFrame>();
            for (int i = 0; i < names.Count; i++)
            {
                string name = names[i];
                IntVector2 pixelSize = pixelSizes[i];
                IntVector2? overrideColliderPixelSize = overrideColliderPixelSizes[i];
                IntVector2? overrideColliderOffset = overrideColliderOffsets[i];
                Vector3? manualOffset = manualOffsets[i];
                bool anchorChangesCollider = anchorsChangeColliders[i];
                bool fixesScale = fixesScales[i];
                if (!manualOffset.HasValue)
                {
                    manualOffset = new Vector2?(Vector2.zero);
                }
                tk2dBaseSprite.Anchor anchor = anchors[i];
                bool lightened = lighteneds[i];
                Projectile overrideProjectileToCopyFrom = overrideProjectilesToCopyFrom[i];
                tk2dSpriteAnimationFrame frame = new tk2dSpriteAnimationFrame();
                frame.spriteId = ETGMod.Databases.Items.ProjectileCollection.inst.GetSpriteIdByName(name);
                frame.spriteCollection = ETGMod.Databases.Items.ProjectileCollection;
                frames.Add(frame);
                int? overrideColliderPixelWidth = null;
                int? overrideColliderPixelHeight = null;
                if (overrideColliderPixelSize.HasValue)
                {
                    overrideColliderPixelWidth = overrideColliderPixelSize.Value.x;
                    overrideColliderPixelHeight = overrideColliderPixelSize.Value.y;
                }
                int? overrideColliderOffsetX = null;
                int? overrideColliderOffsetY = null;
                if (overrideColliderOffset.HasValue)
                {
                    overrideColliderOffsetX = overrideColliderOffset.Value.x;
                    overrideColliderOffsetY = overrideColliderOffset.Value.y;
                }
                tk2dSpriteDefinition def = GunTools.SetupDefinitionForProjectileSprite(name, frame.spriteId, pixelSize.x, pixelSize.y, lightened, overrideColliderPixelWidth, overrideColliderPixelHeight, overrideColliderOffsetX, overrideColliderOffsetY,
                    overrideProjectileToCopyFrom);
                def.ConstructOffsetsFromAnchor(anchor, def.position3, fixesScale, anchorChangesCollider);
                def.position0 += manualOffset.Value;
                def.position1 += manualOffset.Value;
                def.position2 += manualOffset.Value;
                def.position3 += manualOffset.Value;
                if (i == 0)
                {
                    proj.GetAnySprite().SetSprite(frame.spriteCollection, frame.spriteId);
                }
            }
            clip.wrapMode = loops ? tk2dSpriteAnimationClip.WrapMode.Loop : tk2dSpriteAnimationClip.WrapMode.Once;
            clip.frames = frames.ToArray();
            if (proj.sprite.spriteAnimator == null)
            {
                proj.sprite.spriteAnimator = proj.sprite.gameObject.AddComponent<tk2dSpriteAnimator>();
            }
            proj.sprite.spriteAnimator.playAutomatically = true;
            bool flag = proj.sprite.spriteAnimator.Library == null;
            if (flag)
            {
                proj.sprite.spriteAnimator.Library = proj.sprite.spriteAnimator.gameObject.AddComponent<tk2dSpriteAnimation>();
                proj.sprite.spriteAnimator.Library.clips = new tk2dSpriteAnimationClip[0];
                proj.sprite.spriteAnimator.Library.enabled = true;
            }
            proj.sprite.spriteAnimator.Library.clips = proj.sprite.spriteAnimator.Library.clips.Concat(new tk2dSpriteAnimationClip[] { clip }).ToArray();
            proj.sprite.spriteAnimator.DefaultClipId = proj.sprite.spriteAnimator.Library.GetClipIdByName("idle");
            proj.sprite.spriteAnimator.deferNextStartClip = false;
        }
        public static AIActor SetupAIActorDummy(string name, IntVector2 colliderOffset, IntVector2 colliderDimensions)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.SetActive(false);
            FakePrefab.MarkAsFakePrefab(gameObject);
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            SpeculativeRigidbody speculativeRigidbody = gameObject.AddComponent<tk2dSprite>().SetUpSpeculativeRigidbody(colliderOffset, colliderDimensions);
            PixelCollider pixelCollider = new PixelCollider();
            pixelCollider.ColliderGenerationMode = PixelCollider.PixelColliderGeneration.Manual;
            pixelCollider.CollisionLayer = CollisionLayer.EnemyCollider;
            pixelCollider.ManualWidth = colliderDimensions.x;
            pixelCollider.ManualHeight = colliderDimensions.y;
            pixelCollider.ManualOffsetX = colliderOffset.x;
            pixelCollider.ManualOffsetY = colliderOffset.y;
            speculativeRigidbody.PixelColliders.Add(pixelCollider);
            speculativeRigidbody.PrimaryPixelCollider.CollisionLayer = CollisionLayer.EnemyCollider;
            AIActor result = gameObject.AddComponent<AIActor>();
            HealthHaver healthHaver = gameObject.AddComponent<HealthHaver>();
            healthHaver.SetHealthMaximum(10f, null, false);
            healthHaver.ForceSetCurrentHealth(10f);
            BehaviorSpeculator behaviorSpeculator = gameObject.AddComponent<BehaviorSpeculator>();
            ((ISerializedObject)behaviorSpeculator).SerializedObjectReferences = new List<UnityEngine.Object>(0);
            ((ISerializedObject)behaviorSpeculator).SerializedStateKeys = new List<string>
            {
                "OverrideBehaviors",
                "OtherBehaviors",
                "TargetBehaviors",
                "AttackBehaviors",
                "MovementBehaviors"
            };
            ((ISerializedObject)behaviorSpeculator).SerializedStateValues = new List<string>
            {
                "",
                "",
                "",
                "",
                ""
            };
            return result;
        }

        public static void DisableSuperTinting(AIActor actor)
        {
            Material mat = actor.sprite.renderer.material;
            mat.mainTexture = actor.sprite.renderer.material.mainTexture;
            mat.EnableKeyword("BRIGHTNESS_CLAMP_ON");
            mat.DisableKeyword("BRIGHTNESS_CLAMP_OFF");
        }


    }

    public static class AnimateBullet//----------------------------------------------------------------------------------------------
    {
        public static List<T> ConstructListOfSameValues<T>(T value, int length)
        {
            List<T> list = new List<T>();
            for (int i = 0; i < length; i++)
            {
                list.Add(value);
            }
            return list;
        }
    }

}


namespace Planetside
{
    public class FUCK
    {

        //GameManager.Instance.StartCoroutine(FUCK.DoDistortionWaveLocal(obj.transform.position, 1.8f, 0.2f, 10f, 0.4f));

        public static IEnumerator DoReverseDistortionWaveLocal(Vector2 center, float distortionIntensity, float distortionRadius, float maxRadius, float duration)
        {
            Material distMaterial = new Material(ShaderCache.Acquire("Brave/Internal/DistortionWave"));
            Vector4 distortionSettings = FUCK.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
            distMaterial.SetVector("_WaveCenter", distortionSettings);
            Pixelator.Instance.RegisterAdditionalRenderPass(distMaterial);
            float elapsed = 0f;
            while (elapsed < duration)
            {
                elapsed += BraveTime.DeltaTime;
                float t = elapsed / duration;
                t = BraveMathCollege.LinearToSmoothStepInterpolate(0f, 1f, t);
                distortionSettings = FUCK.GetCenterPointInScreenUV(center, distortionIntensity, distortionRadius);
                distortionSettings.w = Mathf.Lerp(distortionSettings.w, 0f, t);
                distMaterial.SetVector("_WaveCenter", distortionSettings);
                float currentRadius = Mathf.Lerp(maxRadius, 0f, t);
                distMaterial.SetFloat("_DistortProgress", (currentRadius / (maxRadius * 5)));
                yield return null;
            }
            Pixelator.Instance.DeregisterAdditionalRenderPass(distMaterial);
            UnityEngine.Object.Destroy(distMaterial);

        }
        private static Vector4 GetCenterPointInScreenUV(Vector2 centerPoint, float dIntensity, float dRadius)
        {
            Vector3 vector = GameManager.Instance.MainCameraController.Camera.WorldToViewportPoint(centerPoint.ToVector3ZUp(0f));
            return new Vector4(vector.x, vector.y, dRadius, dIntensity);
        }
        public AIActor GetNearestEnemy(List<AIActor> activeEnemies, Vector2 position, out float nearestDistance, string[] filter)
        {
            AIActor aiactor = null;
            nearestDistance = float.MaxValue;
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
    }
}