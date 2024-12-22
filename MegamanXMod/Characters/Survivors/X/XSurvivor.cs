using BepInEx.Configuration;
using MegamanXMod.Modules;
using MegamanXMod.Modules.Characters;
using MegamanXMod.Survivors.X.Components;
using MegamanXMod.Survivors.X.SkillStates;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace MegamanXMod.Survivors.X
{
    public class XSurvivor : SurvivorBase<XSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "megamanxv4bundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "XBody"; //if you do not change this, you get the point by now

        //name of the ai master for vengeance and goobo. must be unique
        public override string masterName => "XMonsterMaster"; //if you do not

        //the names of the prefabs you set up in unity that we will use to build your character
        public override string modelPrefabName => "mdlX4";
        public override string displayPrefabName => "X4Display";

        public const string MEGAMAN_x_PREFIX = MegamanXPlugin.DEVELOPER_PREFIX + "_X_";

        //used when registering your survivor's language tokens
        public override string survivorTokenPrefix => MEGAMAN_x_PREFIX;

        //internal static SkillDef LightHyperModeSkillDef;
        //internal static SkillDef GaeaHyperModeSkillDef;

        //ARMORS
        internal static SkillDef CoolDownXArmorSkillDef;
        internal static SkillDef HyperModeLightArmorSkillDef;
        internal static SkillDef HyperModeSecondArmorSkillDef;
        internal static SkillDef HyperModeMaxArmorSkillDef;
        internal static SkillDef HyperModeFourthArmorSkillDef;
        internal static SkillDef HyperModeFalconArmorSkillDef;
        internal static SkillDef HyperModeGaeaArmorSkillDef;

        //PRIMARY
        internal static SkillDef XBusterSkillDef;
        internal static SkillDef XShadowBusterSkillDef;

        //SECONDARY


        //UTILITY
        internal static SkillDef XDashSkillDef;
        internal static SkillDef XFalconDashSkillDef;

        //SPECIAL
        internal static SkillDef XHeadScannerSkillDef;


        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = MEGAMAN_x_PREFIX + "NAME",
            subtitleNameToken = MEGAMAN_x_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("TexX"),
            bodyColor = Color.white,
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 100f,
            armor = 20f,
            healthGrowth = 25f,
            healthRegen = 1.5f,
            damage = 20f,
            shieldGrowth = 0.25f,
            jumpPowerGrowth = 0.35f,
            jumpCount = 1

        };

        public override CustomRendererInfo[] customRendererInfos => new CustomRendererInfo[]
        {
                new CustomRendererInfo
                {
                    childName = "XBodyMesh",
                    material = XAssets.MatX,
                }
                //new CustomRendererInfo
                //{
                //    childName = "XBusterMesh",
                //    //material = assetBundle.LoadMaterial("matXBuster"),
                //}
        };

        public override UnlockableDef characterUnlockableDef => HenryUnlockables.characterUnlockableDef;
        
        public override ItemDisplaysBase itemDisplays => new HenryItemDisplays();

        //set in base classes
        public override AssetBundle assetBundle { get; protected set; }

        public override GameObject bodyPrefab { get; protected set; }
        public override CharacterBody prefabCharacterBody { get; protected set; }
        public override GameObject characterModelObject { get; protected set; }
        public override CharacterModel prefabCharacterModel { get; protected set; }
        public override GameObject displayPrefab { get; protected set; }

        public override void Initialize()
        {
            //uncomment if you have multiple characters
            //ConfigEntry<bool> characterEnabled = Config.CharacterEnableConfig("Survivors", "X");

            //if (!characterEnabled.Value)
            //    return;

            base.Initialize();
        }

        public override void InitializeCharacter()
        {
            //need the character unlockable before you initialize the survivordef
            HenryUnlockables.Init();

            base.InitializeCharacter();

            HenryConfig.Init();
            XStates.Init();
            XTokens.Init();

            XAssets.Init(assetBundle);
            HenryBuffs.Init(assetBundle);

            InitializeEntityStateMachines();
            InitializeSkills();
            InitializeSkins();
            InitializeCharacterMaster();

            AdditionalBodySetup();

            AddHooks();
        }

        private void AdditionalBodySetup()
        {
            AddHitboxes();
            bodyPrefab.AddComponent<XBaseComponent>();
            bodyPrefab.AddComponent<XArmorComponent>();
            bodyPrefab.AddComponent<XHoverComponent>();
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            //Prefabs.SetupHitBoxGroup(characterModelObject, "SwordGroup", "SwordHitbox");
        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
                //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon");
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2");
        }

        #region skills
        public override void InitializeSkills()
        {
            //remove the genericskills from the commando body we cloned
            Skills.ClearGenericSkills(bodyPrefab);
            Skills.CreateFirstExtraSkillFamily(bodyPrefab);
            Skills.CreateSecondExtraSkillFamily(bodyPrefab);
            Skills.CreateThirdExtraSkillFamily(bodyPrefab);
            Skills.CreateFourthExtraSkillFamily(bodyPrefab);
            //add our own
            CreateSkillDefs();
            //AddPassiveSkill();
            AddPrimarySkills();
            AddSecondarySkills();
            AddUtiitySkills();
            AddSpecialSkills();

            AddExtraFirstSkills();
            AddExtraSecondSkills();
            AddExtraThirdSkills();
            AddExtraFourthSkills();



        }

        private void CreateSkillDefs()
        {

            //ARMORS

            //a basic skill. some fields are omitted and will just have default values
            CoolDownXArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "CoolDownXArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "X_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "X_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconX,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.CooldownXArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            //a basic skill. some fields are omitted and will just have default values
            HyperModeLightArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeLightArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_FIRST_LIGHT_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_FIRST_LIGHT_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconLight,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeLightArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            HyperModeSecondArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeSecondArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_FIRST_SECOND_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_FIRST_SECOND_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconSecond,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeSecondArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            HyperModeMaxArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeMaxArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_SECOND_MAX_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_SECOND_MAX_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconMax,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeMaxArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            HyperModeFourthArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeFourthArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_SECOND_FOURTH_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_SECOND_FOURTH_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconFourth,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeFourthArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            HyperModeFalconArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeFalconArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_THIRD_FALCON_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_THIRD_FALCON_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconFalcon,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeFalconArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            HyperModeGaeaArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeGaeaArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_THIRD_GAEA_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_THIRD_GAEA_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconGaea,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeGaeaArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            //PRIMARY
            XShadowBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ShadowBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_SHADOW_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_SHADOW_BUSTER_DESCRIPTION",
                //skillIcon = XAssets.IconFalconDash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(ShadowBuster)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 0f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });


            //UTILITY
            XFalconDashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "FalconDash",
                skillNameToken = MEGAMAN_x_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = XAssets.IconFalconDash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(FalconDash)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 10f,
                baseMaxStock = 5,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            //SPECIAL
            XHeadScannerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HeadScanner",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_HEAD_SCANNER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_HEAD_SCANNER_DESCRIPTION",
                //skillIcon = XAssets.IconFalconDash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(HeadScanner)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 3f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });





        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = MEGAMAN_x_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PASSIVE_DESCRIPTION",
                keywordToken = "KEYWORD_STUNNING",
                icon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryPassive",
                skillNameToken = MEGAMAN_x_PREFIX + "PASSIVE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PASSIVE_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

                //unless you're somehow activating your passive like a skill, none of the following is needed.
                //but that's just me saying things. the tools are here at your disposal to do whatever you like with

                //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
                //activationStateMachineName = "Weapon1",
                //interruptPriority = EntityStates.InterruptPriority.Skill,

                //baseRechargeInterval = 1f,
                //baseMaxStock = 1,

                //rechargeStock = 1,
                //requiredStock = 1,
                //stockToConsume = 1,

                //resetCooldownTimerOnUse = false,
                //fullRestockOnAssign = true,
                //dontAllowPastMaxStocks = false,
                //mustKeyPress = false,
                //beginSkillCooldownOnSkillEnd = false,

                //isCombatSkill = true,
                //canceledFromSprinting = false,
                //cancelSprintingOnActivation = false,
                //forceSprintDuringState = false,

            });
            Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
        }

        //if this is your first look at skilldef creation, take a look at Secondary first
        private void AddPrimarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Primary);

            //the primary skill is created using a constructor for a typical primary
            //it is also a SteppedSkillDef. Custom Skilldefs are very useful for custom behaviors related to casting a skill. see ror2's different skilldefs for reference
            SteppedSkillDef primarySkillDef1 = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
                (
                    "HenrySlash",
                    MEGAMAN_x_PREFIX + "PRIMARY_SLASH_NAME",
                    MEGAMAN_x_PREFIX + "PRIMARY_SLASH_DESCRIPTION",
                    assetBundle.LoadAsset<Sprite>("texPrimaryIcon"),
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.XBuster)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 2;
            primarySkillDef1.stepGraceDuration = 0.5f;
            primarySkillDef1.mustKeyPress = true;

            Skills.AddPrimarySkills(bodyPrefab, primarySkillDef1);

            Skills.AddPrimarySkills(bodyPrefab, XShadowBusterSkillDef);
        }

        private void AddSecondarySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Secondary);

            //here is a basic skill def with all fields accounted for
            SkillDef secondarySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryGun",
                skillNameToken = MEGAMAN_x_PREFIX + "SECONDARY_GUN_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SECONDARY_GUN_DESCRIPTION",
                keywordTokens = new string[] { "KEYWORD_AGILE" },
                skillIcon = assetBundle.LoadAsset<Sprite>("texSecondaryIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShotgunIce)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,

            });

            Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
        }

        private void AddUtiitySkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Utility);

            //here's a skilldef of a typical movement skill.
            SkillDef utilitySkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryRoll",
                skillNameToken = MEGAMAN_x_PREFIX + "UTILITY_ROLL_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "UTILITY_ROLL_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texUtilityIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(FalconDash)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 1f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = true,
            });

            Skills.AddUtilitySkills(bodyPrefab, utilitySkillDef1);

            Skills.AddUtilitySkills(bodyPrefab, XFalconDashSkillDef);

        }

        private void AddSpecialSkills()
        {
            Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, SkillSlot.Special);

            //a basic skill. some fields are omitted and will just have default values
            SkillDef specialSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryBomb",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.ShadowBuster)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2", interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
            Skills.AddSpecialSkills(bodyPrefab, XHeadScannerSkillDef);
        }
        #endregion skills



        private void AddExtraFirstSkills()
        {

            //a basic skill. some fields are omitted and will just have default values
            //SkillDef FESSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryBomb",
            //    skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_NAME",
            //    skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
            //    //skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeLightArmor)),
            //    //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
            //    activationStateMachineName = "Weapon2",
            //    interruptPriority = EntityStates.InterruptPriority.Skill,

            //    baseMaxStock = 1,
            //    baseRechargeInterval = 1f,

            //    isCombatSkill = true,
            //    mustKeyPress = false,
            //});

            Skills.AddFirstExtraSkill(bodyPrefab, HyperModeLightArmorSkillDef);
            Skills.AddFirstExtraSkill(bodyPrefab, HyperModeSecondArmorSkillDef);
        }

        private void AddExtraSecondSkills()
        {

            //a basic skill. some fields are omitted and will just have default values
            //SkillDef SESSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryBomb",
            //    skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_NAME",
            //    skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
            //    //skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeSecondArmor)),
            //    //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
            //    activationStateMachineName = "Weapon2",
            //    interruptPriority = EntityStates.InterruptPriority.Skill,

            //    baseMaxStock = 1,
            //    baseRechargeInterval = 1f,

            //    isCombatSkill = true,
            //    mustKeyPress = false,
            //});

            Skills.AddSecondExtraSkill(bodyPrefab, HyperModeMaxArmorSkillDef);
            Skills.AddSecondExtraSkill(bodyPrefab, HyperModeFourthArmorSkillDef);
        }

        private void AddExtraThirdSkills()
        {

            //a basic skill. some fields are omitted and will just have default values
            //SkillDef TESSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryBomb",
            //    skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_NAME",
            //    skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

            //    activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeMaxArmor)),
            //    //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
            //    activationStateMachineName = "Weapon2",
            //    interruptPriority = EntityStates.InterruptPriority.Skill,

            //    baseMaxStock = 1,
            //    baseRechargeInterval = 1f,

            //    isCombatSkill = true,
            //    mustKeyPress = false,
            //});

            Skills.AddThirdExtraSkill(bodyPrefab, HyperModeFalconArmorSkillDef);
            Skills.AddThirdExtraSkill(bodyPrefab, HyperModeGaeaArmorSkillDef);
        }

        private void AddExtraFourthSkills()
        {

            //a basic skill. some fields are omitted and will just have default values
            SkillDef FESSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HenryBomb",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_BOMB_DESCRIPTION",
                //skillIcon = assetBundle.LoadAsset<Sprite>("texSpecialIcon"),

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeFourthArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = true,
                mustKeyPress = false,
            });

            Skills.AddFourthExtraSkill(bodyPrefab, FESSkillDef);
        }

        #region skins
        public override void InitializeSkins()
        {
            ModelSkinController skinController = prefabCharacterModel.gameObject.AddComponent<ModelSkinController>();
            ChildLocator childLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            CharacterModel.RendererInfo[] defaultRendererinfos = prefabCharacterModel.baseRendererInfos;

            List<SkinDef> skins = new List<SkinDef>();

            #region DefaultSkin
            //this creates a SkinDef with all default fields
            SkinDef defaultSkin = Skins.CreateSkinDef("DEFAULT_SKIN",
                assetBundle.LoadAsset<Sprite>("texMainSkin"),
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "XBodyMesh");

            //add new skindef to our list of skindefs. this is what we'll be passing to the SkinController
            skins.Add(defaultSkin);
            #endregion

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(MEGAMAN_x_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "XGaeaBodyMeshF",
            //    null);

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matGaea");
            ////masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matGaea");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("XBusterMesh"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            ////skins.Add(masterySkin);


            //uncomment this when you have a mastery skin
            #region MasterySkin

            ////creating a new skindef as we did before
            //SkinDef masterySkin = Modules.Skins.CreateSkinDef(MEGAMAN_x_PREFIX + "MASTERY_SKIN_NAME",
            //    assetBundle.LoadAsset<Sprite>("texMasteryAchievement"),
            //    defaultRendererinfos,
            //    prefabCharacterModel.gameObject,
            //    HenryUnlockables.masterySkinUnlockableDef);

            ////adding the mesh replacements as above. 
            ////if you don't want to replace the mesh (for example, you only want to replace the material), pass in null so the order is preserved
            //masterySkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
            //    "meshHenrySwordAlt",
            //    null,//no gun mesh replacement. use same gun mesh
            //    "meshHenryAlt");

            ////masterySkin has a new set of RendererInfos (based on default rendererinfos)
            ////you can simply access the RendererInfos' materials and set them to the new materials for your skin.
            //masterySkin.rendererInfos[0].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[1].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");
            //masterySkin.rendererInfos[2].defaultMaterial = assetBundle.LoadMaterial("matHenryAlt");

            ////here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            //masterySkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            //{
            //    new SkinDef.GameObjectActivation
            //    {
            //        gameObject = childLocator.FindChildGameObject("GunModel"),
            //        shouldActivate = false,
            //    }
            //};
            ////simply find an object on your child locator you want to activate/deactivate and set if you want to activate/deacitvate it with this skin

            //skins.Add(masterySkin);

            #endregion

            skinController.skins = skins.ToArray();
        }
        #endregion skins

        //Character Master is what governs the AI of your character when it is not controlled by a player (artifact of vengeance, goobo)
        public override void InitializeCharacterMaster()
        {
            //you must only do one of these. adding duplicate masters breaks the game.

            //if you're lazy or prototyping you can simply copy the AI of a different character to be used
            //Modules.Prefabs.CloneDopplegangerMaster(bodyPrefab, masterName, "Merc");

            //how to set up AI in code
            HenryAI.Init(bodyPrefab, masterName);

            //how to load a master set up in unity, can be an empty gameobject with just AISkillDriver components
            //assetBundle.LoadMaster(bodyPrefab, masterName);
        }

        private void AddHooks()
        {
            R2API.RecalculateStatsAPI.GetStatCoefficients += RecalculateStatsAPI_GetStatCoefficients;
        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (sender.HasBuff(HenryBuffs.armorBuff))
            {
                args.armorAdd += 300;
            }
        }
    }
}