using BusinessLogic.Model;
using LoggerBase;
using System.ComponentModel.Composition;

namespace ViewModel
{
    class AttributeViewModel : TreeViewItem
    {
        TypeMetadata attributeMetadata;
        TypeViewModel attributeViewModel;

        public AttributeViewModel(TypeMetadata attributeMetadata, [Import(typeof(Logger))] Logger logger)
        {
            this.logger = logger;
            this.attributeMetadata = attributeMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            attributeViewModel = new TypeViewModel(attributeMetadata, logger);
            base.Children.Add(attributeViewModel);
            base.FinishedLoadingChildren();
        }
        public override string ToString()
        {
            return "Attribute: " + attributeMetadata.FullTypeName;
        }
    }
}