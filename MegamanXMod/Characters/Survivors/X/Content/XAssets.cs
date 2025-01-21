using RoR2;
using UnityEngine;
using MegamanXMod.Modules;
using System;
using RoR2.Projectile;
using R2API;
using MegamanXMod.Survivors.X.Components;
namespace MegamanXMod.Survivors.X
{
    public static class XAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        public static GameObject HyperModeEffect;
        public static GameObject ShurikenVFX;
        public static GameObject GaeaGAVFX;
        public static GameObject Charge1VFX;
        public static GameObject Charge2VFX;
        public static GameObject NovaStrikeVFX;
        public static GameObject MeltCreeperVFX;
        public static GameObject MeltCreeperChargeVFX;
        public static GameObject RisingFireVFX;
        public static GameObject FJetVFX;




        public static Mesh XBodyMesh;
        public static Mesh LightBodyMesh;
        public static Mesh SecondBodyMesh;
        public static Mesh MaxBodyMesh;
        public static Mesh FourthBodyMesh;
        public static Mesh FalconBodyMesh;
        public static Mesh GaeaBodyMesh;
        public static Mesh ShadowBodyMesh;
        public static Mesh UltimateBodyMesh;
        public static Mesh RathalosBodyMesh;



        public static Material MatX;
        public static Material MatLight;
        public static Material MatSecond;
        public static Material MatMax;
        public static Material MatMaxGold;
        public static Material MatFourth;
        public static Material MatFalcon;
        public static Material MatGaea;
        public static Material MatShadow;
        public static Material MatUltimate;
        public static Material MatRathalos;

        public static Sprite IconX;
        public static Sprite IconLight;
        public static Sprite IconSecond;
        public static Sprite IconMax;
        public static Sprite IconFourth;
        public static Sprite IconFalcon;
        public static Sprite IconGaea;
        public static Sprite IconXShadow;
        public static Sprite IconUltimate;
        public static Sprite IconXRathalos;

        public static Sprite IconXBuster;
        public static Sprite IconLightBuster;
        public static Sprite IconGigaBuster;
        public static Sprite IconMaxBuster;
        public static Sprite IconFalconBuster;
        public static Sprite IconGaeaBuster;
        public static Sprite IconShadowBuster;
        public static Sprite IconUltimateBuster;
        public static Sprite IconRathalosBuster;

        public static Sprite IconShadowSaber;
        public static Sprite IconRathalosSlash;
        public static Sprite IconShotgunIce;
        public static Sprite IconFireWave;
        public static Sprite IconSqueezeBomb;

        public static Sprite IconFalconDash;
        public static Sprite IconXDash;
        public static Sprite IconXNovaDash;
        public static Sprite IconXNovaStrike;

        public static Sprite IconSkillLock;

        public static Sprite IconHeadScanner;
        public static Sprite IconHyperChip;
        public static Sprite IconGigaAttackGaea;
        public static Sprite IconTrueRathalosSlash;
        public static Sprite IconHomingTorpedo;
        public static Sprite IconChameleonSting;
        public static Sprite IconAcidBurst;
        public static Sprite IconRisingFire;
        public static Sprite IconMeltCreeper;


        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        public static GameObject shurikenProjectilePrefab;
        public static GameObject shurikenProjectilePrefab2;
        public static GameObject xBusterChargeProjectile;
        public static GameObject xBusterMediumProjectile;
        public static GameObject xLightBusterChargeProjectile;
        public static GameObject xLightBusterSmallProjectile;
        public static GameObject xMaxBusterChargeProjectile;
        public static GameObject xMaxBusterSmallProjectile;
        public static GameObject xForceBusterProjectile;
        public static GameObject xShockSphereProjectile;
        public static GameObject xFalconBusterChargeProjectile;
        public static GameObject xGaeaBusterChargeProjectile;
        public static GameObject xGaeaBusterSmallProjectile;
        public static GameObject shotFMJ;
        public static GameObject shotgunIceprefab;
        public static GameObject XRFireProjectile;
        public static GameObject XRFire2Projectile;
        public static GameObject SqueezeBombProjectile;
        public static GameObject MeltCreeperProjectile;
        public static GameObject MeltCreeperChargeProjectile;
        public static GameObject AcidBurstProjectile;
        public static GameObject ChameleonStingProjectile;

        private static AssetBundle _assetBundle;

        public static void Init(AssetBundle assetBundle)
        {

            _assetBundle = assetBundle;

            swordHitSoundEvent = Content.CreateAndAddNetworkSoundEventDef("HenrySwordHit");

            CreateEffects();

            CreateProjectiles();
        }

