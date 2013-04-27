using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Phone.Scheduler;
using Microsoft.Phone.Shell;

namespace Ayls.WP8Toolkit.LiveTile
{
    public class LiveTileAgentManager : INotifyPropertyChanged
    {
        private readonly string _agentName;
        private readonly string _agentDescription;

        public LiveTileAgentManager(string agentName, string agentDescription)
        {
            _agentName = agentName;
            _agentDescription = agentDescription;
        }

        public bool IsAgentEnabled { get; private set; }

        public LiveTileStartupResult StartPeriodicAgent()
        {
            var result = LiveTileStartupResult.Ok;

            var periodicTask = ScheduledActionService.Find(_agentName) as PeriodicTask;
            if (periodicTask != null)
            {
                RemoveAgent();
            }

            periodicTask = new PeriodicTask(_agentName);
            periodicTask.Description = _agentDescription;

            try
            {
                ScheduledActionService.Add(periodicTask);
                IsAgentEnabled = true;

                // If debugging is enabled, use LaunchForTest to launch the agent in one minute.
#if(DEBUG_AGENT)
                ScheduledActionService.LaunchForTest(_agentName, TimeSpan.FromSeconds(30));
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
                UpdatePrimaryTileBadge(0);
                ScheduledActionService.Remove(_agentName);
            }
            catch
            {
                // no action required
            }

            IsAgentEnabled = false;
        }

        public static void UpdatePrimaryTileBadge(int count)
        {
            var primaryTileData = new FlipTileData();
            primaryTileData.Count = count;
            var primaryTile = ShellTile.ActiveTiles.First();
            primaryTile.Update(primaryTileData);
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
