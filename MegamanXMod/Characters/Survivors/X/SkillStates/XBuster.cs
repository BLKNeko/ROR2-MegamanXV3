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
    public class XBuster : BaseSkillState
    {
        public static float damageCoefficient = HenryStaticValues.XBusterDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 1f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 400f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");
        public static GameObject MediumtracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("prefabs/effects/tracers/TracerBanditShotgun");
        private GameObject ShurikenTracerGO;
        private Tracer ShurikenTracer;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

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
            muzzleString = "Muzzle";

            PlayAnimation("LeftArm, Override", "XBusterGun", "XBusterGun.playbackRate", 1.8f);


            //ShurikenTracer.beamObject = XAssets.ShurikenVFX;
            //ShurikenTracer.speed = 200f;
            //ShurikenTracer.beamDensity = 10f;
            //ShurikenTracer.length = 8f;
            //ShurikenTracer.headTransform = XAssets.ShurikenVFX.transform;
            //ShurikenTracer.tailTransform = XAssets.ShurikenVFX.transform;
            //ShurikenTracer.startTransform = characterBody.transform;
            //ShurikenTracer.reverse = false;

            ////ShurikenTracerGO = XAssets.ShurikenVFX;

            //ShurikenTracerGO.AddComponent<Tracer>();

            //ShurikenTracerGO.GetComponent<Tracer>().beamObject = XAssets.ShurikenVFX;
            //ShurikenTracerGO.GetComponent<Tracer>().speed = 200f;
            //ShurikenTracerGO.GetComponent<Tracer>().beamDensity = 10f;
            //ShurikenTracerGO.GetComponent<Tracer>().length = 8f;
            //ShurikenTracerGO.GetComponent<Tracer>().headTransform = characterBody.transform;
            //ShurikenTracerGO.GetComponent<Tracer>().tailTransform = characterBody.transform;
            //ShurikenTracerGO.GetComponent<Tracer>().startTransform = characterBody.transform;
            //ShurikenTracerGO.GetComponent<Tracer>().reverse = false;

            //ShurikenTracerGO.AddComponent<LineRenderer>();

            //ShurikenTracerGO.GetComponent<LineRenderer>()

        }

        public override void OnExit()
        {
            chargeTime = 0f;
            chargeLevel = 0;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            // Carregar o ataque
            if (base.inputBank.skill1.down)
            {
                ChargeShot();
            }
            else // Soltar o ataque
            {
                ReleaseChargeShot();
            }


            //if (fixedAge >= fireTime)
            //{
            //    FireSimpleBullet();
            //}

            //if (fixedAge >= duration && isAuthority)
            //{
            //    outer.SetNextStateToMain();
            //    return;
            //}

            if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 3 && hasTime == true)
            {
                FireChargedBullet();
            } 
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 2 && hasTime == true)
            {
                FireMediumBullet();
            }
            else if ((base.fixedAge >= this.fireTime || !base.inputBank || !base.inputBank.skill1.down) && chargeLevel == 1 && hasTime == true)
            {
                FireSimpleBullet();
            }

            if (base.fixedAge >= this.duration && base.isAuthority && hasTime == true)
            {
                hasTime = false;
                
                this.outer.SetNextStateToMain();
            }

        }

        private void FireSimpleBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    Vector3 raygun1 = new Vector3(aimRay.direction.x + 0.15f, aimRay.direction.y, aimRay.direction.z);
                    Vector3 raygun2 = new Vector3(aimRay.direction.x - 0.15f, aimRay.direction.y, aimRay.direction.z);
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    base.PlayAnimation("Gesture, Override", "ShootBurst", "attackSpeed", this.duration);
                    //Util.PlaySound(Sounds.xChargeShot, base.gameObject);
                    //ProjectileManager.instance.FireProjectile(XAssets.shurikenProjectilePrefab2, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, damageCoefficient * this.damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);

                    FireProjectileInfo ShadowShurikenProjectille = new FireProjectileInfo();
                    ShadowShurikenProjectille.projectilePrefab = XAssets.shurikenProjectilePrefab2;
                    ShadowShurikenProjectille.position = aimRay.origin;
                    ShadowShurikenProjectille.rotation = Util.QuaternionSafeLookRotation(aimRay.direction);
                    ShadowShurikenProjectille.owner = gameObject;
                    ShadowShurikenProjectille.damage = damageCoefficient * damageStat;
                    ShadowShurikenProjectille.force = force;
                    ShadowShurikenProjectille.crit = RollCrit();
                    //ShadowShurikenProjectille.speedOverride = 20f;
                    ShadowShurikenProjectille.damageColorIndex = DamageColorIndex.Default;

                    

                    FireProjectileInfo ShadowShurikenProjectille2 = new FireProjectileInfo();
                    ShadowShurikenProjectille2.projectilePrefab = XAssets.shurikenProjectilePrefab2;
                    ShadowShurikenProjectille2.position = aimRay.origin;
                    ShadowShurikenProjectille2.rotation = Util.QuaternionSafeLookRotation(raygun1.normalized);
                    ShadowShurikenProjectille2.owner = gameObject;
                    ShadowShurikenProjectille2.damage = damageCoefficient * damageStat;
                    ShadowShurikenProjectille2.force = force;
                    ShadowShurikenProjectille2.crit = RollCrit();
                    //ShadowShurikenProjectille2.speedOverride = 20f;
                    ShadowShurikenProjectille2.damageColorIndex = DamageColorIndex.Default;

                    

                    FireProjectileInfo ShadowShurikenProjectille3 = new FireProjectileInfo();
                    ShadowShurikenProjectille3.projectilePrefab = XAssets.shurikenProjectilePrefab2;
                    ShadowShurikenProjectille3.position = aimRay.origin;
                    ShadowShurikenProjectille3.rotation = Util.QuaternionSafeLookRotation(raygun2.normalized);
                    ShadowShurikenProjectille3.owner = gameObject;
                    ShadowShurikenProjectille3.damage = damageCoefficient * damageStat;
                    ShadowShurikenProjectille3.force = force;
                    ShadowShurikenProjectille3.crit = RollCrit();
                    //ShadowShurikenProjectille2.speedOverride = 20f;
                    ShadowShurikenProjectille3.damageColorIndex = DamageColorIndex.Default;



                    ProjectileManager.instance.FireProjectile(ShadowShurikenProjectille);
                    ProjectileManager.instance.FireProjectile(ShadowShurikenProjectille2);
                    ProjectileManager.instance.FireProjectile(ShadowShurikenProjectille3);

                    //new BulletAttack
                    //{
                    //    bulletCount = 1,
                    //    aimVector = aimRay.direction,
                    //    origin = aimRay.origin,
                    //    damage = damageCoefficient * damageStat,
                    //    damageColorIndex = DamageColorIndex.Default,
                    //    damageType = DamageType.Generic,
                    //    falloffModel = BulletAttack.FalloffModel.None,
                    //    maxDistance = range,
                    //    force = force,
                    //    hitMask = LayerIndex.CommonMasks.bullet,
                    //    minSpread = 0f,
                    //    maxSpread = 0f,
                    //    isCrit = RollCrit(),
                    //    owner = gameObject,
                    //    muzzleName = muzzleString,
                    //    smartCollision = true,
                    //    procChainMask = default,
                    //    procCoefficient = procCoefficient,
                    //    radius = 0.75f,
                    //    sniper = false,
                    //    stopperMask = LayerIndex.CommonMasks.bullet,
                    //    weapon = null,
                    //    tracerEffectPrefab = ShurikenTracerGO,
                    //    spreadPitchScale = 1f,
                    //    spreadYawScale = 1f,
                    //    queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                    //    hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FirePistol2.hitEffectPrefab,
                    //}.Fire();
                }
            }
        }

        private void FireMediumBullet()
        {
            if (!hasFired)
            {
                hasFired = true;

                characterBody.AddSpreadBloom(0.8f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    

                    new BulletAttack
                    {
                        bulletCount = 1,
                        aimVector = aimRay.direction,
                        origin = aimRay.origin,
                        damage = (damageCoefficient * 2f) * damageStat,
                        damageColorIndex = DamageColorIndex.Default,
                        damageType = DamageType.Generic,
                        falloffModel = BulletAttack.FalloffModel.None,
                        maxDistance = range,
                        force = force,
                        hitMask = LayerIndex.CommonMasks.bullet,
                        minSpread = 0f,
                        maxSpread = 0f,
                        isCrit = RollCrit(),
                        owner = gameObject,
                        muzzleName = muzzleString,
                        smartCollision = true,
                        procChainMask = default,
                        procCoefficient = procCoefficient,
                        radius = 0.75f,
                        sniper = false,
                        stopperMask = LayerIndex.CommonMasks.bullet,
                        weapon = null,
                        tracerEffectPrefab = tracerEffectPrefab,
                        spreadPitchScale = 1f,
                        spreadYawScale = 1f,
                        queryTriggerInteraction = QueryTriggerInteraction.UseGlobal,
                        hitEffectPrefab = EntityStates.Commando.CommandoWeapon.FireShotgun.hitEffectPrefab,
                    }.Fire();
                }
            }
        }

        private void FireChargedBullet()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;
                base.characterBody.AddSpreadBloom(0.75f);
                Ray aimRay = base.GetAimRay();
                EffectManager.SimpleMuzzleFlash(EntityStates.Mage.Weapon.FireRoller.fireMuzzleflashEffectPrefab, base.gameObject, this.muzzleString, false);
                if (base.isAuthority)
                {
                    base.PlayAnimation("Gesture, Override", "ShootBurst", "attackSpeed", this.duration);
                    //Util.PlaySound(Sounds.xChargeShot, base.gameObject);
                    ProjectileManager.instance.FireProjectile(XAssets.bombProjectilePrefab, aimRay.origin, Util.QuaternionSafeLookRotation(aimRay.direction), base.gameObject, (damageCoefficient * 4f) * damageStat, 0f, Util.CheckRoll(this.critStat, base.characterBody.master), DamageColorIndex.Default, null, -1f);
                }
            }
        }

        //CHARGE LOGIC

        private void ChargeShot()
        {
            chargeTime += Time.fixedDeltaTime;
            base.characterBody.SetAimTimer(2f);

            if (chargeTime > Level1ChargeTime && chargeTime <= Level2ChargeTime && !chargingSFX)
            {
                PlayChargingEffects(1);
                chargingSFX = true;
            }

            if (chargeTime >= Level2ChargeTime && !chargeFullSFX)
            {
                PlayChargingEffects(2);
                chargeFullSFX = true;
                lastChargeTime = chargeTime;
            }

            if ((chargeTime - lastChargeTime) >= ChargeInterval && chargeFullSFX)
            {
                PlayChargingEffects(2);
                lastChargeTime = chargeTime;
            }
        }

        private void ReleaseChargeShot()
        {
            // Determina o nível de carregamento com base no tempo
            if (chargeTime >= Level2ChargeTime)
            {
                chargeLevel = 3; // Nível máximo de carregamento
            }
            else if (chargeTime >= Level1ChargeTime)
            {
                chargeLevel = 2; // Nível intermediário de carregamento
            }
            else
            {
                chargeLevel = 1; // Nível mínimo de carregamento
            }

            chargingSFX = false;
            chargeFullSFX = false;
            hasTime = true;
            //chargeTime = 0; // Reseta o tempo de carga - movido para o onexit

        }

        private void PlayChargingEffects(int level)
        {
            switch (level)
            {
                case 1:
                    //Util.PlaySound(Sounds.charging, base.gameObject);
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1C, base.gameObject, "Center", true);
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect1W, base.gameObject, "Center", true);
                    break;

                case 2:
                    //Util.PlaySound(Sounds.fullCharge, base.gameObject);
                    //EffectManager.SimpleMuzzleFlash(Modules.Assets.chargeeffect2C, base.gameObject, "Center", true);
                    break;

                default:
                    break;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}