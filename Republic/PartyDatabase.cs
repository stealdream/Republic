using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


namespace Republic
{
    public class PartyDatabase
    {
        private static Party[] PartyTemplates = {
            new Party("The Independence Party", new Color(0, 0.733f, 0), "For a self-reliant city!"),
            new Party("The Republican Party", new Color(0.733f, 0, 0), "Free labor, free land, free men!"),
            new Party("The Democratic Party", new Color(0, 0, 0.733f), "The only way for the future is with the democrats!"),
            new Party("The Liberal Party", new Color(0.533f, 0.533f, 1), "For a more liberal government."),
            new Party("The Conservative Party", new Color(1, 0.533f, 0.533f), ""),
            new Party("The Whig Party", new Color(1, 0.733f, 0.4f), ""),
            new Party("The Liberty Party", new Color(0.533f, 1, 0.533f), ""),
            new Party("The Green Party", new Color(0, 1, 0), "For a greener tomorrow!"),
            new Party("The Modern Party", new Color(0.6f, 0.667f, 0.733f), "Only way forward is with progress."),
            new Party("The Reform Party", new Color(1, 0.498f, 0), "We need major change today!"),
            new Party("The Red Party", new Color(1, 0, 0), "Only option is for a more red government."),
            new Party("The Blue Party", new Color(0, 0, 1), "Only option is for a more blue government."),
            new Party("The Freedom Party", new Color(0.365f, 0, 0.867f), "They can take our money with taxes but they can never take our FREEDOM!"),
            new Party("The Power Party", new Color(0.733f, 0.294f, 0), "Power to the people!"),
            new Party("The Best Party", new Color(0.753f, 0.753f, 0.753f), "We're obviously the only choice."),
            new Party("The Okay Party", new Color(0.502f, 0.502f, 0.502f), "A vote would be nice."),
            new Party("The Meh Party", new Color(0.314f, 0.314f, 0.314f), "Why bother?"),
            new Party("The National Party", new Color(1, 1, 1), "For country and mayor!"),
            new Party("The Socialist Party", new Color(0.847f, 0.365f, 0.365f), "Soldiarity and unity!"),
        };

        private List<Party> parties = new List<Party>();

        public PartyDatabase()
        {
        }

        public void Initiate()
        {
        }

        public Party GeneratePartyFor(CitizenIssueData citizen)
        {
            bool unique = false;
            Party template = null;
            while (!unique)
            {
                unique = true;
                int templateIndex = UnityEngine.Random.Range(0, PartyTemplates.Length - 1);
                template = PartyTemplates[templateIndex];
                for (int index = 0, size = this.parties.Count; index < size; index++)
                {
                    if (this.parties[index] == template)
                    {
                        unique = false;
                        break;
                    }
                }
            }
            template.CopyIssues(citizen.Issues);
            this.parties.Add(template);
            return template;
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
