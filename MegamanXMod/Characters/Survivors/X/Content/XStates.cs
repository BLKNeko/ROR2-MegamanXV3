using MegamanXMod.Survivors.X.SkillStates;

namespace MegamanXMod.Survivors.X
{
    public static class XStates
    {
        public static void Init()
        {
            Modules.Content.AddEntityState(typeof(SlashCombo));

            Modules.Content.AddEntityState(typeof(Shoot));

            Modules.Content.AddEntityState(typeof(Roll));

            Modules.Content.AddEntityState(typeof(ThrowBomb));

            Modules.Content.AddEntityState(typeof(Shoot2));

            Modules.Content.AddEntityState(typeof(XBuster));

            Modules.Content.AddEntityState(typeof(HyperModeLightArmor));

            Modules.Content.AddEntityState(typeof(HyperModeSecondArmor));

            Modules.Content.AddEntityState(typeof(HyperModeMaxArmor));

            Modules.Content.AddEntityState(typeof(HyperModeFourthArmor));
        }
    }
}
