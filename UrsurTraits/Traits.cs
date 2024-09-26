using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using Obeliskial_Content;
using static UnityEngine.Mathf;
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

            // activate traits
            // I don't know how to set the combatLog text I need to do that for all of the traits
            // I took the default method from Traits.cs but don't know how to actually get it to work
            if (_trait == "ursurursineblood")
            { // Ursine Blood: Start each combat with 1 extra Energy. Whenever you play a Defense, suffer 2 Bleed. Whenever you play an Attack, suffer 3 Chill. 
              // The 1 extra energy is taken care of in the subclass json
                if (_theEvent==Enums.EventActivation.CastCard){
                    string traitName = "Ursine Blood";
                    WhenYouPlayXGainY(Enums.CardType.Attack,"chill",3,_castedCard,ref _character,traitName);
                    WhenYouPlayXGainY(Enums.CardType.Defense,"bleed",2,_castedCard,ref _character,traitName);
                    }

            }

                    
            else if (_trait == "ursurbristlyhide")
            { //Bristly Hide: When you gain Taunt or Fortify, gain twice as many Thorns. +1 Taunt charge for every 25 stacks of Bleed. +1 Fortify charge for every 25 stacks of Chill.
                if (_theEvent==Enums.EventActivation.AuraCurseSet){
                    string traitName = "Bristly Hide";
                    IncreaseChargesByStacks("taunt", 25.0f,"bleed",ref _character,traitName);
                    IncreaseChargesByStacks("fortify", 25.0f,"chill",ref _character,traitName);
                    
                    WhenYouGainXGainY(_auxString,"taunt","thorns",_auxInt,0,2.0f,ref _character, traitName);
                    WhenYouGainXGainY(_auxString,"fortify","thorns",_auxInt,0,2.0f,ref _character, traitName);

                }
            }
                
             
            else if (_trait == "ursurbearwithit")
            { // Bear With It: At the start of each turn, reduce the cost of Attacks by 1 for every 25 Bleed on Ursur. Reduce the cost of all Defenses by 1 for every 25 Chill.
                if (_theEvent == Enums.EventActivation.BeginTurn){
                    string traitName = "Bear With It";
                    bool applyToAllCards=false;
                    ReduceCostByStacks(Enums.CardType.Attack, "bleed", 25,ref _character,heroHand, ref cardDataList,traitName,applyToAllCards);
                    ReduceCostByStacks(Enums.CardType.Defense, "chill", 25,ref _character,heroHand, ref cardDataList,traitName,applyToAllCards);
                }
               
                
            } 
            else if (_trait == "ursurunbearable")
            { // Unbearable: When you play an Attack, gain 1 Thorns. When you play a Defense, gain 1 Fury.
                string traitName = "Unbearable";
                if (_theEvent==Enums.EventActivation.CastCard){
                    WhenYouPlayXGainY(Enums.CardType.Attack,"thorns",1,_castedCard,ref _character,traitName);
                    WhenYouPlayXGainY(Enums.CardType.Defense,"fury",1,_castedCard,ref _character,traitName);
                    }

            } 
            else if (_trait == "ursurbearlynoticeable")
            { // Bearly Noticeable: Chill +1. Bleed +1. Chill on Ursur increases all resistances by 0.5% per stack. Bleed on Ursur increases all damage by 1.5% per stack.
             // Chill and Bleed taken care of in the ursurbearlynoticeable.json
             //taken care of by the GlobalAuraCurseModificationByTraitsAndItemsPostfix Postfix
            }
        }


        [HarmonyPostfix]
        [HarmonyPatch(typeof(AtOManager),"GlobalAuraCurseModificationByTraitsAndItems")]
        public static void GlobalAuraCurseModificationByTraitsAndItemsPostfix(ref AtOManager __instance, ref AuraCurseData __result, string _type, string _acId, Character _characterCaster, Character _characterTarget){
            // "Bearly Noticeable" increases damage by 1.5%/bleed and resists by 0.25%/chill
            AuraCurseData _AC = UnityEngine.Object.Instantiate<AuraCurseData>(Globals.Instance.GetAuraCurseData(_acId));
            if(_acId=="bleed")
            {
                if(_type=="set")
                {

                    if (_characterTarget != null && __instance.CharacterHaveTrait(_characterTarget.SubclassName, "ursurbearlynoticeable"))
                    {
                        _AC.AuraDamageType2 = Enums.DamageType.All;
                        _AC.AuraDamageIncreasedPercentPerStack2 = 1.5f;
                    }
                }

            }
            if(_acId=="chill")
            {
                if (_type == "set")
                {
                    if (_characterTarget != null && __instance.CharacterHaveTrait(_characterTarget.SubclassName, "ursurbearlynoticeable"))
                    {
                        _AC.ResistModified = Enums.DamageType.All;
                        _AC.ResistModifiedPercentagePerStack = 0.25f;
                    }
                }

            }

        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Trait), "DoTrait")]
        public static bool DoTrait(Enums.EventActivation _theEvent, string _trait, Character _character, Character _target, int _auxInt, string _auxString, CardData _castedCard, ref Trait __instance)
        {
            if ((UnityEngine.Object)MatchManager.Instance == (UnityEngine.Object)null)
                return false;
            Traverse.Create(__instance).Field("character").SetValue(_character);
            Traverse.Create(__instance).Field("target").SetValue(_target);
            Traverse.Create(__instance).Field("theEvent").SetValue(_theEvent);
            Traverse.Create(__instance).Field("auxInt").SetValue(_auxInt);
            Traverse.Create(__instance).Field("auxString").SetValue(_auxString);
            Traverse.Create(__instance).Field("castedCard").SetValue(_castedCard);
            if (Content.medsCustomTraitsSource.Contains(_trait) && myTraitList.Contains(_trait))
            {
                DoCustomTrait(_trait, ref __instance);
                return false;
            }
            return true;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Character), "SetEvent")]
        public static void SetEventPrefix(ref Character __instance, ref Enums.EventActivation theEvent, Character target = null)
        {
            if (theEvent == Enums.EventActivation.AuraCurseSet && !__instance.IsHero && target != null && target.IsHero && target.HaveTrait("ulfvitrconductor") && __instance.HasEffect("spark"))
            { // if NPC has wet applied to them, deal 50% of their sparks as indirect lightning damage
                __instance.IndirectDamage(Enums.DamageType.Lightning, Functions.FuncRoundToInt((float)__instance.GetAuraCharges("spark") * 0.5f));
            }
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
            if (MatchManager.Instance != null && gainedAuraCurse != null && _character.HeroData != null)
                {
                    if (gainedAuraCurse==desiredAuraCurse){
                        int toApply = RoundToInt((n_charges_incoming+n_bonus_charges)*multiplier);
                        _character.SetAuraTrait(_character, appliedAuraCurse, toApply);
                        _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
                    }
                }
        }                        

        public static void WhenYouPlayXGainY(Enums.CardType desiredCardType, string desiredAuraCurse, int n_charges, CardData castedCard, ref Character _character, string traitName){
            // Grants n_charges of desiredAuraCurse to self when you play a desired cardtype
            if (MatchManager.Instance != null && castedCard != null && _character.HeroData != null)
                {
                    if (castedCard.GetCardTypes().Contains(desiredCardType)){
                        _character.SetAuraTrait(_character, desiredAuraCurse, n_charges);
                        _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
                    }
                }
        }

        public static void ReduceCostByStacks(Enums.CardType cardType, string auraCurseName,int n_charges, ref Character _character, List<string> heroHand, ref List<CardData> cardDataList, string traitName,bool applyToAllCards){
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
                    CardData cardData = cardDataList[index];
                    cardData.EnergyReductionTemporal += num;
                    MatchManager.Instance.UpdateHandCards();
                    CardItem fromTableByIndex = MatchManager.Instance.GetCardFromTableByIndex(cardData.InternalId);
                    fromTableByIndex.PlayDissolveParticle();
                    fromTableByIndex.ShowEnergyModification(-num);
                    _character.HeroItem.ScrollCombatText(Texts.Instance.GetText("traits_"+traitName), Enums.CombatScrollEffectType.Trait);
                    MatchManager.Instance.CreateLogCardModification(cardData.InternalId, MatchManager.Instance.GetHero(_character.HeroIndex));
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
    }
}
