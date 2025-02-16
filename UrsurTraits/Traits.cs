using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Obeliskial_Content;
using static UnityEngine.Mathf;
using BepInEx.Logging;
using static Enums;
using UnityEngine;
using static TheTyrant.CustomFunctions;
using static TheTyrant.Plugin;

namespace TheTyrant
{
    [HarmonyPatch]
    internal class Traits
    {
        // list of your trait IDs        
        public static string[] myTraitList = ["ursurursineblood",
                                                "ursurresilience",
                                                "ursurbellowingblows",
                                                "ursurbristlyhide",
                                                "ursurbearwithit",
                                                "ursurgrizzledclaws",
                                                "ursurbestdefense",
                                                "ursurunbearable",
                                                "ursurbearlynoticeable"];

        public static void DoCustomTrait(string _trait, ref Trait __instance)
        {
            //Plugin.Log.LogInfo("Testing Ursur Traits");

            // get info you may need
            Enums.EventActivation _theEvent = Traverse.Create(__instance).Field("theEvent").GetValue<Enums.EventActivation>();
            Character _character = Traverse.Create(__instance).Field("character").GetValue<Character>();
            Character _target = Traverse.Create(__instance).Field("target").GetValue<Character>();
            int _auxInt = Traverse.Create(__instance).Field("auxInt").GetValue<int>();
            string _auxString = Traverse.Create(__instance).Field("auxString").GetValue<string>();
            CardData _castedCard = Traverse.Create(__instance).Field("castedCard").GetValue<CardData>();
            Traverse.Create(__instance).Field("character").SetValue(_character);
            Traverse.Create(__instance).Field("target").SetValue(_target);
            Traverse.Create(__instance).Field("theEvent").SetValue(_theEvent);
            Traverse.Create(__instance).Field("auxInt").SetValue(_auxInt);
            Traverse.Create(__instance).Field("auxString").SetValue(_auxString);
            Traverse.Create(__instance).Field("castedCard").SetValue(_castedCard);
            TraitData traitData = Globals.Instance.GetTraitData(_trait);
            List<CardData> cardDataList = [];
            List<string> heroHand = MatchManager.Instance.GetHeroHand(_character.HeroIndex);
            Hero[] teamHero = MatchManager.Instance.GetTeamHero();
            NPC[] teamNpc = MatchManager.Instance.GetTeamNPC();
            // Plugin.Log.LogDebug("Ursur CustomTrait");
            // Plugin.Log.LogDebug("Ursur: " + _trait);
            // activate traits

            if(!IsLivingHero(_character))
            {
                return;
            }

            string traitName = traitData.TraitName;
            string traitId = _trait;

            if (_trait == "ursurursineblood")
            { // Ursine Blood: Start each combat with 1 extra Energy. 
              // Whenever you play a Defense, suffer 2 Bleed. Whenever you play an Attack, suffer 2 Chill. 
              // The 1 extra energy is taken care of in the subclass json
              //Plugin.Log.LogDebug("Found Ursine Blood Trait");
                if (_castedCard != null && _character.HeroData != null)
                {
                    // string traitName = "Ursine Blood";
                    LogDebug($"Executing Trait {traitId}: {traitName}");
                    
                    WhenYouPlayXGainY(Enums.CardType.Attack, "chill", 2, _castedCard, ref _character, traitName);
                    WhenYouPlayXGainY(Enums.CardType.Defense, "bleed", 2, _castedCard, ref _character, traitName);
                    // DisplayTraitScroll(ref _character, traitData);

                }
            }


            else if (_trait == "ursurbristlyhide")
            { //Bristly Hide: When you gain Taunt or Fortify, gain twice as many Thorns. 
            // +1 Fortify charge for every 16 stacks of Bleed. 
            // +1 Taunt charge for every 20 stacks of Chill.
                // Plugin.Log.LogInfo("Found Bristly Hide");
                if (_character.HeroData != null)
                {
                    // string traitName = "Bristly Hide";
                    
                    // int n_bonus_taunt = FloorToInt((float)_character.GetAuraCharges("bleed") / 16.0f);
                    // int n_bonus_fort = FloorToInt((float)_character.GetAuraCharges("chill") / 20.0f);
                    // traitData.AuracurseBonusValue1 = n_bonus_taunt;
                    // traitData.AuracurseBonusValue2 = n_bonus_fort;

                    // Plugin.Log.LogInfo("Bristly Hide - bonus taunt = " + n_bonus_fort + " actual =" + traitData.AuracurseBonusValue2);
                    WhenYouGainXGainY(_auxString, "taunt", "thorns", _auxInt, 0, 2.0f, ref _character, traitName);
                    WhenYouGainXGainY(_auxString, "fortify", "thorns", _auxInt, 0, 2.0f, ref _character, traitName);
                }

            }


            else if (_trait == "ursurbearwithit")
            { // Bear With It: At the start of each turn, 
            // reduce the cost of Attacks by 1 for every 16 Bleed on Ursur. 
            // Reduce the cost of all Defenses by 1 for every 20 Chill.
                if (_character.HeroData != null)
                {
                    Plugin.Log.LogInfo("bearwithit 1");
                    // string traitName = "Bear With It";
                    bool applyToAllCards = false;
                    
                    ReduceCostByStacks(Enums.CardType.Attack, "bleed", 16, ref _character, ref heroHand, ref cardDataList, traitName, applyToAllCards);
                    ReduceCostByStacks(Enums.CardType.Defense, "chill", 20, ref _character, ref heroHand, ref cardDataList, traitName, applyToAllCards);
                }


            }
            else if (_trait == "ursurunbearable")
            { // "Thorns +1. Fury +1. 
            // When you play an Attack, gain 2 Thorns. 
            // When you play a Defense, gain 1 Fury. 
            // Chill does not reduce Ursur's Speed.",
                Plugin.Log.LogInfo(traitName + " 1");
                if (_castedCard != null && _character.HeroData != null)
                {
                    WhenYouPlayXGainY(Enums.CardType.Attack, "thorns", 2, _castedCard, ref _character, traitName);
                    WhenYouPlayXGainY(Enums.CardType.Defense, "fury", 1, _castedCard, ref _character, traitName);
                }

            }
            else if (_trait == "ursurbearlynoticeable")
            { // Bearly Noticeable: Chill +1. Bleed +1. 
              // Chill on Ursur increases all resistances by 0.25% per stack. 
              // Bleed on Ursur increases all damage by 1.5% per stack.
              // Chill and Bleed taken care of in the ursurbearlynoticeable.json
              // taken care of by the GlobalAuraCurseModificationByTraitsAndItemsPostfix Postfix

            }
        }


