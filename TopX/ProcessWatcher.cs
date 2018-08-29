using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;

namespace TopX
{
    public class ProcessWatcher : IDisposable
    {
        public ManagementEventWatcher ProcessStartWatcher { get; set; }
        public ManagementEventWatcher ProcessStopWatcher { get; set; }

        public event Action<string> ProcessStarted;
        public event Action<string> ProcessStopped;

        public IEnumerable<string> ProcessesToWatch { get; set; }

        public ProcessWatcher(IEnumerable<string> processesToWatch)
        {
            ProcessesToWatch = processesToWatch;

            ProcessStartWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStartTrace"));
            ProcessStartWatcher.EventArrived += new EventArrivedEventHandler(StartWatch_EventArrived);
            ProcessStartWatcher.Start();

            ProcessStopWatcher = new ManagementEventWatcher(new WqlEventQuery("SELECT * FROM Win32_ProcessStopTrace"));
            ProcessStopWatcher.EventArrived += new EventArrivedEventHandler(StopWatch_EventArrived);
            ProcessStopWatcher.Start();
        }

        private void StartWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var startedProcess = e.NewEvent.Properties["ProcessName"].Value.ToString().ToLower().TrimEnd(".exe");
            if (ProcessesToWatch.Contains(startedProcess))
                ProcessStarted?.Invoke(startedProcess);
        }

        private void StopWatch_EventArrived(object sender, EventArrivedEventArgs e)
        {
            var stoppedProcess = e.NewEvent.Properties["ProcessName"].Value.ToString().ToLower().TrimEnd(".exe");
            if (ProcessesToWatch.Contains(stoppedProcess))
                ProcessStopped?.Invoke(stoppedProcess);
        }

        ~ProcessWatcher()
        {
            Dispose();
        }

        public void Dispose()
        {
            ProcessStartWatcher.Stop();
            ProcessStartWatcher.Dispose();

            ProcessStopWatcher.Stop();
            ProcessStopWatcher.Dispose();
        }
    }
}
