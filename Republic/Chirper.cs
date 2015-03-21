using System;
using System.Collections.Generic;
using System.Text;
using ICities;

namespace Republic
{
    public class ChirperManager
    {
        public ChirperManager()
        {
        }

        public void AddMessage(uint citizen, string message)
        {
            MessageManager.instance.QueueMessage(new ChirperMessage(citizen, message));
        }

        public void AddNewPartyMessage(CitizenIssueData creator)
        {
            Party party = creator.Affiliation;
            this.AddMessage(creator.Owner, "Join my new party! " + party.Name + " will bring the future! #politics");
        }

    }

    public class ChirperMessage : MessageBase
    {
        private string author;
        private uint citizen;
        private string message;

        public ChirperMessage(uint citizen, string message)
        {
            this.author = CitizenManager.instance.GetCitizenName(citizen);
            this.citizen = citizen;
            this.message = message;
        }

        public override uint GetSenderID()
        {
            return this.citizen;
        }

        public override string GetSenderName()
        {
            return this.author;
        }

        public override string GetText()
        {
            return this.message;
        }

        public override bool IsSimilarMessage(MessageBase other)
        {
            var m = other as ChirperMessage;
            return m != null && m.author == this.author && m.text == this.text;
        }

        public override void Serialize(ColossalFramework.IO.DataSerializer s)
        {
            s.WriteSharedString(this.author);
            s.WriteSharedString(this.text);
            s.WriteUInt32(this.citizen);
        }

        public override void Deserialize(ColossalFramework.IO.DataSerializer s)
        {
            this.author = s.ReadSharedString();
            this.message = s.ReadSharedString();
            this.citizen = s.ReadUInt32();
        }

        public override void AfterDeserialize(ColossalFramework.IO.DataSerializer s)
        {
        }
    }
}
