﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Republic
{
    public class CitizenIssueDatabase
    {
        List<CitizenIssueData> issues = new List<CitizenIssueData>();

        public CitizenIssueDatabase()
        {
        }

        public void Initiate()
        {
            this.issues.Clear();

            Array32<Citizen> citizens = CitizenManager.instance.m_citizens;
            for(uint index = 0, size = citizens.ItemCount(); index < size; index++)
            {
                Citizen citizen = citizens.m_buffer[index];
                this.issues.Add(this.GenerateDataFor(citizen));
            }
        }

        private CitizenIssueData GenerateDataFor(Citizen citizen)
        {
            CitizenIssueData data = new CitizenIssueData(citizen);
            PartyDatabase parties = RepublicCore.Instance.PartyDatabase;
            return data;
        }
    }
}
