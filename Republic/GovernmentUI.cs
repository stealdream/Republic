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
            uint nCount = 0;
            Array32<Citizen> citizens = CitizenManager.instance.m_citizens;
            for(uint index = 0, size = citizens.ItemCount(); index < size; index++)
            {
                Citizen citizen = citizens.m_buffer[index];
                if (!citizen.Dead)
                    nCount++;
            }
            GUI.Label(new Rect(10, 100, 200, 30), "Population: " + nCount);

            List<Party> parties = RepublicCore.Instance.PartyDatabase.Parties;
            for(int index = 0, size = parties.Count; index < size; index++)
            {
                Party party = parties[index];
                GUI.Label(new Rect(10, 140 + index * 30, 200, 30), party.Name);
                Texture2D colorTexture = new Texture2D(1, 1);
                colorTexture.SetPixel(1, 1, party.Color);
                colorTexture.Apply();
                GUI.DrawTexture(new Rect(150, 140 + index * 30, 26, 26), colorTexture); 
            }
        }
    }
}
