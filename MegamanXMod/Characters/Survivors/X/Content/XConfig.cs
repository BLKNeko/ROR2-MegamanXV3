using BepInEx.Configuration;
using MegamanXMod.Modules;

namespace MegamanXMod.Survivors.X
{
    public static class XConfig
    {
        public static ConfigEntry<bool> enableVoiceBool;
        public static ConfigEntry<int> enableXFootstep;
        public static ConfigEntry<float> midChargeMultiplierFloat;
        public static ConfigEntry<float> fullChargeMultiplierFloat;

        public static ConfigEntry<float> secondArmorSlotLvl;
        public static ConfigEntry<float> thirdArmorSlotLvl;
        public static ConfigEntry<float> fourthArmorSlotLvl;

        // Seção "Unlocks": libera sem precisar completar o achievement (não mexe no perfil do jogador)
        public static ConfigEntry<bool> unlockCMSkin;
        public static ConfigEntry<bool> unlockSecondArmor;
        public static ConfigEntry<bool> unlockXFire;
        public static ConfigEntry<bool> unlockGaeaArmor;
        public static ConfigEntry<bool> unlockBladeArmor;
        public static ConfigEntry<bool> unlockUltimateArmor;
        public static ConfigEntry<bool> unlockRathalosArmor;
        public static ConfigEntry<bool> unlockUltimateArmorCM;

        // Seção "Item Displays": ferramenta de edição (ver XItemDisplayTemplate)
        public static ConfigEntry<bool> showItemDisplays;


        public static void Init()
        {
            // Ícone do X na lista de mods do RiskOfOptions (sprite "XIcon" do asset bundle)
            Config.SetModIconAndDescription(
                XSurvivor.instance.assetBundle.LoadAsset<UnityEngine.Sprite>("XIcon"),
                "Megaman X survivor - time to hunt some maverick monsters! Armors, skins, unlocks and item display options.");

            string section = "X";

            enableVoiceBool = Config.BindAndOptions(
                section,
                "EnableVoice",
                true,
                "At certain moments or when using a skill, X may talk or scream. If you prefer to disable this feature, you can turn it off here.");

            enableXFootstep = Config.BindAndOptions(
                section,
                "Enable X Footstep",
                1,
                0,
                2,
                "Megaman X footstep SFX. \n\n 0 = OFF \n\n 1 = Comand Mission SFX \n\n 2 = MegamanX8 SFX");

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
                3f,
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

            InitUnlocks();

            showItemDisplays = Config.BindAndOptions("Item Displays", "Show Item Displays", false,
                "Shows the items you pick up on X's model. WARNING: item positions were NOT configured for X; " +
                "they are borrowed from Commando, so many items will look out of place. Applies the next time X spawns (next stage or run).");
        }

        private static void InitUnlocks()
        {
            string section = "Unlocks";
            const string note = " Requires a game restart. In multiplayer, every player should use the same setting.";

            unlockCMSkin = Config.BindAndOptions(section, "Unlock Command Mission Skin", false,
                "Unlocks the Command Mission skin without completing 'Commanding the Mission'." + note, true);
            unlockSecondArmor = Config.BindAndOptions(section, "Unlock Giga Armor", false,
                "Unlocks the Giga (Second) Armor without completing 'Capsule Hunter'." + note, true);
            unlockXFire = Config.BindAndOptions(section, "Unlock X-Fire", false,
                "Unlocks X-Fire without completing 'Electric Heat'." + note, true);
            unlockGaeaArmor = Config.BindAndOptions(section, "Unlock Gaea Armor", false,
                "Unlocks the Gaea Armor without completing 'Heavy Lifting'." + note, true);
            unlockBladeArmor = Config.BindAndOptions(section, "Unlock Blade Armor", false,
                "Unlocks the Blade Armor without completing 'Shadow Hunter'." + note, true);
            unlockUltimateArmor = Config.BindAndOptions(section, "Unlock Ultimate Armor", false,
                "Unlocks the Ultimate Armor without completing 'Ultimate Mastery'." + note, true);
            unlockRathalosArmor = Config.BindAndOptions(section, "Unlock Rathalos Armor", false,
                "Unlocks the Rathalos Armor without completing 'Wyvern Slayer'." + note, true);
            unlockUltimateArmorCM = Config.BindAndOptions(section, "Unlock Ultimate Armor CM", false,
                "Unlocks the Ultimate Armor CM without completing 'Final Mission'." + note, true);
        }
    }
}
