using System;
using UnityEngine;
using System.Collections.Generic;

namespace Planetside
{
	public class TinyPlanetMotionModule : ProjectileAndBeamMotionModule
	{
		public static int GetOrbitersInGroup(int group)
		{
			if (TinyPlanetMotionModule.m_currentOrbiters.ContainsKey(group))
			{
				return (TinyPlanetMotionModule.m_currentOrbiters[group] == null) ? 0 : TinyPlanetMotionModule.m_currentOrbiters[group].Count;
			}
			return 0;
		}

		public float BeamOrbitRadius
		{
			get
			{
				return this.m_beamOrbitRadius;
			}
			set
			{
				this.m_beamOrbitRadius = value;
				this.m_beamOrbitRadiusCircumference = 6.28318548f * this.m_beamOrbitRadius;
			}
		}

		public override void UpdateDataOnBounce(float angleDiff)
		{
			if (!float.IsNaN(angleDiff))
			{
				this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
				this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
			}
		}

		public override void AdjustRightVector(float angleDiff)
		{
			if (!float.IsNaN(angleDiff))
			{
				this.m_initialUpVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialUpVector;
				this.m_initialRightVector = Quaternion.Euler(0f, 0f, angleDiff) * this.m_initialRightVector;
			}
		}

		private List<TinyPlanetMotionModule> RegisterSelfWithDictionary()
		{
			if (!TinyPlanetMotionModule.m_currentOrbiters.ContainsKey(this.OrbitGroup))
			{
				TinyPlanetMotionModule.m_currentOrbiters.Add(this.OrbitGroup, new List<TinyPlanetMotionModule>());
			}
			List<TinyPlanetMotionModule> list = TinyPlanetMotionModule.m_currentOrbiters[this.OrbitGroup];
			if (!list.Contains(this))
			{
				list.Add(this);
			}
			return list;
		}

		private void DeregisterSelfWithDictionary()
		{
			if (TinyPlanetMotionModule.m_currentOrbiters.ContainsKey(this.OrbitGroup))
			{
				List<TinyPlanetMotionModule> list = TinyPlanetMotionModule.m_currentOrbiters[this.OrbitGroup];
				list.Remove(this);
			}
		}

		public override void Move(Projectile source, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool Inverted, bool shouldRotate)
		{

			Vector2 vector = (!projectileSprite) ? projectileTransform.position.XY() : projectileSprite.WorldCenter;
			source.sprite.renderer.enabled = true;
			this.m_spawnVFXElapsed += BraveTime.DeltaTime;
			if (this.m_spawnVFXActive && (this.CustomSpawnVFXElapsed > 0f && this.m_spawnVFXElapsed > this.CustomSpawnVFXElapsed))
			{
				this.m_spawnVFXActive = false;
				source.sprite.renderer.enabled = true;
			}
			if (!this.m_initialized)
			{
				this.m_initialized = true;
				this.m_initialRightVector = ((!shouldRotate) ? m_currentDirection : projectileTransform.right.XY());
				this.m_initialUpVector = ((!shouldRotate) ? (Quaternion.Euler(0f, 0f, 90f) * m_currentDirection) : projectileTransform.up);
				this.m_radius = 0.1f;
				this.m_currentAngle = this.m_initialRightVector.ToAngle();
				source.OnDestruction += this.OnDestroyed;
			}
			this.RegisterSelfWithDictionary();
			m_timeElapsed += BraveTime.DeltaTime;
			float radius = this.m_radius + (m_timeElapsed*5);
			float num = source.Speed * BraveTime.DeltaTime;
			float num2 = num / (6.28318548f * radius) * 360f;
			this.m_currentAngle += num2;
			Vector2 vector2 = Vector2.zero;

			//if (radius > 3)
            //{
				//source.projectile.OverrideMotionModule = null;
            //}

			if (this.usesAlternateOrbitTarget)
			{
				vector2 = vector;
			}
			else
			{
				vector2 = vector;
			}
			Vector2 vector3 = vector2 + (Quaternion.Euler(0f, 0f, this.m_currentAngle) * Vector2.right * radius).XY();
			if (this.StackHelix)
			{
				float num3 = 2f;
				float num4 = 1f;
				int num5 = (!this.ForceInvert) ? 1 : -1;
				float d = (float)num5 * num4 * Mathf.Sin(source.GetElapsedDistance() / num3);
				vector3 += (vector3 - vector2).normalized * d;
			}
			Vector2 velocity = (vector3 - vector) / BraveTime.DeltaTime;
			m_currentDirection = velocity.normalized;
			if (shouldRotate)
			{
				float num6 = m_currentDirection.ToAngle();
				if (float.IsNaN(num6) || float.IsInfinity(num6))
				{
					num6 = 0f;
				}
				projectileTransform.localRotation = Quaternion.Euler(0f, 0f, num6);
			}
			specRigidbody.Velocity = velocity/(60*(1+BraveTime.DeltaTime));
		}

