using System;
using System.Collections.Generic;
using System.Text;
using GroogyLib.Core;

namespace Republic
{
    public class CitizenIssueData
    {
        private WeakPtr<Citizen> owner = null;
        private Party affiliation = null;

        public CitizenIssueData(Citizen owner)
        {
            this.owner = new WeakPtr<Citizen>(owner);
        }

        public Citizen Owner
        {
            get
            {
                return this.owner.Target;
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
    }
}
