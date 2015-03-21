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
        private string creationMessage = "";
        private List<PoliticalIssue> issues = new List<PoliticalIssue>();

        public Party(string name, Color color, string creationMessage)
        {
            this.name = name;
            this.color = color;
            this.creationMessage = creationMessage;
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

        public string CreationMessage
        {
            get
            {
                return this.creationMessage;
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
