using System;
using System.Collections.Generic;
using System.Text;

namespace Republic
{
    public class PoliticalIssue
    {
        private string name = "";

        public PoliticalIssue(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }
    }

    public class RaiseTaxesIssue : PoliticalIssue
    {
        public RaiseTaxesIssue() : base("Raise taxes")
        {
        }
    }

    public class LowerTaxesIssue : PoliticalIssue
    {
        public LowerTaxesIssue() : base("Lower taxes")
        {
        }
    }
}
