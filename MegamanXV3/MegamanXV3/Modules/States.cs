using MegamanXV3.SkillStates;
using MegamanXV3.SkillStates.BaseStates;
using System.Collections.Generic;
using System;

namespace MegamanXV3.Modules
{
    public static class States
    {
        internal static void RegisterStates()
        {
            Modules.Content.AddEntityState(typeof(BaseMeleeAttack));
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(shotgunIce));
            Modules.Content.AddEntityState(typeof(Dash));
            Modules.Content.AddEntityState(typeof(greenSpinner));
            Modules.Content.AddEntityState(typeof(meltCreeper));
            Modules.Content.AddEntityState(typeof(FKBuster));
            Modules.Content.AddEntityState(typeof(squeezeBomb));
            Modules.Content.AddEntityState(typeof(FireW));
            Modules.Content.AddEntityState(typeof(FireW2));
            Modules.Content.AddEntityState(typeof(FireW3));
            Modules.Content.AddEntityState(typeof(AcidBurst));
            Modules.Content.AddEntityState(typeof(AcidBurst2));
            Modules.Content.AddEntityState(typeof(Unlimited));
            Modules.Content.AddEntityState(typeof(DeathState));
            Modules.Content.AddEntityState(typeof(chameleonSting));
            Modules.Content.AddEntityState(typeof(novaStrike));


        }
    }
}