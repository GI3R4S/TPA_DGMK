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

        private IFileSelector fileSelector;
        private Logger logger;
        private ObservableCollection<TreeViewItem> items = new ObservableCollection<TreeViewItem>();

        public ObservableCollection<TreeViewItem> Items
        {
            get => items;
            set
            {
                items = value;
            }
        }
        public Logger Logger { get => logger; set => logger = value; }
        public IFileSelector FileSelector { get => fileSelector; set => fileSelector = value; }

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
            if (path == null)
            {
                do
                {
                    Console.WriteLine("Insert path of .dll file.");
                    string loadedPath = FileSelector.SelectSource();
                    if (!File.Exists(loadedPath))
                    {
                        Console.WriteLine("There's no file at given path.\n Press any key to retry, ESC to end program");
                        if (Console.ReadKey().Key != ConsoleKey.Escape)
                        {
                            Console.Clear();
                            continue;
                        }

                        Environment.Exit(-1);
                    }
                    else if (!loadedPath.EndsWith(".dll"))
                    {
                        Console.WriteLine("Selected file doesn't have correct extension\n Press any key to retry, ESC to end program");
                        if (Console.ReadKey().Key != ConsoleKey.Escape)
                        {
                            Console.Clear();
                            continue;
                        }

                        Environment.Exit(-1);
                    }

                    path = loadedPath;
                    break;


                } while (true);
            }
            object assembly =  (object)Assembly.Load(File.ReadAllBytes(path));
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
            get { return readCommand ?? (readCommand = new RelayCommand( () =>  ReadFromFile())); }
        }
    }
}
