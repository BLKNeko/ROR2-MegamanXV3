using JetBrains.Annotations;
using RoR2;
using RoR2.Skills;
using System;
using System.Collections.Generic;
using System.Text;

namespace MegamanXMod.Modules
{
    public class HuntTrackerSkillDef : SkillDef
    {
        public override SkillDef.BaseSkillInstanceData OnAssigned([NotNull] GenericSkill skillSlot)
        {
            return new HuntTrackerSkillDef.InstanceData
            {
                huntressTracker = skillSlot.GetComponent<HuntressTracker>()
            };
        }
        private static bool HasTarget([NotNull] GenericSkill skillSlot)
        {
            HuntressTracker huntressTracker = ((HuntTrackerSkillDef.InstanceData)skillSlot.skillInstanceData).huntressTracker;
            return (huntressTracker != null) ? huntressTracker.GetTrackingTarget() : null;
        }
        public override bool CanExecute([NotNull] GenericSkill skillSlot)
        {
            return HuntTrackerSkillDef.HasTarget(skillSlot) && base.CanExecute(skillSlot);
        }
        public override bool IsReady([NotNull] GenericSkill skillSlot)
        {
            return base.IsReady(skillSlot) && HuntTrackerSkillDef.HasTarget(skillSlot);
        }
        protected class InstanceData : SkillDef.BaseSkillInstanceData
        {
            public HuntressTracker huntressTracker;
        }

    }
}
