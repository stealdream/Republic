using System;
using System.Collections.Generic;
using System.Text;
using ICities;
using UnityEngine;
using GroogyLib.Core;

namespace Republic
{
    public class RepublicMilestonesInterface : IMilestonesExtension
    {
        public void OnCreated(IMilestones milestoneInterface)
        {
        }

        public int OnGetPopulationTarget(int originalTarget, int scaledTarget)
        {
            return scaledTarget * 2;
        }

        public void OnRefreshMilestones()
        {
        }

        public void OnReleased()
        {
        }
    }

    public class MilestonesManager
    {
        private IMilestones milestonesInterface = null;

        public MilestonesManager(IMilestones milestonesInterface)
        {
            this.milestonesInterface = milestonesInterface;
        }

        public void DumpMilestones()
        {
            string[] milestones = this.milestonesInterface.EnumerateMilestones();
            for (int index = 0; index < milestones.Length; index++)
            {
                RepublicCore.Instance.Debugger.Log(milestones[index]);
            }
        }

        public void Update()
        {

        }
    }
}
