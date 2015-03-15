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

        public CitizenIssueData(uint owner)
        {
            this.owner = owner;
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
                return !citizenManager.m_citizens.m_buffer[this.owner].Dead && Citizen.GetAgeGroup(citizenManager.m_citizens.m_buffer[this.owner].Age) == Citizen.AgeGroup.Adult;
            }
        }
    }
}
