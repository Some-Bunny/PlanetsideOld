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
using SaveAPI;

using MonoMod.RuntimeDetour;
using DaikonForge;

namespace Planetside
{

    public class UIToolbox : TimeInvariantMonoBehaviour
    {
		public IEnumerator WriteTextABovePlayer(Color color, string customKey, GameObject attachobject, dfPivotPoint abchor, Vector3 offset, float customDuration = -1f, float TextScale = 1, float FadeInTime = 0, float FadeOutTime = 0, float DelayOnDisplay = 0)
		{
			yield return new WaitForSeconds(DelayOnDisplay);
			UIToolbox foo = new UIToolbox();
			foo.SpecialTextBox(color, customKey, attachobject, abchor, offset, customDuration, TextScale, FadeInTime, FadeOutTime);
			yield break;
		}
		public static void TextBox(Color color, string customKey, GameObject attachobject, dfPivotPoint abchor , Vector3 offset, float customDuration = -1f, float TextScale = 1, float FadeInTime = 0, float FadeOutTime = 0, float DelayDisplay = 0)
		{
			UIToolbox foo = new UIToolbox();
			GameManager.Instance.StartCoroutine(foo.WriteTextABovePlayer(color, customKey, attachobject, abchor, offset, customDuration, TextScale, FadeInTime, FadeOutTime, DelayDisplay));
		}

		public void SpecialTextBox(Color color, string customKey, GameObject attachobject, dfPivotPoint anchor , Vector3 offset, float customDuration = -1f, float SizeMultiplier = 1, float FadeInTime = 0, float FadeOutTime = 0)
		
		{
			GameUIRoot UIRoot = GameUIRoot.Instance;
			Type type = typeof(GameUIRoot); FieldInfo _property = type.GetField("m_displayingReloadNeeded", BindingFlags.NonPublic | BindingFlags.Instance); _property.GetValue(UIRoot);
			List<bool> DisRelNeed = (List<bool>)_property.GetValue(UIRoot);

			Type type1 = typeof(GameUIRoot); FieldInfo _property1 = type1.GetField("m_extantReloadLabels", BindingFlags.NonPublic | BindingFlags.Instance); _property1.GetValue(UIRoot);
			List<dfLabel> ListDF = (List<dfLabel>)_property1.GetValue(UIRoot);
			if (!attachobject)
			{
				return;
			}
			int num = 0;
			if (DisRelNeed == null || num >= DisRelNeed.Count)
			{
				return;
			}
			if (ListDF == null || num >= ListDF.Count)
			{
				return;
			}
			if (DisRelNeed[num])
			{
				return;
			}
			dfLabel dfLabel = ListDF[num];
			if (dfLabel == null || dfLabel.IsVisible)
			{
				return;
			}
			dfFollowObject component = dfLabel.GetComponent<dfFollowObject>();
			dfLabel.IsVisible = true;
			if (component)
			{
				component.enabled = true;
			}
			GameManager.Instance.StartCoroutine(DisplayTextLabel(color, dfLabel, attachobject, anchor, offset, customDuration, customKey, SizeMultiplier, FadeInTime, FadeOutTime));
		}

