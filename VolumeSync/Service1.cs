using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using NAudio.CoreAudioApi;

namespace VolumeSync
{
    public partial class VolumeSync : ServiceBase
    {
        public MMDevice mon1;
        public MMDevice mon2;
        private static MMDeviceEnumerator enumer = new MMDeviceEnumerator();
        private MMDevice defDevice;

        public VolumeSync()
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

        protected override void OnStart(string[] args)
        {
            VolSyncLog.WriteEntry("Begun Syncing");
            //Set Volume of both monitors
            defDevice = enumer.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);
            List<MMDevice> dvcs = enumer.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.All).ToList();

            foreach (MMDevice device in dvcs)
            {
                if (device.DeviceFriendlyName.Contains("MX279-1"))
                {
                    mon1 = device;
                }
                if (device.DeviceFriendlyName.Contains("MX279-2"))
                {
                    mon2 = device;
                }
            }
        }

        protected override void OnStop()
        {
            VolSyncLog.WriteEntry("Stopped Syncing");
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            defDevice.AudioEndpointVolume.OnVolumeNotification += AudioEndpointVolume_OnVolumeNotification;
        }

        void AudioEndpointVolume_OnVolumeNotification(AudioVolumeNotificationData data)
        {
            // This shows data.MasterVolume, you can do whatever you want here
            mon1.AudioEndpointVolume.MasterVolumeLevel = defDevice.AudioEndpointVolume.MasterVolumeLevel;
            mon2.AudioEndpointVolume.MasterVolumeLevel = defDevice.AudioEndpointVolume.MasterVolumeLevel;
        }
    }
}