        #region effects
        private static void CreateEffects()
        {
            CreateBombExplosionEffect();

            swordSwingEffect = _assetBundle.LoadEffect("HenrySwordSwingEffect", true);
            swordHitImpactEffect = _assetBundle.LoadEffect("ImpactHenrySlash");

            XBodyMesh = _assetBundle.LoadAsset<Mesh>("XBodyMesh");
            LightBodyMesh = _assetBundle.LoadAsset<Mesh>("XLightBodyMesh");
            SecondBodyMesh = _assetBundle.LoadAsset<Mesh>("XSecondBodyMesh");
            MaxBodyMesh = _assetBundle.LoadAsset<Mesh>("XMaxBodyMesh");
            FourthBodyMesh = _assetBundle.LoadAsset<Mesh>("XFourthBodyMesh");
            FalconBodyMesh = _assetBundle.LoadAsset<Mesh>("XFalconBodyMesh");
            GaeaBodyMesh = _assetBundle.LoadAsset<Mesh>("XGaeaBodyMesh");
            ShadowBodyMesh = _assetBundle.LoadAsset<Mesh>("XShadowBodyMesh");
            UltimateBodyMesh = _assetBundle.LoadAsset<Mesh>("XUltimateBodyMesh");
            RathalosBodyMesh = _assetBundle.LoadAsset<Mesh>("XRathalosBodyMesh");

            MatX = _assetBundle.LoadAsset<Material>("matX");
            MatLight = _assetBundle.LoadAsset<Material>("matLight");
            MatSecond = _assetBundle.LoadAsset<Material>("matSecond");
            MatMax = _assetBundle.LoadAsset<Material>("matMax");
            MatMaxGold = _assetBundle.LoadAsset<Material>("matMaxG");
            MatFourth = _assetBundle.LoadAsset<Material>("matFourth");
            MatFalcon = _assetBundle.LoadAsset<Material>("matFalcon");
            MatGaea = _assetBundle.LoadAsset<Material>("matGaea");
            MatShadow = _assetBundle.LoadAsset<Material>("matShadowX");
            MatUltimate = _assetBundle.LoadAsset<Material>("matUltimate");
            MatRathalos = _assetBundle.LoadAsset<Material>("matXRathalos");

            IconX = _assetBundle.LoadAsset<Sprite>("XIcon");
            IconLight = _assetBundle.LoadAsset<Sprite>("XLightIcon");
            IconSecond = _assetBundle.LoadAsset<Sprite>("XSecondIcon");
            IconMax = _assetBundle.LoadAsset<Sprite>("XMaxIcon");
            IconFourth = _assetBundle.LoadAsset<Sprite>("XFourthIcon");
            IconFalcon = _assetBundle.LoadAsset<Sprite>("XFalconIcon");
            IconGaea = _assetBundle.LoadAsset<Sprite>("XGaeaIcon");
            IconXShadow = _assetBundle.LoadAsset<Sprite>("XShadowIcon");
            IconUltimate = _assetBundle.LoadAsset<Sprite>("XUltimateIcon");
            IconXRathalos = _assetBundle.LoadAsset<Sprite>("XRathalosIcon");

            IconXBuster = _assetBundle.LoadAsset<Sprite>("XBusterIcon");
            IconLightBuster = _assetBundle.LoadAsset<Sprite>("LightBusterIcon");
            IconGigaBuster = _assetBundle.LoadAsset<Sprite>("GigaBusterIcon");
            IconMaxBuster = _assetBundle.LoadAsset<Sprite>("MaxBusterIcon");
            IconFalconBuster = _assetBundle.LoadAsset<Sprite>("FalconBusterIcon");
            IconGaeaBuster = _assetBundle.LoadAsset<Sprite>("GaeaBusterIcon");
            IconShadowBuster = _assetBundle.LoadAsset<Sprite>("ShadowBusterIcon");
            IconUltimateBuster = _assetBundle.LoadAsset<Sprite>("UltimateBusterIcon");
            IconRathalosBuster = _assetBundle.LoadAsset<Sprite>("RathalosBusterIcon");


            IconShadowSaber = _assetBundle.LoadAsset<Sprite>("XSkillShadowSaber");
            IconRathalosSlash = _assetBundle.LoadAsset<Sprite>("XSkillRathalosSlash");
            IconShotgunIce = _assetBundle.LoadAsset<Sprite>("XSkillShotgunIce");
            IconFireWave = _assetBundle.LoadAsset<Sprite>("XSkillFireWave");
            IconSqueezeBomb = _assetBundle.LoadAsset<Sprite>("XSkillSqueezeBomb");


            IconXDash = _assetBundle.LoadAsset<Sprite>("XSkillDash");
            IconFalconDash = _assetBundle.LoadAsset<Sprite>("XSkillFalconDash");
            IconXNovaDash = _assetBundle.LoadAsset<Sprite>("XSkillNovaDash");
            IconXNovaStrike = _assetBundle.LoadAsset<Sprite>("XSkillNovaStrike");

            IconSkillLock = _assetBundle.LoadAsset<Sprite>("XSkillLock");

            IconHeadScanner = _assetBundle.LoadAsset<Sprite>("XSkillHeadScanner");
            IconHyperChip = _assetBundle.LoadAsset<Sprite>("XSkillHyperChip");
            IconGigaAttackGaea = _assetBundle.LoadAsset<Sprite>("XSkillGigaAttackGaea");
            IconTrueRathalosSlash = _assetBundle.LoadAsset<Sprite>("XSkillTrueRathalosSlash");
            IconHomingTorpedo = _assetBundle.LoadAsset<Sprite>("XSkillHomingTorpedo");
            IconChameleonSting = _assetBundle.LoadAsset<Sprite>("XSkillChameleonSting");
            IconAcidBurst = _assetBundle.LoadAsset<Sprite>("XSkillAcidBurst");
            IconRisingFire = _assetBundle.LoadAsset<Sprite>("XSkillRisingFire");
            IconMeltCreeper = _assetBundle.LoadAsset<Sprite>("XSkillMeltCreeper");

            HyperModeEffect = _assetBundle.LoadEffect("HyperModeEffect", true);
            GaeaGAVFX = _assetBundle.LoadEffect("GaeaGigaAttackVFX", true);
            Charge1VFX = _assetBundle.LoadEffect("Charge1VFX", true);
            Charge2VFX = _assetBundle.LoadEffect("Charge2VFX", true);
            NovaStrikeVFX = _assetBundle.LoadEffect("NovaStrikeVFX", true);
            RisingFireVFX = _assetBundle.LoadEffect("RisingFireVFX", true);
            FJetVFX = _assetBundle.LoadEffect("FJet", true);

            ShurikenVFX = _assetBundle.LoadEffect("ShurikenVFX", false);
            MeltCreeperVFX = _assetBundle.LoadEffect("MeltCreeperVFX", false);
            MeltCreeperChargeVFX = _assetBundle.LoadEffect("MeltCreeperChargeVFX", false);


        }

