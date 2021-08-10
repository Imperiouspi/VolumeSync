namespace VolumeSync
{
    partial class VolumeSync
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VolSyncLog = new System.Diagnostics.EventLog();
            ((System.ComponentModel.ISupportInitialize)(this.VolSyncLog)).BeginInit();
            // 
            // VolSyncLog
            // 
            this.VolSyncLog.EntryWritten += new System.Diagnostics.EntryWrittenEventHandler(this.VolSyncLog_EntryWritten);
            // 
            // VolumeSync
            // 
            this.ServiceName = "VolumeSyncService";
            ((System.ComponentModel.ISupportInitialize)(this.VolSyncLog)).EndInit();

        }

        #endregion

        private System.Diagnostics.EventLog VolSyncLog;
    }
}
