using EntityStates;
using MegamanXV3.Modules;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXV3.SkillStates
{
    public class chameleonSting : BaseSkillState
    {
        public float damageCoefficient = 1.4f;
        public float baseDuration = 2.85f;
        public float recoil = 0.7f;

        private Transform modelTransform;
        private CharacterModel characterModel;


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

            this.modelTransform = base.GetModelTransform();
            if (this.modelTransform)
            {
                this.animator = this.modelTransform.GetComponent<Animator>();
                this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            }

        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void FireCS()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                ShootedCharged = false;
                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                Vector3 raygun1 = new Vector3(aimRay.direction.x + 0.2f, aimRay.direction.y, aimRay.direction.z);
                Vector3 raygun2 = new Vector3(aimRay.direction.x - 0.2f, aimRay.direction.y, aimRay.direction.z);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireShotgun.effectPrefab, base.gameObject, this.muzzleString, false);
                if (base.isAuthority)
                {
                    base.PlayAnimation("Gesture, Override", "ShootBurst", "attackSpeed", this.duration);
                    Util.PlaySound(Sounds.ChameleonSting, base.gameObject);
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.chameleonStingProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.chameleonStingProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(raygun1.normalized), base.gameObject, this.damageCoefficient * 1.25f * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    ProjectileManager.instance.FireProjectile(Modules.Projectiles.chameleonStingProjectile, aimRay.origin, Util.QuaternionSafeLookRotation(raygun2.normalized), base.gameObject, this.damageCoefficient * 1.25f * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                }
            }
        }

        private void FireCSC()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                ShootedCharged = true;
                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FireShotgun.effectPrefab, base.gameObject, this.muzzleString, false);
                if (base.isAuthority)
                {

                    base.PlayAnimation("Gesture, Override", "ShootBurst", "attackSpeed", this.duration);
                    Util.PlaySound(Sounds.ChameleonSting, base.gameObject);
                    //ProjectileManager.instance.FireProjectile(Modules.Projectiles.aBurst, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, this.damageCoefficient * 1.25f * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                    if (NetworkServer.active)
                    {

                        base.characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 8f);

                    }
                    BaseStates.Unlimited.ChameleonIvul = 8f;

                }
            }
        }

        public override void Update()
        {
            base.Update();
            if (base.inputBank.skill4.down)
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

            if (!base.inputBank.skill4.down)
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

           


            if ((base.fixedAge >= this.fireDuration || !base.inputBank || !base.inputBank.skill4.down) && hasCharged == true && hasTime == true)
            {
                FireCSC();
            }

            if ((base.fixedAge >= this.fireDuration || !base.inputBank || !base.inputBank.skill4.down) && hasCharged == false && hasTime == true)
            {
                FireCS();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;
                chargeTime = 0f;
                this.outer.SetNextStateToMain();

            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }

    }
}
