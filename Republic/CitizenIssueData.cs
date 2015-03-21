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
        private uint owner = 0;
        private Party affiliation = null;
        private List<PoliticalIssue> issues = new List<PoliticalIssue>();

        public CitizenIssueData(uint owner)
        {
            this.owner = owner;

            this.DetermineIssues();
        }

        public void DetermineIssues()
        {
            this.issues.Clear();
            this.issues.Add(RepublicCore.Instance.PoliticalIssueDatabase.GetIssue(0));
        }

        public float DetermineIssueSaturation(Party party)
        {
            float saturation = 0;
            List<PoliticalIssue> partyIssues = party.Issues;
            for (int index = 0, size = this.issues.Count; index < size; index++ )
            {
                PoliticalIssue issue = this.issues[index];
                if (partyIssues.Contains(issue))
                    saturation++;
            }
            return saturation / this.issues.Count;
        }

        public uint Owner
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

        public bool CanStartParty
        {
            get
            {
                if (!this.AllowedToVote)
                    return false;

                Citizen[] citizens = citizenManager.m_citizens.m_buffer;
                return citizens[this.owner].EducationLevel > Citizen.Education.OneSchool;
            }
        }

        public List<PoliticalIssue> Issues
        {
            get
            {
                return this.issues;
            }
        }
    }
}
