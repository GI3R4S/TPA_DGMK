using System.ComponentModel.Composition;
using BusinessLogic.Model;
using LoggerBase;

namespace ViewModel
{
    public class AssemblyViewModel : TreeViewItem
    {
        private AssemblyMetadata assemblyMetadata;

        public AssemblyViewModel(AssemblyMetadata assembly, [Import(typeof(Logger))] Logger logger)
        {
            base.logger = logger;
            this.assemblyMetadata = assembly;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public AssemblyViewModel(AssemblyMetadata assembly)
        {
            this.assemblyMetadata = assembly;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            foreach (NamespaceMetadata namespaceMetadata in assemblyMetadata.Namespaces.ReturnEmptyIfItIsNull())
                base.Children.Add(new NamespaceViewModel(namespaceMetadata, logger));
            base.FinishedLoadingChildren();
        }
        public override string ToString()
        {
            return "(Assembly) " + assemblyMetadata.Name;
        }
        protected override bool CanLoadChildren()
        {
            return !assemblyMetadata.Namespaces.CheckIfItIsNullOrEmpty();
        }
    }
}