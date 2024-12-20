using RoR2;
using UnityEngine;
using MegamanXMod.Modules;
using System;
using RoR2.Projectile;

namespace MegamanXMod.Survivors.X
{
    public static class XAssets
    {
        // particle effects
        public static GameObject swordSwingEffect;
        public static GameObject swordHitImpactEffect;

        public static GameObject bombExplosionEffect;

        public static GameObject HyperModeEffect;


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
            Content.AddProjectilePrefab(bombProjectilePrefab);
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
        #endregion projectiles
    }
}
