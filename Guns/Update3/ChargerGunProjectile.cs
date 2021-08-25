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

namespace Planetside
{
	internal class ChargerGunProjectile : MonoBehaviour
	{
		public ChargerGunProjectile()
		{
		}



        public void Start()
        {
            //GameObject original;
            this.projectile = base.GetComponent<Projectile>();
            Projectile projectile = this.projectile;
            PlayerController playerController = projectile.Owner as PlayerController;
            Projectile component = base.gameObject.GetComponent<Projectile>();
            bool flag = component != null;
            bool flag2 = flag;
            if (flag2)
            {
                Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
                mat.mainTexture = component.sprite.renderer.material.mainTexture;
                mat.SetColor("_EmissiveColor", new Color32(255, 224, 163, 255));
                mat.SetFloat("_EmissiveColorPower", 1.55f);
                mat.SetFloat("_EmissivePower", 100);
                component.sprite.renderer.material = mat;

                SpeedMULT = UnityEngine.Random.Range(0.85f, 1.15f);
                ImprovedAfterImage yes = component.gameObject.AddComponent<ImprovedAfterImage>();
                yes.spawnShadows = true;
                yes.shadowLifetime = (UnityEngine.Random.Range(0.1f, 0.2f));
                yes.shadowTimeDelay = 0.001f;
                yes.dashColor = new Color(1, 0.8f, 0.55f, 0.3f);
                yes.name = "Gun Trail";
            }
        }
        public void Update()
        {

        }
        private Projectile projectile;
        private float SpeedMULT;
	}
}

