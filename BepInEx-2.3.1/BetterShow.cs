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
using System.IO;
using BepInEx.Configuration;
using System.CodeDom.Compiler;

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
        public const String GUID = "salmon.pvzrh.bettershow";
        public const String NAME = "BetterShow";
        public const String VER = "2.3.1";
        public const String AUTHOR = "Salmon";
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
        public static String configDir = Path.Combine(Directory.GetCurrentDirectory(), "BepInEx", "config", Info.GUID + ".cfg");
        public static ConfigFile config = new ConfigFile(configDir, true);
        public static ConfigEntry<String> language = config.Bind("Language", "语言 / language", "zh-cn", "使用何种语言显示属性 / Which language to use for display property");
        public static ConfigEntry<bool> isShow = config.Bind("Show", "显示 / Show", true, "是否显示属性 / Whether to display property");

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

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    if (Input.GetKeyDown(KeyCode.C))
                        language.Value = "zh-cn";
                    if (Input.GetKeyDown(KeyCode.E))
                        language.Value = "en-us";
                }
                if (Input.GetKeyDown(KeyCode.P))
                    isShow.Value = !isShow.Value;

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

        public static int GetCharInStringCount(String str, char target)
        {
            int count = 0;

            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == target)
                    count++;
            }

            return count;
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
            if (!main.isShow.Value)
            {
                __instance.healthText.fontSize = 2.5f;
                __instance.healthTextShadow.fontSize = 2.5f;
            }
        }

        [HarmonyPatch("UpdateHealthText")]
        [HarmonyPostfix]
        public static void Postfix(Plant __instance)
        {
            if (main.isShow.Value)
            {
                String language = main.language.Value.ToLower();

                String produceText = "生产冷却:";
                String armingTimeText = "出土:";
                String chompingCoolDownText = "咀嚼冷却:";
                String reloadCooldownText = "装填冷却:";
                String summonCooldownText = "召唤冷却:";
                String charmLeftText = "魅惑次数:";
                String purgeCooldownText = "消化冷却:";
                String impactCooldownText = "普通陨石:";
                String ultimateCooldownText = "究极陨石:";
                String goldRushCooldownText = "大招冷却:";
                String snipeCooldownText = "狙击冷却:";
                String depletionCooldownText = "衰减冷却:";
                String spawnCooldownText = "生成冷却:";
                String transformCooldown = "变身冷却:";
                String attackCooldown = "攻击冷却:";
                String lightLevelText = "光照等级:";
                String solarCooldownText = "太阳CD:";
                String magnetLevelText = "磁力等级:";
                String fireTimesText = "过火次数:";
                String starCountText = "星星数:";
                String storedDamageText = "存储伤害:";

                switch (language)
                {
                    case "en-us":
                        produceText = "Produce Cooldown:";
                        armingTimeText = "Arming Cooldown:";
                        chompingCoolDownText = "Chomping Cooldown:";
                        reloadCooldownText = "Reload Cooldown:";
                        summonCooldownText = "Summon Cooldown:";
                        charmLeftText = "Charm Left:";
                        purgeCooldownText = "Purge Cooldown:";
                        impactCooldownText = "Impact Cooldown:";
                        ultimateCooldownText = "Ultimate Cooldown:";
                        goldRushCooldownText = "Gold Rush Cooldown:";
                        snipeCooldownText = "Snipe Cooldown:";
                        depletionCooldownText = "Depletion Cooldown:";
                        spawnCooldownText = "Spawn Cooldown:";
                        transformCooldown = "Transform Cooldown:";
                        attackCooldown = "Attack Cooldown:";
                        lightLevelText = "Light Level:";
                        solarCooldownText = "Solar Cooldown:";
                        magnetLevelText = "Magnet Level:";
                        fireTimesText = "Fire Times:";
                        starCountText = "Star Count:";
                        storedDamageText = "Stored Damage:";
                        break;
                }

                if (__instance != null && __instance.thePlantType != null)
                {
                    switch (__instance.thePlantType)
                    {
                        case PlantType.SunMine:
                            __instance.healthText.text += '\n' + produceText + __instance.thePlantProduceCountDown.ToString("0.0") + "s\n" + armingTimeText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + produceText + __instance.thePlantProduceCountDown.ToString("0.0") + "s\n" + armingTimeText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.SilverSunflower:
                        case PlantType.SunFlower:
                        case PlantType.PeaSunFlower:
                        case PlantType.TwinFlower:
                        case PlantType.SunNut:
                        case PlantType.SunShroom:
                        case PlantType.SeaSunShroom:
                            __instance.healthText.text += '\n' + produceText + __instance.thePlantProduceCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + produceText + __instance.thePlantProduceCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.SunMagnet:
                            __instance.healthText.text += '\n' + produceText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + produceText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.PotatoMine:
                            __instance.healthText.text += '\n' + armingTimeText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + armingTimeText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.PeaMine:
                            __instance.healthText.text += '\n' + armingTimeText + (__instance.attributeCountdown / 2).ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + armingTimeText + (__instance.attributeCountdown / 2).ToString("0.0") + "s";
                            break;
                        case PlantType.Chomper:
                        case PlantType.PeaChomper:
                        case PlantType.SunChomper:
                        case PlantType.CherryChomper:
                        case PlantType.NutChomper:
                        case PlantType.PotatoChomper:
                        case PlantType.DoomChomper:
                            __instance.healthText.text += '\n' + chompingCoolDownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + chompingCoolDownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.Marigold:
                        case PlantType.TwinMarigold:
                            __instance.healthText.text += '\n' + produceText + __instance.thePlantProduceCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + produceText + __instance.thePlantProduceCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.CobCannon:
                        case PlantType.FireCannon:
                        case PlantType.IceCannon:
                        case PlantType.MelonCannon:
                        case PlantType.UltimateCannon:
                            __instance.healthText.text += '\n' + reloadCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + reloadCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.SuperPumpkin:
                        case PlantType.UltimatePumpkin:
                        case PlantType.BlowerPumpkin:
                            __instance.healthText.text += '\n' + produceText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + produceText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.HypnoEmperor:
                            __instance.healthText.text += '\n' + summonCooldownText + __instance.GetComponent<HyponoEmperor>().summonZombieTime.ToString("0.0") + "s " + '\n' + charmLeftText + __instance.GetComponent<HyponoEmperor>().restHealth;
                            __instance.healthTextShadow.text += '\n' + summonCooldownText + __instance.GetComponent<HyponoEmperor>().summonZombieTime.ToString("0.0") + "s " + '\n' + charmLeftText + __instance.GetComponent<HyponoEmperor>().restHealth;
                            break;
                        case PlantType.HypnoQueen:
                            __instance.healthText.text += '\n' + summonCooldownText + __instance.GetComponent<HypnoQueen>().summonZombieTime.ToString("0.0") + "s " + '\n' + charmLeftText + __instance.GetComponent<HypnoQueen>().restHealth;
                            __instance.healthTextShadow.text += '\n' + summonCooldownText + __instance.GetComponent<HypnoQueen>().summonZombieTime.ToString("0.0") + "s " + '\n' + charmLeftText + __instance.GetComponent<HypnoQueen>().restHealth;
                            break;
                        case PlantType.LanternMagnet:
                        case PlantType.CherryMagnet:
                        case PlantType.Magnetshroom:
                            __instance.healthText.text += '\n' + purgeCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + purgeCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.SuperStar:
                            __instance.healthText.text += '\n' + impactCooldownText + main.board.bigStarPassiveCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + impactCooldownText + main.board.bigStarPassiveCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.UltimateStar:
                            __instance.healthText.text += '\n' + ultimateCooldownText + main.board.ultimateStarCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + ultimateCooldownText + main.board.ultimateStarCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.GoldCabbage:
                        case PlantType.GoldCorn:
                        case PlantType.GoldGarlic:
                        case PlantType.GoldUmbrella:
                        case PlantType.GoldMelon:
                        case PlantType.SuperUmbrella:
                        case PlantType.EmeraldUmbrella:
                        case PlantType.RedEmeraldUmbrella:
                            __instance.healthText.text += '\n' + goldRushCooldownText + __instance.flashCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + goldRushCooldownText + __instance.flashCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.SolarCabbage:
                            __instance.healthText.text += '\n' + goldRushCooldownText + __instance.flashCountDown.ToString("0.0") + "s\n" + solarCooldownText + main.board.solarCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + goldRushCooldownText + __instance.flashCountDown.ToString("0.0") + "s\n" + solarCooldownText + main.board.solarCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.GoldSunflower:
                            __instance.healthText.text += '\n' + goldRushCooldownText + __instance.flashCountDown.ToString("0.0") + "s\n" + produceText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + goldRushCooldownText + __instance.flashCountDown.ToString("0.0") + "s\n" + produceText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.SniperPea:
                            __instance.healthText.text += '\n' + snipeCooldownText + __instance.thePlantAttackCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + snipeCooldownText + __instance.thePlantAttackCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.FireSniper:
                            __instance.healthText.text += '\n' + snipeCooldownText + __instance.thePlantAttackCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + snipeCooldownText + __instance.thePlantAttackCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.UltimateHypno:
                            __instance.healthText.text += '\n' + depletionCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + depletionCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.SquashTorch:
                            __instance.TryGetComponent<SquashTorch>(out SquashTorch squashTorch);
                            __instance.healthText.text += '\n' + fireTimesText + squashTorch.fireTimes;
                            __instance.healthTextShadow.text += '\n' + fireTimesText + squashTorch.fireTimes;
                            break;
                        case PlantType.UltimateTorch:
                            __instance.TryGetComponent<UltimateTorch>(out UltimateTorch ultimateTorch);
                            __instance.healthText.text += '\n' + fireTimesText + ultimateTorch.fireTimes;
                            __instance.healthTextShadow.text += '\n' + fireTimesText + ultimateTorch.fireTimes;
                            break;
                        case PlantType.CherryTorch:
                            __instance.TryGetComponent<CherryTorch>(out CherryTorch cherryTorch);
                            __instance.healthText.text += '\n' + fireTimesText + cherryTorch.fireTimes;
                            __instance.healthTextShadow.text += '\n' + fireTimesText + cherryTorch.fireTimes;
                            break;
                        case PlantType.TorchSpike:
                            __instance.TryGetComponent<CaltropTorch>(out CaltropTorch torchSpike);
                            __instance.healthText.text += '\n' + fireTimesText + torchSpike.count;
                            __instance.healthTextShadow.text += '\n' + fireTimesText + torchSpike.count;
                            break;
                        case PlantType.KelpTorch:
                            __instance.TryGetComponent<KelpTorch>(out KelpTorch kelpTorch);
                            __instance.healthText.text += '\n' + fireTimesText + kelpTorch.count;
                            __instance.healthTextShadow.text += '\n' + fireTimesText + kelpTorch.count;
                            break;
                        case PlantType.Wheat:
                            __instance.healthText.text += '\n' + transformCooldown + (30 - __instance.wheatTime).ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + transformCooldown + (30 - __instance.wheatTime).ToString("0.0") + "s";
                            break;
                        case PlantType.DoomFume:
                            __instance.healthText.text += '\n' + attackCooldown + __instance.thePlantAttackCountDown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + attackCooldown + __instance.thePlantAttackCountDown.ToString("0.0") + "s";
                            break;
                        case PlantType.PotatoPuff:
                            __instance.healthText.text += '\n' + spawnCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            __instance.healthTextShadow.text += '\n' + spawnCooldownText + __instance.attributeCountdown.ToString("0.0") + "s";
                            break;
                        case PlantType.StarBlover:
                            __instance.TryGetComponent<StarBlover>(out StarBlover starBlover);
                            int count = 0;
                            for (int i = 0; i < starBlover.starBullets.Count; i++)
                            {
                                if (starBlover.starBullets[i] != null)
                                    count++;
                            }
                            __instance.healthText.text += '\n' + starCountText + count + '/' + starBlover.maxBullets;
                            __instance.healthTextShadow.text += '\n' + starCountText + count + '/' + starBlover.maxBullets;
                            break;
                        case PlantType.UltimateBlover:
                            __instance.TryGetComponent<UltimateStarBlover>(out UltimateStarBlover ultimateStarBlover);
                            int count_ultimate = 0;
                            for (int i = 0; i < ultimateStarBlover.starBullets.Count; i++)
                            {
                                if (ultimateStarBlover.starBullets[i] != null)
                                    count_ultimate++;
                            }
                            __instance.healthText.text += '\n' + starCountText + count_ultimate + '/' + ultimateStarBlover.maxBullets;
                            __instance.healthTextShadow.text += '\n' + starCountText + count_ultimate + '/' + ultimateStarBlover.maxBullets;
                            break;
                        case PlantType.MelonUmbrella:
                            __instance.TryGetComponent<MelonUmbrella>(out MelonUmbrella melonUmbrella);
                            __instance.healthText.text += '\n' + storedDamageText + melonUmbrella.storgedDamage;
                            __instance.healthTextShadow.text += '\n' + storedDamageText + melonUmbrella.storgedDamage;
                            break;
                    }
                    if (__instance.isFromWheat && __instance.thePlantType != PlantType.Wheat)
                    {
                        __instance.healthText.text += '\n' + transformCooldown + (30 - __instance.wheatTime).ToString("0.0") + "s";
                        __instance.healthTextShadow.text += '\n' + transformCooldown + (30 - __instance.wheatTime).ToString("0.0") + "s";
                    }

                    if (__instance.currentLightLevel != 0 && __instance.currentLightLevel != null)
                    {
                        __instance.healthText.text += '\n' + lightLevelText + __instance.currentLightLevel;
                        __instance.healthTextShadow.text += '\n' + lightLevelText + __instance.currentLightLevel;
                    }

                    if (__instance.magnetCount > 0 && __instance.magnetCount != null)
                    {
                        __instance.healthText.text += '\n' + magnetLevelText + __instance.magnetCount;
                        __instance.healthTextShadow.text += '\n' + magnetLevelText + __instance.magnetCount;
                    }

                    switch (main.GetCharInStringCount(__instance.healthText.text, '\n'))
                    {
                        case 1:
                            __instance.healthText.fontSize = 2.15f;
                            __instance.healthTextShadow.fontSize = 2.15f;
                            break;
                        case 2:
                            __instance.healthText.fontSize = 2.1f;
                            __instance.healthTextShadow.fontSize = 2.1f;
                            break;
                        case 3:
                            __instance.healthText.fontSize = 1.95f;
                            __instance.healthTextShadow.fontSize = 1.95f;
                            break;
                        case 4:
                            __instance.healthText.fontSize = 1.8f;
                            __instance.healthTextShadow.fontSize = 1.8f;
                            break;
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(Zombie))]
    public class Zombie_HealthTextPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        public static void Postfix_Update(Zombie __instance)
        {
            __instance.UpdateHealthText();
            if (!main.isShow.Value)
            {
                __instance.healthText.fontSize = 2.5f;
                __instance.healthTextShadow.fontSize = 2.5f;
            }
        }

        [HarmonyPatch("UpdateHealthText")]
        [HarmonyPostfix]
        public static void Postfix(Zombie __instance)
        {
            if (main.isShow.Value)
            {
                int count = 0;
                foreach (Plant p in main.board.plantArray)
                {
                    if (p != null)
                    {
                        if (p.TryGetComponent<SniperPea>(out SniperPea sniperPea) && sniperPea.targetZombie == __instance)
                        {
                            count += sniperPea.attackCount % 6;
                            break;
                        }
                        if (p.TryGetComponent<FireSniper>(out FireSniper fireSniper) && fireSniper.targetZombie == __instance)
                        {
                            count += fireSniper.attackCount % 6;
                            break;
                        }
                    }
                }

                String language = main.language.Value.ToLower();

                String poisonLevelText = "狙击秒杀:";
                String snipeExecute = "蒜值:";
                String freezeLevelText = "冻结值:";
                String snowZombieBackText = "回头:";
                String jumpTimeText = "起跳:";

                switch (language)
                {
                    case "en-us":
                        snipeExecute = "Snipe Execute:";
                        poisonLevelText = "Poison Level:";
                        freezeLevelText = "Freeze Level:";
                        snowZombieBackText = "Back:";
                        jumpTimeText = "Jump Time:";
                        break;
                }

                if (count > 0)
                {
                    __instance.healthText.text += '\n' + snipeExecute + (6 - (count % 6));
                    __instance.healthTextShadow.text += '\n' + snipeExecute + (6 - (count % 6));
                }

                if (__instance.freezeLevel > 0)
                {
                    __instance.healthText.text += '\n' + freezeLevelText + __instance.freezeLevel + '/' + __instance.freezeMaxLevel;
                    __instance.healthTextShadow.text += '\n' + freezeLevelText + __instance.freezeLevel + '/' + __instance.freezeMaxLevel;
                }

                if (__instance.poisonLevel > 0)
                {
                    __instance.healthText.text += '\n' + poisonLevelText + __instance.poisonLevel;
                    __instance.healthTextShadow.text += '\n' + poisonLevelText + __instance.poisonLevel;
                }

                if (__instance != null && __instance.theZombieType != null)
                {
                    switch (__instance.theZombieType)
                    {
                        case ZombieType.SnowZombie:
                            if (__instance.attributeCountDown > 0)
                            {
                                __instance.healthText.text += '\n' + snowZombieBackText + __instance.attributeCountDown.ToString("0.0") + "s";
                                __instance.healthTextShadow.text += '\n' + snowZombieBackText + __instance.attributeCountDown.ToString("0.0") + "s";
                            }
                            break;
                        case ZombieType.SuperPogoZombie:
                            __instance.TryGetComponent<SuperPogoZombie>(out SuperPogoZombie pogo);
                            if (pogo.waitTime > 0 && pogo.waitTime < 5)
                            {
                                __instance.healthText.text += '\n' + jumpTimeText + (5 - pogo.waitTime).ToString("0.0") + "s";
                                __instance.healthTextShadow.text += '\n' + jumpTimeText + (5 - pogo.waitTime).ToString("0.0") + "s";
                            }
                            break;
                        case ZombieType.JackboxJumpZombie:
                            __instance.TryGetComponent<JackboxJumpZombie>(out JackboxJumpZombie jackbox);
                            if (jackbox.waitTime > 0 && jackbox.waitTime < 5)
                            {
                                __instance.healthText.text += '\n' + jumpTimeText + (5 - jackbox.waitTime).ToString("0.0") + "s";
                                __instance.healthTextShadow.text += '\n' + jumpTimeText + (5 - jackbox.waitTime).ToString("0.0") + "s";
                            }
                            break;
                    }
                }

                switch (main.GetCharInStringCount(__instance.healthText.text, '\n'))
                {
                    case 1:
                        __instance.healthText.fontSize = 2.15f;
                        __instance.healthTextShadow.fontSize = 2.15f;
                        break;
                    case 2:
                        __instance.healthText.fontSize = 2.1f;
                        __instance.healthTextShadow.fontSize = 2.1f;
                        break;
                    case 3:
                        __instance.healthText.fontSize = 1.95f;
                        __instance.healthTextShadow.fontSize = 1.95f;
                        break;
                    case 4:
                        __instance.healthText.fontSize = 1.8f;
                        __instance.healthTextShadow.fontSize = 1.8f;
                        break;
                }
            }
        }
    }
}
