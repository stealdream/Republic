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

            Array32<Citizen> citizens = CitizenManager.instance.m_citizens;
            for(uint index = 0, size = citizens.ItemCount(); index < size; index++)
            {
                if (/*(citizens.m_buffer[index].m_flags & Citizen.Flags.Created) == 0 || */
                    (citizens.m_buffer[index].m_flags & Citizen.Flags.DummyTraffic) != 0 ||
                    (citizens.m_buffer[index].m_flags & Citizen.Flags.Tourist) != 0)
                    continue;

                this.issues.Add(this.GenerateDataFor(index));
            }
        }

        public void Update()
        {
            this.numVoters = 0;
            Array32<Citizen> citizens = CitizenManager.instance.m_citizens;
            for (uint index = 0, size = citizens.ItemCount(); index < size; index++)
            {
                if (/*(citizens.m_buffer[index].m_flags & Citizen.Flags.Created) == 0 ||*/
                    (citizens.m_buffer[index].m_flags & Citizen.Flags.DummyTraffic) != 0 ||
                    (citizens.m_buffer[index].m_flags & Citizen.Flags.Tourist) != 0)
                    continue;

                if(!this.HasDataFor(index))
                    this.issues.Add(this.GenerateDataFor(index));
            }
            for(int index = 0, size = this.issues.Count; index < size; index++)
            {
                CitizenIssueData data = this.issues[index];

                if (data.AllowedToVote)
                    this.numVoters++;
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
            PartyDatabase parties = RepublicCore.Instance.PartyDatabase;
            return data;
        }
    }
}
