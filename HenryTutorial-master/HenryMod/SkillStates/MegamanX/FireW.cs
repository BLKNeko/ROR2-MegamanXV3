using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;

namespace MegamanXV3.SkillStates
{
    public class FireW : BaseSkillState
    {
        public float damageCoefficient = 0.9f;
        public float baseDuration = 0.001f;
        public float recoil = 0.5f;
        //public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("Prefabs/Effects/Tracers/TracerToolbotRebar");
        public static GameObject tracerEffectPrefab = Resources.Load<GameObject>("prefabs/effects/tracers/TracerEmbers");
        public static GameObject hitEffectPrefab = Resources.Load<GameObject>("prefabs/effects/impacteffects/Hitspark1");

        public float chargeTime = 0f;
        public float LastChargeTime = 0f;
        public bool chargeFullSFX = false;
        public bool hasTime = false;
        public bool hasCharged = false;
        public bool chargingSFX = false;
        public bool ShootedCharged;

        private float duration;
        private float fireDuration;
        private bool hasFired;
        private Animator animator;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = this.baseDuration / this.attackSpeedStat;
            this.fireDuration = 0.25f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.animator = base.GetModelAnimator();
            this.muzzleString = "Weapon";


            base.PlayAnimation("Gesture, Override", "ShootPose", "attackSpeed", this.duration);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireFT()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                ShootedCharged = false;
                base.characterBody.AddSpreadBloom(0.05f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    Util.PlaySound(Sounds.FireWave, base.gameObject);
                    Util.PlaySound(Sounds.FireWaveSFX, base.gameObject);

                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0.1f,
                        maxSpread = 0.4f,
                        damage = damageCoefficient * this.damageStat,
                        force = 20f,
                        tracerEffectPrefab = FireW.tracerEffectPrefab,
                        muzzleName = muzzleString,
                        hitEffectPrefab = FireW.hitEffectPrefab,
                        maxDistance = 30f,
                        smartCollision = true,
                        damageType = DamageType.IgniteOnHit,
                        isCrit = Util.CheckRoll(this.critStat, base.characterBody.master)
                    }.Fire();
                }
            }
        }

        private void FireWC()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                ShootedCharged = true;
                base.characterBody.AddSpreadBloom(0.05f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);

                if (base.isAuthority)
                {
                    Util.PlaySound(Sounds.FireWave, base.gameObject);
                    Util.PlaySound(Sounds.FireWaveSFX, base.gameObject);

                    new BulletAttack
                    {
                        owner = base.gameObject,
                        weapon = base.gameObject,
                        origin = aimRay.origin,
                        aimVector = aimRay.direction,
                        minSpread = 0.1f,
                        maxSpread = 0.4f,
                        damage = damageCoefficient * this.damageStat,
                        force = 20f,
                        tracerEffectPrefab = FireW.tracerEffectPrefab,
                        muzzleName = muzzleString,
                        hitEffectPrefab = FireW.hitEffectPrefab,
                        maxDistance = 30f,
                        smartCollision = true,
                        damageType = DamageType.IgniteOnHit,
                        isCrit = Util.CheckRoll(this.critStat, base.characterBody.master)
                    }.Fire();
                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (base.inputBank.skill2.down)
            {
                chargeTime += Time.deltaTime;
                base.characterBody.SetAimTimer(2f);

                if (chargeTime > 0.5f && chargeTime <= 1.8f && chargingSFX == false)
                {
                    Util.PlaySound(Sounds.charging, base.gameObject);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1C, base.gameObject, "Center", true);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1W, base.gameObject, "Center", true);
                    chargingSFX = true;
                }

                if (chargeTime >= 1.8f && chargeFullSFX == false)
                {
                    Util.PlaySound(Sounds.fullCharge, base.gameObject);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect2C, base.gameObject, "Center", true);
                    chargeFullSFX = true;
                    LastChargeTime = chargeTime;
                }

                if ((chargeTime - LastChargeTime) >= 0.68f && chargeFullSFX == true)
                {
                    Util.PlaySound(Sounds.fullCharge, base.gameObject);
                    EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect2C, base.gameObject, "Center", true);
                    LastChargeTime = chargeTime;
                }
            }

            if (!base.inputBank.skill2.down)
            {
                if (chargeTime >= 1.8f)
                    hasCharged = true;
                chargingSFX = false;
                hasTime = true;
            }

        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if ((base.fixedAge >= this.fireDuration || !base.inputBank || !base.inputBank.skill2.down) && hasCharged == true && hasTime == true)
            {
                FireWC();
            }

            if ((base.fixedAge >= this.fireDuration || !base.inputBank || !base.inputBank.skill2.down) && hasCharged == false && hasTime == true)
            {
                FireFT();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;
                chargeTime = 0f;
                FireW2 FW2 = new FireW2();
                FireW3 FW3 = new FireW3();
                if (ShootedCharged)
                    this.outer.SetNextState(FW3);
                else
                    this.outer.SetNextState(FW2);
            }

        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}
