using System;
using System.IO;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using UnityEngine;

namespace GroogyLib
{
    namespace Core
    {
        public class Watch
        {
            object obj = null;
            FieldInfo field = null;
            string fieldName = "";

            public Watch(object obj, string field)
            {
                this.obj = obj;
                this.fieldName = field;
                Type type = this.obj.GetType();
                this.field = type.GetField(field, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            }

            public bool IsValid()
            {
                return this.field != null;
            }

            public object GetValue()
            {
                return this.field.GetValue(this.obj);
            }

            public string GetDisplayString()
            {
                string txt = this.obj.ToString() + "." + this.fieldName + " -> ";
                if (!this.IsValid())
                    txt += "Invalid";
                else
                    txt += this.GetValue().ToString();
                return txt;
            }
        }
    }
}
