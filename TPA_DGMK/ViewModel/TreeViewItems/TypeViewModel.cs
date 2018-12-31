using System.ComponentModel.Composition;
using BusinessLogic.Model;
using LoggerBase;

namespace ViewModel
{
    public class TypeViewModel : TreeViewItem
    {
        private TypeMetadata typeMetadata;

        public TypeViewModel(TypeMetadata typeMetadata, [Import(typeof(Logger))] Logger logger)
        {
            this.logger = logger;
            this.typeMetadata = typeMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }
        public TypeViewModel(TypeMetadata typeMetadata)
        {
            this.typeMetadata = typeMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            foreach (PropertyMetadata propertyMetadata in typeMetadata.Properties.ReturnEmptyIfItIsNull())
                base.Children.Add(new PropertyViewModel(propertyMetadata, logger));
            foreach (TypeMetadata typeMetadata in typeMetadata.NestedTypes.ReturnEmptyIfItIsNull())
                base.Children.Add(new TypeViewModel(typeMetadata, logger));
            foreach (TypeMetadata typeMetadata in typeMetadata.ImplementedInterfaces.ReturnEmptyIfItIsNull())
                base.Children.Add(new TypeViewModel(typeMetadata, logger));
            foreach (MethodMetadata methodMetadata in typeMetadata.Methods.ReturnEmptyIfItIsNull())
                base.Children.Add(new MethodViewModel(methodMetadata, logger));
            foreach (MethodMetadata methodMetadata in typeMetadata.Constructors.ReturnEmptyIfItIsNull())
                base.Children.Add(new MethodViewModel(methodMetadata, logger));
            foreach (ParameterMetadata parameterMetadata in typeMetadata.Fields.ReturnEmptyIfItIsNull())
                base.Children.Add(new ParameterViewModel(parameterMetadata, logger));
            foreach (TypeMetadata typeMetadata in typeMetadata.GenericArguments.ReturnEmptyIfItIsNull())
                base.Children.Add(new TypeViewModel(typeMetadata, logger));
            if (typeMetadata.BaseType != null)
                base.Children.Add(new TypeViewModel(typeMetadata.BaseType, logger));
            if (typeMetadata.DeclaringType != null)
                base.Children.Add(new TypeViewModel(typeMetadata.DeclaringType, logger));
            base.FinishedLoadingChildren();
        }
        protected override bool CanLoadChildren()
        {
            return !(typeMetadata.Properties.CheckIfItIsNullOrEmpty() && typeMetadata.GenericArguments.CheckIfItIsNullOrEmpty()
                && typeMetadata.NestedTypes.CheckIfItIsNullOrEmpty() && typeMetadata.Methods.CheckIfItIsNullOrEmpty()
                && typeMetadata.Constructors.CheckIfItIsNullOrEmpty() && typeMetadata.Fields.CheckIfItIsNullOrEmpty() 
                && typeMetadata.ImplementedInterfaces.CheckIfItIsNullOrEmpty() && typeMetadata.BaseType == null && typeMetadata.DeclaringType == null);
        }
        public override string ToString()
        {
            return "(Type) " + typeMetadata.ToString();
        }
    }
}
