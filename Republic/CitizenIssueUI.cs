using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Republic
{

    class CitizenIssueUI : DefaultTool
    {
        public CitizenIssueUI()
        {
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void OnToolGUI()
        {

            if (this.m_toolController.IsInsideUI)
            {
                return;
            }


            Event current = Event.current;
            if (current.type == EventType.MouseDown)
            {
                if (current.button == 0)
                {
                    InstanceID hoverInstance = this.m_hoverInstance;

                    //Log.info(m_mousePosition.ToString());

                    try
                    {
                        RepublicCore.Instance.Debugger.Log("You clicked on " + hoverInstance.ToString());
                        RepublicCore.Instance.Debugger.Log(hoverInstance.Type.ToString());
                    }
                    catch (Exception e)
                    {
                        RepublicCore.Instance.Debugger.Log(e.ToString());
                        RepublicCore.Instance.Debugger.Log(e.StackTrace);
                    }
                }
            }

            base.OnToolGUI();
        }
    }
}
