using System;
using System.Collections.Generic;
using System.Text;

namespace Republic
{
    class Party
    {
        private string name = "";

        public Party(string name)
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
}
