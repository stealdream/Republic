using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Republic
{
    public class PartyDatabase
    {
        private static Color[] PotentialColors = {
            new Color(1, 1, 1, 1), new Color(0, 0, 0, 1), new Color(0, 0, 1, 1), new Color(0, 1, 1, 1), new Color(0.5f, 0.5f, 0.5f, 1), 
            new Color(0, 1, 0, 1), new Color(1, 0, 1, 1), new Color(0.75f, 0, 0, 1), new Color(1, 0.92f, 0.016f, 1), new Color(1, 0, 0, 1),
        };
        private static string[] PotentialNames = {
            "Whigs Party", "Fascist party", "Moderats", "Independents", "Republicans",
            "Green party", "Weird people party", "Communist party", "Liberal party", "Socialist party",
        };

        private List<Party> parties = new List<Party>();

        public PartyDatabase()
        {
        }

        public void Initiate()
        {
        }

        public Party CreateParty(string name, Color color)
        {
            Party party = new Party(name, color);
            this.parties.Add(party);
            return party;
        }

        public Party GenerateRandomParty()
        {
            bool unique = false;
            int template = 0;
            while(!unique)
            {
                unique = true;
                template = UnityEngine.Random.Range(0, PotentialNames.Length - 1);
                string name = PotentialNames[template];
                for(int index = 0, size = this.parties.Count; index < size; index++)
                {
                    if (this.parties[index].Name == name)
                    {
                        unique = false;
                        break;
                    }
                }
            }
            return CreateParty(PotentialNames[template], PotentialColors[template]);
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
