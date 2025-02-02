using BepInEx.Configuration;
using MegamanXMod.Modules;
using MegamanXMod.Modules.Characters;
using MegamanXMod.Survivors.X.Components;
using MegamanXMod.Survivors.X.SkillStates;
using RoR2;
using R2API;
using RoR2.Projectile;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.ParticleSystem.PlaybackState;
using MegamanXMod.Modules.BaseContent.BaseStates;
using EmotesAPI;
using System.Security.Cryptography;
using System.Collections;
using R2API.Utils;

namespace MegamanXMod.Survivors.X
{
    public class XSurvivor : SurvivorBase<XSurvivor>
    {
        //used to load the assetbundle for this character. must be unique
        public override string assetBundleName => "megamanxv4bundle"; //if you do not change this, you are giving permission to deprecate the mod

        //the name of the prefab we will create. conventionally ending in "Body". must be unique
        public override string bodyName => "MegamanXBody"; //if you do not change this, you get the point by now

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

        private float xTakeDamageValue = 0f;
        private CharacterMaster xMaster;

        //ARMORS
        internal static SkillDef CoolDownXArmorSkillDef;
        internal static SkillDef HyperModeLightArmorSkillDef;
        internal static SkillDef HyperModeSecondArmorSkillDef;
        internal static SkillDef HyperModeMaxArmorSkillDef;
        internal static SkillDef HyperModeFourthArmorSkillDef;
        internal static SkillDef HyperModeFalconArmorSkillDef;
        internal static SkillDef HyperModeGaeaArmorSkillDef;
        internal static SkillDef HyperModeShadowArmorSkillDef;
        internal static SkillDef HyperModeUltimateArmorSkillDef;
        internal static SkillDef HyperModeRathalosArmorSkillDef;
        internal static SkillDef LockArmorSkillDef;

        //PRIMARY
        internal static SkillDef XBusterSkillDef;
        internal static SkillDef XLightBusterSkillDef;
        internal static SkillDef XGigaBusterSkillDef;
        internal static SkillDef XMaxBusterSkillDef;
        internal static SkillDef XForceBusterSkillDef;
        internal static SkillDef XFalconBusterSkillDef;
        internal static SkillDef XGaeaBusterSkillDef;
        internal static SkillDef XShadowBusterSkillDef;
        internal static SkillDef XUltimateBusterSkillDef;
        internal static SkillDef XRathalosBusterSkillDef;

        //SECONDARY
        internal static SteppedSkillDef XShadowSaberSkillDef;
        internal static SteppedSkillDef XRathalosSaberSkillDef;
        internal static SkillDef XShotgunIceSkillDef;
        internal static SkillDef XSqueezeBombSkillDef;
        internal static SkillDef XFireWaveSkillDef;

        //UTILITY
        internal static SkillDef XDashSkillDef;
        internal static SkillDef XFalconDashSkillDef;
        internal static SkillDef XNovaDashSkillDef;
        internal static SkillDef XNovaStrikeSkillDef;

        //SPECIAL
        internal static SkillDef XHeadScannerSkillDef;
        internal static SkillDef XHyperChipSkillDef;
        internal static SkillDef XGaeaGigaAttackSkillDef;
        internal static SkillDef XRathalosSlashSkillDef;
        internal static SkillDef XRisingFireSkillDef;
        internal static SkillDef XAcidBurstSkillDef;
        internal static SkillDef XChameleonStingSkillDef;
        internal static SkillDef XMeltCreeperSkillDef;

        internal static HuntTrackerSkillDef HomingTorpedoSkillDef;

        internal bool setupEmoteSkeleton = false;


        public override BodyInfo bodyInfo => new BodyInfo
        {
            bodyName = bodyName,
            bodyNameToken = MEGAMAN_x_PREFIX + "NAME",
            subtitleNameToken = MEGAMAN_x_PREFIX + "SUBTITLE",

            characterPortrait = assetBundle.LoadAsset<Texture>("TexX"),
            bodyColor = new Color(0.0f, 0.24f, 0.48f),
            sortPosition = 100,

            crosshair = Asset.LoadCrosshair("Standard"),
            podPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/NetworkedObjects/SurvivorPod"),

            maxHealth = 100f,
            armor = 20f,
            armorGrowth = 1f,
            healthGrowth = 25f,
            healthRegen = 1.5f,
            regenGrowth = 0.2f,
            damage = 10f,
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
                },
                new CustomRendererInfo
                {
                    childName = "XShadowSaber",
                    //material = XAssets.MatX,
                },
                new CustomRendererInfo
                {
                    childName = "XRathalosSaber",
                    //material = XAssets.MatX,
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

            XConfig.Init();
            XStates.Init();
            XTokens.Init();

            XAssets.Init(assetBundle);
            XBuffs.Init(assetBundle);

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

            ChildLocator cLocator = prefabCharacterModel.GetComponent<ChildLocator>();

            bodyPrefab.AddComponent<XBaseComponent>();
            bodyPrefab.AddComponent<XArmorComponent>();
            bodyPrefab.AddComponent<XHoverComponent>();

            bodyPrefab.AddComponent<HuntressTracker>();
            HuntressTracker huntressTracker = bodyPrefab.GetComponent<HuntressTracker>();
            if (huntressTracker)
            {
                huntressTracker.maxTrackingDistance = 500f;
                huntressTracker.maxTrackingAngle = 4f;
            }
            //bodyPrefab.AddComponent<HuntressTrackerComopnent>();
            //anything else here
        }

