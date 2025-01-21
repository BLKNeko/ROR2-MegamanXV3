using EntityStates;
using MegamanXMod.Survivors.X;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace MegamanXMod.Survivors.X.SkillStates
{
    public class GigaAttackGaea : BaseSkillState
    {
        public static float damageCoefficient = XStaticValues.GigaAttackGaeaDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 5f;
        //delay on firing is usually ass-feeling. only set this if you know what you're doing
        public static float firePercentTime = 0.0f;
        public static float force = 300f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private BlastAttack gaeaBlastAttack;

        private Vector3 gaeaGAPos;
        private float gaeasize = 5f;

        private Transform modelTransform;
        private CharacterModel characterModel;
        private SkinnedMeshRenderer meshRenderer;
        private ChildLocator childLocator;

        private float SkillTime = 5f;
        private float SkillTimer = 0f;

        private float timer = 0f;
        private float timeLimit = 0.1f;

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            duration = baseDuration;
            fireTime = firePercentTime * duration;
            characterBody.SetAimTimer(5f);
            muzzleString = "GaeaGAPosition";

            this.childLocator = base.GetModelTransform().GetComponent<ChildLocator>();
            //Debug.Log("ChildLocator" + childLocator);

            //childLocator = characterModel.GetComponent<ChildLocator>();

            PlayAnimation("FullBody, Override", "GaeaGA", "HyperMode.playbackRate", duration);

            //this.modelTransform = base.GetModelTransform();
            //this.characterModel = this.modelTransform.GetComponent<CharacterModel>();
            //childLocator = this.characterModel.GetComponent<ChildLocator>();

            if (NetworkServer.active)
            {
                characterBody.AddTimedBuff(RoR2Content.Buffs.Immune, 6f);
            }

            gaeaGAPos = childLocator.FindChild("GaeaGAPosition").transform.position;
            //Debug.Log("GaeaPOS" + gaeaGAPos);

            EffectManager.SimpleMuzzleFlash(XAssets.GaeaGAVFX, gameObject, muzzleString, true);

            //EffectManager.SpawnEffect(XAssets.GaeaGAVFX, new EffectData
            //{
            //    origin = childLocator.FindChild("GaeaGAPosition").transform.position,
            //    scale = 12f,

            //}, true);

        }

        public override void OnExit()
        {
            gaeasize = 6f;
            base.OnExit();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            SkillTimer += Time.fixedDeltaTime;
            timer += Time.fixedDeltaTime;

            if ((timer > timeLimit) && SkillTime > SkillTimer)
            {
                FireGigaAttack();
                gaeasize += 0.1f;
                timer = 0f;
            }

            if ((fixedAge >= duration && isAuthority) && SkillTime < SkillTimer)
            {
                SkillTimer = 0f;
                PlayAnimation("FullBody, Override", "BufferEmpty", "HyperMode.playbackRate", duration);
                outer.SetNextStateToMain();
                return;
            }

            

        }

        private void FireGigaAttack()
        {
                //characterBody.AddSpreadBloom(0.8f);
                //EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, gameObject, muzzleString, false);
                Util.PlaySound("HenryXBusterPistol", gameObject);

                if (isAuthority)
                {
                    Ray aimRay = GetAimRay();
                    //AddRecoil(-1f * recoil, -2f * recoil, -0.5f * recoil, 0.5f * recoil);

                    //Util.PlaySound(Sounds.xChargeShot, base.gameObject);

                    gaeaBlastAttack = new BlastAttack();
                    gaeaBlastAttack.attacker = base.gameObject;
                    gaeaBlastAttack.inflictor = base.gameObject;
                    gaeaBlastAttack.teamIndex = TeamComponent.GetObjectTeam(base.gameObject);
                    gaeaBlastAttack.baseDamage = damageCoefficient * damageStat;
                    gaeaBlastAttack.baseForce = force;
                    //gaeaBlastAttack.position = base.characterBody.corePosition;
                    gaeaBlastAttack.position = gaeaGAPos;
                    gaeaBlastAttack.radius = gaeasize;
                    gaeaBlastAttack.bonusForce = new Vector3(1f, 1f, 1f);
                    gaeaBlastAttack.damageType |= DamageType.PoisonOnHit;
                    gaeaBlastAttack.damageType |= DamageType.Shock5s;
                    gaeaBlastAttack.damageType |= DamageType.WeakOnHit;
                    gaeaBlastAttack.damageType |= DamageType.BypassArmor;
                    gaeaBlastAttack.damageType |= DamageType.SlowOnHit;
                    gaeaBlastAttack.damageColorIndex = DamageColorIndex.Poison;

                    gaeaBlastAttack.Fire();
                }
            
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}