using BusinessLogic.Mapping;
using BusinessLogic.Model;
using BusinessLogic.Reflection;
using Data;
using Data.DataMetadata;
using LoggerBase;
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

        [Import(typeof(IFileSelector))] public IFileSelector FileSelector { get; set; }
        [Import(typeof(IDatabaseSelector))] public IDatabaseSelector DatabaseSelector { get; set; }
        [Import(typeof(Logger))] public Logger Logger { get; set; }
        [Import(typeof(IDisplay))] public IDisplay Display { get; set; }
        [Import(typeof(ISerializer))] public ISerializer Serializer { get; set; }
        [Import(typeof(AssemblyMetadataBase))] public AssemblyMetadataBase AssemblyMetadataBase { get; set; }

        public RelayCommand readCommand;
        public RelayCommand deserializeCommand;
        public RelayCommand serializeCommand;

        private ObservableCollection<TreeViewItem> items = new ObservableCollection<TreeViewItem>();

        public ObservableCollection<TreeViewItem> Items
        {
            get => items;
            private set
            {
                items = value;
            }
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
            Logger.Write(SeverityEnum.Information, "Loading completed");
            Display.DisplayInformation("Loading Completed");
        }

        private async Task Serialize(string path = null)
        {
            Logger.Write(SeverityEnum.Information, "The option to 'serialize' was chosen: ");
            if (reflector.AssemblyMetadata == null)
                return;

            if (path == null)
            {
                if (Serializer.ToString().Contains("Database"))
                    path = DatabaseSelector.SelectTarget();
                else
                    path = FileSelector.SelectTarget();
            }
            await Task.Run(() => Serializer.Serialize(AssemblyMetadataMapper.MapToSerialize(reflector.AssemblyMetadata, AssemblyMetadataBase.GetType()), path));
            Logger.Write(SeverityEnum.Information, "Serialization completed");
            Display.DisplayInformation("Serialization Completed");
        }

        private async Task Deserialize(string path = null)
        {
            Logger.Write(SeverityEnum.Information, "The option to 'deserialize' was chosen");

            if (path == null)
            {
                if (Serializer.ToString().Contains("Database"))
                    path = DatabaseSelector.SelectSource();
                else
                    path = FileSelector.SelectSource();
            }

            AssemblyMetadata assemblyMetadata = await Task.Run(() => AssemblyMetadataMapper.MapToDeserialize(Serializer.Deserialize(path)));

            try
            {
                reflector = new Reflector(assemblyMetadata);
            }
            catch
            {
                Logger.Write(SeverityEnum.Error, "Reflection error while deserializing");
            }
            Items.Clear();
            Items.Add(new AssemblyViewModel(reflector.AssemblyMetadata, Logger));
            Logger.Write(SeverityEnum.Information, "Deserialization completed");
            Display.DisplayInformation("Deserialization Completed");
        }

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