		float CalculateCenterXoffset(dfLabel label)
		{
			return label.GetCenter().x - label.transform.position.x;
		}
		public IEnumerator DisplayTextLabel(Color color, dfControl target, GameObject attachobject, dfPivotPoint anchor, Vector3 offset, float customDuration, string customStringKey = "", float SizeMultiplier = 1, float FadeInTime = 0, float FadeOutTime = 0)
		{
			GameUIRoot UIRoot = GameUIRoot.Instance;
			Type type = typeof(GameUIRoot); FieldInfo _property = type.GetField("m_displayingReloadNeeded", BindingFlags.NonPublic | BindingFlags.Instance); _property.GetValue(UIRoot);
			List<bool> DisRelNeed = (List<bool>)_property.GetValue(UIRoot);

			Type type1 = typeof(GameUIRoot); FieldInfo _property1 = type1.GetField("m_isDisplayingCustomReload", BindingFlags.NonPublic | BindingFlags.Instance); _property1.GetValue(UIRoot);
			bool Displaying = (bool)_property1.GetValue(UIRoot);

			PlayerController player = GameManager.Instance.BestActivePlayer;
			int targetIndex = (!player.IsPrimaryPlayer) ? 1 : 0;
			DisRelNeed[targetIndex] = true;
			target.transform.localScale = Vector3.one / GameUIRoot.GameUIScalar;


			dfLabel targetLabel = target as dfLabel;
			string customString = string.Empty;

			dfFollowObject component = targetLabel.gameObject.AddComponent<dfFollowObject>();
			component.attach = attachobject.gameObject;
			component.enabled = true;
			component.mainCamera = GameManager.Instance.MainCameraController.Camera;
			component.anchor = anchor;
			component.offset = new Vector3(CalculateCenterXoffset(targetLabel), 0) + offset;

			customDuration += FadeInTime;
			customDuration += FadeOutTime;

			if (!string.IsNullOrEmpty(customStringKey))
			{
				customString = customStringKey;
			}
			if (customDuration > 0f)
			{
				Displaying = true;
				float outerElapsed = 0f;
				float ExitFade = FadeOutTime;
				while (outerElapsed < customDuration && !GameManager.Instance.IsPaused)
				{
					
					if (outerElapsed < FadeInTime && FadeInTime != 0)
                    {
						float t = outerElapsed / FadeInTime;
						target.IsVisible = true;
						targetLabel.Text = customString;
						targetLabel.Color = color;
						targetLabel.TextScale = SizeMultiplier;
						targetLabel.Opacity = t;
						outerElapsed += BraveTime.DeltaTime;
						yield return null;
					}
				
					else if (outerElapsed > customDuration - FadeOutTime && FadeOutTime != 0)
                    {
						float t = ExitFade / FadeOutTime;
						target.IsVisible = true;
						targetLabel.Text = customString;
						targetLabel.Color = color;
						targetLabel.TextScale = SizeMultiplier;
						targetLabel.Opacity = t;
						outerElapsed += BraveTime.DeltaTime;
						ExitFade -= BraveTime.DeltaTime;
						yield return null;
					}
					else if (outerElapsed > FadeInTime && outerElapsed < customDuration - FadeOutTime)
					{
						target.IsVisible = true;
						targetLabel.Text = customString;
						targetLabel.Color = color;
						targetLabel.TextScale = SizeMultiplier;
						targetLabel.Opacity = 1;
						outerElapsed += BraveTime.DeltaTime;
						yield return null;
					}
					else if (outerElapsed < customDuration && !GameManager.Instance.IsPaused)
					{
						target.IsVisible = true;
						targetLabel.Text = customString;
						targetLabel.Color = color;
						targetLabel.TextScale = SizeMultiplier;
						targetLabel.Opacity = 1;
						outerElapsed += BraveTime.DeltaTime;
					}
					yield return null;
				}
				Displaying = false;
			}
			else
			{
				while (DisRelNeed[targetIndex] && !GameManager.Instance.IsPaused)
				{
					target.IsVisible = true;
					if (!string.IsNullOrEmpty(customString))
					{
						targetLabel.Text = customString;
						targetLabel.Color = Color.white;
					}
					else
					{
						targetLabel.Text = "no";
						targetLabel.Color = Color.red;
					}
					bool shouldShowEver = customDuration > 0f;
					float elapsed = 0f;
					while (elapsed < 0.6f)
					{
						elapsed += this.m_deltaTime;
						if (!DisRelNeed[targetIndex])
						{
							target.IsVisible = false;
							yield break;
						}
						if (!shouldShowEver)
						{
							target.IsVisible = false;
						}
						if (GameManager.Instance.IsPaused)
						{
							target.IsVisible = false;
						}
						yield return null;
					}
					target.IsVisible = false;
					elapsed = 0f;
					while (elapsed < 0.6f)
					{
						elapsed += this.m_deltaTime;
						if (!DisRelNeed[targetIndex])
						{
							target.IsVisible = false;
							yield break;
						}
						yield return null;
					}
				}
			}
			DisRelNeed[targetIndex] = false;
			target.IsVisible = false;
			yield break;
		}
	}
}