        private static void CreateBombExplosionEffect()
        {
            bombExplosionEffect = _assetBundle.LoadEffect("BombExplosionEffect", "HenryBombExplosion");

            if (!bombExplosionEffect)
                return;

            ShakeEmitter shakeEmitter = bombExplosionEffect.AddComponent<ShakeEmitter>();
            shakeEmitter.amplitudeTimeDecay = true;
            shakeEmitter.duration = 0.5f;
            shakeEmitter.radius = 200f;
            shakeEmitter.scaleShakeRadiusWithLocalScale = false;

            shakeEmitter.wave = new Wave
            {
                amplitude = 1f,
                frequency = 40f,
                cycleOffset = 0f
            };

        }
        #endregion effects

        #region projectiles
        private static void CreateProjectiles()
        {
            CreateBombProjectile();
            CreateShurikenProjectile();
            CreateShurikenProjectile2();
            CreateShotgunIce();
            CreateShotFMJ();
            CreateXBusterChargeProjectile();
            CreateXBusterMediumProjectile();
            CreateXLightBusterChargeProjectile();
            CreateXLightBusterSmallProjectile();
            CreateXMaxBusterChargeProjectile();
            CreateXMaxBusterSmallProjectile();
            CreateXForceBusterProjectile();
            CreateXShockSphereProjectile();
            CreateXGaeaBusterChargeProjectile();
            CreateXGaeaBusterSmallProjectile();
            CreateXFalconBusterChargeProjectile();
            CreateXRFireProjectile();
            CreateXRFire2Projectile();
            CreateSqueezeBombProjectile();
            CreateMeltCreeperProjectile();
            CreateMeltCreeperChargeProjectile();
            CreateXAcidBurstProjectile();
            CreateChameleonStingProjectile();


            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(shurikenProjectilePrefab);
            Content.AddProjectilePrefab(shurikenProjectilePrefab2);
            Content.AddProjectilePrefab(shotgunIceprefab);
            Content.AddProjectilePrefab(shotFMJ);
            Content.AddProjectilePrefab(xBusterChargeProjectile);
            Content.AddProjectilePrefab(xBusterMediumProjectile);
            Content.AddProjectilePrefab(xLightBusterChargeProjectile);
            Content.AddProjectilePrefab(xLightBusterSmallProjectile);
            Content.AddProjectilePrefab(xMaxBusterChargeProjectile);
            Content.AddProjectilePrefab(xMaxBusterSmallProjectile);
            Content.AddProjectilePrefab(xForceBusterProjectile);
            Content.AddProjectilePrefab(xShockSphereProjectile);
            Content.AddProjectilePrefab(xGaeaBusterChargeProjectile);
            Content.AddProjectilePrefab(xGaeaBusterSmallProjectile);
            Content.AddProjectilePrefab(xFalconBusterChargeProjectile);
            Content.AddProjectilePrefab(XRFireProjectile);
            Content.AddProjectilePrefab(XRFire2Projectile);
            Content.AddProjectilePrefab(SqueezeBombProjectile);
            Content.AddProjectilePrefab(MeltCreeperProjectile);
            Content.AddProjectilePrefab(MeltCreeperChargeProjectile);
            Content.AddProjectilePrefab(AcidBurstProjectile);
            Content.AddProjectilePrefab(ChameleonStingProjectile);
        }

        private static void CreateBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            bombProjectilePrefab = Asset.CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(bombProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion bombImpactExplosion = bombProjectilePrefab.AddComponent<ProjectileImpactExplosion>();
            
            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.blastDamageCoefficient = 1f;
            bombImpactExplosion.falloffModel = BlastAttack.FalloffModel.None;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = bombExplosionEffect;
            bombImpactExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("HenryBombGhost") != null)
                bombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("HenryBombGhost");
            
