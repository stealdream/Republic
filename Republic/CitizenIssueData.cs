using System;
using System.Collections.Generic;
using System.Text;
using GroogyLib.Core;
using ColossalFramework;

namespace Republic
{
    public class CitizenIssueData
    {
        static private CitizenManager citizenManager = Singleton<CitizenManager>.instance;
        private int owner = 0;
        private Party affiliation = null;
        private List<PoliticalIssue> issues = new List<PoliticalIssue>();

        public CitizenIssueData(int owner)
        {
            this.owner = owner;

            this.DetermineIssues();
        }

        public void DetermineIssues()
        {
            this.issues.Clear();
            this.issues.Add(RepublicCore.Instance.PoliticalIssueDatabase.GetIssue(0));
        }

        public int Owner
        {
            get
            {
                return this.owner;
            }
        }

        public Party Affiliation
        {
            get
            {
                return this.affiliation;
            }
            set
            {
                this.affiliation = value;
            }
        }

        public bool AllowedToVote
        {
            get
            {
                Citizen[] citizens = citizenManager.m_citizens.m_buffer;
                return !citizens[this.owner].Dead && Citizen.GetAgeGroup(citizens[this.owner].Age) >= Citizen.AgeGroup.Young;
            }
        }
    }
}
