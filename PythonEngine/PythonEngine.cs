// This is an independent project of an individual developer. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++, C#, and Java: http://www.viva64.com 

using System;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using Interfaces;
using IronPython.Hosting;
using Microsoft.Scripting.Hosting;

namespace PythonEngine
{
    public class PythonEngine
    {
        private readonly ScriptEngine _engine;
        //private readonly ScriptScope  _scope;

        public PythonEngine()
        {
            _engine = Python.CreateEngine();
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),"Lib");
            if (Directory.Exists(path))
            {

                var listOfPath = Directory.GetDirectories(path);
                listOfPath = listOfPath.Concat(new[] {path}).ToArray();
                _engine.SetSearchPaths(listOfPath);
                
            }
            else
            {
               //Logger.Log.LazyWrite("PythonEngine","Warning","для коректної роботи потрібно вказати шляхи до бібліотек пітону у файлі 'pythonEnginePathes.txt'");
            }
            
            //_scope = _engine.CreateScope();
            //dynamic proxy = new ExpandoObject();
            //proxy.GetClipboardText = new Func<object>(GetClipboardText);
            //((dynamic)_scope).proxy = proxy;
        }

        public ScriptScope CreateScope()
        {
            var scope = _engine.CreateScope();
            dynamic proxy = new ExpandoObject();
            proxy.GetClipboardText = new Func<object>(GetClipboardText);
            ((dynamic)scope).proxy = proxy;

            return scope;
        }

        object GetClipboardText()
        {
            object idat = null;
           // Exception threadEx = null;
            Thread staThread = new Thread(
                delegate ()
                {
                    try
                    {
                        idat = Clipboard.GetText();
                    }

                    catch 
                    {
                        //threadEx = ex;
                        idat = string.Empty;
                    }
                });
            staThread.SetApartmentState(ApartmentState.STA);
            staThread.Start();
            staThread.Join();
            // at this point either you have clipboard data or an exception


            return idat;
        }


        //public void ExecuteFile(string scriptPath,string fileName)
        //{
        //    try
        //    {
        //        _scope.SetVariable("FileName",fileName);
        //        _engine.ExecuteFile(scriptPath, _scope);
        //    }
        //    catch (Exception e)
        //    {
        //        MessageBox.Show(e.Message, e.Source);
        //    }
        //}


        public void ExecuteFile(ScriptScope scope, string scriptPath, object props)
        {
            try
            {
                
                var properties = props.GetType().GetProperties();
                foreach (var property in properties)
                {
                    SetVariable(scope,property.Name,property.GetValue(props,null));
                }

                _engine.ExecuteFile(scriptPath, scope);

            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message, e.Source);
            }
        }

        public void ExecuteFile(ScriptScope scope,string scriptPath)
        {
            try
            {
                _engine.ExecuteFile(scriptPath, scope);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Source);
            }
        }

        public void ExecuteScript(ScriptScope scope,IScriptRunParameters parameters)
        {
            try
            {
                _engine.ExecuteFile(parameters.ScriptPath,scope);
            }catch(Exception e)
            {
                MessageBox.Show(e.Message, e.Source);
            }
        }

        public bool IsScript(string path)
        {
            if (path == null) return false;

            var ext = Path.GetExtension(path);
            return ext?.Equals(".py", StringComparison.InvariantCultureIgnoreCase) ?? false;
        }

        public void SetVariable(ScriptScope scope,string variable, object value)
        {
            scope.SetVariable(variable,value);
        }

        public dynamic GetFunction(ScriptScope scope,string funcName)
        {
            return scope.ContainsVariable(funcName) ? scope.GetVariable(funcName) : null;
        }
      
    }
}
