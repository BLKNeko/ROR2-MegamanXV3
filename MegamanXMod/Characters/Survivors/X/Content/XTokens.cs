using System;
using MegamanXMod.Modules;
using MegamanXMod.Survivors.X.Achievements;
using R2API;

namespace MegamanXMod.Survivors.X
{
    public static class XTokens
    {
        public static void Init()
        {
            AddHenryTokens();
            AddXTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("X.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddXTokens()
        {
            string prefix = XSurvivor.MEGAMAN_x_PREFIX;

            string desc = "Megaman X, the B class maverick hunter<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X can transform either of his arms into a powerful buster to shoot bullets of compressed solar energy, and has an energy amplifier that allows it to be charged up and release a more powerful shot." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X-Buster is powerful but slow and his charged shot have a limited range, FK-Buster is weaker but faster and have no range limit</color>" + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X fires a shard of ice that can freeze and damage a target." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > Emergency Acceleration System(DASH) is a move that temporarily speeds up the character." + Environment.NewLine + Environment.NewLine;
            desc = desc + "< ! > X's Limitless Potencial make it easy to turn on the table, but be wary of it's high cooldown." + Environment.NewLine + Environment.NewLine;

            string outro = "Back to the Hunter Base.";
            string outroFailure = "Sorry...I...Failed....";

            LanguageAPI.Add(prefix + "NAME", "Megaman X");
            LanguageAPI.Add(prefix + "DESCRIPTION", desc);
            LanguageAPI.Add(prefix + "SUBTITLE", "Megaman X, B class Hunter");
            LanguageAPI.Add(prefix + "LORE", "sample lore");
            LanguageAPI.Add(prefix + "OUTRO_FLAVOR", outro);
            LanguageAPI.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            LanguageAPI.Add(prefix + "DEFAULT_SKIN_NAME", "Default");
            LanguageAPI.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            LanguageAPI.Add(prefix + "X_KEYWORD_CHARGE", "[CHARGE]\nHold the skill key to charge the attack. Charging increases damage or triggers a different action!");


            #region Armors

            LanguageAPI.Add(prefix + "X_ARMOR_NAME", "X");
            LanguageAPI.Add(prefix + "X_ARMOR_DESCRIPTION", "X regular armor");

            LanguageAPI.Add(prefix + "EXTRA_FIRST_LIGHT_ARMOR_NAME", "Light Armor");
            LanguageAPI.Add(prefix + "EXTRA_FIRST_LIGHT_ARMOR_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");

            LanguageAPI.Add(prefix + "EXTRA_FIRST_SECOND_ARMOR_NAME", "Giga Armor");
            LanguageAPI.Add(prefix + "EXTRA_FIRST_SECOND_ARMOR_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");

            LanguageAPI.Add(prefix + "EXTRA_SECOND_MAX_ARMOR_NAME", "Max Armor");
            LanguageAPI.Add(prefix + "EXTRA_SECOND_MAX_ARMOR_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");

            LanguageAPI.Add(prefix + "EXTRA_SECOND_FOURTH_ARMOR_NAME", "Force Armor");
            LanguageAPI.Add(prefix + "EXTRA_SECOND_FOURTH_ARMOR_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");

            LanguageAPI.Add(prefix + "EXTRA_THIRD_FALCON_ARMOR_NAME", "Falcon Armor");
            LanguageAPI.Add(prefix + "EXTRA_THIRD_FALCON_ARMOR_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");

            LanguageAPI.Add(prefix + "EXTRA_THIRD_GAEA_ARMOR_NAME", "Gaea Armor");
            LanguageAPI.Add(prefix + "EXTRA_THIRD_GAEA_ARMOR_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>170 % damage</style>.");

            #endregion

            #region Passive
            //LanguageAPI.Add(prefix + "PASSIVE_NAME", "Limitless Potential");
            //LanguageAPI.Add(prefix + "PASSIVE_DESCRIPTION", "<style=cIsUtility>X's true potential still unachieved, but his adaptation and improvement grow's super fast.</style> <style=cIsHealing>When X HP gets Low, he uses his true powers, getting temporary stronger and generating a shield</style>, <style=cIsDamage> but after this he need to recharge before use this again.</style>");

            LanguageAPI.Add(prefix + "X_PASSIVE_NAME", "X-Heart and Limitless Potential");
            LanguageAPI.Add(prefix + "X_PASSIVE_DESCRIPTION",
                "<style=cIsUtility>X's true potential remains untapped, but his ability to adapt and improve grows rapidly in intense moments.</style> " +
                "<style=cIsHealing>When X's HP drops low, he unleashes his true power, temporarily becoming stronger and generating a shield.</style> " +
                "<style=cIsDamage>However, he needs to recharge before using this ability again. Additionally, X starts with one extra life.</style>");

            #endregion

            #region Primary

            LanguageAPI.Add(prefix + "PRIMARY_X_BUSTER_NAME", "X-Buster");
            LanguageAPI.Add(prefix + "PRIMARY_X_BUSTER_DESCRIPTION", "Shoot with X - Buster, dealing <style=cIsDamage>150 % damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_LIGHT_BUSTER_NAME", "Light-Buster");
            LanguageAPI.Add(prefix + "PRIMARY_LIGHT_BUSTER_DESCRIPTION", "Light-Buster");

            LanguageAPI.Add(prefix + "PRIMARY_GIGA_BUSTER_NAME", "Giga-Buster");
            LanguageAPI.Add(prefix + "PRIMARY_GIGA_BUSTER_DESCRIPTION", "Giga-Buster");



            #endregion

            #region Secondary

            LanguageAPI.Add(prefix + "SECONDARY_SHADOW_SABER_NAME", "Shadow-Saber");
            LanguageAPI.Add(prefix + "SECONDARY_SHADOW_SABER_DESCRIPTION", "Shoot an IceMissle that pierce enemies, dealing <style=cIsDamage> 200 % damage </style>.");

            LanguageAPI.Add(prefix + "SECONDARY_RATHALOS_SABER_NAME", "Rathalos Slash");
            LanguageAPI.Add(prefix + "SECONDARY_RATHALOS_SABER_DESCRIPTION", "Shoot an IceMissle that pierce enemies, dealing <style=cIsDamage> 200 % damage </style>.");

            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_ICE_NAME", "ShotgunIce");
            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_ICE_DESCRIPTION", "Shoot an Ice Missile that pierces enemies, dealing <style=cIsDamage>200% damage</style> and <style=cIsUtility>freezing</style> them.");

            LanguageAPI.Add(prefix + "SECONDARY_SQUEEZE_BOMB_NAME", "Squeeze Bomb");
            LanguageAPI.Add(prefix + "SECONDARY_SQUEEZE_BOMB_DESCRIPTION", "A gravity-based weapon. Creates localized black holes that hold up enemies.");

            LanguageAPI.Add(prefix + "SECONDARY_FIRE_WAVE_NAME", "Fire Wave");
            LanguageAPI.Add(prefix + "SECONDARY_FIRE_WAVE_DESCRIPTION", "X releases a constant stream of flames from his buster");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "DASH_DESCRIPTION", "<style=cIsDamage>Perform a Dash</style>.</style>");

            LanguageAPI.Add(prefix + "UTILITY_FALCON_DASH_NAME", "Falcon Dash");
            LanguageAPI.Add(prefix + "UTILITY_FALCON_DASH_DESCRIPTION", "<style=cIsDamage>X first surrounds his body with immense energy, then performs an invincible flying tackle</style>.</style>");

            LanguageAPI.Add(prefix + "UTILITY_NOVA_DASH_NAME", "Nova Dash");
            LanguageAPI.Add(prefix + "UTILITY_NOVA_DASH_DESCRIPTION", "<style=cIsDamage>X first surrounds his body with immense energy, then performs an invincible flying tackle</style>.</style>");

            LanguageAPI.Add(prefix + "UTILITY_NOVA_STRIKE_NAME", "Nova Strike");
            LanguageAPI.Add(prefix + "UTILITY_NOVA_STRIKE_DESCRIPTION", "<style=cIsDamage>X first surrounds his body with immense energy, then performs an invincible flying tackle</style>.</style>");
            #endregion

            #region Special

            LanguageAPI.Add(prefix + "SPECIAL_HEAD_SCANNER_NAME", "Head-Scanner");
            LanguageAPI.Add(prefix + "SPECIAL_HEAD_SCANNER_DESCRIPTION", "fires a small missile that seeks out enemies, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_HYPER_CHIP_NAME", "Hyper-Chip");
            LanguageAPI.Add(prefix + "SPECIAL_HYPER_CHIP_DESCRIPTION", "fires a small missile that seeks out enemies, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_GIGA_ATTACK_GAEA_NAME", "Giga-Attack: Gaea Armor");
            LanguageAPI.Add(prefix + "SPECIAL_GIGA_ATTACK_GAEA_DESCRIPTION", "fires a small missile that seeks out enemies, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_RATHALOS_SLASH_NAME", "True Rathalos Slash");
            LanguageAPI.Add(prefix + "SPECIAL_RATHALOS_SLASH_DESCRIPTION", "Shoot a small missle tha follow some targets, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_MELTCREEPER_NAME", "Meltcreeper");
            LanguageAPI.Add(prefix + "SPECIAL_MELTCREEPER_DESCRIPTION", "Shoot a small missle tha follow some targets, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_ACID_BURST_NAME", "Acid Burst R");
            LanguageAPI.Add(prefix + "SPECIAL_ACID_BURST_DESCRIPTION", "When fired, it creates a glob of acid which, upon contact with any surface, will create acid crystals, dealing <style=cIsDamage> 125 % base damage </style> and poisoning enemies, When charged, X will fire two balls of acid dealing a little more damage");

            LanguageAPI.Add(prefix + "SPECIAL_CHAMELEON_STING_NAME", "Chameleon Sting");
            LanguageAPI.Add(prefix + "SPECIAL_CHAMELEON_STING_DESCRIPTION", "X fires tree beams in a wide angle, dealing <style=cIsDamage> 160 % base damage </style>. When charged, the Chameleon Sting make X temporarily invulnerable");

            LanguageAPI.Add(prefix + "SPECIAL_HOMMING_TORPEDO_NAME", "Homming Torpedo");
            LanguageAPI.Add(prefix + "SPECIAL_HOMMING_TORPEDO_DESCRIPTION", "Shoot a small missle tha follow some targets, dealing <style=cIsDamage> 145 % base damage </style>.");

            LanguageAPI.Add(prefix + "RISINGFIRER_NAME", "Rising Fire R");
            LanguageAPI.Add(prefix + "RISINGFIRER_DESCRIPTION", "When equipped with this weapon, X raises his arm in the air and shoots firebombs upwards dealing <style=cIsDamage> 200 % base damage </style>.");

            


            #endregion

            #region Achievements
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_NAME", "Henry: Mastery");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_ACHIEVEMENT_DESC", "As Henry, beat the game or obliterate on Monsoon.");
            LanguageAPI.Add(prefix + "MASTERYUNLOCKABLE_UNLOCKABLE_NAME", "Henry: Mastery");
            #endregion


        }

