using System;
using System.IO;
using System.Reflection;
using ICities;
using UnityEngine;
using GroogyLib.Core;

namespace Republic
{
    public class RepublicMod : IUserMod
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

    public class RepublicSetup : ILoadingExtension
    {
        void ILoadingExtension.OnCreated(ILoading loading)
        {
        }

        void ILoadingExtension.OnLevelLoaded(LoadMode mode)
        {
            RepublicCore.instance.Initiate();
        }

        void ILoadingExtension.OnLevelUnloading()
        {
            RepublicCore.DestroyInstance();
        }

        void ILoadingExtension.OnReleased()
        {
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
}