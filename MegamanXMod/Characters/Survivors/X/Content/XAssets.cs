using RoR2;
using UnityEngine;
using MegamanXMod.Modules;
using System;
using RoR2.Projectile;
using R2API;

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


        public static Mesh XBodyMesh;
        public static Mesh LightBodyMesh;
        public static Mesh SecondBodyMesh;
        public static Mesh MaxBodyMesh;
        public static Mesh FourthBodyMesh;
        public static Mesh FalconBodyMesh;
        public static Mesh GaeaBodyMesh;


        
        public static Material MatX;
        public static Material MatLight;
        public static Material MatSecond;
        public static Material MatMax;
        public static Material MatFourth;
        public static Material MatFalcon;
        public static Material MatGaea;

        public static Sprite IconX;
        public static Sprite IconLight;
        public static Sprite IconSecond;
        public static Sprite IconMax;
        public static Sprite IconFourth;
        public static Sprite IconFalcon;
        public static Sprite IconGaea;

        public static Sprite IconFalconDash;
        public static Sprite IconSkillLock;


        // networked hit sounds
        public static NetworkSoundEventDef swordHitSoundEvent;

        //projectiles
        public static GameObject bombProjectilePrefab;

        public static GameObject shurikenProjectilePrefab;
        public static GameObject shurikenProjectilePrefab2;
        internal static GameObject shotFMJ;
        internal static GameObject shotgunIceprefab;

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

            MatX = _assetBundle.LoadAsset<Material>("matX");
            MatLight = _assetBundle.LoadAsset<Material>("matLight");
            MatSecond = _assetBundle.LoadAsset<Material>("matSecond");
            MatMax = _assetBundle.LoadAsset<Material>("matMax");
            MatFourth = _assetBundle.LoadAsset<Material>("matFourth");
            MatFalcon = _assetBundle.LoadAsset<Material>("matFalcon");
            MatGaea = _assetBundle.LoadAsset<Material>("matGaea");

            IconX = _assetBundle.LoadAsset<Sprite>("XIcon");
            IconLight = _assetBundle.LoadAsset<Sprite>("XLightIcon");
            IconSecond = _assetBundle.LoadAsset<Sprite>("XSecondIcon");
            IconMax = _assetBundle.LoadAsset<Sprite>("XMaxIcon");
            IconFourth = _assetBundle.LoadAsset<Sprite>("XFourthIcon");
            IconFalcon = _assetBundle.LoadAsset<Sprite>("XFalconIcon");
            IconGaea = _assetBundle.LoadAsset<Sprite>("XGaeaIcon");

            IconFalconDash = _assetBundle.LoadAsset<Sprite>("XSkillFalconDash");
            IconSkillLock = _assetBundle.LoadAsset<Sprite>("XSkillLock");

            HyperModeEffect = _assetBundle.LoadEffect("HyperModeEffect", true);
            ShurikenVFX = _assetBundle.LoadEffect("ShurikenVFX", false);


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

            Content.AddProjectilePrefab(bombProjectilePrefab);
            Content.AddProjectilePrefab(shurikenProjectilePrefab);
            Content.AddProjectilePrefab(shurikenProjectilePrefab2);
            Content.AddProjectilePrefab(shotgunIceprefab);
            Content.AddProjectilePrefab(shotFMJ);
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


        #endregion projectiles
    }
}
