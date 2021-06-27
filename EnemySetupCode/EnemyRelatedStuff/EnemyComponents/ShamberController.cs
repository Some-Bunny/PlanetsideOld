using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.ObjectModel;
using System.Collections;
using Planetside;
using Brave.BulletScript;


public class ShamberController : BraveBehaviour
{
	public void Start()
	{
		base.aiActor.bulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
		BulletsEaten = 0;
		Material mat = new Material(EnemyDatabase.GetOrLoadByName("GunNut").sprite.renderer.material);
		mat.mainTexture = base.aiActor.sprite.renderer.material.mainTexture;
		mat.SetColor("_EmissiveColor", new Color32(255, 255, 255, 255));
		mat.SetFloat("_EmissivePower", 40);
		mat.SetFloat("_EmissiveThresholdSensitivity", 1f);
		mat.SetFloat("_EmissiveColorPower", 2f);

		base.sprite.renderer.material = mat;
		CanSucc = true;
		base.healthHaver.OnPreDeath += this.OnPreDeath;
	}


	protected override void OnDestroy()
	{
		if (base.healthHaver)
		{
			base.healthHaver.OnPreDeath -= this.OnPreDeath;
		}
		base.OnDestroy();
	}

	protected void Update()
	{
		Vector3 vector = base.sprite.WorldBottomLeft.ToVector3ZisY(0f);
		Vector3 vector2 = base.sprite.WorldTopRight.ToVector3ZisY(0f);
		Vector3 a = vector2 - vector;
		vector += a * 0.25f;
		vector2 -= a * 0.25f;
		float num = (vector2.y - vector.y) * (vector2.x - vector.x);
		int num2 = Mathf.CeilToInt(40f * num);
		int num3 = num2;
		Vector3 minPosition = vector;
		Vector3 maxPosition = vector2;
		Vector3 direction = Vector3.up / 1.11f;
		float angleVariance = 120f;
		float magnitudeVariance = 0.1f;
		float? startLifetime = new float?(UnityEngine.Random.Range(0.1f, 2f));
		GlobalSparksDoer.DoRandomParticleBurst(num3, minPosition, maxPosition, direction, angleVariance, magnitudeVariance, null, startLifetime, null, GlobalSparksDoer.SparksType.FLOATY_CHAFF);
		if (CanSucc == true)
        {
			ReadOnlyCollection<Projectile> allProjectiles = StaticReferenceManager.AllProjectiles;
			if (allProjectiles != null)
			{
				foreach (Projectile proj in allProjectiles)
				{
					bool ae = Vector2.Distance(proj.sprite.WorldCenter, base.sprite.WorldCenter) < 3f && proj != null && proj.specRigidbody != null;
					if (ae)
					{
						GameManager.Instance.Dungeon.StartCoroutine(this.HandleBulletSuck(proj));
					}
				}
			}
		}
	}
	public List<Projectile> activeBullets;
	private IEnumerator HandleBulletSuck(Projectile target)
	{
		if(BulletsEaten <= 299)
        {
			this.BulletsEaten += 1;
		}
		//ETGModConsole.Log(BulletsEaten.ToString());
		Transform copySprite = this.CreateEmptySprite(target);
		Destroy(target.gameObject);
		Vector3 startPosition = copySprite.transform.position;
		float elapsed = 0f;
		float duration = 0.666f;
		while (elapsed < duration)
		{
			elapsed += BraveTime.DeltaTime;
			bool flag3 = copySprite && base.aiActor != null;
			if (flag3)
			{
				Vector3 position = base.sprite.WorldCenter;
				float t = elapsed / duration * (elapsed / duration);
				copySprite.position = Vector3.Lerp(startPosition, position, t);
				copySprite.rotation = Quaternion.Euler(0f, 0f, 60f * BraveTime.DeltaTime) * copySprite.rotation;
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
	private Transform CreateEmptySprite(Projectile target)
	{
		GameObject gameObject = new GameObject("suck image");
		gameObject.layer = target.gameObject.layer;
		tk2dSprite tk2dSprite = gameObject.AddComponent<tk2dSprite>();
		gameObject.transform.parent = SpawnManager.Instance.VFX;
		tk2dSprite.SetSprite(target.sprite.Collection, target.sprite.spriteId);
		tk2dSprite.transform.position = target.sprite.transform.position;
		GameObject gameObject2 = new GameObject("image parent");
		gameObject2.transform.position = tk2dSprite.WorldCenter;
		tk2dSprite.transform.parent = gameObject2.transform;

		return gameObject2.transform;
	}
	private void OnDamaged(float resultValue, float maxValue, CoreDamageTypes damageTypes, DamageCategory damageCategory, Vector2 damageDirection)
	{
		
	}

	private void OnPreDeath(Vector2 obj)
	{
		CanSucc = false;
		for (int j = 0; j < BulletsEaten; j++)
		{
			GameObject gameObject = new GameObject();
			gameObject.transform.position = base.sprite.WorldCenter;
			BulletScriptSource source = gameObject.GetOrAddComponent<BulletScriptSource>();
			gameObject.AddComponent<BulletSourceKiller>();
			var bulletScriptSelected = new CustomBulletScriptSelector(typeof(BLLLARGH));
			AIActor aIActor = EnemyDatabase.GetOrLoadByGuid("01972dee89fc4404a5c408d50007dad5");
			AIBulletBank bulletBank = aIActor.GetComponent<AIBulletBank>();
			bulletBank = OtherTools.CopyAIBulletBank(aIActor.bulletBank);//to prevent our gun from affecting the bulletbank of the enemy
			bulletBank.CollidesWithEnemies = false;
			source.BulletManager = bulletBank;
			source.BulletScript = bulletScriptSelected;
			source.Initialize();//to fire the script once
			//Destroy(gameObject);
		}

	}
	private bool CanSucc;
	public int BulletsEaten;
	protected void OnProjCreated(Projectile projectile)
	{
		if (!projectile.Owner.aiActor.CanTargetPlayers && projectile.Owner.aiActor.CanTargetEnemies)
		{
			projectile.collidesWithPlayer = false;
			projectile.collidesWithEnemies = true;

		}
		else
        {
			projectile.collidesWithPlayer = true;
			projectile.collidesWithEnemies = false;
		}
	}
}


public class BLLLARGH : Script
{
	protected override IEnumerator Top()
	{
		float FUCKYOUYOUPIECEOFFUCKINGSHITIHOPEYOUROTINAFUCKINGFREEZER = UnityEngine.Random.Range(-180, 180);
		base.BulletBank.Bullets.Add(EnemyDatabase.GetOrLoadByGuid("1bc2a07ef87741be90c37096910843ab").bulletBank.GetBullet("reversible"));
		base.Fire(new Direction(FUCKYOUYOUPIECEOFFUCKINGSHITIHOPEYOUROTINAFUCKINGFREEZER), new Speed(UnityEngine.Random.Range(4f, 5.5f), SpeedType.Absolute), new BurstBullet());
		yield break;
	}
	public class BurstBullet : Bullet
	{
		public BurstBullet() : base("reversible", false, false, false)
		{
		}
		protected override IEnumerator Top()
		{
			float speed = base.Speed;
			base.ChangeSpeed(new Speed(speed * 3.6f, SpeedType.Absolute), 90);
			yield break;
		}
	}
}
