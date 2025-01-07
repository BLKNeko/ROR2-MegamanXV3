using R2API;
using RoR2;
using RoR2.Orbs;
using System;
using UnityEngine;

namespace MegamanXMod.Modules
{
    public class HomingTorpedoOrb : GenericDamageOrb
    {

        // Token: 0x06004097 RID: 16535 RVA: 0x0010B789 File Offset: 0x00109989
        public override void Begin()
        {
            this.speed = 100f;
            base.Begin();
        }

        // Token: 0x06004098 RID: 16536 RVA: 0x0010B79C File Offset: 0x0010999C
        public override GameObject GetOrbEffect()
        {
            return OrbStorageUtility.Get("Prefabs/Effects/OrbEffects/MicroMissileOrbEffect");
        }

    }
}
