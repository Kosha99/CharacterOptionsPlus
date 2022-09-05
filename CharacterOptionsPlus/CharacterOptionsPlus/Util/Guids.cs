﻿using CharacterOptionsPlus.Feats;

namespace CharacterOptionsPlus.Util
{
  /// <summary>
  /// List of new Guids.
  /// </summary>
  internal static class Guids
  {
    //** Feats **//
    internal const string FuriousFocusFeat = "de9a75d3-1289-4098-a0b7-fda465a79576";

    internal const string HurtfulFeat = "9474814d-363c-401d-9385-c2ce59fe2e3c";
    internal const string HurtfulAbility = "9ef73549-6706-4d33-abd0-bdfaa3ada6a8";
    internal const string HurtfulBuff = "4fd8ee4d-cf60-4b99-9316-e57a71be80ea";

    internal const string SkaldsVigorFeat = "55dd527b-8721-426b-aaa2-036ccc9a0458";
    internal const string SkaldsVigorGreaterFeat = "ee4756c6-797f-4848-a814-4a27a159641d";
    internal const string SkaldsVigorBuff = "9e67121d-0433-4706-a107-7796187df3e1";
    //***********//

    internal static readonly (string guid, string displayName)[] Feats =
      new (string, string)[]
      {
        (FuriousFocusFeat, FuriousFocus.FeatDisplayName),
        (HurtfulFeat, Hurtful.FeatDisplayName),
        (SkaldsVigorFeat, SkaldsVigor.FeatureDisplayName),
      };
  }
}