        public void AddHitboxes()
        {
            //example of how to create a HitBoxGroup. see summary for more details
            //Prefabs.SetupHitBoxGroup(characterModelObject, "ShadowSaberGroup", "ShadowSaberHitBox");

            ChildLocator childLocator = bodyPrefab.GetComponentInChildren<ChildLocator>();
            GameObject model = childLocator.gameObject;

            Transform hitboxTransform = childLocator.FindChild("ShadowSaberHitBox");
            Prefabs.SetupHitBoxGroup(model, "ShadowSaberGroup", "ShadowSaberHitBox");
            //hitboxTransform.localScale = new Vector3(5.2f, 5.2f, 5.2f);
            hitboxTransform.localScale = new Vector3(6f, 6f, 6f);

            Transform hitboxTransform2 = childLocator.FindChild("BodyDashHitbox");
            Prefabs.SetupHitBoxGroup(characterModelObject, "BodyDashHitbox", "BodyDashHitbox");
            hitboxTransform2.localScale = new Vector3(6f, 6f, 6f);

        }

        public override void InitializeEntityStateMachines() 
        {
            //clear existing state machines from your cloned body (probably commando)
            //omit all this if you want to just keep theirs
            Prefabs.ClearEntityStateMachines(bodyPrefab);

            //the main "Body" state machine has some special properties
            //Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(EntityStates.GenericCharacterMain), typeof(EntityStates.SpawnTeleporterState));
            //if you set up a custom main characterstate, set it up here
            //don't forget to register custom entitystates in your HenryStates.cs

            Prefabs.AddMainEntityStateMachine(bodyPrefab, "Body", typeof(XHeart), typeof(EntityStates.GenericCharacterMain));
            bodyPrefab.GetComponent<CharacterDeathBehavior>().deathState = new EntityStates.SerializableEntityStateType(typeof(DeathState));
            bodyPrefab.GetComponent<EntityStateMachine>().initialStateType = new EntityStates.SerializableEntityStateType(typeof(SpawnState));

            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon", typeof(XHeart), typeof(EntityStates.GenericCharacterMain));
            Prefabs.AddEntityStateMachine(bodyPrefab, "Weapon2", typeof(XHeart), typeof(EntityStates.GenericCharacterMain));
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
            AddPassiveSkill();
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
            #region XArmors

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

                isCombatSkill = false,
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
                baseRechargeInterval = 2f,

                isCombatSkill = false,
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
                baseRechargeInterval = 2f,

                isCombatSkill = false,
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
                baseRechargeInterval = 3f,

                isCombatSkill = false,
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
                baseRechargeInterval = 3f,

                isCombatSkill = false,
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
                baseRechargeInterval = 4f,

                isCombatSkill = false,
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
                baseRechargeInterval = 4f,

                isCombatSkill = false,
                mustKeyPress = false,
            });

            HyperModeShadowArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeShadowArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_FOURTH_SHADOW_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_FOURTH_SHADOW_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconXShadow,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeShadowArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 5f,