        [HarmonyPrefix]
        [HarmonyPatch(typeof(Trait), "DoTrait")]
        public static bool DoTrait(EventActivation _theEvent, string _trait, Character _character, Character _target, int _auxInt, string _auxString, CardData _castedCard, ref Trait __instance)
        {
            //Plugin.Log.LogDebug("Ursur DoTrait");

            if ((UnityEngine.Object)MatchManager.Instance == (UnityEngine.Object)null)
                return false;
            Traverse.Create(__instance).Field("character").SetValue(_character);
            Traverse.Create(__instance).Field("target").SetValue(_target);
            Traverse.Create(__instance).Field("theEvent").SetValue(_theEvent);
            Traverse.Create(__instance).Field("auxInt").SetValue(_auxInt);
            Traverse.Create(__instance).Field("auxString").SetValue(_auxString);
            Traverse.Create(__instance).Field("castedCard").SetValue(_castedCard);
            //Plugin.Log.LogDebug("Ursur DoTrait inside conditions " + _trait +" "+ String.Join("\n", Content.medsCustomTraitsSource));

            if (Content.medsCustomTraitsSource.Contains(_trait) && myTraitList.Contains(_trait))
            {
                DoCustomTrait(_trait, ref __instance);
                Plugin.Log.LogDebug("Ursur DoTrait inside conditions " + _trait);

                return false;
            }
            return true;
        }

        

