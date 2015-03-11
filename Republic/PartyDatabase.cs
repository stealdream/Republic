using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


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
            int numParties = UnityEngine.Random.Range(2, 8);
            Color[] PotentialColors = {
                new Color(1, 1, 1, 1), new Color(0, 0, 0, 1), new Color(0, 0, 1, 1), new Color(0, 1, 1, 1), new Color(0.5f, 0.5f, 0.5f, 1), 
                new Color(0, 1, 0, 1), new Color(1, 0, 1, 1), new Color(0.75f, 0, 0, 1), new Color(1, 0.92f, 0.016f, 1), new Color(1, 0, 0, 1),
            };
            string[] PotentialNames = {
                "Whigs Party", "Fascist party", "Moderats", "Independents", "Republicans",
                "Green party", "Weird people party", "Communist party", "Liberal party", "Socialist party",
            };

            List<int> used = new List<int>();

            for(int index = 0; index < numParties; index++)
            {
                int template = 0;
                do
                {
                    template = UnityEngine.Random.Range(0, PotentialNames.Length-1);
                } while (used.Contains(template));
                used.Add(template);
                string name = PotentialNames[template];
                Color color = PotentialColors[template];
                CreateParty(name, color);
            }
        }

        public Party CreateParty(string name, Color color)
        {
            Party party = new Party(name, color);
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
