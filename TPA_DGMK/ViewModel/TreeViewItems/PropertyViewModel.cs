using System.ComponentModel.Composition;
using BusinessLogic.Model;
using LoggerBase;

namespace ViewModel
{
    class PropertyViewModel : TreeViewItem
    {
        private PropertyMetadata propertyMetadata;

        public PropertyViewModel(PropertyMetadata propertyMetadata, [Import(typeof(Logger))] Logger logger)
        {
            this.logger = logger;
            this.propertyMetadata = propertyMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            if (propertyMetadata.TypeMetadata != null)
                base.Children.Add(new TypeViewModel(propertyMetadata.TypeMetadata, logger));
            base.FinishedLoadingChildren();
        }
        public override string ToString()
        {
            return "(Property) " + propertyMetadata.TypeMetadata.TypeName + " " + propertyMetadata.Name;
        }
    }
}
