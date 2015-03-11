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

        public Party(string name, Color color)
        {
            this.name = name;
            this.color = color;
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
    }
}
