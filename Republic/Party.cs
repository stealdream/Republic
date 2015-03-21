using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Republic
{
    public class Party
    {
        private string name = "";
        private Color color = Color.white;
        private List<PoliticalIssue> issues = new List<PoliticalIssue>();

        public Party(string name, Color color)
        {
            this.name = name;
            this.color = color;
        }

        public void CopyIssues(List<PoliticalIssue> issues)
        {
            this.issues.Clear();
            this.issues.AddRange(issues);
        }

        public string Name
        {
            get
            {
                return this.name;
            }
        }

        public Color Color
        {
            get
            {
                return this.color;
            }
        }

        public List<PoliticalIssue> Issues
        {
            get
            {
                return this.issues;
            }
        }
    }
}
