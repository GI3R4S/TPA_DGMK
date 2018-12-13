using Data_De_Serialization;
using Logging;
using Model;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModel
{
    public class ViewModelBase
    {
        private int selection = 0;
        private Reflector reflector;

        public IFileSelector FileSelector { get; private set; }
        public IFileSelector DatabaseSelector { get; private set; }
        public SerializerTemplate serializer { get; private set; }

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

        [ImportingConstructor]
        public ViewModelBase(IFileSelector fileSelector, IFileSelector databaseSelector, [Import(typeof(Logger))] Logger logger,
            [Import(typeof(SerializerTemplate))] SerializerTemplate serializer)
        {
            this.Logger = logger;
            this.DatabaseSelector = databaseSelector;
            this.FileSelector = fileSelector;
            this.serializer = serializer;
        }

        public ViewModelBase()
        {

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
        private void LoadDllFile(string path = null)
        {
            Logger.Write(SeverityEnum.Information, "The option to load was selected");

            do
            {
                path = FileSelector.SelectSource();
                if (path == null || !path.EndsWith(".dll"))
                {
                    FileSelector.FailureAlert();
                }
            } while (path == null || !path.EndsWith(".dll"));

            try
            {
                reflector = new Reflector(path);
            }
            catch
            {
                Logger.Write(SeverityEnum.Error, "Reflection error while loading dll");
            }
            Items.Clear();
            Items.Add(new AssemblyViewModel(reflector.AssemblyMetadata, Logger));
        }

        private async Task Deserialize(string path = null)
        {
            Logger.Write(SeverityEnum.Information, "The option to 'deserialize' was chosen");

            if (path == null)
            {
                if (serializer.ToString().ToLower().Contains("database"))
                    path = DatabaseSelector.SelectSource();
                else
                    path = FileSelector.SelectSource();
            }

            AssemblyMetadata assemblyMetadata = await Task.Run(() => serializer.Deserialize<AssemblyMetadata>(path));

            try
            {
                reflector = new Reflector(assemblyMetadata);
            }
            catch
            {
                Logger.Write(SeverityEnum.Error, "Reflection error while deserializing");
            }
            Items.Clear();
            Items.Add(new AssemblyViewModel(assemblyMetadata, Logger));
        }

        private async Task Serialize(string path = null)
        {
            Logger.Write(SeverityEnum.Information, "The option to 'serialize' was chosen: ");
            if (reflector.AssemblyMetadata == null)
                return;

            if (path == null)
            {
                if (serializer.ToString().ToLower().Contains("database"))
                    path = DatabaseSelector.SelectTarget();
                else
                    path = FileSelector.SelectTarget();
            }
            await Task.Run(() => serializer.Serialize(reflector.AssemblyMetadata, path));
        }

        private RelayCommand readCommand;
        private RelayCommand deserializeCommand;
        private RelayCommand serializeCommand;

        public ICommand ReadCommand
        {
            get { return readCommand ?? (readCommand = new RelayCommand(() => LoadDllFile())); }
        }
        public ICommand DeserializeCommand
        {
            get { return deserializeCommand ?? (deserializeCommand = new RelayCommand(async () => await Deserialize())); }
        }
        public ICommand SerializeCommand
        {
            get { return serializeCommand ?? (serializeCommand = new RelayCommand(async () => await Serialize())); }
        }
    }
}
