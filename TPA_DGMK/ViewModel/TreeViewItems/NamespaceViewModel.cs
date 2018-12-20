using System.ComponentModel.Composition;
using BusinessLogic.Model;
using LoggerBase;

namespace ViewModel
{
    class NamespaceViewModel : TreeViewItem
    {
        private NamespaceMetadata namespaceMetadata;

        public NamespaceViewModel(NamespaceMetadata namespaceMetadata, [Import(typeof(Logger))] Logger logger)
        {
            this.logger = logger;
            this.namespaceMetadata = namespaceMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            foreach (TypeMetadata typeMetadata in namespaceMetadata.Types.ReturnEmptyIfItIsNull())
                base.Children.Add(new TypeViewModel(typeMetadata, logger));
            base.FinishedLoadingChildren();
        }
        protected override bool CanLoadChildren()
        {
            return !namespaceMetadata.Types.CheckIfItIsNullOrEmpty();
        }
        public override string ToString()
        {
            return "Namespace: " + namespaceMetadata.NamespaceName;
        }
    }
}
