using System.Collections.Generic;
using System.Text;
using Model;
using Logging;
using System;

namespace ViewModel
{
    class MethodViewModel : TreeViewItem
    {
        private MethodMetadata methodMetadata;

        public MethodViewModel(MethodMetadata methodMetadata, Logger logger)
        {
            this.logger = logger;
            this.methodMetadata = methodMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            if (methodMetadata.ReturnType != null)
                base.Children.Add(new TypeViewModel(methodMetadata.ReturnType, logger));
            foreach (TypeMetadata attribute in methodMetadata.AttributesMetadata.ReturnEmptyIfItIsNull())
                base.Children.Add(new AttributeViewModel(attribute, logger));
            foreach (ParameterMetadata parameter in methodMetadata.Parameters.ReturnEmptyIfItIsNull())
                base.Children.Add(new ParameterViewModel(parameter, logger));
            base.FinishedLoadingChildren();
        }
        protected override bool CanLoadChildren()
        {
            return !(methodMetadata.ReturnType == null && methodMetadata.AttributesMetadata.CheckIfItIsNullOrEmpty()
                && methodMetadata.Parameters.CheckIfItIsNullOrEmpty());
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (methodMetadata.Name.Equals(methodMetadata.ReflectedType.TypeName))
                builder.Append("Constructor: ");
            else
                builder.Append("Method: ");
            builder.Append(ModifiersToString());
            if (methodMetadata.ReturnType != null)
                builder.Append(methodMetadata.ReturnType.TypeName + " ");
            else if (methodMetadata.ReflectedType.TypeName != methodMetadata.Name)
                builder.Append("void ");
            builder.Append(methodMetadata.Name);
            builder.Append(ParametersToString(methodMetadata.Parameters));
            return builder.ToString();
        }
        private string ModifiersToString()
        {
            StringBuilder builder = new StringBuilder();
            Tuple<AccessLevel, AbstractEnum, StaticEnum, VirtualEnum> modifiers = methodMetadata.Modifiers;
            builder.Append(methodMetadata.Modifiers.Item1.ToString().Substring(2).ToLower() + " ");
            builder.Append(methodMetadata.Modifiers.Item2 == AbstractEnum.Abstract ? "abstract " : "");
            builder.Append(methodMetadata.Modifiers.Item3 == StaticEnum.Static ? "static " : "");
            builder.Append(methodMetadata.Modifiers.Item4 == VirtualEnum.Virtual ? "virtual " : "");
            return builder.ToString();
        }
        private string ParametersToString(ICollection<ParameterMetadata> parameters)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(" (");
            foreach (ParameterMetadata parameter in parameters)
                builder.Append(parameter.TypeMetadata.TypeName + " " + parameter.Name + ", ");
            if (builder.Length > 2)
                builder.Remove(builder.Length - 2, 2);
            builder.Append(")");
            return builder.ToString();
        }
    }
}
