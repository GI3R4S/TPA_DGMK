using System.Collections.Generic;
using System.Text;
using System.ComponentModel.Composition;
using BusinessLogic.Model;
using LoggerBase;

namespace ViewModel
{
    class MethodViewModel : TreeViewItem
    {
        private MethodMetadata methodMetadata;

        public MethodViewModel(MethodMetadata methodMetadata, [Import(typeof(Logger))] Logger logger)
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
            foreach (ParameterMetadata parameter in methodMetadata.Parameters.ReturnEmptyIfItIsNull())
                base.Children.Add(new ParameterViewModel(parameter, logger));
            base.FinishedLoadingChildren();
        }
        protected override bool CanLoadChildren()
        {
            return !(methodMetadata.ReturnType == null && methodMetadata.Parameters.CheckIfItIsNullOrEmpty());
        }
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            if (methodMetadata.ReturnType != null)
                builder.Append(methodMetadata.ReturnType.TypeName + " ");
            builder.Append(methodMetadata.Name);
            builder.Append(ParametersToString(methodMetadata.Parameters));
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
