using BepInEx.Configuration;
using MegamanXV3.Modules.Characters;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MegamanXV3.Modules.Survivors
{
    internal class MyCharacter : SurvivorBase
    {
        public override string bodyName => "MegamanXV3";

        public const string MEGAMAN_PREFIX = MegamanXV3Plugin.DEVELOPER_PREFIX + "_MEGAMANXV3_BODY_";
        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => MEGAMAN_PREFIX;

        public override BodyInfo bodyInfo { get; set; } = new BodyInfo
        {
            bodyName = "MegamanXV3Body",
            bodyNameToken = MegamanXV3Plugin.DEVELOPER_PREFIX + "_MEGAMANXV3_BODY_" + "NAME",
            subtitleNameToken = MegamanXV3Plugin.DEVELOPER_PREFIX + "_MEGAMANXV3_BODY_" + "SUBTITLE",

            characterPortrait = Modules.Assets.XIcon,
            bodyColor = new Color(0.0f, 0.24f, 0.48f),

            crosshair = Modules.Assets.LoadCrosshair("Standard"),
            podPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 100f,
            armor = 20f,
            healthGrowth = 25f,
            healthRegen = 1.5f,
            damage = 20f,
            shieldGrowth = 0.25f,
            jumpPowerGrowth = 0.35f,
            jumpCount = 1,
        };

        public override CustomRendererInfo[] customRendererInfos { get; set; } = new CustomRendererInfo[] 
        {
                //new CustomRendererInfo
                //{
                //    childName = "SwordModel",
                //    material = Materials.CreateHopooMaterial("matHenry"),
                //},
                //new CustomRendererInfo
               // {
                //    childName = "GunModel",
               // },
                //new CustomRendererInfo
               // {
                //    childName = "Model",
                //}


                new CustomRendererInfo
                {
                    childName = "Buster",
                    material = Materials.CreateHopooMaterial("matBuster"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMeshLM",
                    material = Materials.CreateHopooMaterial("matX"),
                },
                new CustomRendererInfo
                {
                    childName = "HandMeshLC",
                    material = Materials.CreateHopooMaterial("matX"),
                },
                new CustomRendererInfo
                {
                    childName = "FaceMeshC",
                    material = Materials.CreateHopooMaterial("matX"),
                },
                new CustomRendererInfo
                {
                    childName = "BodyMeshM",
                    material = Materials.CreateHopooMaterial("matX"),
                },
                new CustomRendererInfo
                {
                    childName = "BodyMeshC",
                    material = Materials.CreateHopooMaterial("matX"),
                }

        };

        public override UnlockableDef characterUnlockableDef => null;

        //public override Type characterMainState => typeof(EntityStates.GenericCharacterMain);

        public override Type characterMainState => typeof(SkillStates.BaseStates.Unlimited);

        public override Type characterDeathState => typeof(SkillStates.BaseStates.DeathState);

        public override ItemDisplaysBase itemDisplays => new HenryItemDisplays();

                                                                          //if you have more than one character, easily create a config to enable/disable them like this
        public override ConfigEntry<bool> characterEnabledConfig => null; //Modules.Config.CharacterEnableConfig(bodyName);

        private static UnlockableDef masterySkinUnlockableDef;

        public override void InitializeCharacter()
        {
            base.InitializeCharacter();
        }

        public override void InitializeUnlockables()
        {
            //uncomment this when you have a mastery skin. when you do, make sure you have an icon too
            //masterySkinUnlockableDef = Modules.Unlockables.AddUnlockable<Modules.Achievements.MasteryAchievement>();
        }

        public override void InitializeHitboxes()
        {
            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("NovaDashBox");
            Modules.Prefabs.SetupHitbox(model, hitboxTransform, "NovaDashBox");
            hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);

            //example of how to create a hitbox
            //Transform hitboxTransform = childLocator.FindChild("SwordHitbox");
            //Modules.Prefabs.SetupHitbox(model, hitboxTransform, "Sword");
        }

        public override void InitializeSkills()
        {
            Modules.Skills.CreateSkillFamilies(bodyPrefab);
            string prefix = MegamanXV3Plugin.DEVELOPER_PREFIX + "_MEGAMANXV3_BODY_";

            Modules.Skills.PassiveSetup(bodyPrefab);

            #region Primary
            //Modules.Skills.AddPrimarySkill(bodyPrefab, Modules.Skills.CreatePrimarySkillDef(new EntityStates.SerializableEntityStateType(typeof(SkillStates.SlashCombo)), "Weapon", prefix + "_HENRY_BODY_PRIMARY_SLASH_NAME", prefix + "_HENRY_BODY_PRIMARY_SLASH_DESCRIPTION", Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texPrimaryIcon"), true));

            SkillDef chargeshootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "CHARGE_SHOT_NAME",
                skillNameToken = prefix + "CHARGE_SHOT_NAME",
                skillDescriptionToken = prefix + "CHARGE_SHOT_DESCRIPTION",
                skillIcon = Modules.Assets.ChargeShotIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.chargeShot)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0.4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddPrimarySkills(bodyPrefab, chargeshootSkillDef);

            //----------------FKBUSTER ---------------------------

            SkillDef FKBusterSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "FK_BUSTER_NAME",
                skillNameToken = prefix + "FK_BUSTER_NAME",
                skillDescriptionToken = prefix + "FK_BUSTER_DESCRIPTION",
                skillIcon = Modules.Assets.FKBusterIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FKBuster)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 0.3f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddPrimarySkills(bodyPrefab, FKBusterSkillDef);

            #endregion

            #region Secondary

            SkillDef shootSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SHOTGUNICE_NAME",
                skillNameToken = prefix + "SHOTGUNICE_NAME",
                skillDescriptionToken = prefix + "SHOTGUNICE_DESCRIPTION",
                skillIcon = Modules.Assets.ShotgunIceIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.shotgunIce)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, shootSkillDef);

            //--------------SQUEEZE BOMB------------------------

            SkillDef squeezeBombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "SQUEEZEBOMB_NAME",
                skillNameToken = prefix + "SQUEEZEBOMB_NAME",
                skillDescriptionToken = prefix + "SQUEEZEBOMB_DESCRIPTION",
                skillIcon = Modules.Assets.SqueezeBombIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.squeezeBomb)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, squeezeBombSkillDef);

            //---------------FIRE WAVE-----------------------

            SkillDef fireWaveSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "FIREWAVE_NAME",
                skillNameToken = prefix + "FIREWAVE_NAME",
                skillDescriptionToken = prefix + "FIREWAVE_DESCRIPTION",
                skillIcon = Modules.Assets.FireWaveIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.FireW)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 1,
                baseRechargeInterval = 8f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,
                //keywordTokens = new string[] { "KEYWORD_AGILE" }
            });

            Modules.Skills.AddSecondarySkills(bodyPrefab, fireWaveSkillDef);

            #endregion

            #region Utility

            SkillDef dashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "DASH_NAME",
                skillNameToken = prefix + "DASH_NAME",
                skillDescriptionToken = prefix + "DASH_DESCRIPTION",
                skillIcon = Modules.Assets.DashIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Dash)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 3f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, dashSkillDef);

            // -------------------------- NOVADASH --------------------------

            SkillDef novaDashSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "NOVADASH_NAME",
                skillNameToken = prefix + "NOVADASH_NAME",
                skillDescriptionToken = prefix + "NOVADASH_DESCRIPTION",
                skillIcon = Modules.Assets.DashIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.novaStrike)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, novaDashSkillDef);

            /*
            SkillDef rollSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_UTILITY_ROLL_NAME",
                skillNameToken = prefix + "_HENRY_BODY_UTILITY_ROLL_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_UTILITY_ROLL_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texUtilityIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Roll)),
                activationStateMachineName = "Body",
                baseMaxStock = 1,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = true,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = false,
                mustKeyPress = false,
                cancelSprintingOnActivation = false,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddUtilitySkills(bodyPrefab, rollSkillDef);
            */
            #endregion

            #region Special

            /*
            SkillDef bombSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "_HENRY_BODY_SPECIAL_BOMB_NAME",
                skillNameToken = prefix + "_HENRY_BODY_SPECIAL_BOMB_NAME",
                skillDescriptionToken = prefix + "_HENRY_BODY_SPECIAL_BOMB_DESCRIPTION",
                skillIcon = Modules.Assets.mainAssetBundle.LoadAsset<Sprite>("texSpecialIcon"),
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ThrowBomb)),
                activationStateMachineName = "Slide",
                baseMaxStock = 1,
                baseRechargeInterval = 10f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = false,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, bombSkillDef);
            */

            SkillDef greenNSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "HOMINGTORPEDO_NAME",
                skillNameToken = prefix + "HOMINGTORPEDO_NAME",
                skillDescriptionToken = prefix + "HOMINGTORPEDO_DESCRIPTION",
                skillIcon = Modules.Assets.HomingTorpedoIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HomingTorpedo)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 5,
                baseRechargeInterval = 2.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, greenNSkillDef);

            //----------------MELT CREEPER----------------------------

            SkillDef meltCreeperSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "RISINGFIRER_NAME",
                skillNameToken = prefix + "RISINGFIRER_NAME",
                skillDescriptionToken = prefix + "RISINGFIRER_DESCRIPTION",
                skillIcon = Modules.Assets.RisingFireIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.meltCreeper)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 4f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, meltCreeperSkillDef);

            //----------------- ACID BURST -----------------------

            SkillDef acidBurstSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "ACIDBURST_NAME",
                skillNameToken = prefix + "ACIDBURST_NAME",
                skillDescriptionToken = prefix + "ACIDBURST_DESCRIPTION",
                skillIcon = Modules.Assets.AcidBurstIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.AcidBurst)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 2,
                baseRechargeInterval = 4.5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, acidBurstSkillDef);

            //----------------- CHAMELEON STING -----------------------

            SkillDef ChameleonStingSkillDef = Modules.Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = prefix + "CHAMELEONSTING_NAME",
                skillNameToken = prefix + "CHAMELEONSTING_NAME",
                skillDescriptionToken = prefix + "CHAMELEONSTING_DESCRIPTION",
                skillIcon = Modules.Assets.ChameleonStingIcon,
                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.chameleonSting)),
                activationStateMachineName = "Weapon",
                baseMaxStock = 3,
                baseRechargeInterval = 5f,
                beginSkillCooldownOnSkillEnd = false,
                canceledFromSprinting = false,
                forceSprintDuringState = false,
                fullRestockOnAssign = true,
                interruptPriority = EntityStates.InterruptPriority.Skill,
                resetCooldownTimerOnUse = false,
                isCombatSkill = true,
                mustKeyPress = true,
                cancelSprintingOnActivation = true,
                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1
            });

            Modules.Skills.AddSpecialSkills(bodyPrefab, ChameleonStingSkillDef);

            #endregion
        }

        public override void InitializeSkins()
        {
            GameObject model = bodyPrefab.GetComponentInChildren<ModelLocator>().modelTransform.gameObject;
            CharacterModel characterModel = model.GetComponent<CharacterModel>();

            ModelSkinController skinController = model.AddComponent<ModelSkinController>();
            ChildLocator childLocator = model.GetComponent<ChildLocator>();

            SkinnedMeshRenderer mainRenderer = characterModel.mainSkinnedMeshRenderer;

            CharacterModel.RendererInfo[] defaultRenderers = characterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            SkinDef defaultSkin = Modules.Skins.CreateSkinDef(MegamanXV3Plugin.DEVELOPER_PREFIX + "_MEGAMANXV3_BODY_" + "DEFAULT_SKIN_NAME",
                Assets.DefaultSkinIcon,
                defaultRenderers,
                mainRenderer,
                model);

            defaultSkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                //place your mesh replacements here
                //unnecessary if you don't have multiple skins
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySword"),
                //    renderer = defaultRenderers[0].renderer
                //},
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryGun"),
                //    renderer = defaultRenderers[1].renderer
                //},
                //new SkinDef.MeshReplacement
                //{
                //    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenry"),
                //    renderer = defaultRenderers[2].renderer
                //}
            };

            skins.Add(defaultSkin);
            #endregion

            //uncomment this when you have a mastery skin
            #region MasterySkin
            /*
            Material masteryMat = Modules.Materials.CreateHopooMaterial("matHenryAlt");
            CharacterModel.RendererInfo[] masteryRendererInfos = SkinRendererInfos(defaultRenderers, new Material[]
            {
                masteryMat,
                masteryMat,
                masteryMat,
                masteryMat
            });

            SkinDef masterySkin = Modules.Skins.CreateSkinDef(HenryPlugin.DEVELOPER_PREFIX + "_HENRY_BODY_MASTERY_SKIN_NAME",
                Assets.mainAssetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
                masteryRendererInfos,
                mainRenderer,
                model,
                masterySkinUnlockableDef);

            masterySkin.meshReplacements = new SkinDef.MeshReplacement[]
            {
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenrySwordAlt"),
                    renderer = defaultRenderers[0].renderer
                },
                new SkinDef.MeshReplacement
                {
                    mesh = Modules.Assets.mainAssetBundle.LoadAsset<Mesh>("meshHenryAlt"),
                    renderer = defaultRenderers[2].renderer
                }
            };

            skins.Add(masterySkin);
            */
            #endregion

            skinController.skins = skins.ToArray();
        }
    }
}