        public static void IncreaseChargesByStacks(string auraCurseToModify, float stacks_per_bonus, string auraCurseDependent, ref Character _character, string traitName)
        {
            // increases the amount of ACtoModify that by. 
            // For instance if you want to increase the amount of burn you apply by 1 per 10 stacks of spark, then IncreaseChargesByStacks("burn",10,"spark",..)
            // Currently does not output anything to the combat log, because I don't know if it should
            int n_stacks = _character.GetAuraCharges(auraCurseDependent);
            int toIncrease = FloorToInt(n_stacks / stacks_per_bonus);
            _character.ModifyAuraCurseQuantity(auraCurseToModify, toIncrease);
        }

        
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager), "GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget)
        {            
            // Bearly Noticeable: Chill +1. Bleed +1. 
            // Chill on Ursur increases all resistances by 0.25% per stack. 
            // Bleed on Ursur increases all damage by 1.5% per stack.
            // Bleed deals half damage when consumed
            
            // Unbearable :Thorns +1. Fury +1. 
            // When you play an Attack, gain 2 Thorns. When you play a Defense, gain 1 Fury. 
            // Chill does not reduce Ursur's Speed.

            // LogInfo($"GACM {subclassName}");

            Character characterOfInterest = _type == "set" ? _characterTarget : _characterCaster;
            string traitOfInterest;

            switch (_acId)
            {
                case "bleed":
                    traitOfInterest = "ursurbearlynoticeable";
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.ThisHero))
                    {
                        LogDebug($"Executing {traitOfInterest} GACM");
                        __result.AuraDamageType2 = Enums.DamageType.All;
                        __result.AuraDamageIncreasedPercentPerStack2 = 1.5f;

                        // __result.ProduceDamageWhenConsumed = false;
                        __result.DamageWhenConsumedPerCharge *= 0.5f;

                    }
                    break;

                case "chill":
                    traitOfInterest = "ursurbearlynoticeable";
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.ThisHero))
                    {
                        LogDebug($"Executing {traitOfInterest} GACM");
                        __result.ResistModified = Enums.DamageType.All;
                        __result.ResistModifiedPercentagePerStack = 0.25f;

                    }

                    traitOfInterest = "ursurunbearable";
                    if (IfCharacterHas(characterOfInterest, CharacterHas.Trait, traitOfInterest, AppliesTo.ThisHero))
                    {
                        LogDebug($"Executing {traitOfInterest} GACM");
                        __result.CharacterStatModified = CharacterStat.None;
                        __result.CharacterStatChargesMultiplierNeededForOne = 0;

                    }
                    break;
            }           
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character), nameof(Character.GetTraitAuraCurseModifiers))]
        public static void GetTraitAuraCurseModifiersPostfix(ref Character __instance, ref Dictionary<string,int> __result)
        {
            //Bristly Hide: When you gain Taunt or Fortify, gain twice as many Thorns. 
            // +1 Fortify charge for every 16 stacks of Bleed. 
            // +1 Taunt charge for every 20 stacks of Chill.
            if (IsLivingHero(__instance) && __instance.HaveTrait(""))
            {
                int nChill = __instance.EffectCharges("chill");
                int nBleed = __instance.EffectCharges("bleed");
                int bonusTauntCharges = FloorToInt(nBleed * 0.0625f);
                int bonusFortifyCharges = FloorToInt(nChill * 0.05f);

                if(__result.ContainsKey("taunt"))
                {
                    __result["taunt"] += bonusTauntCharges;
                }
                else 
                {
                    __result["taunt"] = bonusTauntCharges;
                }

                if(__result.ContainsKey("fortify"))
                {
                    __result["fortify"] += bonusFortifyCharges;
                }
                else 
                {
                    __result["fortify"] = bonusFortifyCharges;
                }

            }

        }

        // [HarmonyPrefix]
        // [HarmonyPatch(typeof(Character), "SetEvent")]
        // public static void SetEventPrefix(ref Character __instance, ref Enums.EventActivation theEvent, Character target = null)
        // {

        //     if (__instance.IsHero && (theEvent == EventActivation.BeginTurn) && __instance.Traits.Contains("ursurbristlyhide"))
        //     {
        //         Plugin.Log.LogDebug("Binbin -- Bristly Hide calc values");
        //         int n_bonus_fort = FloorToInt((float)__instance.GetAuraCharges("bleed") / 16.0f);
        //         int n_bonus_taunt = FloorToInt((float)__instance.GetAuraCharges("chill") / 20.0f);
        //         TraitData traitData = Globals.Instance.GetTraitData("ursurbristlyhide");
        //         traitData.AuracurseBonusValue1 = n_bonus_taunt;
        //         traitData.AuracurseBonusValue2 = n_bonus_fort;
        //     }
        //     if (__instance.IsHero && (theEvent == EventActivation.BeginCombatEnd) && __instance.Traits.Contains("ursurbristlyhide"))
        //     {
        //         Plugin.Log.LogDebug("Binbin -- Bristly Hide inside end turn");
        //         TraitData traitData = Globals.Instance.GetTraitData("ursurbristlyhide");
        //         traitData.AuracurseBonusValue1 = 0;
        //         traitData.AuracurseBonusValue2 = 0;
        //     }
        // }


    }

}
