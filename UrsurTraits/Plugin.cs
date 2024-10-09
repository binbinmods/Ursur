using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;
using System.IO;
using Obeliskial_Essentials;
using Obeliskial_Content;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;


namespace TheTyrant
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    [BepInDependency("com.stiffmeds.obeliskialessentials")]
    [BepInDependency("com.stiffmeds.obeliskialcontent")]
    [BepInProcess("AcrossTheObelisk.exe")]
    public class Plugin : BaseUnityPlugin
    {
        internal const int ModDate = 20240925;
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        public static ManualLogSource Log;

        public static string characterName = "Ursur";
        public static string subclassName = "tyrant";

        private void Awake()
        {
            Log = Logger;
            Log.LogInfo($"{PluginInfo.PLUGIN_GUID} {PluginInfo.PLUGIN_VERSION} has loaded!");
            // register with Obeliskial Essentials
            RegisterMod(
                _name: PluginInfo.PLUGIN_NAME,
                _author: "binbin",
                _description: "Ursur, The Tyrant.",
                _version: PluginInfo.PLUGIN_VERSION,
                _date: ModDate,
                _link: @"https://github.com/binbinmods/Ursur",
                _contentFolder: "Ursur",
                _type: ["content", "hero", "trait"]
            );
            // Add text for trait test
            medsTexts["trait_Ursine Blood"] = "Testing String";
            //Log.LogDebug("Ursur Prepatch - 1");

            // apply patches
            harmony.PatchAll();
        }
    
    [HarmonyPatch]
    internal class Patches
    {

        [HarmonyPrefix]
        [HarmonyPatch(typeof(EventData),"Init")]
        public static void InitPrefix(ref Globals __instance){
            // Trying to add the character to the list of all characters so that they will be added to events with the "RepeatForAllCharacters" flag
            // not sure if this is the correct place to put this function. It feels weird to put it into the Traits.cs file
            //Plugin.Log.LogDebug("Binbin -- Attempting to add subclass to list for all events");
            
            //Plugin.Log.LogDebug("Binbin -- After Adding List: " + string.Join(";", Globals.Instance.SubClass.Select(x => x.Key).ToArray()));
            //Plugin.Log.LogDebug("Binbin -- medsSubClassesSource: " + string.Join(";", Content.medsSubClassesSource.Select(x => x.Key).ToArray()));
            string p = Path.Combine(Paths.ConfigPath, "Obeliskial_importing",characterName , "subclass");
            if (Directory.Exists(p))
            {
                //Plugin.Log.LogDebug("Binbin -- Path: " + p);
                FileInfo[] medsFI = new DirectoryInfo(Path.Combine(Paths.ConfigPath, "Obeliskial_importing", characterName, "subclass")).GetFiles("*.json");
                foreach (FileInfo f in medsFI){
                    try
                        {
                            SubClassData subclass = Obeliskial_Content.DataTextConvert.ToData(JsonUtility.FromJson<SubClassDataText>(File.ReadAllText(f.ToString())));
                            //Log.LogInfo("Binbin -- subclass to add : " + subclass.SubClassName);
                            if (subclass!=null && !Globals.Instance.SubClass.ContainsKey(subclass.SubClassName)){
                                Globals.Instance.SubClass.Add(subclass.SubClassName.ToLower(),subclass);
                                
                                //Plugin.Log.LogDebug("Binbin -- Subclass Would be added: " + subclass.SubClassName);
                            }
                        }
                    catch (Exception e) { Log.LogError("Error loading custom " + characterName + " subclass " + f.Name + ": " + e.Message); }
                }
            }


        }   
        [HarmonyPostfix]
        [HarmonyPatch(typeof(EventData),"Init")]
        public static void InitPostfix(ref Globals __instance){
            // Trying to add the character to the list of all characters so that they will be added to events with the "RepeatForAllCharacters" flag
            // not sure if this is the correct place to put this function. It feels weird to put it into the Traits.cs file
            //Plugin.Log.LogDebug("Binbin -- Attempting to add subclass to list for all events");
            //Dictionary<string, SubClassData> dataSource = Content.medsSubClassesSource;
            //Plugin.Log.LogDebug("Binbin -- Before Removing "+ subclassName + ": " + string.Join(";", Globals.Instance.SubClass.Select(x => x.Key).ToArray()));

            if (Globals.Instance.SubClass.ContainsKey(subclassName)){
                Globals.Instance.SubClass.Remove(subclassName);
            }
            
            //Plugin.Log.LogDebug("Binbin -- After Removing tyrant: " + string.Join(";", Globals.Instance.SubClass.Select(x => x.Key).ToArray()));

        }   
        }
    }
}
