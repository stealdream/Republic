using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using UnityEngine;

namespace GroogyLib
{
    namespace Core
    {
        public class Debugger : MonoBehaviour
        {
            private StreamWriter logWriter = null;
            private DebuggerDisplay display = null;
            private DebuggerEvaluator evaluator = null;
            private List<Watch> watches = new List<Watch>();

            public Debugger()
            {
            }

            public void Initiate(string[] namespaces)
            {
                this.evaluator = new DebuggerEvaluator(this, namespaces);
            }

            public void SetupGUI(Rect buttonRect, string buttonText, Vector2 windowSize)
            {
                this.display = this.gameObject.AddComponent<DebuggerDisplay>();
                this.display.Initiate(this, buttonRect, buttonText, windowSize);
            }

            public void OpenLog(string filename)
            {
                this.logWriter = new StreamWriter(filename);
            }

            public Debugger Log(string content)
            {
                if (this.logWriter != null)
                {
                    this.logWriter.Write(content + Environment.NewLine);
                    this.logWriter.Flush();
                }
                if(this.display != null)
                {
                    this.display.RecordHistory(content);
                }
                return this;
            }

            public Debugger ClearWatches()
            {
                this.watches.Clear();
                return this;
            }

            public Debugger Watch(object obj, string field)
            {
                this.Log(obj.ToString() + "." + field);
                this.watches.Add(new Watch(obj, field));
                return this;
            }

            public Debugger Watch(string obj, string field)
            {
                return Watch(GameObject.Find(obj), field);
            }

            public Debugger DumpFields(object obj)
            {
                FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
                this.Log("Instance fields: " + fields.Length);
                foreach(FieldInfo field in fields)
                {
                    this.Log(obj.ToString() + "." + field.Name);
                }
                fields = obj.GetType().GetFields(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                this.Log("Static fields: " + fields.Length);
                foreach (FieldInfo field in fields)
                {
                    this.Log(obj.ToString() + "." + field.Name);
                }
                return this;
            }

            public Debugger DumpFields(string obj)
            {
                return DumpFields(GameObject.Find(obj));
            }

            private void Update()
            {
            }

            private string Indent(int depth)
            {
                string str = string.Empty;
                for (int i = 0; i < depth; i++)
                    str += "\t";
                return str;
            }

            private string DumpComponents(Transform t)
            {
                string info = " (";
                Component[] components = t.GetComponents<Component>();
                int length = components.Length;
                for (int i = 0; i < length; i++)
                {
                    info += components[i].GetType().ToString();
                    if (i != length - 1)
                        info += ", ";
                }
                info += ")";
                return info;
            }

            private string DumpGameObject(Transform t, int depth)
            {
                string hierarchy = Indent(depth) + t.name + DumpComponents(t) + Environment.NewLine;
                foreach (Transform child in t)
                    hierarchy += DumpGameObject(child, depth + 1);
                return hierarchy;
            }

            internal void DumpUnitySceneHierarchy()
            {
                string hierarchy = string.Empty;

                object[] allObjects = GameObject.FindObjectsOfType(typeof(GameObject));
                foreach (object o in allObjects)
                {
                    GameObject g = o as GameObject;
                    if (g != null)
                    {
                        // upper most object
                        if (g.transform.parent == null)
                            hierarchy += DumpGameObject(g.transform, 0);
                    }
                }

                this.Log("################################################");
                this.Log("##       DUMPING UNITY SCENE HIEARCHY         ##");
                this.Log("################################################");
                this.Log(hierarchy);
                this.Log("################################################");
                this.Log("##            SCENE HIEARCHY END              ##");
                this.Log("################################################");
            }

            internal object Evaluate(string evalCode)
            {
                object result = this.evaluator.Run(evalCode);
                if (result != null)
                    this.Log(result.ToString());
                return result;
            }

            internal List<Watch> GetWatches()
            {
                return this.watches;
            }
        }

