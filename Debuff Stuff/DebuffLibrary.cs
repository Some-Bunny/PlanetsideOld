using System;
using ItemAPI;
using UnityEngine;

namespace Planetside
{
	public static class DebuffLibrary
	{
		public static FrailtyHealthEffect Frailty = new FrailtyHealthEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "Frailty",
			AffectsEnemies = true,
			resistanceType = EffectResistanceType.Poison,
			duration = 5f,
			TintColor = new Color(3f, 3f, 3f),
			AppliesTint = true,
			AppliesDeathTint = true,
			PlaysVFXOnActor = true

		};
		public static PossessedEffect Possessed = new PossessedEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "Possession",
			AffectsEnemies = true,
			resistanceType = EffectResistanceType.None,
			duration = 60f,
			TintColor = new Color(3f, 2f, 0f),
			AppliesTint = true,
			AppliesDeathTint = false,

		};

		public static BrokenArmorEffect brokenArmor = new BrokenArmorEffect
		{
			DamagePerSecondToEnemies = 0f,
			effectIdentifier = "broken Armor",
			AffectsEnemies = true,
			resistanceType = EffectResistanceType.None,
			duration = 6f,
			//TintColor = new Color(3f, 2f, 0f),
			AppliesTint = false,
			AppliesDeathTint = false,

		};

		public static GoopDefinition PossesedPuddle = new GoopDefinition
		{
			CanBeIgnited = false,
			damagesEnemies = false,
			damagesPlayers = false,
			baseColor32 = new Color32(49, 100, 100, byte.MaxValue),
			goopTexture = ResourceExtractor.GetTextureFromResource("Planetside/Resources/possessed_standard_base_001.png"),
			AppliesDamageOverTime = true,
			HealthModifierEffect = DebuffLibrary.Possessed

		};
		public static Color LightGreen = new Color(0.55f, 1.76428568f, 0.871428549f);
	}
}
