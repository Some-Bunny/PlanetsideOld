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
using Pathfinding;


namespace Planetside
{
	public class PetrifyThing : BraveBehaviour
	{
		public PetrifyThing()
        {
			this.Time = 4f;
		}

		public bool HasTriggered;
		public void Start()
		{
			//UnityEngine.Object.Destroy(cm); 
			base.healthHaver.OnPreDeath += this.OnPreDeath;
			//base.healthHaver.minimumHealth += this.CheatDeath;
		}
		private void OnPreDeath(Vector2 obj)
		{
			base.StartCoroutine(this.LaunchWave(base.aiActor.sprite.WorldCenter));
		}
		public GameObject EyesVFX;

		private IEnumerator LaunchWave(Vector2 startPoint)
		{
			AkSoundEngine.PostEvent("Play_ENM_gorgun_gaze_01", base.gameObject);
			float m_prevWaveDist = 0f;
			float distortionMaxRadius = 20f;
			float distortionDuration = 1f;
			float distortionIntensity = 0.5f;
			float distortionThickness = 0.04f;
			Exploder.DoDistortionWave(startPoint, distortionIntensity, distortionThickness, distortionMaxRadius, distortionDuration);
			float waveRemaining = distortionDuration - (BraveTime.DeltaTime);
			while (waveRemaining > 0f)
			{
				waveRemaining -= BraveTime.DeltaTime;
				float waveDist = BraveMathCollege.LinearToSmoothStepInterpolate(0f, distortionMaxRadius, 1f - waveRemaining / distortionDuration);
				for (int i = 0; i < GameManager.Instance.AllPlayers.Length; i++)
				{
					PlayerController playerController = GameManager.Instance.AllPlayers[i];
					if (!playerController.healthHaver.IsDead)
					{
						if (!playerController.spriteAnimator.QueryInvulnerabilityFrame() && playerController.healthHaver.IsVulnerable)
						{
							Vector2 unitCenter = playerController.specRigidbody.GetUnitCenter(ColliderType.HitBox);
							float num = Vector2.Distance(unitCenter, startPoint);
							if (num >= m_prevWaveDist - 3f && num <= waveDist +3)
							{
								playerController.CurrentStoneGunTimer = Time;
							}
						}
					}
				}
				m_prevWaveDist = waveDist;
				yield return null;
			}
		}
		public void Update()
		{
			
		}
		public float Time;
		public float minimumHealth;
		public float CheatDeath = 2f;
	}
}