            bombController.startSound = "";
        }

        private static void CreateShurikenProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            shurikenProjectilePrefab = Asset.CloneProjectilePrefab("Bandit2ShivProjectile", "XShurikenProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(shurikenProjectilePrefab.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XShurkenExplosion = shurikenProjectilePrefab.AddComponent<ProjectileImpactExplosion>();

            XShurkenExplosion.blastRadius = 1f;
            XShurkenExplosion.blastDamageCoefficient = 1f;
            XShurkenExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XShurkenExplosion.destroyOnEnemy = true;
            XShurkenExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XShurkenExplosion.timerAfterImpact = true;
            XShurkenExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            shurikenProjectilePrefab.GetComponent<ProjectileDamage>().damage = 1f;
            shurikenProjectilePrefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            shurikenProjectilePrefab.GetComponent<ProjectileDamage>().damageType = DamageType.BleedOnHit;
            shurikenProjectilePrefab.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Bleed;

            // register it for networking
            if (shurikenProjectilePrefab) PrefabAPI.RegisterNetworkPrefab(shurikenProjectilePrefab);


            ProjectileController XShurikenController = shurikenProjectilePrefab.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("XShurikenGhost") != null)
                XShurikenController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("XShurikenGhost");

            XShurikenController.startSound = "";
        }

        private static void CreateShurikenProjectile2()
        {
            // clone FMJ's syringe projectile prefab here to use as our own projectile
            shurikenProjectilePrefab2 = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("Prefabs/Projectiles/Bandit2ShivProjectile"), "Prefabs/Projectiles/Bandit2ShivProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);
            //shurikenProjectilePrefab2 = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/Bandit2ShivProjectile"), "XShurikenProjectile2");

            // just setting the numbers to 1 as the entitystate will take care of those
            shurikenProjectilePrefab2.GetComponent<ProjectileDamage>().damage = 1f;
            shurikenProjectilePrefab2.GetComponent<ProjectileController>().procCoefficient = 1f;
            shurikenProjectilePrefab2.GetComponent<ProjectileDamage>().damageType = DamageType.BleedOnHit;

            // register it for networking
            if (shurikenProjectilePrefab2) PrefabAPI.RegisterNetworkPrefab(shurikenProjectilePrefab2);

            ProjectileController XShurikenController2 = shurikenProjectilePrefab2.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("ShurikenVFX") != null)
                XShurikenController2.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShurikenVFX");

            XShurikenController2.startSound = "";
        }

        private static void CreateXBusterMediumProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xBusterMediumProjectile = Asset.CloneProjectilePrefab("FMJ", "XBusterMediumProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xBusterMediumProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XBusterMediumExplosion = xBusterMediumProjectile.AddComponent<ProjectileImpactExplosion>();

            XBusterMediumExplosion.blastRadius = 4f;
            XBusterMediumExplosion.blastDamageCoefficient = 1f;
            XBusterMediumExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XBusterMediumExplosion.destroyOnEnemy = true;
            XBusterMediumExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XBusterMediumExplosion.timerAfterImpact = true;
            XBusterMediumExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xBusterMediumProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xBusterMediumProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xBusterMediumProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xBusterMediumProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xBusterMediumProjectile) PrefabAPI.RegisterNetworkPrefab(xBusterMediumProjectile);


            ProjectileController XBusterMediumController = xBusterMediumProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("XBusterHalfChargeProjectille") != null)
                XBusterMediumController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("XBusterHalfChargeProjectille");

            XBusterMediumController.startSound = "";
        }

        private static void CreateXBusterChargeProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xBusterChargeProjectile = Asset.CloneProjectilePrefab("FMJ", "XBusterChargeProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xBusterChargeProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XBusterChargeExplosion = xBusterChargeProjectile.AddComponent<ProjectileImpactExplosion>();

            XBusterChargeExplosion.blastRadius = 10f;
            XBusterChargeExplosion.blastDamageCoefficient = 1f;
            XBusterChargeExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XBusterChargeExplosion.destroyOnEnemy = true;
            XBusterChargeExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XBusterChargeExplosion.timerAfterImpact = true;
            XBusterChargeExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xBusterChargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xBusterChargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xBusterChargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xBusterChargeProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xBusterChargeProjectile) PrefabAPI.RegisterNetworkPrefab(xBusterChargeProjectile);


            ProjectileController XBusterChargeController = xBusterChargeProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("XBusterChargeProjectille") != null)
                XBusterChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("XBusterChargeProjectille");

            XBusterChargeController.startSound = "";
        }

        private static void CreateXLightBusterChargeProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xLightBusterChargeProjectile = Asset.CloneProjectilePrefab("FMJ", "XLightBusterChargeProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xLightBusterChargeProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XLightBusterChargeExplosion = xLightBusterChargeProjectile.AddComponent<ProjectileImpactExplosion>();

            XLightBusterChargeExplosion.blastRadius = 14f;
            XLightBusterChargeExplosion.blastDamageCoefficient = 1f;
            XLightBusterChargeExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XLightBusterChargeExplosion.destroyOnEnemy = true;
            XLightBusterChargeExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XLightBusterChargeExplosion.timerAfterImpact = true;
            XLightBusterChargeExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xLightBusterChargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xLightBusterChargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xLightBusterChargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xLightBusterChargeProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xLightBusterChargeProjectile) PrefabAPI.RegisterNetworkPrefab(xLightBusterChargeProjectile);


            ProjectileController XLightBusterChargeController = xLightBusterChargeProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("LightBusterChargeProjectille") != null)
                XLightBusterChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("LightBusterChargeProjectille");

            XLightBusterChargeController.startSound = "";
        }

        private static void CreateXLightBusterSmallProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xLightBusterSmallProjectile = Asset.CloneProjectilePrefab("FMJ", "XLightBusterSmallProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xLightBusterSmallProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XLightBusterSmallExplosion = xLightBusterSmallProjectile.AddComponent<ProjectileImpactExplosion>();

            XLightBusterSmallExplosion.blastRadius = 2f;
            XLightBusterSmallExplosion.blastDamageCoefficient = 1f;
            XLightBusterSmallExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XLightBusterSmallExplosion.destroyOnEnemy = true;
            XLightBusterSmallExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XLightBusterSmallExplosion.timerAfterImpact = true;
            XLightBusterSmallExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xLightBusterSmallProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xLightBusterSmallProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xLightBusterSmallProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xLightBusterSmallProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xLightBusterSmallProjectile) PrefabAPI.RegisterNetworkPrefab(xLightBusterSmallProjectile);


            ProjectileController XLightBusterSmallController = xLightBusterSmallProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("LightBusterSmallProjectille") != null)
                XLightBusterSmallController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("LightBusterSmallProjectille");

            XLightBusterSmallController.startSound = "";
        }

        private static void CreateXMaxBusterChargeProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xMaxBusterChargeProjectile = Asset.CloneProjectilePrefab("FMJ", "XMaxBusterChargeProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xMaxBusterChargeProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XMaxBusterChargeExplosion = xMaxBusterChargeProjectile.AddComponent<ProjectileImpactExplosion>();

            XMaxBusterChargeExplosion.blastRadius = 14f;
            XMaxBusterChargeExplosion.blastDamageCoefficient = 1f;
            XMaxBusterChargeExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XMaxBusterChargeExplosion.destroyOnEnemy = true;
            XMaxBusterChargeExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XMaxBusterChargeExplosion.timerAfterImpact = true;
            XMaxBusterChargeExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xMaxBusterChargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xMaxBusterChargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xMaxBusterChargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xMaxBusterChargeProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xMaxBusterChargeProjectile) PrefabAPI.RegisterNetworkPrefab(xMaxBusterChargeProjectile);


            ProjectileController XMaxBusterChargeController = xMaxBusterChargeProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("MaxBusterChargeProjectille") != null)
                XMaxBusterChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MaxBusterChargeProjectille");

            XMaxBusterChargeController.startSound = "";
        }

        private static void CreateXMaxBusterSmallProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xMaxBusterSmallProjectile = Asset.CloneProjectilePrefab("FMJ", "XMaxBusterSmallProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xMaxBusterSmallProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XMaxBusterSmallExplosion = xMaxBusterSmallProjectile.AddComponent<ProjectileImpactExplosion>();

            XMaxBusterSmallExplosion.blastRadius = 2f;
            XMaxBusterSmallExplosion.blastDamageCoefficient = 1f;
            XMaxBusterSmallExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XMaxBusterSmallExplosion.destroyOnEnemy = true;
            XMaxBusterSmallExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XMaxBusterSmallExplosion.timerAfterImpact = true;
            XMaxBusterSmallExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xMaxBusterSmallProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xMaxBusterSmallProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xMaxBusterSmallProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xMaxBusterSmallProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xMaxBusterSmallProjectile) PrefabAPI.RegisterNetworkPrefab(xMaxBusterSmallProjectile);


            ProjectileController XMaxBusterSmallController = xMaxBusterSmallProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("MaxBusterSmallProjectille") != null)
                XMaxBusterSmallController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MaxBusterSmallProjectille");

            XMaxBusterSmallController.startSound = "";
        }

        private static void CreateXForceBusterProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xForceBusterProjectile = Asset.CloneProjectilePrefab("FMJRamping", "XForceBusterProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xForceBusterProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XForceBusterExplosion = xForceBusterProjectile.AddComponent<ProjectileImpactExplosion>();

            XForceBusterExplosion.blastRadius = 16f;
            XForceBusterExplosion.blastDamageCoefficient = 1f;
            XForceBusterExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XForceBusterExplosion.destroyOnEnemy = true;
            XForceBusterExplosion.lifetime = 12f;
            //XForceBusterExplosion.impactEffect = bombExplosionEffect;
            //XForceBusterExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XForceBusterExplosion.timerAfterImpact = true;
            XForceBusterExplosion.lifetimeAfterImpact = 5f;

            //xForceBusterProjectile.AddComponent<XForceBusterComponent>(); // Adicionar o script



            // just setting the numbers to 1 as the entitystate will take care of those
            xForceBusterProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xForceBusterProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xForceBusterProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xForceBusterProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xForceBusterProjectile) PrefabAPI.RegisterNetworkPrefab(xForceBusterProjectile);


            ProjectileController XForceBusterController = xForceBusterProjectile.GetComponent<ProjectileController>();
            


            if (_assetBundle.LoadAsset<GameObject>("UltimateBusterChargeProjectille") != null)
                XForceBusterController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("UltimateBusterChargeProjectille");

            XForceBusterController.startSound = "";
        }

        private static void CreateXShockSphereProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xShockSphereProjectile = Asset.CloneProjectilePrefab("FMJ", "xShockSphereProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xShockSphereProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XShockSphereExplosion = xShockSphereProjectile.AddComponent<ProjectileImpactExplosion>();

            XShockSphereExplosion.blastRadius = 5f;
            XShockSphereExplosion.blastDamageCoefficient = 1f;
            XShockSphereExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XShockSphereExplosion.destroyOnEnemy = false;
            XShockSphereExplosion.lifetime = 12f;
            //XForceBusterExplosion.impactEffect = bombExplosionEffect;
            //XForceBusterExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XShockSphereExplosion.timerAfterImpact = true;
            XShockSphereExplosion.lifetimeAfterImpact = 5f;

            xShockSphereProjectile.AddComponent<XShockSphereComponent>(); // Adicionar o script


            // just setting the numbers to 1 as the entitystate will take care of those
            xShockSphereProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xShockSphereProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xShockSphereProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;
            xShockSphereProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Luminous;


            // register it for networking
            if (xShockSphereProjectile) PrefabAPI.RegisterNetworkPrefab(xShockSphereProjectile);


            ProjectileController XShockSphereController = xShockSphereProjectile.GetComponent<ProjectileController>();



            if (_assetBundle.LoadAsset<GameObject>("ShockSphere") != null)
                XShockSphereController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShockSphere");

            XShockSphereController.startSound = "";
            XShockSphereController.shouldPlaySounds = false;
        }

        private static void CreateXFalconBusterChargeProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xFalconBusterChargeProjectile = Asset.CloneProjectilePrefab("FMJ", "xFalconBusterChargeProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xFalconBusterChargeProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XFalconBusterChargeExplosion = xFalconBusterChargeProjectile.AddComponent<ProjectileImpactExplosion>();

            XFalconBusterChargeExplosion.blastRadius = 14f;
            XFalconBusterChargeExplosion.blastDamageCoefficient = 1f;
            XFalconBusterChargeExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XFalconBusterChargeExplosion.destroyOnEnemy = true;
            XFalconBusterChargeExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XFalconBusterChargeExplosion.timerAfterImpact = true;
            XFalconBusterChargeExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xFalconBusterChargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xFalconBusterChargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xFalconBusterChargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
            xFalconBusterChargeProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (xFalconBusterChargeProjectile) PrefabAPI.RegisterNetworkPrefab(xFalconBusterChargeProjectile);


            ProjectileController XFalconBusterChargeController = xFalconBusterChargeProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("XFalconBusterProjectille") != null)
                XFalconBusterChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("XFalconBusterProjectille");

            XFalconBusterChargeController.startSound = "";
        }

        private static void CreateXGaeaBusterChargeProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xGaeaBusterChargeProjectile = Asset.CloneProjectilePrefab("FMJ", "XGaeaBusterChargeProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xGaeaBusterChargeProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XGaeaBusterChargeExplosion = xGaeaBusterChargeProjectile.AddComponent<ProjectileImpactExplosion>();

            XGaeaBusterChargeExplosion.blastRadius = 14f;
            XGaeaBusterChargeExplosion.blastDamageCoefficient = 1f;
            XGaeaBusterChargeExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XGaeaBusterChargeExplosion.destroyOnEnemy = true;
            XGaeaBusterChargeExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XGaeaBusterChargeExplosion.timerAfterImpact = true;
            XGaeaBusterChargeExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xGaeaBusterChargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xGaeaBusterChargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xGaeaBusterChargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.PoisonOnHit;
            xGaeaBusterChargeProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Poison;

            // register it for networking
            if (xGaeaBusterChargeProjectile) PrefabAPI.RegisterNetworkPrefab(xGaeaBusterChargeProjectile);


            ProjectileController XGaeaBusterChargeController = xGaeaBusterChargeProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("GaeaBusterChargeProjectille") != null)
                XGaeaBusterChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("GaeaBusterChargeProjectille");

            XGaeaBusterChargeController.startSound = "";
        }

        private static void CreateXGaeaBusterSmallProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            xGaeaBusterSmallProjectile = Asset.CloneProjectilePrefab("FMJ", "xGaeaBusterSmallProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(xGaeaBusterSmallProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XGaeaBusterSmallExplosion = xGaeaBusterSmallProjectile.AddComponent<ProjectileImpactExplosion>();

            XGaeaBusterSmallExplosion.blastRadius = 4f;
            XGaeaBusterSmallExplosion.blastDamageCoefficient = 1f;
            XGaeaBusterSmallExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XGaeaBusterSmallExplosion.destroyOnEnemy = true;
            XGaeaBusterSmallExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XGaeaBusterSmallExplosion.timerAfterImpact = true;
            XGaeaBusterSmallExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            xGaeaBusterSmallProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            xGaeaBusterSmallProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            xGaeaBusterSmallProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.PoisonOnHit;
            xGaeaBusterSmallProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Poison;

            // register it for networking
            if (xGaeaBusterSmallProjectile) PrefabAPI.RegisterNetworkPrefab(xGaeaBusterSmallProjectile);


            ProjectileController XGaeaBusterSmallController = xGaeaBusterSmallProjectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("GaeaBusterSmallProjectille") != null)
                XGaeaBusterSmallController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("GaeaBusterSmallProjectille");

            XGaeaBusterSmallController.startSound = "";
        }

        private static void CreateShotFMJ()
        {
            // clone Lunar's syringe projectile prefab here to use as our own projectile
            shotFMJ = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/FMJ"), "Prefabs/Projectiles/shotFMJProjectile", true, "", "RegisterCharacter", 155);


            // just setting the numbers to 1 as the entitystate will take care of those
            shotFMJ.GetComponent<ProjectileDamage>().damage = 1f;
            shotFMJ.GetComponent<ProjectileController>().procCoefficient = 1f;
            shotFMJ.GetComponent<ProjectileDamage>().damageType = DamageType.Shock5s;

            // register it for networking
            if (shotFMJ) PrefabAPI.RegisterNetworkPrefab(shotFMJ);


            ProjectileController shotFMJController = shotFMJ.GetComponent<ProjectileController>();
            if (_assetBundle.LoadAsset<GameObject>("shotFMJGhost") != null)
                shotFMJController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("shotFMJGhost");

            shotFMJController.startSound = "";

        }

        private static void CreateShotgunIce()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            shotgunIceprefab = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageIceBombProjectile"), "Prefabs/Projectiles/ShotgIceProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            shotgunIceprefab.GetComponent<ProjectileDamage>().damage = 1f;
            shotgunIceprefab.GetComponent<ProjectileController>().procCoefficient = 1f;
            shotgunIceprefab.GetComponent<ProjectileDamage>().damageType = DamageType.Freeze2s;

            // register it for networking
            if (shotgunIceprefab) PrefabAPI.RegisterNetworkPrefab(shotgunIceprefab);

            ProjectileController shotgunIceController = shotgunIceprefab.GetComponent<ProjectileController>();
            if (_assetBundle.LoadAsset<GameObject>("ShotgunIceGhost") != null) shotgunIceController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("ShotgunIceGhost");
            shotgunIceController.startSound = "";
        }

        private static void CreateXRFireProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            XRFireProjectile = Asset.CloneProjectilePrefab("Fireball", "XRFireProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(XRFireProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XRFireExplosion = XRFireProjectile.AddComponent<ProjectileImpactExplosion>();

            XRFireExplosion.blastRadius = 4f;
            XRFireExplosion.blastDamageCoefficient = 1f;
            XRFireExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XRFireExplosion.destroyOnEnemy = true;
            XRFireExplosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XRFireExplosion.timerAfterImpact = true;
            XRFireExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            XRFireProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            XRFireProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            XRFireProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;
            XRFireProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (XRFireProjectile) PrefabAPI.RegisterNetworkPrefab(XRFireProjectile);


            ProjectileController XRFireController = XRFireProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("XRFireProjectille") != null)
            //    XRFireController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("XBusterHalfChargeProjectille");

            XRFireController.startSound = "";
        }

        private static void CreateXRFire2Projectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            XRFire2Projectile = Asset.CloneProjectilePrefab("FMJ", "XRFire2Projectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(XRFire2Projectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XRFire2Explosion = XRFire2Projectile.AddComponent<ProjectileImpactExplosion>();

            XRFire2Explosion.blastRadius = 6f;
            XRFire2Explosion.blastDamageCoefficient = 1f;
            XRFire2Explosion.falloffModel = BlastAttack.FalloffModel.None;
            XRFire2Explosion.destroyOnEnemy = true;
            XRFire2Explosion.lifetime = 12f;
            //XShurkenExplosion.impactEffect = bombExplosionEffect;
            //XShurkenExplosion.lifetimeExpiredSound = Content.CreateAndAddNetworkSoundEventDef("HenryBombExplosion");
            XRFire2Explosion.timerAfterImpact = true;
            XRFire2Explosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            XRFire2Projectile.GetComponent<ProjectileDamage>().damage = 1f;
            XRFire2Projectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            XRFire2Projectile.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;
            XRFire2Projectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;


            // register it for networking
            if (XRFire2Projectile) PrefabAPI.RegisterNetworkPrefab(XRFire2Projectile);


            ProjectileController XRFire2Controller = XRFire2Projectile.GetComponent<ProjectileController>();

            if (_assetBundle.LoadAsset<GameObject>("RathalosBusterChargeProjectille") != null)
                XRFire2Controller.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("RathalosBusterChargeProjectille");

            XRFire2Controller.startSound = "";
        }

        private static void CreateXAcidBurstProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            AcidBurstProjectile = Asset.CloneProjectilePrefab("PoisonOrbProjectile", "XAcidBurstProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            //UnityEngine.Object.Destroy(AcidBurstProjectile.GetComponent<ProjectileImpactExplosion>());
            ProjectileImpactExplosion XAcidBurstExplosion = AcidBurstProjectile.GetComponent<ProjectileImpactExplosion>();

            XAcidBurstExplosion.blastRadius = 6f;
            XAcidBurstExplosion.blastDamageCoefficient = 1f;
            //XAcidBurstExplosion.falloffModel = BlastAttack.FalloffModel.None;
            XAcidBurstExplosion.destroyOnEnemy = true;
            XAcidBurstExplosion.lifetime = 12f;
            XAcidBurstExplosion.timerAfterImpact = true;
            XAcidBurstExplosion.lifetimeAfterImpact = 0.1f;

            // just setting the numbers to 1 as the entitystate will take care of those
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            AcidBurstProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.PoisonOnHit;
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.WeakOnHit;
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.BypassArmor;
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.BypassBlock;
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.SlowOnHit;
            AcidBurstProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Poison;


            ProjectileController XAcidBurstController = AcidBurstProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("RathalosBusterChargeProjectille") != null)
            //    XRFire2Controller.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("RathalosBusterChargeProjectille");

            XAcidBurstController.startSound = "";
        }

        private static void CreateSqueezeBombProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            SqueezeBombProjectile = Asset.CloneProjectilePrefab("GravSphere", "XSqueezeBombProjectile");

            //remove their ProjectileImpactExplosion component and start from default values
            UnityEngine.Object.Destroy(SqueezeBombProjectile.GetComponent<ProjectileImpactExplosion>());

            // just setting the numbers to 1 as the entitystate will take care of those
            SqueezeBombProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            SqueezeBombProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            SqueezeBombProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.WeakOnHit;
            SqueezeBombProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            // register it for networking
            if (SqueezeBombProjectile) PrefabAPI.RegisterNetworkPrefab(SqueezeBombProjectile);


            ProjectileController SqueezeBombController = SqueezeBombProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("RathalosBusterChargeProjectille") != null)
            //    SqueezeBombController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("RathalosBusterChargeProjectille");

            SqueezeBombController.startSound = "";
        }

        private static void CreateMeltCreeperProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            MeltCreeperProjectile = Asset.CloneProjectilePrefab("FMJ", "XMeltCreeperProjectile");

            // just setting the numbers to 1 as the entitystate will take care of those
            MeltCreeperProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            MeltCreeperProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            MeltCreeperProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;
            MeltCreeperProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;

            

            // register it for networking
            if (MeltCreeperProjectile) PrefabAPI.RegisterNetworkPrefab(MeltCreeperProjectile);


            ProjectileController MeltCreeperController = MeltCreeperProjectile.GetComponent<ProjectileController>();

            MeltCreeperProjectile.AddComponent<XMeltCreeperComponent>();

            if (_assetBundle.LoadAsset<GameObject>("MCSmallVFX") != null)
                MeltCreeperController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MCSmallVFX");

            MeltCreeperController.startSound = "";
        }

        private static void CreateMeltCreeperChargeProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            MeltCreeperChargeProjectile = Asset.CloneProjectilePrefab("FMJ", "MeltCreeperChargeProjectile");

            // just setting the numbers to 1 as the entitystate will take care of those
            MeltCreeperChargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            MeltCreeperChargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            MeltCreeperChargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;
            MeltCreeperChargeProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Default;


            ProjectileController MeltCreeperChargeController = MeltCreeperChargeProjectile.GetComponent<ProjectileController>();

            MeltCreeperChargeProjectile.AddComponent<XMeltCreeperChargeComponent>();

            if (_assetBundle.LoadAsset<GameObject>("MCSmallVFX") != null)
                MeltCreeperChargeController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MCSmallVFX");

            MeltCreeperChargeController.startSound = "";
        }

        private static void CreateChameleonStingProjectile()
        {
            //highly recommend setting up projectiles in editor, but this is a quick and dirty way to prototype if you want
            ChameleonStingProjectile = Asset.CloneProjectilePrefab("Arrow", "XChameleonStingProjectile");

            // just setting the numbers to 1 as the entitystate will take care of those
            ChameleonStingProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            ChameleonStingProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            ChameleonStingProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.BonusToLowHealth;
            ChameleonStingProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.BypassArmor;
            ChameleonStingProjectile.GetComponent<ProjectileDamage>().damageType |= DamageType.BypassBlock;
            ChameleonStingProjectile.GetComponent<ProjectileDamage>().damageColorIndex = DamageColorIndex.Luminous;


            ProjectileController ChameleonStingController = MeltCreeperProjectile.GetComponent<ProjectileController>();

            //if (_assetBundle.LoadAsset<GameObject>("MCSmallVFX") != null)
            //    MeltCreeperController.ghostPrefab = _assetBundle.CreateProjectileGhostPrefab("MCSmallVFX");

            ChameleonStingController.startSound = "";
        }


        #endregion projectiles
    }
}
