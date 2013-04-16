using System;
using System.ComponentModel;
using Microsoft.Phone.Scheduler;

namespace Ayls.WP8Toolkit.LiveTile
{
    public class LiveTileAgentManager : INotifyPropertyChanged
    {
        private readonly ILiveTileSettings _settings;
        private bool _resettedAlready = false;

        public LiveTileAgentManager(ILiveTileSettings settings)
        {
            _settings = settings;
        }

        public bool IsAgentEnabled { 
            get { return _settings.IsLiveTileEnabled; }
            private set { _settings.IsLiveTileEnabled = value; } 
        }

        public void ResetSchedule()
        {
            if (!_resettedAlready)
            {
                if (_settings.IsLiveTileEnabled)
                {
                    StartPeriodicAgent();
                }
                else
                {
                    RemoveAgent();
                }

                _resettedAlready = true;
            }
        }

        public LiveTileStartupResult StartPeriodicAgent()
        {
            var result = LiveTileStartupResult.Ok;

            var periodicTask = ScheduledActionService.Find(_settings.LiveTileAgentName) as PeriodicTask;
            if (periodicTask != null)
            {
                RemoveAgent();
            }

            periodicTask = new PeriodicTask(_settings.LiveTileAgentName);
            periodicTask.Description = _settings.LiveTileAgentDescription;

            try
            {
                ScheduledActionService.Add(periodicTask);
                IsAgentEnabled = true;

                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG_AGENT)
                ScheduledActionService.LaunchForTest(PeriodicTaskName, TimeSpan.FromSeconds(30));
#endif
            }
            catch (InvalidOperationException exception)
            {
                if (exception.Message.Contains("BNS Error: The action is disabled"))
                {
                    result = LiveTileStartupResult.Disabled;
                }

                IsAgentEnabled = false;
            }
            catch (SchedulerServiceException)
            {
                IsAgentEnabled = false;
                result = LiveTileStartupResult.Error;
            }

            NotifyPropertyChanged("IsAgentEnabled");

            return result;
        }

        public void RemoveAgent()
        {
            try
            {
                LiveTileHelper.UpdatePrimaryTileBadge(0);
                ScheduledActionService.Remove(_settings.LiveTileAgentName);
            }
            catch
            {
                // no action required
            }

            IsAgentEnabled = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
