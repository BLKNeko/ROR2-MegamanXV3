using BepInEx.Configuration;
using MegamanXMod.Modules;

namespace MegamanXMod.Survivors.X
{
    public static class XConfig
    {
        public static ConfigEntry<bool> enableVoiceBool;
        public static ConfigEntry<float> midChargeMultiplierFloat;
        public static ConfigEntry<float> fullChargeMultiplierFloat;

        public static ConfigEntry<float> secondArmorSlotLvl;
        public static ConfigEntry<float> thirdArmorSlotLvl;
        public static ConfigEntry<float> fourthArmorSlotLvl;


        public static void Init()
        {
            string section = "X";

            enableVoiceBool = Config.BindAndOptions(
                section,
                "EnableVoice",
                true,
                "At certain moments or when using a skill, X may talk or scream. If you prefer to disable this feature, you can turn it off here.");

            midChargeMultiplierFloat = Config.BindAndOptions(
                section,
                "MidChargeDamageMultiplier",
                1.8f,
                1.5f,
                5f,
                "This is the medium charge damage multiplier.");

            fullChargeMultiplierFloat = Config.BindAndOptions(
                section,
                "FullChargeDamageMultiplier",
                4f,
                2f,
                10f,
                "This is the full charge damage multiplier.");

            secondArmorSlotLvl = Config.BindAndOptions(
                section,
                "SecondArmorSlotLvl",
                3f,
                2f,
                4f,
                "Lvl required to unlock the second armor slot.");

            thirdArmorSlotLvl = Config.BindAndOptions(
                section,
                "ThirdArmorSlotLvl",
                5f,
                4f,
                7f,
                "Lvl required to unlock the third armor slot.");

            fourthArmorSlotLvl = Config.BindAndOptions(
                section,
                "FourthArmorSlotLvl",
                8f,
                7f,
                10f,
                "Lvl required to unlock the fourt armor slot.");


        }
    }
}
