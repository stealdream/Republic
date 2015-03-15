using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Republic
{
    class GovernmentUI : MonoBehaviour
    {
        bool openWindow = false;
        Rect openButtonRect = new Rect(0, 0, 35, 35);
        Rect windowRect = new Rect(0, 0, 1150, 710);
        int windowId = 0;

        public void Initiate()
        {
            this.openButtonRect.x = Screen.width / 2 + 445;
            this.openButtonRect.y = Screen.height - 85;
            this.windowRect.x = Screen.width / 2 - this.windowRect.width / 2;
            this.windowRect.y = Screen.height - this.windowRect.height - 115;
            this.windowId = RepublicCore.Instance.GenerateWindowId();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Escape))
            {
                this.openWindow = false;
            }
        }

        private void OnGUI()
        {
            if (GUI.Button(this.openButtonRect, "Gov"))
            {
                this.openWindow = !this.openWindow;
            }

            if (this.openWindow)
            {
                GUI.Window(this.windowId, windowRect, WindowFunc, "Government");
            }
        }

        private void WindowFunc(int windowID)
        {
            int nCountPopulation = 0;
            int nCountChildren = 0;
            int nCountGrownups = 0;
            int nTotalAge = 0;
            Citizen[] citizens = Singleton<CitizenManager>.instance.m_citizens.m_buffer;
            for (int index = 0, size = citizens.Length; index < size; index++)
            {
                if( !citizens[index].Dead && 
                    (citizens[index].m_flags & Citizen.Flags.Created) != 0 &&
                    (citizens[index].m_flags & Citizen.Flags.DummyTraffic) == 0 &&
                    (citizens[index].m_flags & Citizen.Flags.Tourist) == 0 &&
                    (citizens[index].m_flags & Citizen.Flags.MovingIn) == 0)
                {
                    Citizen.AgeGroup group = Citizen.GetAgeGroup(citizens[index].Age);
                    switch (group)
                    {
                        case Citizen.AgeGroup.Adult:
                        case Citizen.AgeGroup.Senior:
                        case Citizen.AgeGroup.Young:
                            nCountGrownups++;
                            break;
                        default:
                            nCountChildren++;
                            break;
                    }
                    nTotalAge += citizens[index].Age;
                    nCountPopulation++;
                }
            }

            GUI.Label(new Rect(10, 100, 150, 30), "Voters: " + RepublicCore.Instance.CitizenIssueDatabase.NumVoters);
            GUI.Label(new Rect(10, 130, 150, 30), "Citizen count: " + nCountPopulation);
            GUI.Label(new Rect(10, 160, 150, 30), "Young count: " + nCountChildren);
            GUI.Label(new Rect(10, 190, 150, 30), "Old count: " + nCountGrownups);
            GUI.Label(new Rect(10, 220, 150, 30), "Total age: " + nTotalAge);
            
            List<Party> parties = RepublicCore.Instance.PartyDatabase.Parties;
            for(int index = 0, size = parties.Count; index < size; index++)
            {
                Party party = parties[index];
                GUI.Label(new Rect(200, 100 + index * 30, 200, 30), party.Name);
                Texture2D colorTexture = new Texture2D(1, 1);
                colorTexture.SetPixel(1, 1, party.Color);
                colorTexture.Apply();
                GUI.DrawTexture(new Rect(350, 100 + index * 30, 26, 26), colorTexture); 
            }
        }
    }
}

