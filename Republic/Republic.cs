using System;
using System.IO;
using System.Reflection;
using ICities;
using UnityEngine;
using GroogyLib.Core;

namespace Republic
{
    public class RepublicMode : IUserMod
    {
        public void OnLoad()
        {
            File.WriteAllText("republicmod.txt", "Hello world!\n");
        }

        public void OnUnload()
        {
        }

        public string Name
        {
            get { return "RepublicMod"; }
        }

        public string Description
        {
            get
            {
                return "Hello world!";
            }
        }
    }

    public class RepublicCore
    {
        static private RepublicCore ourInstance = null;

        static public RepublicCore instance
        {
            get 
            {
                if(ourInstance == null)
                    ourInstance = new RepublicCore();
                return ourInstance; 
            }
        }

        static public void DestroyInstance()
        {
            ourInstance = null;
        }

        private GameObject coreObject = null;
        private Debugger debuggerComponent = null;
        private GovernmentUI governmentUI = null;
        private int windowIdCounter = 0;

        public RepublicCore()
        {
        }

        public void Initiate()
        {
            string[] namespaces = { "Republic" };
            GameObject coreTemplate = new GameObject("RepublicCore");
            this.coreObject = GameObject.Instantiate(coreTemplate);
            this.debuggerComponent = this.coreObject.AddComponent<Debugger>();
            this.debuggerComponent.Initiate(namespaces);
            this.debuggerComponent.SetupGUI(new Rect(Screen.width - 60, 65, 50, 50), "Debug", new Vector2(800, 600));
            this.debuggerComponent.OpenLog("RepublicCore.log");
            this.debuggerComponent.enabled = true;
            this.governmentUI = this.coreObject.AddComponent<GovernmentUI>();
            this.governmentUI.Initiate();
        }

        public Debugger debugger
        {
            get
            {
                return this.debuggerComponent;
            }
        }

        public int generateWindowId()
        {
            return this.windowIdCounter++;
        }
    }

    public class RepublicEconomyClass : EconomyExtensionBase
    {
        private bool m_Done;

        public override long OnUpdateMoneyAmount(long internalMoneyAmount)
        {
            if (!m_Done)
            {
                RepublicCore.instance.Initiate();
                m_Done = true;
            }
            return internalMoneyAmount;
        }

        public override bool OverrideDefaultPeekResource
        {
            get { return true; }
        }

        public override int OnPeekResource(EconomyResource resource, int amount)
        {
            return amount;
        }
    }
}