        public static void AddHenryTokens()
        {
            string prefix = XSurvivor.MEGAMAN_x_PREFIX;

            string desc = "X is a skilled fighter who makes use of a wide arsenal of weaponry to take down his foes.<color=#CCD3E0>" + Environment.NewLine + Environment.NewLine
             + "< ! > Sword is a good all-rounder while Boxing Gloves are better for laying a beatdown on more powerful foes." + Environment.NewLine + Environment.NewLine
             + "< ! > Pistol is a powerful anti air, with its low cooldown and high damage." + Environment.NewLine + Environment.NewLine
             + "< ! > Roll has a lingering armor buff that helps to use it aggressively." + Environment.NewLine + Environment.NewLine
             + "< ! > Bomb can be used to wipe crowds with ease." + Environment.NewLine + Environment.NewLine;

            string outro = "..and so he left, searching for a new identity.";
            string outroFailure = "..and so he vanished, forever a blank slate.";

            Language.Add(prefix + "NAME", "X");
            Language.Add(prefix + "DESCRIPTION", desc);
            Language.Add(prefix + "SUBTITLE", "The Chosen One");
            Language.Add(prefix + "LORE", "sample lore");
            Language.Add(prefix + "OUTRO_FLAVOR", outro);
            Language.Add(prefix + "OUTRO_FAILURE", outroFailure);

            #region Skins
            Language.Add(prefix + "MASTERY_SKIN_NAME", "Alternate");
            #endregion

            #region Passive
            Language.Add(prefix + "PASSIVE_NAME", "X passive");
            Language.Add(prefix + "PASSIVE_DESCRIPTION", "Sample text.");
            #endregion

            #region Primary
            Language.Add(prefix + "PRIMARY_SLASH_NAME", "Sword");
            Language.Add(prefix + "PRIMARY_SLASH_DESCRIPTION", Tokens.agilePrefix + $"Swing forward for <style=cIsDamage>{100f * XStaticValues.swordDamageCoefficient}% damage</style>.");
            #endregion

            #region Secondary
            Language.Add(prefix + "SECONDARY_GUN_NAME", "Handgun");
            Language.Add(prefix + "SECONDARY_GUN_DESCRIPTION", Tokens.agilePrefix + $"Fire a handgun for <style=cIsDamage>{100f * XStaticValues.gunDamageCoefficient}% damage</style>.");
            #endregion

            #region Utility
            Language.Add(prefix + "UTILITY_ROLL_NAME", "Roll");
            Language.Add(prefix + "UTILITY_ROLL_DESCRIPTION", "Roll a short distance, gaining <style=cIsUtility>300 armor</style>. <style=cIsUtility>You cannot be hit during the roll.</style>");
            #endregion

            #region Special
            Language.Add(prefix + "SPECIAL_BOMB_NAME", "Bomb");
            Language.Add(prefix + "SPECIAL_BOMB_DESCRIPTION", $"Throw a bomb for <style=cIsDamage>{100f * XStaticValues.bombDamageCoefficient}% damage</style>.");
            #endregion

            #region Achievements
            Language.Add(Tokens.GetAchievementNameToken(HenryMasteryAchievement.identifier), "X: Mastery");
            Language.Add(Tokens.GetAchievementDescriptionToken(HenryMasteryAchievement.identifier), "As X, beat the game or obliterate on Monsoon.");
            #endregion
        }
    }
}
