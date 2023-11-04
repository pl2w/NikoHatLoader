using BepInEx;
using GorillaUtils.Events;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace NikoHatLoader
{
    [BepInPlugin("pl2w.nikohatloader", "NikoHatLoader", "1.0.0")]
    public class Plugin : BaseUnityPlugin
    {
        Plugin() => new Harmony("pl2w.nikohatloader").PatchAll(Assembly.GetExecutingAssembly());
        static GameObject hat, scarf;
        public static void OnLoad()
        {
            Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("NikoHatLoader.Resources.nikohat");
            AssetBundle bundle = AssetBundle.LoadFromStream(stream);
            stream.Close();

            GameObject cosmeticSet = Instantiate(bundle.LoadAsset<GameObject>("NikoCosmetics"));

            hat = cosmeticSet.transform.Find("HatHair").gameObject;
            hat.transform.SetParent(GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body/head").transform, false);
            hat.transform.localPosition = new Vector3(0, -0.4f, 0.015f);

            scarf = cosmeticSet.transform.Find("Scarf").gameObject;
            scarf.transform.SetParent(GameObject.Find("Player Objects/Local VRRig/Local Gorilla Player/rig/body").transform, false);
            scarf.transform.localPosition = new Vector3(0, 0.025f, 0);

            bundle.Unload(false);
        }

        public void OnDisable()
        {
            hat.SetActive(false);
            scarf.SetActive(false);
        }

        public void OnEnable()
        {
            hat?.SetActive(true);
            scarf?.SetActive(true);
        }
    }

    [HarmonyPatch(typeof(GorillaTagger), "Awake"), HarmonyWrapSafe]
    public class OnGameInit
    {
        public static void Postfix()
        {
            Plugin.OnLoad();
        }
    }
}
