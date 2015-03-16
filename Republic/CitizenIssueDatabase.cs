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
            for (int index = 0, size = citizens.Length; index < size; index++)
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
            this.numVoters = 0;
            Citizen[] citizens = Singleton<CitizenManager>.instance.m_citizens.m_buffer;
            for (int index = 0, size = citizens.Length; index < size; index++)
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
            }
        }

        public bool HasDataFor(int citizen)
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

        private CitizenIssueData GenerateDataFor(int citizen)
        {
            CitizenIssueData data = new CitizenIssueData(citizen);
            PartyDatabase parties = RepublicCore.Instance.PartyDatabase;
            return data;
        }
    }
}
