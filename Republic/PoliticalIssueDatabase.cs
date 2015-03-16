using System;
using System.Collections.Generic;
using System.Text;

namespace Republic
{
    public class PoliticalIssueDatabase
    {
        List<PoliticalIssue> issues = new List<PoliticalIssue>();

        public PoliticalIssueDatabase()
        {
        }

        public void Initiate()
        {
            this.issues.Add(new LowerTaxesIssue());
            this.issues.Add(new RaiseTaxesIssue());
        }

        public PoliticalIssue GetIssue(int index)
        {
            return this.issues[index];
        }

        public int GetNumIssues()
        {
            return this.issues.Count;
        }
    }
}
