using Model;
using Logging;
using System.ComponentModel.Composition;

namespace ViewModel
{
    class ParameterViewModel : TreeViewItem
    {
        ParameterMetadata parameterMetadata;

        public ParameterViewModel(ParameterMetadata parameterMetadata, [Import(typeof(Logger))] Logger logger)
        {
            this.logger = logger;
            this.parameterMetadata = parameterMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            base.Children.Add(new TypeViewModel(parameterMetadata.TypeMetadata, logger));
            foreach (TypeMetadata attribute in parameterMetadata.Attributes.ReturnEmptyIfItIsNull())
                base.Children.Add(new AttributeViewModel(attribute, logger));
            base.FinishedLoadingChildren();
        }
        public override string ToString()
        {
            return "Parameter: " + parameterMetadata.TypeMetadata.TypeName + " " + parameterMetadata.Name;
        }
    }
}
