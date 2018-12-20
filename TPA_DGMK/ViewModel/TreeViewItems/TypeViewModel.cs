using System.Text;
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
            foreach (MethodMetadata methodMetadata in typeMetadata.Methods.ReturnEmptyIfItIsNull())
                base.Children.Add(new MethodViewModel(methodMetadata, logger));
            foreach (MethodMetadata methodMetadata in typeMetadata.Constructors.ReturnEmptyIfItIsNull())
                base.Children.Add(new MethodViewModel(methodMetadata, logger));
            foreach (FieldMetadata fieldMetadata in typeMetadata.Fields.ReturnEmptyIfItIsNull())
                base.Children.Add(new FieldViewModel(fieldMetadata, logger));
            if (typeMetadata.BaseType != null)
                base.Children.Add(new TypeViewModel(typeMetadata.BaseType, logger));
            foreach (TypeMetadata implementedInterface in typeMetadata.ImplementedInterfaces.ReturnEmptyIfItIsNull())
                base.Children.Add(new TypeViewModel(implementedInterface, logger));
            base.FinishedLoadingChildren();
        }
        protected override bool CanLoadChildren()
        {
            return !(typeMetadata.Properties.CheckIfItIsNullOrEmpty()
                && typeMetadata.NestedTypes.CheckIfItIsNullOrEmpty() && typeMetadata.Methods.CheckIfItIsNullOrEmpty() &&
                typeMetadata.Constructors.CheckIfItIsNullOrEmpty() && typeMetadata.Fields.CheckIfItIsNullOrEmpty() &&
                typeMetadata.BaseType == null);
        }
        public override string ToString()
        {
            string modifiers = ModifiersToString();
            return modifiers + typeMetadata.TypeName + GenericArgumentsToString() + ExtendsAndImplementsToString();
        }

        private string ModifiersToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(typeMetadata.IsSealed ? "sealed " : "");
            builder.Append(typeMetadata.IsAbstract ? "abstract " : "");
            return builder.ToString();
        }
        private string ExtendsAndImplementsToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(typeMetadata.BaseType != null ? " : " + typeMetadata.BaseType.FullTypeName : "");
            if (!typeMetadata.ImplementedInterfaces.CheckIfItIsNullOrEmpty())
            {
                builder.Append(typeMetadata.BaseType == null ? " : " : ", ");
                foreach (TypeMetadata implementedInterface in typeMetadata.ImplementedInterfaces)
                {
                    builder.Append(implementedInterface.TypeName + ", ");
                }
                builder.Length = builder.Length - 2;
            }
            return builder.ToString();
        }
        private string GenericArgumentsToString()
        {
            if (typeMetadata.GenericArguments.CheckIfItIsNullOrEmpty())
                return "";
            StringBuilder builder = new StringBuilder();
            builder.Append("<");
            foreach (TypeMetadata arg in typeMetadata.GenericArguments)
                builder.Append(arg.TypeName + ",");
            builder.Remove(builder.Length - 1, 1);
            builder.Append(">");
            return builder.ToString();
        }
    }
}
