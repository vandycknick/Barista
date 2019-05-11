using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AppKit;
using Barista.Core.Data;
using Barista.MacOS.Views.Preferences;
using Foundation;

namespace Barista.MacOS.ViewModels
{
    public class StatusBarViewModel : NSObject
    {
        private readonly PluginManager _pluginManager;
        private readonly PreferencesWindowFactory _preferencesWindowFactory;

        public StatusBarViewModel(PluginManager pluginManager, PreferencesWindowFactory preferencesWindowFactory)
        {
            _pluginManager = pluginManager;
            _preferencesWindowFactory = preferencesWindowFactory;

            Plugins = new ReadOnlyObservableCollection<Plugin>(_pluginManager.ListPlugins());
        }

        public ReadOnlyObservableCollection<Plugin> Plugins { get; private set; }

        public List<IObservable<IReadOnlyCollection<IPluginMenuItem>>> PluginExecutions
        {
            get => _pluginManager.ListPlugins()
                    .Where(plugin => plugin.Enabled)
                    .Select(plugin => _pluginManager.Monitor(plugin))
                    .ToList();
        }

        public void OnStatusItemClicked(IPluginMenuItem item)
        {
            if (item.IsCommand)
            {
                _pluginManager.InvokeCommand(item.Command);
            }
        }

        public void OnOpenPreferences()
        {
            var controller = _preferencesWindowFactory.Create();
            controller.Show();

            NSApplication.SharedApplication.ActivateIgnoringOtherApps(true);
            controller.Window.MakeKeyAndOrderFront(this);
        }

        public void OnRefreshAll() => _pluginManager.RunAll();

        public void OnExit() => NSApplication.SharedApplication.Terminate(NSApplication.SharedApplication);
    }
}
