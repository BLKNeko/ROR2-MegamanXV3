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
            //AddHenryTokens();
            AddXTokens();

            ////uncomment this to spit out a lanuage file with all the above tokens that people can translate
            ////make sure you set Language.usingLanguageFolder and printingEnabled to true
            //Language.PrintOutput("X.txt");
            ////refer to guide on how to build and distribute your mod with the proper folders
        }

        public static void AddXTokens()
        {
            string prefix = XSurvivor.MEGAMAN_x_PREFIX;

            string desc = "<color=#CCD3E0>Mega Man X, the B-Class Maverick Hunter.</color>\n\n";
            desc += "< ! > The <color=#f79902>X-Buster</color> is a powerful weapon, capable of charging energy to unleash devastating charged shots!\n\n";
            desc += "< ! > X can also charge some of his <color=#2302f7>skills</color>, allowing him to deal increased damage or perform unique actions.</color>\n\n";
            desc += "< ! > The <color=#CCD3E0>Emergency Acceleration System (DASH)</color> temporarily boosts X's speed, making it ideal for repositioning or evading danger.\n\n";
            desc += "< ! > <color=#f79902>X-Heart</color> and <color=#f79902>Limitless Potential</color> grant X the ability to turn the tide of battle, but beware of its high cooldown.\n\n";
            desc += "< ! > X's full potential is unlocked as he levels up, granting access to different armors with unique capabilities.</color>\n\n";
            desc += "< ! > Choose X's armor wisely—each comes with distinct advantages to suit your strategy.</color>\n\n";


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
            LanguageAPI.Add(prefix + "EXTRA_FIRST_LIGHT_ARMOR_DESCRIPTION", "X's first armor. The Light Buster can hit enemies multiple times, and this armor provides a small boost to X's stats.");

            LanguageAPI.Add(prefix + "EXTRA_FIRST_SECOND_ARMOR_NAME", "Giga Armor");
            LanguageAPI.Add(prefix + "EXTRA_FIRST_SECOND_ARMOR_DESCRIPTION", "X's second armor. The Giga Buster can store an additional charge, and the helmet replaces X's special skill with the <color=#f79902>HEAD SCANNER</color>.");

            LanguageAPI.Add(prefix + "EXTRA_SECOND_MAX_ARMOR_NAME", "Max Armor");
            LanguageAPI.Add(prefix + "EXTRA_SECOND_MAX_ARMOR_DESCRIPTION", "X's third armor. The Max Buster fires additional projectiles in a wide range, and this armor replaces X's special skill with the <color=#f79902>HYPER CHIP</color>.");

            LanguageAPI.Add(prefix + "EXTRA_SECOND_FOURTH_ARMOR_NAME", "Force Armor");
            LanguageAPI.Add(prefix + "EXTRA_SECOND_FOURTH_ARMOR_DESCRIPTION", "X's fourth armor. This powerful armor equips the Ultimate Buster, which leaves a shock sphere on enemies, causing damage over time. It also replaces X's DASH with the <color=#f79902>NOVA DASH</color>.");

            LanguageAPI.Add(prefix + "EXTRA_THIRD_FALCON_ARMOR_NAME", "Falcon Armor");
            LanguageAPI.Add(prefix + "EXTRA_THIRD_FALCON_ARMOR_DESCRIPTION", "This armor focuses on mobility, replacing X's DASH with an advanced form that allows him to fly and hover in the air.");

            LanguageAPI.Add(prefix + "EXTRA_THIRD_GAEA_ARMOR_NAME", "Gaea Armor");
            LanguageAPI.Add(prefix + "EXTRA_THIRD_GAEA_ARMOR_DESCRIPTION", "This armor is extremely heavy, providing a massive boost to defense. The Gaea Buster can poison enemies, and X's special skill is replaced with the <color=#012906>GAEA GIGA ATTACK</color>.");

            LanguageAPI.Add(prefix + "EXTRA_FOURTH_SHADOW_ARMOR_NAME", "Shadow Armor");
            LanguageAPI.Add(prefix + "EXTRA_FOURTH_SHADOW_ARMOR_DESCRIPTION", "This armor is both fast and powerful. While equipped, X's secondary skill is replaced with the Shadow Saber, and his Shadow Buster fires three kunai in a spread pattern, causing enemies to bleed.");

            LanguageAPI.Add(prefix + "EXTRA_FOURTH_ULTIMATE_ARMOR_NAME", "Ultimate Armor");
            LanguageAPI.Add(prefix + "EXTRA_FOURTH_ULTIMATE_ARMOR_DESCRIPTION", "One of the most powerful armors. The Ultimate Buster deals increased damage, and X's DASH is replaced with the <color=#f79902>NOVA STRIKE</color>.");

            LanguageAPI.Add(prefix + "EXTRA_FOURTH_RATHALOS_ARMOR_NAME", "Rathalos Armor");
            LanguageAPI.Add(prefix + "EXTRA_FOURTH_RATHALOS_ARMOR_DESCRIPTION", "This ancient armor is based on the powerful Flying Wyvern, Rathalos. The buster is designed to channel the monster's fire breath, while his saber delivers powerful blows that crush enemies.");

            LanguageAPI.Add(prefix + "EXTRA_LOCK_ARMOR_NAME", "Lock Armor");
            LanguageAPI.Add(prefix + "EXTRA_LOCK_ARMOR_DESCRIPTION", "Lock Armor");

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

            LanguageAPI.Add(prefix + "PRIMARY_X_BUSTER_NAME", "X Buster");
            LanguageAPI.Add(prefix + "PRIMARY_X_BUSTER_DESCRIPTION", $"Shoot with X - Buster, dealing <style=cIsDamage>{100f * XStaticValues.XBusterDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_LIGHT_BUSTER_NAME", "Light Buster");
            LanguageAPI.Add(prefix + "PRIMARY_LIGHT_BUSTER_DESCRIPTION", $"The Light Buster enhances X's Charge Shot, making it stronger and followed by six additional small projectiles. Deals <style=cIsDamage>{100f * (XStaticValues.XLightBusterDamageCoefficient)}% damage</style> on the main shot and <style=cIsDamage>{100f * (XStaticValues.XLightBusterDamageCoefficient / XStaticValues.XFullChargeDamageCoefficient)}% damage per projectile (x6)</style>.");


            LanguageAPI.Add(prefix + "PRIMARY_GIGA_BUSTER_NAME", "Giga Buster");
            LanguageAPI.Add(prefix + "PRIMARY_GIGA_BUSTER_DESCRIPTION", $"The Giga Buster can store one charge, allowing for a powerful shot that deals <style=cIsDamage>{100f * XStaticValues.XGigaBusterDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_MAX_BUSTER_NAME", "Max Buster");
            LanguageAPI.Add(prefix + "PRIMARY_MAX_BUSTER_DESCRIPTION", $"The Max Buster enhances X's Charge Shot, making it stronger and followed by eight additional small projectiles in a short spread area. Deals <style=cIsDamage>{100f * (XStaticValues.XMaxBusterDamageCoefficient)}% damage</style> on the main shot and <style=cIsDamage>{100f * (XStaticValues.XMaxBusterDamageCoefficient / XStaticValues.XFullChargeDamageCoefficient)}% damage per projectile (x8)</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_FORCE_BUSTER_NAME", "Force Buster");
            LanguageAPI.Add(prefix + "PRIMARY_FORCE_BUSTER_DESCRIPTION", $"This powerful buster delivers devastating attacks, dealing <style=cIsDamage>{100f * XStaticValues.XForceBusterDamageCoefficient}% damage</style>. On hit, it leaves behind a shock sphere that continuously deals <style=cIsDamage>{100f * XStaticValues.XShockSphereDamageCoefficient}% damage</style> over time.");

            LanguageAPI.Add(prefix + "PRIMARY_FALCON_BUSTER_NAME", "Falcon Buster");
            LanguageAPI.Add(prefix + "PRIMARY_FALCON_BUSTER_DESCRIPTION", $"This buster fires slightly faster but deals less damage compared to others, inflicting <style=cIsDamage>{100f * XStaticValues.XFalconBusterDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_GAEA_BUSTER_NAME", "Gaea Buster");
            LanguageAPI.Add(prefix + "PRIMARY_GAEA_BUSTER_DESCRIPTION", $"The Gaea Buster unleashes powerful attacks that can <color=#044005>poison</color> enemies, dealing <style=cIsDamage>{100f * XStaticValues.XGaeaBusterDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "PRIMARY_SHADOW_BUSTER_NAME", "Shadow Buster");
            LanguageAPI.Add(prefix + "PRIMARY_SHADOW_BUSTER_DESCRIPTION", $"The Shadow Buster is a fast and powerful weapon, firing three kunai in a spread pattern, each dealing <style=cIsDamage>{100f * XStaticValues.ShadowBusterDamageCoefficient}% damage</style> and causing <color=#6b0202>bleeding</color> to enemies.");

            LanguageAPI.Add(prefix + "PRIMARY_ULTIMATE_BUSTER_Name", "Ultimate Buster");
            LanguageAPI.Add(prefix + "PRIMARY_ULTIMATE_BUSTER_DESCRIPTION", $"This ultimate powerful buster delivers devastating attacks, dealing <style=cIsDamage>{100f * XStaticValues.XUltimateBusterDamageCoefficient}% damage</style>. On hit, it leaves behind a shock sphere that continuously deals <style=cIsDamage>{100f * XStaticValues.XUltimateBusterDamageCoefficient}% damage</style> over time.");

            LanguageAPI.Add(prefix + "PRIMARY_RATHALOS_BUSTER_NAME", "Rathalos Buster");
            LanguageAPI.Add(prefix + "PRIMARY_RATHALOS_BUSTER_DESCRIPTION", $"This buster harnesses the powerful flames of the wyvern Rathalos, dealing <style=cIsDamage>{100f * XStaticValues.XRathalosBusterDamageCoefficient}% damage</style> and <color=#a63b05>igniting</color> enemies.");



            #endregion

            #region Secondary

            LanguageAPI.Add(prefix + "SECONDARY_SHADOW_SABER_NAME", "Shadow Saber");
            LanguageAPI.Add(prefix + "SECONDARY_SHADOW_SABER_DESCRIPTION", $"Perform a slash with the Shadow Saber, dealing <style=cIsDamage>{100f * XStaticValues.XSSlashComboDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_RATHALOS_SABER_NAME", "Rathalos Slash");
            LanguageAPI.Add(prefix + "SECONDARY_RATHALOS_SABER_DESCRIPTION", $"Perform a slash with the Rathalos Saber, dealing <style=cIsDamage>{100f * XStaticValues.XRSlashComboDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_ICE_NAME", "ShotgunIce");
            LanguageAPI.Add(prefix + "SECONDARY_SHOTGUN_ICE_DESCRIPTION", $"Shoot an Ice Missile that pierces enemies, dealing <style=cIsDamage>{100f * XStaticValues.XShotgunIceDamageCoefficient}% damage</style> and <style=cIsUtility>freezing</style> them.");

            LanguageAPI.Add(prefix + "SECONDARY_SQUEEZE_BOMB_NAME", "Squeeze Bomb");
            LanguageAPI.Add(prefix + "SECONDARY_SQUEEZE_BOMB_DESCRIPTION", "A gravity-based weapon. Creates localized black holes that hold up enemies.");

            LanguageAPI.Add(prefix + "SECONDARY_FIRE_WAVE_NAME", "Fire Wave");
            LanguageAPI.Add(prefix + "SECONDARY_FIRE_WAVE_DESCRIPTION", $"X releases a constant stream of flames from his buster, dealing <style=cIsDamage> {100f * XStaticValues.XFireWaveDamageCoefficient}% damage</style>. ");

            #endregion

            #region Utility
            LanguageAPI.Add(prefix + "UTILITY_X_DASH_NAME", "Dash");
            LanguageAPI.Add(prefix + "UTILITY_X_DASH_DESCRIPTION", "The <color=#CCD3E0>Emergency Acceleration System (DASH)</color> temporarily boosts X's speed, making it ideal for repositioning or evading danger.");

            LanguageAPI.Add(prefix + "UTILITY_FALCON_DASH_NAME", "Falcon Dash");
            LanguageAPI.Add(prefix + "UTILITY_FALCON_DASH_DESCRIPTION", "The advanced form of the <color=#CCD3E0>Emergency Acceleration System (DASH)</color>, allowing X to use it up to 5 times, with the ability to refresh when X lands on any surface.");

            LanguageAPI.Add(prefix + "UTILITY_NOVA_DASH_NAME", "Nova Dash");
            LanguageAPI.Add(prefix + "UTILITY_NOVA_DASH_DESCRIPTION", "<style=cIsDamage>X first surrounds his body with immense energy, then performs an invincible flying tackle</style>.</style>");

            LanguageAPI.Add(prefix + "UTILITY_NOVA_STRIKE_NAME", "Nova Strike");
            LanguageAPI.Add(prefix + "UTILITY_NOVA_STRIKE_DESCRIPTION", "<style=cIsDamage>X first surrounds his body with immense energy, then performs an invincible flying tackle</style>.</style>");
            #endregion

            #region Special

            LanguageAPI.Add(prefix + "SPECIAL_HEAD_SCANNER_NAME", "Head Scanner");
            LanguageAPI.Add(prefix + "SPECIAL_HEAD_SCANNER_DESCRIPTION", "X uses the scanner on his helmet to reveal the location of important items on the map.");

            LanguageAPI.Add(prefix + "SPECIAL_HYPER_CHIP_NAME", "Hyper Chip");
            LanguageAPI.Add(prefix + "SPECIAL_HYPER_CHIP_DESCRIPTION", "This chip unlocks the true potential of the MAX ARMOR, granting a significant buff to X's combat stats.");

            LanguageAPI.Add(prefix + "SPECIAL_GIGA_ATTACK_GAEA_NAME", "Giga Attack: Gaea Armor");
            LanguageAPI.Add(prefix + "SPECIAL_GIGA_ATTACK_GAEA_DESCRIPTION", $"X concentrates a great amount of energy in his buster, then creates a massive sphere of energy, dealing <style=cIsDamage>{100f * XStaticValues.GigaAttackGaeaDamageCoefficient}% damage</style> over time, while covering the armor and granting immunity during the skill's duration.");

            LanguageAPI.Add(prefix + "SPECIAL_RATHALOS_SLASH_NAME", "True Rathalos Slash");
            LanguageAPI.Add(prefix + "SPECIAL_RATHALOS_SLASH_DESCRIPTION", $"X concentrates the power of the wyvern Rathalos in his sword, unleashing a devastating blow that deals <style=cIsDamage>{100f * XStaticValues.XRathalosSlashCombo1DamageCoefficient}% damage</style> and <color=#a63b05>ignites</color> enemies.");

            LanguageAPI.Add(prefix + "SPECIAL_MELTCREEPER_NAME", "Meltcreeper");
            LanguageAPI.Add(prefix + "SPECIAL_MELTCREEPER_DESCRIPTION", $"X sends a wave of flames on the ground from his current position, dealing <style=cIsDamage> {100f * XStaticValues.MeltCreeperDamageCoefficient}% damage</style>.");

            LanguageAPI.Add(prefix + "SPECIAL_ACID_BURST_NAME", "Acid Burst R");
            LanguageAPI.Add(prefix + "SPECIAL_ACID_BURST_DESCRIPTION", $"When fired, it creates a glob of acid which, upon contact with any surface, will create acid crystals, dealing <style=cIsDamage> {100f * XStaticValues.AcidBurstDamageCoefficient}% damage </style> and poisoning enemies.");

            LanguageAPI.Add(prefix + "SPECIAL_CHAMELEON_STING_NAME", "Chameleon Sting");
            LanguageAPI.Add(prefix + "SPECIAL_CHAMELEON_STING_DESCRIPTION", $"X fires tree beams in a wide angle, dealing <style=cIsDamage> {100f * XStaticValues.ChameleonStingDamageCoefficient}% damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_HOMMING_TORPEDO_NAME", "Homming Torpedo");
            LanguageAPI.Add(prefix + "SPECIAL_HOMMING_TORPEDO_DESCRIPTION", $"Shoot a small missle tha follow some targets, dealing <style=cIsDamage> {100f * XStaticValues.HomingTorpedoDamageCoefficient}% damage </style>.");

            LanguageAPI.Add(prefix + "SPECIAL_RISING_FIRE_NAME", "Rising Fire R");
            LanguageAPI.Add(prefix + "SPECIAL_RISING_FIRE_DESCRIPTION", $"When equipped with this weapon, X raises his arm in the air and shoots firebombs upwards dealing <style=cIsDamage> {100f * XStaticValues.RisingFireDamageCoefficient}% damage </style>.");

            


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
