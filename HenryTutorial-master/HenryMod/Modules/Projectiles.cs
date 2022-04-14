using R2API;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXV3.Modules
{
    internal static class Projectiles
    {
        internal static GameObject bombPrefab;


        internal static GameObject shotgunIceprefab;

        internal static GameObject chargeProjectile; // prefab for chargeshot

        internal static GameObject greenNProjectile; // prefab for chargeshot

        internal static GameObject meltCreeper; //prefab para o meltCreeper normal

        internal static GameObject meltCreeperC; //prefab para o melt creeper Carregado

        internal static GameObject squeezeBomb; // prefab for Squeeze Bomb

        internal static GameObject shotgunIceCharge; //prefab for shotgunIce charge

        internal static GameObject redShot; //prefab for falcon Buster shot

        internal static GameObject shotFMJ; //prefab for falcon buster CHARGE shot

        internal static GameObject aBurst; //prefab for AcidBurst

        internal static GameObject chameleonStingProjectile; //prefab for Chameleon Sting

        internal static void RegisterProjectiles()
        {
            CreateBomb();
            CreateShotgunIce();
            CreateChargeProjectile();
            CreateGreenNProjectile();
            CreateMeltCreeper();
            CreateMeltCreeperC();
            CreateSqueezeBomb();
            CreateShotFMJ();
            CreateABurst();
            CreateChameleonStingProjectile();

            AddProjectile(bombPrefab);
            AddProjectile(shotgunIceprefab);
            AddProjectile(chargeProjectile);
            AddProjectile(greenNProjectile);
            AddProjectile(meltCreeper);
            AddProjectile(meltCreeperC);
            AddProjectile(squeezeBomb);
            AddProjectile(shotFMJ);
            AddProjectile(aBurst);
            AddProjectile(chameleonStingProjectile);
        }

        internal static void AddProjectile(GameObject projectileToAdd)
        {
            Modules.Content.AddProjectilePrefab(projectileToAdd);
        }

        private static void CreateBomb()
        {
            bombPrefab = CloneProjectilePrefab("CommandoGrenadeProjectile", "HenryBombProjectile");

            ProjectileImpactExplosion bombImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(bombImpactExplosion);

            bombImpactExplosion.blastRadius = 16f;
            bombImpactExplosion.destroyOnEnemy = true;
            bombImpactExplosion.lifetime = 12f;
            bombImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            bombImpactExplosion.timerAfterImpact = true;
            bombImpactExplosion.lifetimeAfterImpact = 0.1f;

            ProjectileController bombController = bombPrefab.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("HenryBombGhost") != null) bombController.ghostPrefab = CreateGhostPrefab("HenryBombGhost");
            bombController.startSound = "";
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
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ShotgunIceGhost") != null) shotgunIceController.ghostPrefab = CreateGhostPrefab("ShotgunIceGhost");
            shotgunIceController.startSound = "";
        }

        private static void CreateChargeProjectile()
        {

            // clone FMJ's syringe projectile prefab here to use as our own projectile
            chargeProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageFirebolt"), "Prefabs/Projectiles/XShotCProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            chargeProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            chargeProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            chargeProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            // register it for networking
            if (chargeProjectile) PrefabAPI.RegisterNetworkPrefab(chargeProjectile);

            ProjectileController chargeProjectileController = chargeProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("ChargeProjectileGhost") != null) chargeProjectileController.ghostPrefab = CreateGhostPrefab("ChargeProjectileGhost");
            chargeProjectileController.startSound = "";
        }

        private static void CreateGreenNProjectile()
        {

            // clone Lunar's syringe projectile prefab here to use as our own projectile
            greenNProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MissileProjectile"), "Prefabs/Projectiles/GreenNProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            ProjectileImpactExplosion greenNImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(greenNImpactExplosion);

            greenNImpactExplosion.blastRadius = 8f;
            greenNImpactExplosion.destroyOnEnemy = true;
            greenNImpactExplosion.lifetime = 22f;
            greenNImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            greenNImpactExplosion.timerAfterImpact = true;
            greenNImpactExplosion.lifetimeAfterImpact = 0.1f;


            // just setting the numbers to 1 as the entitystate will take care of those
            greenNProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            greenNProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            greenNProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;

            // register it for networking
            if (greenNProjectile) PrefabAPI.RegisterNetworkPrefab(greenNProjectile);

            ProjectileController greenNProjectileController = greenNProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("greenNProjectileGhost") != null) greenNProjectileController.ghostPrefab = CreateGhostPrefab("greenNProjectileGhost");
            greenNProjectileController.startSound = "";
        }

        private static void CreateMeltCreeper()
        {

            // clone Lunar's syringe projectile prefab here to use as our own projectile
            //meltCreeper = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageFirewallWalkerProjectile"), "Prefabs/Projectiles/MeltCProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            meltCreeper = CloneProjectilePrefab("MageFirewallWalkerProjectile", "meltCreeper");

            // just setting the numbers to 1 as the entitystate will take care of those
            meltCreeper.GetComponent<ProjectileDamage>().damage = 1f;
            meltCreeper.GetComponent<ProjectileController>().procCoefficient = 1f;
            meltCreeper.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;

            // register it for networking
            if (meltCreeper) PrefabAPI.RegisterNetworkPrefab(meltCreeper);

            ProjectileController meltCreeperController = meltCreeper.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("meltCreeperGhost") != null) meltCreeperController.ghostPrefab = CreateGhostPrefab("meltCreeperGhost");
            meltCreeperController.startSound = "";
        }

        private static void CreateMeltCreeperC()
        {

            // clone Lunar's syringe projectile prefab here to use as our own projectile
            meltCreeperC = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/MageFirewallSeedProjectile"), "Prefabs/Projectiles/MeltCCProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);

            // just setting the numbers to 1 as the entitystate will take care of those
            meltCreeperC.GetComponent<ProjectileDamage>().damage = 1f;
            meltCreeperC.GetComponent<ProjectileController>().procCoefficient = 1f;
            meltCreeperC.GetComponent<ProjectileDamage>().damageType = DamageType.IgniteOnHit;


            // register it for networking
            if (meltCreeperC) PrefabAPI.RegisterNetworkPrefab(meltCreeperC);

            ProjectileController meltCreeperCController = meltCreeperC.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("meltCreeperCGhost") != null) meltCreeperCController.ghostPrefab = CreateGhostPrefab("meltCreeperCGhost");
            meltCreeperCController.startSound = "";
        }

        private static void CreateSqueezeBomb()
        {

            // clone Lunar's syringe projectile prefab here to use as our own projectile
            squeezeBomb = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/GravSphere"), "Prefabs/Projectiles/SqueezeBProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);


            // just setting the numbers to 1 as the entitystate will take care of those
            squeezeBomb.GetComponent<ProjectileDamage>().damage = 1f;
            squeezeBomb.GetComponent<ProjectileController>().procCoefficient = 1f;
            squeezeBomb.GetComponent<ProjectileDamage>().damageType = DamageType.WeakOnHit;

            // register it for networking
            if (squeezeBomb) PrefabAPI.RegisterNetworkPrefab(squeezeBomb);

            ProjectileController squeezeBombController = squeezeBomb.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("squeezeBombGhost") != null) squeezeBombController.ghostPrefab = CreateGhostPrefab("squeezeBombGhost");
            squeezeBombController.startSound = "";
        }

        private static void CreateShotFMJ()
        {
            // clone Lunar's syringe projectile prefab here to use as our own projectile
            shotFMJ = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/FMJ"), "Prefabs/Projectiles/shotFMJProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);


            // just setting the numbers to 1 as the entitystate will take care of those
            shotFMJ.GetComponent<ProjectileDamage>().damage = 1f;
            shotFMJ.GetComponent<ProjectileController>().procCoefficient = 1f;
            shotFMJ.GetComponent<ProjectileDamage>().damageType = DamageType.BypassArmor;

            // register it for networking
            if (shotFMJ) PrefabAPI.RegisterNetworkPrefab(shotFMJ);

            ProjectileImpactExplosion shotFMJImpactExplosion = bombPrefab.GetComponent<ProjectileImpactExplosion>();
            InitializeImpactExplosion(shotFMJImpactExplosion);

            shotFMJImpactExplosion.blastRadius = 5f;
            shotFMJImpactExplosion.destroyOnEnemy = true;
            shotFMJImpactExplosion.lifetime = 22f;
            shotFMJImpactExplosion.impactEffect = Modules.Assets.bombExplosionEffect;
            //bombImpactExplosion.lifetimeExpiredSound = Modules.Assets.CreateNetworkSoundEventDef("HenryBombExplosion");
            shotFMJImpactExplosion.timerAfterImpact = true;
            shotFMJImpactExplosion.lifetimeAfterImpact = 0.1f;


            ProjectileController shotFMJController = shotFMJ.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("shotFMJGhost") != null) shotFMJController.ghostPrefab = CreateGhostPrefab("shotFMJGhost");
            shotFMJController.startSound = "";
        }

        private static void CreateABurst()
        {

            // clone Lunar's syringe projectile prefab here to use as our own projectile
            aBurst = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/PoisonOrbProjectile"), "Prefabs/Projectiles/RShieldProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);


            // just setting the numbers to 1 as the entitystate will take care of those
            aBurst.GetComponent<ProjectileDamage>().damage = 1f;
            aBurst.GetComponent<ProjectileController>().procCoefficient = 1f;
            aBurst.GetComponent<ProjectileDamage>().damageType = DamageType.PoisonOnHit;

            // register it for networking
            if (aBurst) PrefabAPI.RegisterNetworkPrefab(aBurst);

            ProjectileController aBurstController = aBurst.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("aBurstGhost") != null) aBurstController.ghostPrefab = CreateGhostPrefab("aBurstGhost");
            aBurstController.startSound = "";
        }

        private static void CreateChameleonStingProjectile()
        {

            // clone Lunar's syringe projectile prefab here to use as our own projectile
            chameleonStingProjectile = PrefabAPI.InstantiateClone(Resources.Load<GameObject>("prefabs/projectiles/Arrow"), "Prefabs/Projectiles/ChameleonStingProjectile", true, "C:\\Users\\test\\Documents\\ror2mods\\MegamanX\\MegamanX\\MegamanX\\MegamanX.cs", "RegisterCharacter", 155);


            // just setting the numbers to 1 as the entitystate will take care of those
            chameleonStingProjectile.GetComponent<ProjectileDamage>().damage = 1f;
            chameleonStingProjectile.GetComponent<ProjectileController>().procCoefficient = 1f;
            chameleonStingProjectile.GetComponent<ProjectileDamage>().damageType = DamageType.BonusToLowHealth;

            // register it for networking
            if (chameleonStingProjectile) PrefabAPI.RegisterNetworkPrefab(chameleonStingProjectile);

            ProjectileController chameleonStingProjectileController = chameleonStingProjectile.GetComponent<ProjectileController>();
            if (Modules.Assets.mainAssetBundle.LoadAsset<GameObject>("chameleonStingProjectileGhost") != null) chameleonStingProjectileController.ghostPrefab = CreateGhostPrefab("chameleonStingProjectileGhost");
            chameleonStingProjectileController.startSound = "";
        }

        private static void InitializeImpactExplosion(ProjectileImpactExplosion projectileImpactExplosion)
        {
            projectileImpactExplosion.blastDamageCoefficient = 1f;
            projectileImpactExplosion.blastProcCoefficient = 1f;
            projectileImpactExplosion.blastRadius = 1f;
            projectileImpactExplosion.bonusBlastForce = Vector3.zero;
            projectileImpactExplosion.childrenCount = 0;
            projectileImpactExplosion.childrenDamageCoefficient = 0f;
            projectileImpactExplosion.childrenProjectilePrefab = null;
            projectileImpactExplosion.destroyOnEnemy = false;
            projectileImpactExplosion.destroyOnWorld = false;
            projectileImpactExplosion.falloffModel = RoR2.BlastAttack.FalloffModel.None;
            projectileImpactExplosion.fireChildren = false;
            projectileImpactExplosion.impactEffect = null;
            projectileImpactExplosion.lifetime = 0f;
            projectileImpactExplosion.lifetimeAfterImpact = 0f;
            projectileImpactExplosion.lifetimeRandomOffset = 0f;
            projectileImpactExplosion.offsetForLifetimeExpiredSound = 0f;
            projectileImpactExplosion.timerAfterImpact = false;

            projectileImpactExplosion.GetComponent<ProjectileDamage>().damageType = DamageType.Generic;
        }

        private static GameObject CreateGhostPrefab(string ghostName)
        {
            GameObject ghostPrefab = Modules.Assets.mainAssetBundle.LoadAsset<GameObject>(ghostName);
            if (!ghostPrefab.GetComponent<NetworkIdentity>()) ghostPrefab.AddComponent<NetworkIdentity>();
            if (!ghostPrefab.GetComponent<ProjectileGhostController>()) ghostPrefab.AddComponent<ProjectileGhostController>();

            Modules.Assets.ConvertAllRenderersToHopooShader(ghostPrefab);

            return ghostPrefab;
        }

        private static GameObject CloneProjectilePrefab(string prefabName, string newPrefabName)
        {
            GameObject newPrefab = PrefabAPI.InstantiateClone(RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Projectiles/" + prefabName), newPrefabName);
            return newPrefab;
        }
    }
}