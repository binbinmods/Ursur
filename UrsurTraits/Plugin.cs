using BepInEx;
using BepInEx.Logging;
using BepInEx.Configuration;
using HarmonyLib;
using static Obeliskial_Essentials.Essentials;

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
    }
}
