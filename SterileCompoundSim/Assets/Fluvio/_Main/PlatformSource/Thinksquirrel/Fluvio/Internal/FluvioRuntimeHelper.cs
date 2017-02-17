using System;
using System.Runtime.InteropServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Thinksquirrel.Fluvio.Internal
{
    using Threading;

    // This class sets up implementations of certain platform-specific features.
    [ExecuteInEditMode]
    [AddComponentMenu("")]
    class FluvioRuntimeHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        static int s_ProcessId = -1;
#endif
#if !UNITY_WEBGL && !UNITY_WINRT
        IThreadFactory m_ThreadFactory;
        IThreadHandler m_ThreadHandler;
        IInterlocked m_Interlocked;
#endif

        void OnEnable()
        {
            // Inject platform-specific dependencies

#if UNITY_EDITOR
            if (s_ProcessId < 0) s_ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;

            // Create settings object in the editor
            var serializedInstance = Resources.Load("FluvioManager", typeof(FluvioSettings)) as FluvioSettings;
            if (!serializedInstance)
            {
                var instance = FluvioSettings.GetFluvioSettingsObject();
                serializedInstance = CreateProjectSettingsAsset(instance, "Resources", "FluvioManager.asset");
            }
            FluvioSettings.SetFluvioSettingsObject(serializedInstance);
            FluvioComputeShader.SetIncludeParser(new ComputeIncludeParser());
            UnityEditor.EditorApplication.update += EditorUpdate;
#endif

            // OpenCL support
#if UNITY_STANDALONE// || UNITY_ANDROID // TODO - Android support for OpenCL is WIP
            Cloo.Bindings.CLInterface.SetInterface(new Cloo.Bindings.CL12());
#endif

            // Multithreading
#if !UNITY_WEBGL && !UNITY_WINRT
            m_ThreadFactory = new ThreadFactory();
            m_ThreadHandler = new ThreadHandler();
            m_Interlocked = new Interlocked();
            FluidBase.onFluidEnabled += OnFluidEnabled;
            FluidBase.onFluidDisabled += OnFluidDisabled;
            OnFluidEnabled(null);
#endif
        }
#if UNITY_EDITOR
        void EditorUpdate()
        {
            if (!(Application.isPlaying && Application.runInBackground))
            {
                CheckApplicationFocus();
            }
        }
#endif
        void OnFluidEnabled(FluidBase fluid)
        {
#if !UNITY_WEBGL && !UNITY_WINRT
            if (FluidBase.fluidCount > 0 && !Parallel.isInitialized)
            {
                Parallel.Initialize(m_ThreadFactory, m_ThreadHandler, m_Interlocked);
            }
#endif
        }
        static void OnFluidDisabled(FluidBase fluid)
        {
#if !UNITY_WEBGL && !UNITY_WINRT
            if (FluidBase.fluidCount == 0)
            {
                Parallel.Reset();
            }
#endif
        }
#if !UNITY_WEBGL && !UNITY_WINRT
        void OnApplicationPause(bool isPaused)
        {
            if (!Application.isEditor) UpdateThreads(!isPaused);
        }
        void OnApplicationFocus(bool isFocused)
        {
            if (!Application.isEditor) UpdateThreads(isFocused);
        }
        void UpdateThreads(bool isFocused)
        {
            if (isFocused && FluidBase.fluidCount > 0 && !Parallel.isInitialized)
            {
                Parallel.Initialize(m_ThreadFactory, m_ThreadHandler, m_Interlocked);
            }
            else if (!isFocused)
            {
                Parallel.Reset();
            }
        }
#endif
#if UNITY_EDITOR_WIN
        void CheckApplicationFocus()
        {
            var activatedHandle = GetForegroundWindow();
            if (activatedHandle == IntPtr.Zero)
                return;

            int activeProcId;
            GetWindowThreadProcessId(activatedHandle, out activeProcId);

            UpdateThreads(activeProcId == s_ProcessId);
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr handle, out int processId);
#elif UNITY_EDITOR_OSX
#if !UNITY_WEBPLAYER
		System.Diagnostics.Process m_FocusProcess;
		bool m_IsActive = true;
        const string kAppleScript =
        "-e \"global frontApp, frontAppId\" "+
        "-e \"tell application \\\"System Events\\\"\" "+
        "-e \"set frontApp to first application process whose frontmost is true\" " +
		"-e \"set frontAppId to unix id of frontApp\" " +
		"-e \"end tell\" " +
		"-e \"return {frontAppId}\"";
#endif
        void CheckApplicationFocus()
        {           
#if !UNITY_WEBPLAYER  // No support for this when in Web Player mode
            try
        	{      		
       	        if (m_IsActive) UpdateThreads(true);
       	        if (m_FocusProcess != null && !m_FocusProcess.HasExited) return;
        		
                var file = new System.IO.FileInfo(System.IO.Path.GetTempPath() + "/fluvio-active-window.sh");
        	
        	    if (!file.Exists)
        	    {
        		    System.IO.File.WriteAllText(file.FullName, "#!/bin/sh\necho `osascript " + kAppleScript + "`");
        		    var procChmod = new System.Diagnostics.Process
            	    {
                	    StartInfo =
                	    {
                		    UseShellExecute = false,
                   		    FileName = "chmod",
                   		    Arguments = "+x \"" + file.FullName + "\""
                	    }
            	    };
            	    procChmod.Start();
            	    procChmod.WaitForExit();
        	    }
        	
                var proc = new System.Diagnostics.Process
                {
                    StartInfo =
                    {
                	    UseShellExecute = false,
                        FileName = file.FullName,
                        RedirectStandardOutput = true
                    }
                };               
        	    proc.OutputDataReceived += (sender, args) =>
        	    {
        	    	if (args.Data == null) return;
        	        var activeProcId = int.Parse(args.Data);
        	        m_IsActive = activeProcId == s_ProcessId;
        	        if (!m_IsActive) UpdateThreads(false);
        	    };
        	    m_FocusProcess = proc;
        	    proc.Start();
        	    proc.BeginOutputReadLine();
        	}
            catch {}
#endif
        }
#elif UNITY_EDITOR_LINUX
        // TODO: Support for pausing Linux editor
        void CheckApplicationFocus() {}
#endif
        void OnDisable()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.update -= EditorUpdate;
#endif
            FluidBase.onFluidEnabled -= OnFluidEnabled;
            FluidBase.onFluidDisabled -= OnFluidDisabled;
        }

#if UNITY_EDITOR
        static T CreateProjectSettingsAsset<T>(T obj, string folder, string fileName) where T : Object
        {
            string path;

            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                path = Application.dataPath.Replace("/", "\\") + "\\Fluvio-ProjectSettings\\" + folder;
            }
            else
            {
                path = Application.dataPath + "/Fluvio-ProjectSettings/" + folder;
            }
            System.IO.Directory.CreateDirectory(path);

            var path2 = "Assets/Fluvio-ProjectSettings/" + folder + "/" + fileName;

            var currentObj = UnityEditor.AssetDatabase.LoadAssetAtPath(path2, typeof(T)) as T;
            if (currentObj)
            {
                UnityEditor.EditorUtility.CopySerialized(obj, currentObj);
                UnityEditor.AssetDatabase.Refresh();
            }
            else
            {
                UnityEditor.AssetDatabase.CreateAsset(obj, path2);
                UnityEditor.AssetDatabase.Refresh();
                currentObj = obj;
            }

            return currentObj;
        }
#endif
    }
}