        internal class DebuggerEvaluator
        {
            private Debugger debugger;
            private string[] namespaces;
            private int numEvals = 0;

            public DebuggerEvaluator(Debugger debugger, string[] namespaces)
            {
                this.debugger = debugger;
                this.namespaces = namespaces;
            }

            public object Run(string evalCode)
            {
                object result = null;
                try
                {
                    string completeCode = this.ProcessCode(evalCode);
                    Assembly generatedCode = null;
                    bool success = this.CompileCode(completeCode, out generatedCode);

                    if(success)
                    {
                        object evalInstance = generatedCode.CreateInstance("DebugSpace.EvalClass" + this.numEvals);
                        Module[] modules = generatedCode.GetModules(false);
                        Type[] types = modules[0].GetTypes(); 
                        foreach(Type type in types)
                        {
                            if(type.Name == "EvalClass" + this.numEvals)
                            { 
                                MethodInfo method = type.GetMethod("Eval");
                                method.Invoke(evalInstance, null);
                                FieldInfo field = type.GetField("result", BindingFlags.Public | BindingFlags.Instance);
                                result = field.GetValue(evalInstance);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    this.debugger.Log("Failed to eval code: " + evalCode);
                    this.debugger.Log("threw exception: " + ex.ToString());
                }
                finally
                {
                    this.numEvals++;
                }
                return result;
            }

            private string ProcessCode(string evalCode)
            {
                string code = "";
                code += "using GroogyLib.Core;" + Environment.NewLine;
                code += "using UnityEngine;" + Environment.NewLine;
                foreach(string space in this.namespaces)
                {
                    code += "using " + space + ";" + Environment.NewLine;
                }
                code += "namespace DebugSpace {" + Environment.NewLine;
                code += "\tpublic class EvalClass" + this.numEvals + Environment.NewLine;
                code += "\t{" + Environment.NewLine;
                code += "\t\tpublic object result = null;" + Environment.NewLine;
                code += "\t\tpublic void Eval() { " + evalCode + "; }" + Environment.NewLine;
                code += "\t}" + Environment.NewLine;
                code += "}" + Environment.NewLine;
                return code;
            }

            private bool CompileCode(string code, out Assembly assembly)
            {
                string options =  "/target:library /optimize ";
                foreach(string space in this.namespaces)
                {
                    options += "/lib:\"C:\\Users\\Groogy\\AppData\\Local\\Colossal Order\\Cities_Skylines\\Addons\\Mods\\" + space + "\" ";
                }
                options += "/lib:\"C:\\Program Files (x86)\\Steam\\steamapps\\common\\Cities_Skylines\\Cities_Data\\Managed\"";
                
                CodeDomProvider codeProvider = new CSharpCodeProvider();
                CompilerParameters parameters = new CompilerParameters();
                parameters.CompilerOptions = options;
                parameters.GenerateExecutable = false;
                parameters.GenerateInMemory = true;
                parameters.IncludeDebugInformation = false;
                parameters.ReferencedAssemblies.Add("mscorlib.dll");
                parameters.ReferencedAssemblies.Add("System.dll");
                parameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
                parameters.ReferencedAssemblies.Add("ICities.dll");
                parameters.ReferencedAssemblies.Add("UnityEngine.dll");
                parameters.ReferencedAssemblies.Add("Assembly-CSharp.dll");
                parameters.ReferencedAssemblies.Add("Assembly-CSharp-firstpass.dll");
                foreach(string space in this.namespaces)
                {
                    parameters.ReferencedAssemblies.Add(space + ".dll");
                }
                
                CompilerResults compileResult = codeProvider.CompileAssemblyFromSource(parameters, code);

                if ( compileResult.NativeCompilerReturnValue != 0 )
                {
                    this.debugger.Log("Failed to eval code: " + code);
                    this.debugger.Log("Result: " + compileResult.NativeCompilerReturnValue);
                    string[] output = new string[compileResult.Output.Count];
                    compileResult.Output.CopyTo(output, 0);
                    this.debugger.Log(String.Join("\n", output));
                    assembly = null;
                    return false;
                }
                else
                {
                    assembly = compileResult.CompiledAssembly;
                    return true;
                }
            }
        }

        internal class DebuggerDisplay : MonoBehaviour
        {
            private Debugger debugger;
            private Rect buttonRect;
            private string buttonText;
            private bool displayingPanel = false;
            private Vector2 windowSize;
            private string commandLine = "";
            private string history = "";
            private Vector2 scrollPosition = Vector2.zero;

            public DebuggerDisplay()
            {

            }

            public void Initiate(Debugger debugger, Rect buttonRect, string buttonText, Vector2 windowSize)
            {
                this.debugger = debugger;
                this.buttonRect = buttonRect;
                this.buttonText = buttonText;
                this.windowSize = windowSize;
            }

            public void RecordHistory(string content)
            {
                this.history += content + "\n";
            }

            private void OnGUI()
            {

                if (GUI.Button(this.buttonRect, this.buttonText))
                {
                    this.displayingPanel = !this.displayingPanel;
                }
                if (this.displayingPanel)
                {
                    Rect windowRect = new Rect(this.buttonRect.x, this.buttonRect.y + this.buttonRect.height + 2, this.windowSize.x, this.windowSize.y);
                    if (windowRect.xMax > Screen.width - 10)
                        windowRect.x = Screen.width - windowRect.width - 10;
                    if (windowRect.yMax > Screen.height - 10)
                        windowRect.y = Screen.height - windowRect.height - 10;
                    if (windowRect.xMin < 10)
                        windowRect.x = 10;
                    if (windowRect.yMin < 10)
                        windowRect.y = 10;
                    GUI.Window(10000, windowRect, MainWindowFunc, "Debugger");
                    windowRect.x -= 400;
                    windowRect.width = 400;
                    windowRect.height = 400;
                    GUI.Window(10001, windowRect, WatchWindowFunc, "Watches");
                }
            }

            private void Update()
            {
            }

            private void MainWindowFunc(int windowID)
            {
                Event ev = Event.current;
                bool scrollToEnd = false;
                if (ev.keyCode == KeyCode.Return && ev.type == EventType.KeyUp)
                {
                    this.history += "----------------\n";
                    this.history += this.commandLine + "\n";
                    object result = this.debugger.Evaluate(this.commandLine);
                    this.history += "-> " + (result == null ? "null" : result.ToString()) + "\n";
                    this.commandLine = "";
                    scrollToEnd = true;
                }
                this.commandLine = GUI.TextField(new Rect(4, this.windowSize.y - 24, this.windowSize.x - 6, 20), this.commandLine);

                float height = GUI.skin.label.CalcHeight(new GUIContent(this.history), this.windowSize.x - 16);
                this.scrollPosition = GUI.BeginScrollView(new Rect(4, 16, this.windowSize.x - 16, this.windowSize.y - 46), scrollPosition, new Rect(0, 0, this.windowSize.x - 18, Math.Max(this.windowSize.y - 46, height)), false, true);
                GUI.Label(new Rect(0, 0, this.windowSize.x - 18, height), this.history);
                if(scrollToEnd)
                    GUI.ScrollTo(new Rect(0, height, 1, 1));
                GUI.EndScrollView();
            }

            private void WatchWindowFunc(int windowId)
            {
                List<Watch> watches = this.debugger.GetWatches();
                GUI.Label(new Rect(4, 20, 396, 56), "Watches: " + watches.Count);
                for(int index = 0, size = watches.Count; index < size; index++ )
                {
                    Watch watch = watches[index];
                    Rect rect = new Rect(4, 20 + 34 * (index + 1), 396, 34);
                    GUI.Label(rect, watch.GetDisplayString());
                }
            }
        }
    }
}
