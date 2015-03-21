using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Text;

namespace Republic
{
    public class CitizenIssueDatabase
    {
        private List<CitizenIssueData> issues = new List<CitizenIssueData>();
        private uint numVoters = 0;

        public CitizenIssueDatabase()
        {
        }

        public void Initiate()
        {
            this.issues.Clear();
            this.numVoters = 0;

            Citizen[] citizens = Singleton<CitizenManager>.instance.m_citizens.m_buffer;
            for (uint index = 0, size = (uint)citizens.Length; index < size; index++)
            {
                if (!citizens[index].Dead &&
                    (citizens[index].m_flags & Citizen.Flags.Created) != 0 &&
                    (citizens[index].m_flags & Citizen.Flags.DummyTraffic) == 0 &&
                    (citizens[index].m_flags & Citizen.Flags.Tourist) == 0 &&
                    (citizens[index].m_flags & Citizen.Flags.MovingIn) == 0)
                {
                    this.issues.Add(this.GenerateDataFor(index));
                }
            }
        }

        public void Update()
        {
            PartyDatabase parties = RepublicCore.Instance.PartyDatabase;
            this.numVoters = 0;

            Citizen[] citizens = Singleton<CitizenManager>.instance.m_citizens.m_buffer;
            for (uint index = 0, size = (uint) citizens.Length; index < size; index++)
            {
                if (!citizens[index].Dead &&
                    (citizens[index].m_flags & Citizen.Flags.Created) != 0 &&
                    (citizens[index].m_flags & Citizen.Flags.DummyTraffic) == 0 &&
                    (citizens[index].m_flags & Citizen.Flags.Tourist) == 0 &&
                    (citizens[index].m_flags & Citizen.Flags.MovingIn) == 0)
                {
                    if (!this.HasDataFor(index))
                        this.issues.Add(this.GenerateDataFor(index));
                }
            }
            for(int index = 0, size = this.issues.Count; index < size; index++)
            {
                CitizenIssueData data = this.issues[index];

                if (data.AllowedToVote)
                    this.numVoters++;

                if(this.ShouldStartParty(data))
                {
                    data.Affiliation = parties.GeneratePartyFor(data);
                    RepublicCore.Instance.Chirper.AddNewPartyMessage(data);
                }
            }
        }

        public bool HasDataFor(uint citizen)
        {
            for(int index = 0, size = this.issues.Count; index < size; index++)
            {
                CitizenIssueData data = this.issues[index];
                if(data.Owner == citizen)
                    return true;
            }
            return false;
        }

        public uint NumVoters
        {
            get
            {
                return this.numVoters;
            }
        }

        public int NumIssues
        {
            get
            {
                return this.issues.Count;
            }
        }

        private CitizenIssueData GenerateDataFor(uint citizen)
        {
            CitizenIssueData data = new CitizenIssueData(citizen);
            data.DetermineIssues();
            return data;
        }

        private bool ShouldStartParty(CitizenIssueData data)
        {
            if(!data.CanStartParty)
                return false;

            PartyDatabase parties = RepublicCore.Instance.PartyDatabase;
            float issueSaturation = 0;
            for(int index = 0, size = parties.NumParties; index < size; index++)
            {
                Party party = parties.GetParty(index);
                float partySaturation = data.DetermineIssueSaturation(party);
                if (partySaturation > 0)
                    issueSaturation = partySaturation;
            }

            return issueSaturation <= 0.25f;
        }
    }
}
