using System.Text;
using Model;
using Logging;

namespace ViewModel
{
    class PropertyViewModel : TreeViewItem
    {
        private PropertyMetadata propertyMetadata;

        public PropertyViewModel(PropertyMetadata propertyMetadata, Logger logger)
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
            foreach (TypeMetadata attribute in propertyMetadata.AttributesMetadata.ReturnEmptyIfItIsNull())
                base.Children.Add(new AttributeViewModel(attribute, logger));
            base.Children.Add(new TypeViewModel(propertyMetadata.TypeMetadata, logger));
            base.FinishedLoadingChildren();
        }
        public override string ToString()
        {
            return "Property: " + propertyMetadata.TypeMetadata.TypeName + GenericArgumentsToString() + " " + propertyMetadata.Name;
        }
        private string GenericArgumentsToString()
        {
            if (propertyMetadata.TypeMetadata.GenericArguments.CheckIfItIsNullOrEmpty())
                return "";
            StringBuilder builder = new StringBuilder();
            builder.Append("<");
            foreach (TypeMetadata arg in propertyMetadata.TypeMetadata.GenericArguments)
                builder.Append(arg.TypeName + ",");
            builder.Remove(builder.Length - 1, 1);
            builder.Append(">");
            return builder.ToString();
        }
    }
}
