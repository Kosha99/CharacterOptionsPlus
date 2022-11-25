﻿using CharacterOptionsPlus.Util;
using HarmonyLib;
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes.Prerequisites;
using Kingmaker.Blueprints.Facts;
using Kingmaker.Blueprints.JsonSystem;
using Kingmaker.UnitLogic;
using Kingmaker.UnitLogic.Class.LevelUp;
using System;
using System.Linq;
using static UnityModManagerNet.UnityModManager.ModEntry;

namespace CharacterOptionsPlus.Components
{
  [AllowedOn(typeof(BlueprintUnitFact))]
  [AllowMultipleComponents]
  [TypeId("3792e31d-00ee-4679-aa37-08176046f8c4")]
  internal class QualifyForPrerequisiteFeature : UnitFactComponentDelegate
  {
    private static readonly ModLogger Logger = Logging.GetLogger(nameof(QualifyForPrerequisiteFeature));

    private readonly BlueprintFeatureReference Feature;

    /// <summary>
    /// When checking pre-requisites this unit is considered to have <paramref name="feature"/>
    /// </summary>
    public QualifyForPrerequisiteFeature(BlueprintFeatureReference feature)
    {
      Feature = feature;
    }

    [HarmonyPatch(typeof(PrerequisiteFeature))]
    static class PrerequisiteFeature_Patch
    {
      [HarmonyPatch(nameof(PrerequisiteFeature.ConsiderFulfilled)), HarmonyPostfix]
      static bool ConsiderFulfilled(PrerequisiteFeature __instance, UnitDescriptor unit, ref bool __result)
      {
        try
        {
          foreach (var replacement in unit.Progression.Features.SelectFactComponents<QualifyForPrerequisiteFeature>())
          {
            if (__instance.Feature == replacement.Feature.Get())
            {
              Logger.NativeLog($"Qualifying {replacement.Fact.Name} in place of {__instance.Feature.name}");
              __result = true;
              return false;
            }
          }
        }
        catch (Exception e)
        {
          Logger.LogException("PrerequisiteFeature_Patch.ConsiderFulfilled", e);
        }
        return true;
      }
    }

    [HarmonyPatch(typeof(PrerequisiteFeaturesFromList))]
    static class PrerequisiteFeaturesFromList_Patch
    {
      [HarmonyPatch(nameof(PrerequisiteFeaturesFromList.ConsiderFulfilled)), HarmonyPostfix]
      static bool ConsiderFulfilled(
        PrerequisiteFeaturesFromList __instance,
        FeatureSelectionState selectionState,
        UnitDescriptor unit,
        LevelUpState state,
        ref bool __result)
      {
        try
        {
          var replacementCount = 0;
          foreach (var replacement in unit.Progression.Features.SelectFactComponents<QualifyForPrerequisiteFeature>())
          {
            var feature = replacement.Feature.Get();
            if (__instance.Features.Contains(feature))
            {
              Logger.NativeLog($"Qualifying {replacement.Fact.Name} in place of {feature.name} in list");
              replacementCount++;
            }
          }
          if (replacementCount > 0)
          {
            var originalCount = __instance.Amount;
            __instance.Amount -= replacementCount;
            var checkInternal = __instance.CheckInternal(selectionState, unit, state);
            __instance.Amount = originalCount;

            if (__instance.Amount < 1 || checkInternal)
            {
              __result = true;
              return false;
            }
          }
        }
        catch (Exception e)
        {
          Logger.LogException("PrerequisiteFeaturesFromList_Patch.ConsiderFulfilled", e);
        }
        return true;
      }
    }
  }
}
