using Logging;
using Model;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelBase
    {
        private int selection = 0;
        private AssemblyMetadata assemblyMetadata;

        public IFileSelector FileSelector { get; private set; }
        public Logger Logger { get; set; }
        private ObservableCollection<TreeViewItem> items = new ObservableCollection<TreeViewItem>();

        public ObservableCollection<TreeViewItem> Items
        {
            get => items;
            private set
            {
                items = value;
            }
        }

        public ViewModelBase(IFileSelector fileSelector, Logger logger)
        {
            this.Logger = logger;
            this.FileSelector = fileSelector;
        }
        public bool Select(int selection)
        {
            this.selection = selection;
            return Update();
        }
        private bool Update()
        {
            if (selection != 0 && items.Count >= selection)
            {
                items[selection - 1].IsExpanded = true;
                items = items[selection - 1].Children;
                Logger.Write(SeverityEnum.Information, "The list of elements was updated");
                return true;
            }
            return false;
        }
        private void ReadFromFile(string path = null)
        {
            Logger.Write(SeverityEnum.Information, "The option to load was selected");
            path = FileSelector.SelectSource();

            object assembly = (object)Assembly.Load(File.ReadAllBytes(path));
            try
            {
                assemblyMetadata = (AssemblyMetadata)assembly;
            }
            catch
            {
                assemblyMetadata = (AssemblyMetadata)Activator.CreateInstance(typeof(AssemblyMetadata), assembly);
            }
            Items.Clear();
            Items.Add(new AssemblyViewModel(assemblyMetadata, Logger));
        }

        private RelayCommand readCommand;

        public ICommand ReadCommand
        {
            get { return readCommand ?? (readCommand = new RelayCommand(() => ReadFromFile())); }
        }
    }
}
