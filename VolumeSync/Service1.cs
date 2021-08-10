using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using NAudio.CoreAudioApi;

//Documentation: https://docs.microsoft.com/en-us/dotnet/framework/windows-services/walkthrough-creating-a-windows-service-application-in-the-component-designer
//Possibly used this: https://www.c-sharpcorner.com/article/create-windows-services-in-c-sharp/
//API used: https://github.com/naudio/NAudio
//Debug: https://docs.microsoft.com/en-us/dotnet/framework/windows-services/how-to-debug-windows-service-applications

namespace VolumeSync
{
    public partial class VolumeSync : ServiceBase
    {
        public MMDevice[] monitors = { };
        private MMDevice defDevice;


        public VolumeSync(string[] args)
        {
            InitializeComponent();

            VolSyncLog = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("VolSyncSRC"))
            {
                System.Diagnostics.EventLog.CreateEventSource("VolSyncSRC", "VolSyncLog");
            }

            VolSyncLog.Source = "VolSyncSRC";
            VolSyncLog.Log = "VolSyncLog";
            
        }

        #region Implementation
        protected override void OnStart(string[] args)
        {
            MMDeviceEnumerator enumer = new MMDeviceEnumerator();
            VolSyncLog.WriteEntry("Begun Syncing");
            defDevice = enumer.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            List<MMDevice> dvcs = enumer.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All).ToList();
            //trim devices list

            dvcs.RemoveAll(BadDevice);
            dvcs.RemoveAll(Device_Unplugged);

            VolSyncLog.WriteEntry("found " + dvcs.Count +" devices");
            int next = 0;
            foreach (MMDevice device in dvcs)
            {
                if (device.DeviceFriendlyName.Contains("MX279"))
                {
                    monitors[next] = device;
                    next++;
                }
            }
            //register event on service start
            defDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
        }

        protected override void OnStop()
        {
            VolSyncLog.WriteEntry("Stopped Syncing");
        }
        #endregion

        #region filter predicates or whatever
        private static bool BadDevice(MMDevice dev)
        {
            try
            {
                string test = dev.FriendlyName;
            }
            catch (System.Runtime.InteropServices.COMException e1)
            {
                return true;
            }
            return false;
        }

        private static bool Device_Unplugged(MMDevice dev)
        {
            return !(dev.State == DeviceState.Active);
        }
        #endregion

        void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            //VolSyncLog.WriteEntry("Setting the volume to " + data.MasterVolume);
            //This shows data.MasterVolume, you can do whatever you want here
            foreach(MMDevice m2 in monitors)
            {
                m2.AudioEndpointVolume.MasterVolumeLevel = data.MasterVolume;//defDevice.AudioEndpointVolume.MasterVolumeLevel;
            }
        }

        private void VolSyncLog_EntryWritten(object sender, System.Diagnostics.EntryWrittenEventArgs e)
        {

        }

        internal void TestStartup(string[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();
        }
    }
}
