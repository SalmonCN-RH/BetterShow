using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;
using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace BetterShow
{
    public class Debug
    {
        public static ManualLogSource logger;

        public static void Log(object str)
        {
            logger.LogInfo(str);
        }
        public static void LogError(object str)
        {
            logger.LogError(str);
        }

        public static void LogWarning(object str)
        {
            logger.LogWarning(str);
        }
    }

    public class Info
    {
        public const String GUID = "salmon.pvzrh.bettershow";   //GUID
        public const String NAME = "BetterShow";   //NAME
        public const String VER = "0.0.1";   //VER
        public const String AUTHOR = "Salmon";   //AUTHOR
    }

    [BepInPlugin(Info.GUID, Info.NAME, Info.VER)]
    public class BetterShow : BasePlugin
    {
        public static Harmony harmony = new Harmony(Info.GUID);

        public static BetterShow Instance;

        public static Board board = new Board();

        public override void Load()
        {
            Debug.logger = base.Log;
            harmony.PatchAll();
            BetterShow.Instance = this;
            main.CreateInstance(this);
        }
    }

    public class main : MonoBehaviour
    {
        public BetterShow loader;
        public static Board board = new Board();

        #pragma warning disable
        void Start()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Debug.Log(Info.NAME + " 加载成功！");
        }

        void Update()
        {
            try
            {
                board = GameAPP.board.GetComponent<Board>();
            }
            catch (Exception e) { }
        }

        public static void CreateInstance(BetterShow loader)
        {
            main main = loader.AddComponent<main>();
            main.loader = loader;
            UnityEngine.Object.DontDestroyOnLoad(main.gameObject);
            main.hideFlags |= HideFlags.HideAndDontSave;
        }
    }

    [HarmonyPatch(typeof(Plant))]
    public class Plant_HealthTextPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Postfix_Update(Plant __instance)
        {
            __instance.UpdateHealthText();
        }

        [HarmonyPatch("UpdateHealthText")]
        [HarmonyPostfix]
        public static void Postfix(Plant __instance)
        {
            // flashcountdown
            switch (__instance.thePlantType)
            {
                case PlantType.SunMine:
                    __instance.healthText.text += '\n' + "生产冷却:" + (((int)__instance.thePlantProduceCountDown).ToString() + "s" + "\n出土:" + (((int)__instance.attributeCountdown).ToString() + 's'));
                    __instance.healthTextShadow.text += '\n' + "生产冷却:" + (((int)__instance.thePlantProduceCountDown).ToString() + "s" + "\n出土:" + (((int)__instance.attributeCountdown).ToString() + 's'));
                    break;
                case PlantType.SunFlower:
                case PlantType.PeaSunFlower:
                case PlantType.TwinFlower:
                case PlantType.SunNut:
                case PlantType.SunShroom:
                case PlantType.SeaSunShroom:
                    __instance.healthText.text += '\n' + "生产冷却:" + (((int)__instance.thePlantProduceCountDown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "生产冷却:" + (((int)__instance.thePlantProduceCountDown).ToString() + 's');
                    break;
                case PlantType.SunMagnet:
                    __instance.healthText.text += '\n' + "生产冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "生产冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    break;
                case PlantType.PotatoMine:
                    __instance.healthText.text += '\n' + "出土:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "出土:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    break;
                case PlantType.PeaMine:
                    __instance.healthText.text += '\n' + "出土:" + (((int)(__instance.attributeCountdown / 2)).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "出土:" + (((int)(__instance.attributeCountdown / 2)).ToString() + 's');
                    break;
                case PlantType.Chomper:
                case PlantType.PeaChomper:
                case PlantType.SunChomper:
                case PlantType.CherryChomper:
                case PlantType.NutChomper:
                case PlantType.PotatoChomper:
                    __instance.healthText.text += '\n' + "咀嚼冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "咀嚼冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    break;
                case PlantType.Marigold:
                case PlantType.TwinMarigold:
                    __instance.healthText.text += '\n' + "生产冷却:" + (((int)__instance.thePlantProduceCountDown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "生产冷却:" + (((int)__instance.thePlantProduceCountDown).ToString() + 's');
                    break;
                case PlantType.CobCannon:
                case PlantType.FireCannon:
                case PlantType.IceCannon:
                case PlantType.MelonCannon:
                case PlantType.UltimateCannon:
                    __instance.healthText.text += '\n' + "生产冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "生产冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    break;
                case PlantType.SuperPumpkin:
                case PlantType.FinalPumpkin:
                case PlantType.BlowerPumpkin:
                    __instance.healthText.text += '\n' + "生产冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "生产冷却:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    break;
                case PlantType.HypnoEmperor:
                    __instance.healthText.text += '\n' + "生成冷却:" + (((int)__instance.GetComponent<HyponoEmperor>().summonZombieTime).ToString() + "s " + "剩余魅惑次数:" + (__instance.GetComponent<HyponoEmperor>().restHealth));
                    __instance.healthTextShadow.text += '\n' + "生成冷却:" + (((int)__instance.GetComponent<HyponoEmperor>().summonZombieTime).ToString() + "s " + "剩余魅惑次数:" + (__instance.GetComponent<HyponoEmperor>().restHealth));
                    break;
                case PlantType.HypnoQueen:
                    __instance.healthText.text += '\n' + "生成冷却:" + (((int)__instance.GetComponent<HypnoQueen>().summonZombieTime).ToString() + "s " + "剩余魅惑次数:" + (__instance.GetComponent<HypnoQueen>().restHealth));
                    __instance.healthTextShadow.text += '\n' + "生成冷却:" + (((int)__instance.GetComponent<HypnoQueen>().summonZombieTime).ToString() + "s " + "剩余魅惑次数:" + (__instance.GetComponent<HypnoQueen>().restHealth));
                    break;
                case PlantType.LanternMagnet:
                case PlantType.CherryMagnet:
                case PlantType.Magnetshroom:
                    __instance.healthText.text += '\n' + "消化:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "消化:" + (((int)__instance.attributeCountdown).ToString() + 's');
                    break;
                case PlantType.SuperStar:
                    __instance.healthText.text += '\n' + "普通陨石:" + (((int)main.board.bigStarPassiveCountDown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "普通陨石:" + (((int)main.board.bigStarPassiveCountDown).ToString() + 's');
                    break;
                case PlantType.UltimateStar:
                    __instance.healthText.text += '\n' + "究极陨石:" + ((int)main.board.ultimateStarCountDown).ToString() + 's';
                    __instance.healthTextShadow.text += '\n' + "究极陨石:" + ((int)main.board.ultimateStarCountDown).ToString() + 's';
                    break;
                case PlantType.GoldCabbage:
                case PlantType.GoldCorn:
                case PlantType.GoldGarlic:
                case PlantType.GoldUmbrella:
                case PlantType.GoldMelon:
                case PlantType.SuperUmbrella:
                case PlantType.EmeraldUmbrella:
                case PlantType.RedEmeraldUmbrella:
                    __instance.healthText.text += '\n' + "大招冷却:" + (((int)__instance.flashCountDown).ToString() + 's');
                    __instance.healthTextShadow.text += '\n' + "大招冷却:" + (((int)__instance.flashCountDown).ToString() + 's');
                    break;
            }
        }
    }
}
