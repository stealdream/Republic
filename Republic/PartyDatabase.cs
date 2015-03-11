using System;
using System.Collections.Generic;
using System.Text;

namespace Republic
{
    public class PartyDatabase
    {
        private List<Party> parties = new List<Party>();

        public PartyDatabase()
        {
        }

        public void Initiate()
        {
            int numParties = RepublicCore.Instance.Randomizer.Int32(2, 8);
            for(int index = 0; index < numParties; index++)
            {
                CreateParty("Party #" + index);
            }
        }

        public Party CreateParty(string name)
        {
            Party party = new Party(name);
            this.parties.Add(party);
            return party;
        }

        public Party GetParty(int index)
        {
            return this.parties[index];
        }

        public List<Party> Parties
        {
            get
            {
                return this.parties;
            }
        }

        public int NumParties
        {
            get
            {
                return this.parties.Count;
            }
        }
    }
}
