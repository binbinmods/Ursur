using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Obeliskial_Content;
using static UnityEngine.Mathf;
using BepInEx.Logging;
using static Enums;
using UnityEngine;

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
            Plugin.Log.LogDebug("Ursur CustomTrait");
            Plugin.Log.LogDebug("Ursur: " + _trait);
            // activate traits
            // I don't know how to set the combatLog text I need to do that for all of the traits
            // I took the default method from Traits.cs but don't know how to actually get it to work
            if (_trait == "ursurursineblood")
            { // Ursine Blood: Start each combat with 1 extra Energy. Whenever you play a Defense, suffer 2 Bleed. Whenever you play an Attack, suffer 3 Chill. 
              // The 1 extra energy is taken care of in the subclass json
                //Plugin.Log.LogDebug("Found Ursine Blood Trait");
                if (_castedCard!=null && _character.HeroData != null){
                    //Plugin.Log.LogInfo("Ursine Blood");
                    string traitName = "Ursine Blood";
                    WhenYouPlayXGainY(Enums.CardType.Attack,"chill",3,_castedCard,ref _character,traitName);
                    WhenYouPlayXGainY(Enums.CardType.Defense,"bleed",2,_castedCard,ref _character,traitName);
                    }
                

            }

                    
            else if (_trait == "ursurbristlyhide")
            { //Bristly Hide: When you gain Taunt or Fortify, gain twice as many Thorns. +1 Taunt charge for every 25 stacks of Bleed. +1 Fortify charge for every 25 stacks of Chill.
                Plugin.Log.LogInfo("Found Bristly Hide");
                if (_character.HeroData != null){
                    string traitName = "Bristly Hide";
                    //IncreaseChargesByStacks("taunt", 25.0f,"bleed",ref _character,traitName);
                    //IncreaseChargesByStacks("fortify", 25.0f,"chill",ref _character,traitName);
                    
                    //Failed attempt
                    int n_bonus_taunt = FloorToInt((float )_character.GetAuraCharges("bleed")/25.0f);
                    int n_bonus_fort = FloorToInt((float )_character.GetAuraCharges("chill")/25.0f);
                    traitData.AuracurseBonusValue1=n_bonus_taunt;
                    traitData.AuracurseBonusValue2=n_bonus_fort;

                    Plugin.Log.LogInfo("Bristly Hide - bonus taunt = " + n_bonus_fort + " actual =" + traitData.AuracurseBonusValue2);
                    //Traverse.Create(__instance).Field("AuracurseBonus1").SetValue("taunt");
                    //Traverse.Create(__instance).Field("AuracurseBonus2").SetValue("fort");
                    //Traverse.Create(__instance).Field("auracurseBonusValue1").SetValue(n_bonus_taunt);
                    //Traverse.Create(__instance).Field("auracurseBonusValue2").SetValue(n_bonus_fort);

                    WhenYouGainXGainY(_auxString,"taunt","thorns",_auxInt,0,2.0f,ref _character, traitName);
                    WhenYouGainXGainY(_auxString,"fortify","thorns",_auxInt,0,2.0f,ref _character, traitName);
                }
                
            }
                
             
            else if (_trait == "ursurbearwithit")
            { // Bear With It: At the start of each turn, reduce the cost of Attacks by 1 for every 25 Bleed on Ursur. Reduce the cost of all Defenses by 1 for every 25 Chill.
                if (_character.HeroData != null){
                    Plugin.Log.LogInfo("bearwithit 1");
                    string traitName = "Bear With It";
                    bool applyToAllCards=false;
                    //ReduceCostByStacks(Enums.CardType.Attack, "bleed", 5,ref _character,heroHand, ref cardDataList,traitName,applyToAllCards);
                    //ReduceCostByStacks(Enums.CardType.Defense, "chill", 5,ref _character,heroHand, ref cardDataList,traitName,applyToAllCards);
                    ReduceCostByStacks(Enums.CardType.Attack, "bleed", 25,ref _character,ref heroHand, ref cardDataList,traitName,applyToAllCards);
                    ReduceCostByStacks(Enums.CardType.Defense, "chill", 25,ref _character,ref heroHand, ref cardDataList,traitName,applyToAllCards);
                }
               
                
            } 
            else if (_trait == "ursurunbearable")
            { // Unbearable: When you play an Attack, gain 1 Thorns. When you play a Defense, gain 1 Fury.
                string traitName = "Unbearable";
                Plugin.Log.LogInfo(traitName+" 1");
                if (_castedCard!=null && _character.HeroData != null){

                    WhenYouPlayXGainY(Enums.CardType.Attack,"thorns",1,_castedCard,ref _character,traitName);
                    WhenYouPlayXGainY(Enums.CardType.Defense,"fury",1,_castedCard,ref _character,traitName);
                }

            } 
            else if (_trait == "ursurbearlynoticeable")
            { // Bearly Noticeable: Chill +1. Bleed +1. Chill on Ursur increases all resistances by 0.25% per stack. Bleed on Ursur increases all damage by 1.5% per stack.
             // Chill and Bleed taken care of in the ursurbearlynoticeable.json
             // taken care of by the GlobalAuraCurseModificationByTraitsAndItemsPostfix Postfix
            //Plugin.Log.LogDebug("Found ursurbearlynoticeable");
                
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

        public static void TraitHeal(ref Character _character, ref Character _target, int healAmount, string traitName)
        {
            int _hp = healAmount;
            if (_target.GetHpLeftForMax() < healAmount)
                _hp = _target.GetHpLeftForMax();
            if (_hp <= 0)
                return;
            _target.ModifyHp(_hp);
            CastResolutionForCombatText _cast = new CastResolutionForCombatText();
            _cast.heal = _hp;
            if ((UnityEngine.Object) _target.HeroItem != (UnityEngine.Object) null)
            {
                _target.HeroItem.ScrollCombatTextDamageNew(_cast);
                EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, _target.HeroItem.CharImageT, false);
            }
            else
            {
            _target.NPCItem.ScrollCombatTextDamageNew(_cast);
            EffectsManager.Instance.PlayEffectAC("healimpactsmall", true, _target.NPCItem.CharImageT, false);
            }
            _target.SetEvent(Enums.EventActivation.Healed);
            _character.SetEvent(Enums.EventActivation.Heal, _target);
            _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
        }

        public static void WhenYouGainXGainY(string gainedAuraCurse, string desiredAuraCurse, string appliedAuraCurse, int n_charges_incoming, int n_bonus_charges, float multiplier, ref Character _character, string traitName){
            // Grants a multiplier or bonus charged amount of a second auraCurse given a first auraCurse
            Plugin.Log.LogDebug("WhenYouGainXGainY Debug Start");
            if (MatchManager.Instance != null && gainedAuraCurse != null && _character.HeroData != null)
            {
                Plugin.Log.LogDebug("WhenYouGainXGainY inside conditions 1");
                if (gainedAuraCurse==desiredAuraCurse){
                    Plugin.Log.LogDebug("WhenYouGainXGainY inside conditions 2");
                    int toApply = RoundToInt((n_charges_incoming+n_bonus_charges)*multiplier);
                    _character.SetAuraTrait(_character, appliedAuraCurse, toApply);
                    _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
                }
            }
        }                        

        public static void WhenYouPlayXGainY(Enums.CardType desiredCardType, string desiredAuraCurse, int n_charges, CardData castedCard, ref Character _character, string traitName){
            // Grants n_charges of desiredAuraCurse to self when you play a desired cardtype
            //Plugin.Log.LogDebug("WhenYouPlayXGainY Debug Start");
            if (MatchManager.Instance != null && castedCard != null && _character.HeroData != null)
                {
                    //Plugin.Log.LogDebug("WhenYouPlayXGainY inside conditions 1");
                    if (castedCard.GetCardTypes().Contains(desiredCardType)){
                        //Plugin.Log.LogDebug("WhenYouPlayXGainY inside conditions 2");

                        _character.SetAuraTrait(_character, desiredAuraCurse, n_charges);
                        _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
                    }
                }
        }

        public static void ReduceCostByStacks(Enums.CardType cardType, string auraCurseName,int n_charges, ref Character _character, ref List<string> heroHand, ref List<CardData> cardDataList, string traitName,bool applyToAllCards){
            // Reduces the cost of all cards of cardType by 1 for every n_charges of the auraCurse
            if (!((UnityEngine.Object) _character.HeroData != (UnityEngine.Object) null))
                return;
            int num = FloorToInt((float) (_character.EffectCharges(auraCurseName) / n_charges));
            if (num <= 0)
                return;
            for (int index = 0; index < heroHand.Count; ++index)
            {
                CardData cardData = MatchManager.Instance.GetCardData(heroHand[index]);
                if ((cardData.GetCardFinalCost() > 0 ) && (cardData.GetCardTypes().Contains(cardType)||applyToAllCards)) //previous .Contains(Enums.CardType.Attack)
                    cardDataList.Add(cardData);
            }
            for (int index = 0; index < cardDataList.Count; ++index)
            {
                cardDataList[index].EnergyReductionTemporal += num;
                MatchManager.Instance.UpdateHandCards();
                CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardDataList[index].InternalId);
                fromTableByIndex.PlayDissolveParticle();
                fromTableByIndex.ShowEnergyModification(-num);
                _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
                MatchManager.Instance.CreateLogCardModification(cardDataList[index].InternalId, MatchManager.Instance.GetHero(_character.HeroIndex));
            }
        }

        public static void IncreaseChargesByStacks(string auraCurseToModify, float stacks_per_bonus,string auraCurseDependent, ref Character _character, string traitName){
            // increases the amount of ACtoModify that by. 
            // For instance if you want to increase the amount of burn you apply by 1 per 10 stacks of spark, then IncreaseChargesByStacks("burn",10,"spark",..)
            // Currently does not output anything to the combat log, because I don't know if it should
            int n_stacks = _character.GetAuraCharges(auraCurseDependent);
            int toIncrease = FloorToInt(n_stacks/stacks_per_bonus);
            _character.ModifyAuraCurseQuantity(auraCurseToModify,toIncrease);
        }

        public static string TextChargesLeft(int currentCharges, int chargesTotal)
        {
            int cCharges = currentCharges;
            int cTotal = chargesTotal;
            return "<br><color=#FFF>" + cCharges.ToString() + "/" + cTotal.ToString() + "</color>";
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager),"GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget){
            // "Bearly Noticeable" increases damage by 1.5%/bleed and resists by 0.25%/chill
            AuraCurseData _AC = UnityEngine.Object.Instantiate<AuraCurseData>(Globals.Instance.GetAuraCurseData(_acId));
            //Plugin.Log.LogInfo("Testing ursurbearlynoticeable");
            bool flag2 = false;
            if (_characterTarget != null && _characterTarget.IsHero)
			{
				flag2 = true;
			}

            if(_acId=="bleed" && flag2)
            {
                if(_type=="set")
                {

                    if (_characterTarget != null && __instance.CharacterHaveTrait(_characterTarget.SubclassName, "ursurbearlynoticeable"))
                    {
                        //Plugin.Log.LogInfo("Testing ursurbearlynoticeable inside conditions for " + _characterTarget.SubclassName + " damage");
                        __result.AuraDamageType2 = Enums.DamageType.All;
                        __result.AuraDamageIncreasedPercentPerStack2 = 1.5f;
                    }
                }

            }
            if(_acId=="chill")
            {
                if (_type == "set")
                {
                    if (_characterTarget != null && __instance.CharacterHaveTrait(_characterTarget.SubclassName, "ursurbearlynoticeable"))
                    {                        
                        //Plugin.Log.LogInfo("Testing ursurbearlynoticeable inside conditions for " + _characterTarget.SubclassName + " resistance");

                        __result.ResistModified = Enums.DamageType.All;
                        __result.ResistModifiedPercentagePerStack = 0.25f;
                    }
                    
                }

            }

        }
    
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Character),"GetTraitAuraCurseModifiers")]
        public static void GetTraitAuraCurseModifiersPostfix(ref Character __instance){
            if (__instance.HeroData!=null){
                int n_chill = __instance.EffectCharges("chill");
                int n_bleed = __instance.EffectCharges("bleed");
                int n_bonus_taunt = FloorToInt((float) (n_bleed / 25.0f));
                int n_bonus_fort = FloorToInt((float) (n_chill / 25.0f));

            }

        }
    }
    
}
