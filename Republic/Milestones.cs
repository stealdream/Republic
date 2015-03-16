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
        private bool unlockedGovernment = false;
        private bool unlockedParties = false;

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
            if(!this.unlockedGovernment)
            {
                this.unlockedGovernment = this.CheckMilestone("Basic Road Created");
            }
            if(!this.unlockedParties)
            {
                this.unlockedParties = this.CheckMilestone("Milestone1");
            }
        }

        public bool CheckMilestone(string name)
        {
            Dictionary<string, MilestoneInfo> milestones = UnlockManager.instance.m_allMilestones;
            MilestoneInfo value;
            milestones.TryGetValue(name, out value);
            return UnlockManager.instance.Unlocked(value);
        }

        public bool GovernmentUnlocked
        {
            get
            {
                return this.unlockedGovernment;
            }
        }

        public bool PartiesUnlocked
        {
            get
            {
                return this.unlockedParties;
            }
        }
    }
}
