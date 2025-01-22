using EntityStates;
using MegamanXMod.Survivors.X;
using R2API;
using RoR2;
using RoR2.Projectile;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class XFireWave3 : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.XFireWaveDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 200f;
        public static float recoil = 3f;
        public static float range = 50f;
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/TracerEmbers");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/FireMeatBallExplosion");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        private BulletAttack FireWave2BulletAttack;
        private int repeatFire;

        private const float Level1ChargeTime = 0.5f; // Tempo para ativar o primeiro nível de carregamento
        private const float Level2ChargeTime = 1.8f; // Tempo para ativar o segundo nível de carregamento
        private const float ChargeInterval = 0.68f;  // Intervalo entre efeitos de carregamento completo

        private float chargeTime = 0f;
        private float lastChargeTime = 0f;
        private bool chargeFullSFX = false;
        private bool hasTime = false;
        private int chargeLevel = 0;
        private bool chargingSFX = false;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration / attackSpeedStat;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(2f);
            muzzleString = "BusterMuzzPos";

            

        }

        public override void OnExit()
        {
            //base.PlayAnimation("Gesture, Override", "BufferEmpty", "attackSpeed", this.duration);
            chargeTime = 0f;
            chargeLevel = 0;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                FireWaveAttack();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                if (repeatFire <= 80)
                {
                    repeatFire++;
                    FireWaveAttack();
                    base.characterBody.SetAimTimer(1f);
                }
                else
                {
                    this.outer.SetNextStateToMain();
                }
                //FireW2 FTR2 = new FireW2();
                //this.outer.SetNextState(FTR2);
            }
        }

        private void FireWaveAttack()
        {
            if (!hasFired)
            {
                //hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);
                PlayAnimation("Gesture, Override", "XBusterAttack", "attackSpeed", this.duration);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    //AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    //if (repeatFire == 1)
                    //    Util.PlaySound(Sounds.FireWaveSFX, base.gameObject);

                    //if (repeatFire % 10 == 0 && repeatFire > 10 && repeatFire < 140)
                    //    Util.PlaySound(Sounds.FireWaveSFX, base.gameObject);

                    FireWave2BulletAttack = new BulletAttack();
                    FireWave2BulletAttack.bulletCount = 1;
                    FireWave2BulletAttack.aimVector = aimRay.direction;
                    FireWave2BulletAttack.origin = aimRay.origin;
                    FireWave2BulletAttack.damage = (damageCoefficient * XStaticValues.XMidChargeDamageCoefficient) * damageStat;
                    FireWave2BulletAttack.damageColorIndex = DamageColorIndex.Default;
                    FireWave2BulletAttack.damageType = DamageType.IgniteOnHit;
                    FireWave2BulletAttack.falloffModel = BulletAttack.FalloffModel.None;
                    FireWave2BulletAttack.maxDistance = range;
                    FireWave2BulletAttack.force = force;
                    FireWave2BulletAttack.hitMask = LayerIndex.CommonMasks.bullet;
                    FireWave2BulletAttack.minSpread = 0f;
                    FireWave2BulletAttack.maxSpread = 0f;
                    FireWave2BulletAttack.isCrit = RollCrit();
                    FireWave2BulletAttack.owner = gameObject;
                    FireWave2BulletAttack.muzzleName = muzzleString;
                    FireWave2BulletAttack.smartCollision = true;
                    FireWave2BulletAttack.procChainMask = default;
                    FireWave2BulletAttack.procCoefficient = procCoefficient;
                    FireWave2BulletAttack.radius = 0.75f;
                    FireWave2BulletAttack.sniper = false;
                    FireWave2BulletAttack.stopperMask = LayerIndex.CommonMasks.bullet;
                    FireWave2BulletAttack.weapon = null;
                    FireWave2BulletAttack.tracerEffectPrefab = tracerEffectPrefab;
                    FireWave2BulletAttack.spreadPitchScale = 1f;
                    FireWave2BulletAttack.spreadYawScale = 1f;
                    FireWave2BulletAttack.queryTriggerInteraction = QueryTriggerInteraction.UseGlobal;
                    FireWave2BulletAttack.hitEffectPrefab = hitEffectPrefab;

                    FireWave2BulletAttack.Fire();
                }
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}