		public void BeamDestroyed()
		{
			this.OnDestroyed(null);
		}

		// Token: 0x060072FD RID: 29437 RVA: 0x002DB420 File Offset: 0x002D9620
		private void OnDestroyed(Projectile obj)
		{
			this.DeregisterSelfWithDictionary();
			if (this.m_isBeam)
			{
				this.m_isBeam = false;
				OrbitProjectileMotionModule.ActiveBeams--;
			}
		}

		// Token: 0x060072FE RID: 29438 RVA: 0x002DB448 File Offset: 0x002D9648
		public override void SentInDirection(ProjectileData baseData, Transform projectileTransform, tk2dBaseSprite projectileSprite, SpeculativeRigidbody specRigidbody, ref float m_timeElapsed, ref Vector2 m_currentDirection, bool shouldRotate, Vector2 dirVec, bool resetDistance, bool updateRotation)
		{
		}

		public void RegisterAsBeam(BeamController beam)
		{
			if (!this.m_isBeam)
			{
				BasicBeamController basicBeamController = beam as BasicBeamController;
				if (basicBeamController && !basicBeamController.IsReflectedBeam)
				{
					basicBeamController.IgnoreTilesDistance = this.m_beamOrbitRadiusCircumference;
				}
				this.m_isBeam = true;
				OrbitProjectileMotionModule.ActiveBeams++;
			}
		}

		// Token: 0x06007300 RID: 29440 RVA: 0x002DB4A0 File Offset: 0x002D96A0
		public override Vector2 GetBoneOffset(BasicBeamController.BeamBone bone, BeamController sourceBeam, bool inverted)
		{
			PlayerController playerController = sourceBeam.Owner as PlayerController;
			Vector2 vector = playerController.unadjustedAimPoint.XY() - playerController.CenterPosition;
			float num = vector.ToAngle();
			Vector2 barrel = playerController.CurrentGun.barrelOffset.transform.position;
			Vector2 b = bone.Position - barrel;
			Vector2 vector2;
			{
				float aaaa = bone.PosX;
				float num2 = aaaa / this.m_beamOrbitRadiusCircumference * 360f + num;
				float Fard = 0.25f * (aaaa) * 2;
				float x = Mathf.Cos(0.0174532924f * num2) * Fard;
				float y = Mathf.Sin(0.0174532924f * num2) * Fard;
				bone.RotationAngle = num2 + 36f;
				vector2 = new Vector2(x, y) - b;
			}
			
			return vector2;
		}

		// Token: 0x04007444 RID: 29764
		private static Dictionary<int, List<TinyPlanetMotionModule>> m_currentOrbiters = new Dictionary<int, List<TinyPlanetMotionModule>>();

		// Token: 0x04007445 RID: 29765
		public float MinRadius = 0f;

		// Token: 0x04007446 RID: 29766
		public float MaxRadius = 5f;

		// Token: 0x04007447 RID: 29767
		[NonSerialized]
		public float CustomSpawnVFXElapsed = -1f;

		// Token: 0x04007448 RID: 29768
		[NonSerialized]
		public bool HasSpawnVFX;

		// Token: 0x04007449 RID: 29769
		[NonSerialized]
		public GameObject SpawnVFX;

		// Token: 0x0400744A RID: 29770
		public bool ForceInvert;

		// Token: 0x0400744B RID: 29771
		private float m_radius;

		// Token: 0x0400744C RID: 29772
		private float m_currentAngle;

		// Token: 0x0400744D RID: 29773
		private bool m_initialized;

		// Token: 0x0400744E RID: 29774
		private Vector2 m_initialRightVector;

		// Token: 0x0400744F RID: 29775
		private Vector2 m_initialUpVector;

		// Token: 0x04007450 RID: 29776
		[NonSerialized]

		// Token: 0x04007451 RID: 29777
		public int OrbitGroup = -1;

		// Token: 0x04007452 RID: 29778
		[NonSerialized]

		// Token: 0x04007453 RID: 29779
		public float lifespan = -1f;

		// Token: 0x04007454 RID: 29780
		[NonSerialized]
		public bool usesAlternateOrbitTarget;

		// Token: 0x04007455 RID: 29781
		[NonSerialized]
		public SpeculativeRigidbody alternateOrbitTarget;

		// Token: 0x04007456 RID: 29782
		private float m_beamOrbitRadius = 1f;

		// Token: 0x04007457 RID: 29783
		private float m_beamOrbitRadiusCircumference = 17.27876f;

		// Token: 0x04007458 RID: 29784
		private bool m_spawnVFXActive;

		// Token: 0x04007459 RID: 29785

		// Token: 0x0400745A RID: 29786
		private float m_spawnVFXElapsed;

		// Token: 0x0400745B RID: 29787
		public bool m_isBeam;

		// Token: 0x0400745C RID: 29788
		public static int ActiveBeams = 0;

		// Token: 0x0400745D RID: 29789
		public bool StackHelix;

	}
}