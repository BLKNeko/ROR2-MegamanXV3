using EntityStates;
using MegamanXMod.Survivors.X;
using Rewired;
using RoR2;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class FireBuster : GenericBulletBaseState
    {
        public static float XDamageCoefficient = XStaticValues.gunDamageCoefficient;
        public static float XBaseDuration = 1f;
        public static float XForce = 1000f;
        public static float XRange = 300f;

        // Explosão de fogo no impacto
        public static float XBlastDamageCoefficient = 1f; // multiplicador sobre o dano do personagem
        public static float XBlastRadius = 6f;
        public static float XBlastProcCoefficient = 0.5f;
        public static float XBlastForce = 500f;

        private static GameObject blastEffectPrefab;

        public override void OnEnter()
        {
            // Configura os valores herdados
            damageCoefficient = XDamageCoefficient;
            baseDuration = XBaseDuration / attackSpeedStat;
            force = XForce;
            maxDistance = XRange;

            bulletCount = 1;
            procCoefficient = 1f;

            bulletRadius = 0.9f;
            useSmartCollision = true;

            minSpread = 0f;
            maxSpread = 0f;

            muzzleName = "BusterMuzzPos";

            spreadPitchScale = 1f;
            spreadYawScale = 1f;

            recoilAmplitudeY = 4f;
            recoilAmplitudeX = 2f;

            spreadBloomValue = 1.5f;

            _damageType = DamageTypeCombo.GenericPrimary | DamageType.BypassArmor | DamageType.BypassBlock | DamageType.IgniteOnHit;

            tracerEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/TracerRailgun.prefab").WaitForCompletion();

            hitEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/DLC1/Railgunner/ImpactRailgunLight.prefab").WaitForCompletion();

            muzzleFlashPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab;

            fireSoundString = XStaticValues.X_Mid_Bullet;

            characterBody.SetAimTimer(2f);

            if (!blastEffectPrefab)
                blastEffectPrefab = Addressables.LoadAssetAsync<GameObject>("RoR2/Base/IgniteOnKill/IgniteExplosionVFX.prefab").WaitForCompletion();

            base.OnEnter();
        }

        public override void ModifyBullet(BulletAttack bulletAttack)
        {
            base.ModifyBullet(bulletAttack);

            bool exploded = false;

            bulletAttack.hitCallback = (BulletAttack attack, ref BulletAttack.BulletHit hitInfo) =>
            {
                // Mantém o comportamento padrão (dano do tiro, efeito de hit etc.)
                bool result = BulletAttack.defaultHitCallback(attack, ref hitInfo);

                // Uma explosão por tiro, no primeiro ponto de impacto (inimigo ou cenário)
                if (!exploded)
                {
                    exploded = true;
                    FireImpactBlast(attack, hitInfo.point);
                }

                return result;
            };
        }

        private void FireImpactBlast(BulletAttack attack, Vector3 position)
        {
            BlastAttack blast = new BlastAttack
            {
                attacker = gameObject,
                inflictor = gameObject,
                teamIndex = GetTeam(),
                position = position,
                radius = XBlastRadius,
                baseDamage = damageStat * XBlastDamageCoefficient,
                baseForce = XBlastForce,
                bonusForce = Vector3.zero,
                crit = attack.isCrit,
                procCoefficient = XBlastProcCoefficient,
                falloffModel = BlastAttack.FalloffModel.None,
                damageColorIndex = DamageColorIndex.Default,
                damageType = DamageTypeCombo.GenericPrimary | DamageType.IgniteOnHit,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
            };
            blast.Fire();

            if (blastEffectPrefab)
            {
                EffectManager.SpawnEffect(blastEffectPrefab, new EffectData
                {
                    origin = position,
                    scale = XBlastRadius,
                }, true);
            }
        }

        public override void PlayFireAnimation()
        {
            PlayAnimation(
                "Gesture, Override",
                "XBusterChargeAttack",
                "ShootGun.playbackRate",
                duration);
        }

        public override void OnExit()
        {
            PlayCrossfade(
                    "Gesture, Override",
                    "BufferEmpty",
                    "ShootGun.playbackRate",
                    duration,
                    0.03f);

            base.OnExit();
        }


        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}