                isCombatSkill = false,
                mustKeyPress = false,
            });

            HyperModeUltimateArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeUltimateArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_FOURTH_ULTIMATE_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_FOURTH_ULTIMATE_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconUltimate,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeUltimateArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 5f,

                isCombatSkill = false,
                mustKeyPress = false,
            });

            HyperModeRathalosArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperModeRathalosArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_FOURTH_RATHALOS_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_FOURTH_RATHALOS_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconXRathalos,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.HyperModeRathalosArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 5f,

                isCombatSkill = false,
                mustKeyPress = false,
            });

            LockArmorSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "LockArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "EXTRA_LOCK_ARMOR_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "EXTRA_LOCK_ARMOR_DESCRIPTION",
                skillIcon = XAssets.IconSkillLock,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.LockArmor)),
                //setting this to the "weapon2" EntityStateMachine allows us to cast this skill at the same time primary, which is set to the "weapon" EntityStateMachine
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseMaxStock = 1,
                baseRechargeInterval = 1f,

                isCombatSkill = false,
                mustKeyPress = false,
            });
            #endregion

            //PRIMARY
            #region XPrimary
            XBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "XBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_X_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_X_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconXBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XLightBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "LightBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_LIGHT_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_LIGHT_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconLightBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XLightBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XGigaBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GigaBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_GIGA_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_GIGA_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconGigaBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XGigaBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XMaxBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MaxBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_MAX_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_MAX_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconMaxBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XMaxBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XForceBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ForceBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_FORCE_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_FORCE_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconUltimateBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XForceBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XFalconBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "FalconBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_FALCON_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_FALCON_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconFalconBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XFalconBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XGaeaBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GaeaBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_GAEA_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_GAEA_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconGaeaBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XGaeaBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XShadowBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ShadowBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_SHADOW_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_SHADOW_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconShadowBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

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

            XUltimateBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "UltimateBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_ULTIMATE_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_ULTIMATE_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconUltimateBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XUltimateBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XRathalosBusterSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "RathalosBuster",
                skillNameToken = MEGAMAN_x_PREFIX + "PRIMARY_RATHALOS_BUSTER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "PRIMARY_RATHALOS_BUSTER_DESCRIPTION",
                skillIcon = XAssets.IconRathalosBuster,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XRathalosBuster)),
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
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });
            #endregion

            //SECONDARY
            #region XSecondary
            XShadowSaberSkillDef = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            {
                skillName = "ShadowSaber",
                skillNameToken = MEGAMAN_x_PREFIX + "SECONDARY_SHADOW_SABER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SECONDARY_SHADOW_SABER_DESCRIPTION",
                skillIcon = XAssets.IconShadowSaber,

                activationState = new EntityStates.SerializableEntityStateType(typeof(XSSlashCombo)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

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
            XShadowSaberSkillDef.stepCount = 2;
            XShadowSaberSkillDef.stepGraceDuration = 0.5f;
            //XShadowSaberSkillDef.mustKeyPress = false;

            XRathalosSaberSkillDef = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            {
                skillName = "RathalosSaber",
                skillNameToken = MEGAMAN_x_PREFIX + "SECONDARY_RATHALOS_SABER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SECONDARY_RATHALOS_SABER_DESCRIPTION",
                skillIcon = XAssets.IconRathalosSlash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(XRSlashCombo)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

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
            XRathalosSaberSkillDef.stepCount = 2;
            XRathalosSaberSkillDef.stepGraceDuration = 0.5f;

            XShotgunIceSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ShotgunIce",
                skillNameToken = MEGAMAN_x_PREFIX + "SECONDARY_SHOTGUN_ICE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SECONDARY_SHOTGUN_ICE_DESCRIPTION",
                skillIcon = XAssets.IconShotgunIce,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XShotgunIce)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XFireWaveSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "FireWave",
                skillNameToken = MEGAMAN_x_PREFIX + "SECONDARY_FIRE_WAVE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SECONDARY_FIRE_WAVE_DESCRIPTION",
                skillIcon = XAssets.IconFireWave,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XFireWave)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XSqueezeBombSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "SqueezeBomb",
                skillNameToken = MEGAMAN_x_PREFIX + "SECONDARY_SQUEEZE_BOMB_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SECONDARY_SQUEEZE_BOMB_DESCRIPTION",
                skillIcon = XAssets.IconSqueezeBomb,

                activationState = new EntityStates.SerializableEntityStateType(typeof(SqueezeBomb)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
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
            #endregion

            //UTILITY
            #region XUtility

            XDashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "XDash",
                skillNameToken = MEGAMAN_x_PREFIX + "UTILITY_X_DASH_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "UTILITY_X_DASH_DESCRIPTION",
                skillIcon = XAssets.IconXDash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(XDash2)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 5f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = true,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XFalconDashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "FalconDash",
                skillNameToken = MEGAMAN_x_PREFIX + "UTILITY_FALCON_DASH_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "UTILITY_FALCON_DASH_DESCRIPTION",
                skillIcon = XAssets.IconFalconDash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(FalconDash)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 10f,
                baseMaxStock = 5,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XNovaDashSkillDef = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            {
                skillName = "NovaDash",
                skillNameToken = MEGAMAN_x_PREFIX + "UTILITY_NOVA_DASH_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "UTILITY_NOVA_DASH_DESCRIPTION",
                skillIcon = XAssets.IconXNovaDash,

                activationState = new EntityStates.SerializableEntityStateType(typeof(NovaDash)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 8f,
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

            XNovaStrikeSkillDef = Skills.CreateSkillDef<SteppedSkillDef>(new SkillDefInfo
            {
                skillName = "NovaStrike",
                skillNameToken = MEGAMAN_x_PREFIX + "UTILITY_NOVA_STRIKE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "UTILITY_NOVA_STRIKE_DESCRIPTION",
                skillIcon = XAssets.IconXNovaStrike,

                activationState = new EntityStates.SerializableEntityStateType(typeof(NovaStrike)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 6f,
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
            #endregion

            //SPECIAL
            #region XSpecial
            XHeadScannerSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HeadScanner",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_HEAD_SCANNER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_HEAD_SCANNER_DESCRIPTION",
                skillIcon = XAssets.IconHeadScanner,

                activationState = new EntityStates.SerializableEntityStateType(typeof(HeadScanner)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 60f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = false,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XHyperChipSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "HyperChip",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_HYPER_CHIP_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_HYPER_CHIP_DESCRIPTION",
                skillIcon = XAssets.IconHyperChip,

                activationState = new EntityStates.SerializableEntityStateType(typeof(HyperChip)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 30f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = false,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XGaeaGigaAttackSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "GigaAttackGaeaArmor",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_GIGA_ATTACK_GAEA_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_GIGA_ATTACK_GAEA_DESCRIPTION",
                skillIcon = XAssets.IconGigaAttackGaea,

                activationState = new EntityStates.SerializableEntityStateType(typeof(GigaAttackGaea)),
                activationStateMachineName = "Body",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 40f,
                baseMaxStock = 1,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = false,
                dontAllowPastMaxStocks = false,
                mustKeyPress = false,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = true,
                forceSprintDuringState = false,
            });

            XRathalosSlashSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "XRathalosSlash",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_RATHALOS_SLASH_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_RATHALOS_SLASH_DESCRIPTION",
                skillIcon = XAssets.IconTrueRathalosSlash,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(XRathalosSlashCombo1)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.PrioritySkill,

                baseRechargeInterval = 8f,
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

            XMeltCreeperSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "MeltCreeper",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_MELTCREEPER_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_MELTCREEPER_DESCRIPTION",
                skillIcon = XAssets.IconMeltCreeper,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(MeltCreeper)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 8f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XAcidBurstSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "AcidBurst",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_ACID_BURST_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_ACID_BURST_DESCRIPTION",
                skillIcon = XAssets.IconAcidBurst,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(AcidBurst)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 2,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XChameleonStingSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "ChameleonSting",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_CHAMELEON_STING_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_CHAMELEON_STING_DESCRIPTION",
                skillIcon = XAssets.IconChameleonSting,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(ChameleonSting)),
                activationStateMachineName = "Weapon2",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 5f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            XRisingFireSkillDef = Skills.CreateSkillDef(new SkillDefInfo
            {
                skillName = "RisingFire",
                skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_RISING_FIRE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_RISING_FIRE_DESCRIPTION",
                skillIcon = XAssets.IconRisingFire,
                keywordTokens = new string[]
                {
                    MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"
                },

                activationState = new EntityStates.SerializableEntityStateType(typeof(RisingFire)),
                activationStateMachineName = "Weapon",
                interruptPriority = EntityStates.InterruptPriority.Skill,

                baseRechargeInterval = 10f,
                baseMaxStock = 3,

                rechargeStock = 1,
                requiredStock = 1,
                stockToConsume = 1,

                resetCooldownTimerOnUse = false,
                fullRestockOnAssign = true,
                dontAllowPastMaxStocks = false,
                mustKeyPress = true,
                beginSkillCooldownOnSkillEnd = true,

                isCombatSkill = true,
                canceledFromSprinting = false,
                cancelSprintingOnActivation = false,
                forceSprintDuringState = false,
            });

            HomingTorpedoSkillDef = ScriptableObject.CreateInstance<HuntTrackerSkillDef>();
            HomingTorpedoSkillDef.skillName = MEGAMAN_x_PREFIX + "SPECIAL_HOMMING_TORPEDO_NAME";
            HomingTorpedoSkillDef.skillNameToken = MEGAMAN_x_PREFIX + "SPECIAL_HOMMING_TORPEDO_NAME";
            HomingTorpedoSkillDef.skillDescriptionToken = MEGAMAN_x_PREFIX + "SPECIAL_HOMMING_TORPEDO_DESCRIPTION";
            HomingTorpedoSkillDef.icon = XAssets.IconHomingTorpedo;
            HomingTorpedoSkillDef.keywordTokens = new string[] {MEGAMAN_x_PREFIX + "X_KEYWORD_CHARGE"};
            HomingTorpedoSkillDef.activationState = new EntityStates.SerializableEntityStateType(typeof(HomingTorpedo));
            HomingTorpedoSkillDef.activationStateMachineName = "Weapon";
            HomingTorpedoSkillDef.baseMaxStock = 10;
            HomingTorpedoSkillDef.baseRechargeInterval = 3f;
            HomingTorpedoSkillDef.beginSkillCooldownOnSkillEnd = false;
            HomingTorpedoSkillDef.canceledFromSprinting = false;
            HomingTorpedoSkillDef.forceSprintDuringState = false;
            HomingTorpedoSkillDef.fullRestockOnAssign = true;
            HomingTorpedoSkillDef.interruptPriority = EntityStates.InterruptPriority.Skill;
            HomingTorpedoSkillDef.resetCooldownTimerOnUse = false;
            HomingTorpedoSkillDef.isCombatSkill = true;
            HomingTorpedoSkillDef.mustKeyPress = true;
            HomingTorpedoSkillDef.cancelSprintingOnActivation = false;
            HomingTorpedoSkillDef.rechargeStock = 1;
            HomingTorpedoSkillDef.requiredStock = 1;
            HomingTorpedoSkillDef.stockToConsume = 1;
            ((ScriptableObject)HomingTorpedoSkillDef).name = "HomingTorpedo";

            #endregion



        }

        //skip if you don't have a passive
        //also skip if this is your first look at skills
        private void AddPassiveSkill()
        {
            //option 1. fake passive icon just to describe functionality we will implement elsewhere
            bodyPrefab.GetComponent<SkillLocator>().passiveSkill = new SkillLocator.PassiveSkill
            {
                enabled = true,
                skillNameToken = MEGAMAN_x_PREFIX + "X_PASSIVE_NAME",
                skillDescriptionToken = MEGAMAN_x_PREFIX + "X_PASSIVE_DESCRIPTION",
                icon = XAssets.IconXPassive,
            };

            //option 2. a new SkillFamily for a passive, used if you want multiple selectable passives
            //GenericSkill passiveGenericSkill = Skills.CreateGenericSkillWithSkillFamily(bodyPrefab, "PassiveSkill");
            //SkillDef passiveSkillDef1 = Skills.CreateSkillDef(new SkillDefInfo
            //{
            //    skillName = "HenryPassive",
            //    skillNameToken = MEGAMAN_x_PREFIX + "PASSIVE_NAME",
            //    skillDescriptionToken = MEGAMAN_x_PREFIX + "PASSIVE_DESCRIPTION",
            //    keywordTokens = new string[] { "KEYWORD_AGILE" },
            //    skillIcon = assetBundle.LoadAsset<Sprite>("texPassiveIcon"),

            //    //unless you're somehow activating your passive like a skill, none of the following is needed.
            //    //but that's just me saying things. the tools are here at your disposal to do whatever you like with

            //    //activationState = new EntityStates.SerializableEntityStateType(typeof(SkillStates.Shoot)),
            //    //activationStateMachineName = "Weapon1",
            //    //interruptPriority = EntityStates.InterruptPriority.Skill,

            //    //baseRechargeInterval = 1f,
            //    //baseMaxStock = 1,

            //    //rechargeStock = 1,
            //    //requiredStock = 1,
            //    //stockToConsume = 1,

            //    //resetCooldownTimerOnUse = false,
            //    //fullRestockOnAssign = true,
            //    //dontAllowPastMaxStocks = false,
            //    //mustKeyPress = false,
            //    //beginSkillCooldownOnSkillEnd = false,

            //    //isCombatSkill = true,
            //    //canceledFromSprinting = false,
            //    //cancelSprintingOnActivation = false,
            //    //forceSprintDuringState = false,

            //});
            //Skills.AddSkillsToFamily(passiveGenericSkill.skillFamily, passiveSkillDef1);
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
                    new EntityStates.SerializableEntityStateType(typeof(SkillStates.XSSlashCombo)),
                    "Weapon",
                    true
                ));
            //custom Skilldefs can have additional fields that you can set manually
            primarySkillDef1.stepCount = 2;
            primarySkillDef1.stepGraceDuration = 0.5f;
            primarySkillDef1.mustKeyPress = false;
            

            //Skills.AddPrimarySkills(bodyPrefab, XAcidBurstSkillDef);

            Skills.AddPrimarySkills(bodyPrefab, XBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XLightBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XGigaBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XMaxBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XForceBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XFalconBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XGaeaBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XShadowBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XUltimateBusterSkillDef);
            //Skills.AddPrimarySkills(bodyPrefab, XRathalosBusterSkillDef);
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

                activationState = new EntityStates.SerializableEntityStateType(typeof(XBuster)),
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

            //Skills.AddSecondarySkills(bodyPrefab, secondarySkillDef1);
            //Skills.AddSecondarySkills(bodyPrefab, XShadowSaberSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, XShotgunIceSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, XFireWaveSkillDef);
            Skills.AddSecondarySkills(bodyPrefab, XSqueezeBombSkillDef);
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

            Skills.AddUtilitySkills(bodyPrefab, XDashSkillDef);

            //Skills.AddUtilitySkills(bodyPrefab, XFalconDashSkillDef);
            //Skills.AddUtilitySkills(bodyPrefab, XNovaDashSkillDef);
            //Skills.AddUtilitySkills(bodyPrefab, XNovaStrikeSkillDef);

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

            //Skills.AddSpecialSkills(bodyPrefab, specialSkillDef1);
            //Skills.AddSpecialSkills(bodyPrefab, XHeadScannerSkillDef);
            //Skills.AddSpecialSkills(bodyPrefab, XHyperChipSkillDef);
            //Skills.AddSpecialSkills(bodyPrefab, XGaeaGigaAttackSkillDef);
            //Skills.AddSpecialSkills(bodyPrefab, XRathalosSlashSkillDef);

            Skills.AddSpecialSkills(bodyPrefab, HomingTorpedoSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, XMeltCreeperSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, XAcidBurstSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, XChameleonStingSkillDef);
            Skills.AddSpecialSkills(bodyPrefab, XRisingFireSkillDef);
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

            //Skills.AddFourthExtraSkill(bodyPrefab, FESSkillDef);
            Skills.AddFourthExtraSkill(bodyPrefab, HyperModeShadowArmorSkillDef);
            Skills.AddFourthExtraSkill(bodyPrefab, HyperModeUltimateArmorSkillDef);
            Skills.AddFourthExtraSkill(bodyPrefab, HyperModeRathalosArmorSkillDef);
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
                XAssets.IconX,
                defaultRendererinfos,
                prefabCharacterModel.gameObject);

            //these are your Mesh Replacements. The order here is based on your CustomRendererInfos from earlier
            //pass in meshes as they are named in your assetbundle
            //currently not needed as with only 1 skin they will simply take the default meshes
            //uncomment this when you have another skin
            defaultSkin.meshReplacements = Modules.Skins.getMeshReplacements(assetBundle, defaultRendererinfos,
                "XBodyMesh",
                null,
                null);

            //here's a barebones example of using gameobjectactivations that could probably be streamlined or rewritten entirely, truthfully, but it works
            defaultSkin.gameObjectActivations = new SkinDef.GameObjectActivation[]
            {
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("XShadowSaber"),
                    shouldActivate = false,
                },
                new SkinDef.GameObjectActivation
                {
                    gameObject = childLocator.FindChildGameObject("XRathalosSaber"),
                    shouldActivate = false,
                }
            };

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
            On.RoR2.GlobalEventManager.OnHitEnemy += GlobalEventManager_OnHitEnemy;
            On.RoR2.CharacterModel.Awake += CharacterModel_Awake;
            On.RoR2.SurvivorCatalog.Init += SurvivorCatalog_Init;
            CustomEmotesAPI.animChanged += CustomEmotesAPI_animChanged;
            On.RoR2.CharacterMaster.OnBodyStart += RestoreHPAfterRespawn;
        }

        private void RestoreHPAfterRespawn(On.RoR2.CharacterMaster.orig_OnBodyStart orig, CharacterMaster self, CharacterBody newBody)
        {
            orig(self, newBody);

            //Debug.Log("xTakeDamageValue: " + xTakeDamageValue);
            //Debug.Log("xMaster: " + xMaster);
            //Debug.Log("self: " + self);

            if (self == xMaster) // Certifica-se de que estamos restaurando o HP do personagem correto
            {
                float restoredHP = xTakeDamageValue;

                if (newBody && newBody.healthComponent)
                {
                    newBody.healthComponent.health = Mathf.Clamp(restoredHP, 1f, newBody.healthComponent.fullHealth);
                    //Debug.Log($"HP restaurado para {newBody.healthComponent.health}");
                }
            }
        }

        private void CustomEmotesAPI_animChanged(string newAnimation, BoneMapper mapper)
        {
            //Debug.Log("newAnimation: " + newAnimation);
            //Debug.Log("mapper: " + mapper);
            //Debug.Log("mapper.bodyPrefab.name: " + mapper.bodyPrefab.name);

            if (mapper.bodyPrefab.name.Contains("MegamanXBody"))
            {
                if (newAnimation == "none")
                {
                    if (mapper.bodyPrefab.GetComponent<CharacterBody>())
                    {

                        //Debug.Log("ANim changed to NONE");

                        //NA MORAL VOU DEIXAR ISSO TUDO COMENTADO PELO ÓDIO QUE EU SENTI!

                        float savedHP = mapper.bodyPrefab.GetComponent<CharacterBody>().healthComponent.health;

                        xTakeDamageValue = savedHP;
                        xMaster = mapper.bodyPrefab.GetComponent<CharacterBody>().master;


                        //Debug.Log("xTakeDamageValue: " + xTakeDamageValue);

                        // Mata o personagem atual (sem contar como "morte real")
                        GameObject.Destroy(mapper.bodyPrefab.GetComponent<CharacterBody>().gameObject);

                        // Força o CharacterMaster a reaparecer o personagem
                        mapper.bodyPrefab.GetComponent<CharacterBody>().master.Respawn(mapper.bodyPrefab.GetComponent<CharacterBody>().footPosition, Quaternion.identity);

                        //mapper.bodyPrefab.GetComponent<CharacterBody>().healthComponent.health = xTakeDamageValue;

                        //mapper.bodyPrefab.GetComponent<CharacterBody>().healthComponent.health = XH;

                        //Animator animator = mapper.bodyPrefab.GetComponent<CharacterBody>().characterDirection.modelAnimator;
                        //if (animator)
                        //{
                        //    animator.Rebind(); // Força o reset do Animator
                        //    animator.Update(0f); // Atualiza a nova posição
                        //}
                        //mapper.a1.Rebind();
                        //mapper.a1.Update(0f);
                        //mapper.a2.Rebind();
                        //mapper.a2.Update(0f);


                        //// Ajustamos a posição para cima caso ele tenha afundado no chão
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().characterMotor.Motor.SetPosition(bodyPrefab.GetComponent<CharacterBody>().transform.position + Vector3.up * 0.5f, true);

                        //// Resetamos a velocidade para evitar que ele deslize
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().characterMotor.velocity = Vector3.zero;

                        //mapper.bodyPrefab.GetComponent<CharacterBody>().GetComponent<ModelLocator>().modelTransform.localPosition = Vector3.zero;
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().mainHurtBox.transform.localPosition = Vector3.zero;

                        //mapper.bodyPrefab.GetComponent<CharacterBody>().SetBodyStateToPreferredInitialState();

                        //// Tenta forçar a ressincronização do modelo
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().modelLocator.modelTransform.localPosition = Vector3.zero;
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().modelLocator.modelTransform.localRotation = Quaternion.identity;

                        //// Ajusta a posição do personagem
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().characterMotor.Motor.SetPosition(bodyPrefab.GetComponent<CharacterBody>().footPosition, true);
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().characterMotor.velocity = Vector3.zero;

                        //// Força a atualização do CharacterBody
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().RecalculateStats();
                        //mapper.bodyPrefab.GetComponent<CharacterBody>().mainHurtBox.transform.position = bodyPrefab.GetComponent<CharacterBody>().mainHurtBox.transform.position;


                    }
                }
            }
        }

        

        private void SurvivorCatalog_Init(On.RoR2.SurvivorCatalog.orig_Init orig)
        {
            orig();
            if (!setupEmoteSkeleton)
            {
                setupEmoteSkeleton = true;
                foreach (var item in SurvivorCatalog.allSurvivorDefs)
                {
                    if (item.bodyPrefab.name == "MegamanXBody")
                    {
                        var skele = XAssets.XEmotePrefab;
                        //Debug.Log("Before Emote: " + item.bodyPrefab.transform.position);
                        CustomEmotesAPI.ImportArmature(item.bodyPrefab, skele);
                        CustomEmotesAPI.CreateNameTokenSpritePair(MEGAMAN_x_PREFIX + "NAME", XAssets.IconXEmote);
                        //skele.GetComponentInChildren<BoneMapper>().scale = 1.05f;
                        //item.bodyPrefab.GetComponentInChildren<BoneMapper>().scale = 0.5f;
                        //skele.GetComponentInChildren<BoneMapper>().scale = 0.5f;
                        //Debug.Log("after Emote: " + item.bodyPrefab.transform.position);
                        //Debug.Log("skele pos: " + skele.transform.position);
                    }
                }
            }
        }

        private void GlobalEventManager_OnHitEnemy(On.RoR2.GlobalEventManager.orig_OnHitEnemy orig, GlobalEventManager self, DamageInfo damageInfo, GameObject victim)
        {
            // Verifique se algum objeto é nulo antes de prosseguir
            if (self == null || damageInfo == null || damageInfo.attacker == null || victim == null)
            {
                //Debug.LogWarning("[XSurvivor] Um dos objetos passados para OnHitEnemy é nulo.");
                return;
            }

            // Verifique se attacker e victim possuem CharacterBody
            var attackerBody = damageInfo.attacker.GetComponent<CharacterBody>();
            var victimBody = victim.GetComponent<CharacterBody>();
            if (attackerBody == null || victimBody == null)
            {
                //Debug.LogWarning("[XSurvivor] Um dos CharacterBody é nulo.");
                return;
            }

            // Verifique se inflictor não é nulo antes de usar
            if (damageInfo.inflictor == null)
            {
                //Debug.LogWarning("[XSurvivor] Inflictor é nulo.");
                return;
            }

            // Log básico para depuração
            //Debug.Log($"[XSurvivor] OnHitEnemy chamado com attacker: {attackerBody.name}, victim: {victimBody.name}, inflictor: {damageInfo.inflictor.name}");

            // Verifique as condições específicas antes de executar sua lógica customizada
            if (damageInfo.attacker.name.Contains("XBody") && !victim.name.Contains("XBody") && damageInfo.inflictor.name.Contains("XForceBusterProjectile"))
            {
                // Crie o projétil
                var position = victim.transform.position;
                if (position == Vector3.zero)
                {
                    //Debug.LogWarning("[XSurvivor] Posição do victim é inválida.");
                    return;
                }

                var XShockSphereProjectile = new FireProjectileInfo
                {
                    projectilePrefab = XAssets.xShockSphereProjectile,
                    position = position,
                    rotation = Util.QuaternionSafeLookRotation(position),
                    owner = damageInfo.attacker,
                    damage = 1f,
                    force = 200f,
                    crit = Util.CheckRoll(attackerBody.crit),
                    speedOverride = 0f,
                    damageColorIndex = DamageColorIndex.Default
                };

                ProjectileManager.instance.FireProjectile(XShockSphereProjectile);
                //Debug.Log("[XSurvivor] Projétil XShockSphere criado com sucesso.");
            }

            // Chame o método original
            orig.Invoke(self, damageInfo, victim);
        }

        private void CharacterModel_Awake(On.RoR2.CharacterModel.orig_Awake orig, CharacterModel self)
        {
            orig(self);
            if (self)
            {

                if (self.gameObject.name.Contains("X4"))
                {
                    AkSoundEngine.PostEvent(XStaticValues.X_Awake_VSFX, self.gameObject);

                    //I think TeaL used this on DekuMod to make the character select menu audio
                }

            }


        }

        private void RecalculateStatsAPI_GetStatCoefficients(CharacterBody sender, R2API.RecalculateStatsAPI.StatHookEventArgs args)
        {

            //if (sender.HasBuff(XBuffs.armorBuff))
            //{
            //    args.armorAdd += 300;
            //}

            if (sender.HasBuff(XBuffs.LightArmorBuff))
            {
                args.armorAdd += 30;
                args.armorAdd *= 1.4f;
                args.healthMultAdd *= 1.5f;
                args.damageMultAdd *= 1.2f;
                args.attackSpeedMultAdd *= 1.3f;
                args.regenMultAdd *= 1.3f;
                args.jumpPowerMultAdd *= 1.2f;
                args.moveSpeedMultAdd *= 1.2f;
                args.shieldMultAdd *= 1.4f;
                args.critDamageMultAdd *= 1f;
            }

            if (sender.HasBuff(XBuffs.SecondArmorBuff))
            {
                args.armorAdd += 20;
                args.armorAdd *= 1.2f;
                args.healthMultAdd *= 1.3f;
                args.damageMultAdd *= 1.1f;
                args.attackSpeedMultAdd *= 1.2f;
                args.regenMultAdd *= 1.3f;
                args.jumpPowerMultAdd *= 1.2f;
                args.moveSpeedMultAdd *= 1.25f;
                args.shieldMultAdd *= 1f;
                args.critDamageMultAdd *= 1.3f;
            }

            if (sender.HasBuff(XBuffs.MaxArmorBuff))
            {
                args.armorAdd += 40;
                args.armorAdd *= 1.9f;
                args.healthMultAdd *= 1.8f;
                args.damageMultAdd *= 1.3f;
                args.attackSpeedMultAdd *= 1.5f;
                args.regenMultAdd *= 1.8f;
                args.jumpPowerMultAdd *= 1.4f;
                args.moveSpeedMultAdd *= 1.4f;
                args.shieldMultAdd *= 1.7f;
                args.critDamageMultAdd *= 1.5f;
            }

            if (sender.HasBuff(XBuffs.FourthArmorBuff))
            {
                args.armorAdd += 70;
                args.armorAdd *= 2.1f;
                args.healthMultAdd *= 1.5f;
                args.damageMultAdd *= 1.5f;
                args.attackSpeedMultAdd *= 1.4f;
                args.regenMultAdd *= 1.5f;
                args.jumpPowerMultAdd *= 1.4f;
                args.moveSpeedMultAdd *= 1.4f;
                args.shieldMultAdd *= 1.5f;
                args.critDamageMultAdd *= 2f;
            }

            if (sender.HasBuff(XBuffs.FalconArmorBuff))
            {
                args.armorAdd += 20;
                args.armorAdd *= 1.1f;
                args.healthMultAdd *= 1.1f;
                args.damageMultAdd *= 1f;
                args.attackSpeedMultAdd *= 2f;
                args.regenMultAdd *= 1.4f;
                args.jumpPowerMultAdd += 0.25f;
                args.jumpPowerMultAdd *= 1.5f;
                args.moveSpeedMultAdd *= 2.5f;
                args.shieldMultAdd *= 1.2f;
                args.critDamageMultAdd *= 2f;
            }

            if (sender.HasBuff(XBuffs.GaeaArmorBuff))
            {
                args.armorAdd += 150;
                args.armorAdd *= 4f;
                args.healthMultAdd *= 3f;
                args.damageMultAdd *= 1.3f;
                args.attackSpeedMultAdd *= 1f;
                args.regenMultAdd *= 2f;
                args.jumpPowerMultAdd *= 1f;
                args.moveSpeedMultAdd *= 0.9f;
                args.shieldMultAdd *= 3f;
                args.critDamageMultAdd *= 2f;
            }

            if (sender.HasBuff(XBuffs.ShadowArmorBuff))
            {
                args.armorAdd += 70;
                args.armorAdd *= 2.4f;
                args.healthMultAdd *= 2f;
                args.damageMultAdd *= 1.5f;
                args.attackSpeedMultAdd *= 3f;
                args.regenMultAdd *= 1.5f;
                args.jumpPowerMultAdd += 0.5f;
                args.jumpPowerMultAdd *= 2f;
                args.moveSpeedMultAdd *= 3f;
                args.shieldMultAdd *= 1.8f;
                args.critDamageMultAdd *= 4f;
            }

            if (sender.HasBuff(XBuffs.UltimateArmorBuff))
            {
                args.armorAdd += 100;
                args.armorAdd *= 2.8f;
                args.healthMultAdd *= 2f;
                args.damageMultAdd *= 2.5f;
                args.attackSpeedMultAdd *= 1.8f;
                args.regenMultAdd *= 1.8f;
                args.jumpPowerMultAdd *= 1.8f;
                args.moveSpeedMultAdd *= 1.8f;
                args.shieldMultAdd *= 2f;
                args.critDamageMultAdd *= 3f;
            }

            if (sender.HasBuff(XBuffs.RathalosArmorBuff))
            {
                args.armorAdd += 100;
                args.armorAdd *= 3f;
                args.healthMultAdd *= 2f;
                args.damageMultAdd *= 2.5f;
                args.attackSpeedMultAdd *= 1.5f;
                args.regenMultAdd *= 1.8f;
                args.jumpPowerMultAdd *= 1.8f;
                args.moveSpeedMultAdd *= 1.5f;
                args.shieldMultAdd *= 2f;
                args.critDamageMultAdd *= 3f;
            }

            if (sender.HasBuff(XBuffs.HyperChipBuff))
            {
                args.armorAdd += 140;
                args.armorAdd *= 2f;
                args.healthMultAdd *= 2f;
                args.damageMultAdd *= 2f;
                args.attackSpeedMultAdd *= 2f;
                args.regenMultAdd *= 3f;
                args.jumpPowerMultAdd *= 1.6f;
                args.moveSpeedMultAdd *= 1.8f;
                args.shieldMultAdd *= 2f;
                args.critDamageMultAdd *= 2f;
            }


        }
    }
}