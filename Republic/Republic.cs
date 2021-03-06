﻿using System;
using System.IO;
using System.Reflection;
using ICities;
using UnityEngine;
using GroogyLib.Core;

namespace Republic
{
    public class RepublicMod : IUserMod
    {
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
            RepublicCore.Instance.Initiate();
        }

        void ILoadingExtension.OnLevelUnloading()
        {
            RepublicCore.DestroyInstance();
        }

        void ILoadingExtension.OnReleased()
        {
        }
    }

    public class RepublicMainThread : IThreadingExtension
    {
        private IThreading threading = null;
        private RepublicCore core = null;

        public void OnAfterSimulationFrame()
        {
        }

        public void OnAfterSimulationTick()
        {
        }

        public void OnBeforeSimulationFrame()
        {
        }

        public void OnBeforeSimulationTick()
        {
        }

        public void OnCreated(IThreading threading)
        {
            this.threading = threading;
            this.core = RepublicCore.Instance;
            this.core.managers = threading.managers;
        }

        public void OnReleased()
        {
            this.threading = null;
            this.core = null;
        }

        public void OnUpdate(float realTimeDelta, float simulationTimeDelta)
        {
            if (this.core.Initiated)
            {
                this.core.CitizenIssueDatabase.Update();
                this.core.Milestones.Update();
            }
        }
    }

    public class RepublicCore
    {
        static private RepublicCore ourInstance = null;

        static public RepublicCore Instance
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

        public IManagers managers = null;
        private GameObject coreObject = null;
        private Debugger debuggerComponent = null;
        private GovernmentUI governmentUI = null;
        private PartyDatabase partyDatabase = null;
        private CitizenIssueDatabase citizenDatabase = null;
        private PoliticalIssueDatabase issueDatabase = null;
        private MilestonesManager milestones = null;
        private ChirperManager chirper = null;
        private int windowIdCounter = 0;
        private bool initiated = false;

        public RepublicCore()
        {
        }
        

        public void Initiate()
        {
            string[] namespaces = { "Republic" };
            GameObject coreTemplate = new GameObject("RepublicCore");
            this.coreObject = GameObject.Instantiate(coreTemplate);
            GameObject.FindWithTag("GameController").AddComponent<CitizenIssueUI>();
            ToolsModifierControl.SetTool<DefaultTool>();
            this.debuggerComponent = this.coreObject.AddComponent<Debugger>();
            this.debuggerComponent.Initiate(namespaces);
            this.debuggerComponent.SetupGUI(new Rect(Screen.width - 60, 65, 50, 50), "Debug", new Vector2(800, 600));
            this.debuggerComponent.OpenLog("RepublicCore.log");
            this.debuggerComponent.enabled = true;
            this.governmentUI = this.coreObject.AddComponent<GovernmentUI>();
            this.governmentUI.Initiate();

            this.partyDatabase = new PartyDatabase();
            this.partyDatabase.Initiate();

            this.issueDatabase = new PoliticalIssueDatabase();
            this.issueDatabase.Initiate();

            this.citizenDatabase = new CitizenIssueDatabase();
            this.citizenDatabase.Initiate();

            this.milestones = new MilestonesManager(this.managers.milestones);
            this.chirper = new ChirperManager();

            this.initiated = true;
        }

        public bool Initiated
        {
            get
            {
                return this.initiated;
            }
        }

        public Debugger Debugger
        {
            get
            {
                return this.debuggerComponent;
            }
        }

        public PartyDatabase PartyDatabase
        {
            get 
            {
                return this.partyDatabase;
            }
        }

        public CitizenIssueDatabase CitizenIssueDatabase
        {
            get
            {
                return this.citizenDatabase;
            }
        }

        public PoliticalIssueDatabase PoliticalIssueDatabase
        {
            get
            {
                return this.issueDatabase;
            }
        }

        public MilestonesManager Milestones
        {
            get
            {
                return this.milestones;
            }
        }

        public ChirperManager Chirper
        {
            get
            {
                return this.chirper;
            }
        }

        public int GenerateWindowId()
        {
            return this.windowIdCounter++;
        }
    }
}