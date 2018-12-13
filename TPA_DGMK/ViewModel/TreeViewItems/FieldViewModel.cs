using System.Text;
using Model;
using Logging;
using System.ComponentModel.Composition;

namespace ViewModel
{
    class FieldViewModel : TreeViewItem
    {
        FieldMetadata fieldMetadata;

        public FieldViewModel(FieldMetadata fieldMetadata, [Import(typeof(Logger))] Logger logger)
        {
            this.logger = logger;
            this.fieldMetadata = fieldMetadata;
            if (CanLoadChildren())
                Children.Add(null);
        }

        public override string Name => this.ToString();

        protected override void LoadChildren()
        {
            base.LoadChildren();
            base.Children.Add(new TypeViewModel(fieldMetadata.TypeMetadata, logger));
            base.FinishedLoadingChildren();
        }
        public override string ToString()
        {
            return "Field: " + ModifiersToString() + fieldMetadata.TypeMetadata.TypeName + " " + fieldMetadata.Name + GenericArgumentsToString();
        }
        private string ModifiersToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(fieldMetadata.AccessLevel.ToString().Substring(2).ToLower() + " ");
            builder.Append(fieldMetadata.IsStatic ? "static " : "");
            return builder.ToString();
        }
        private string GenericArgumentsToString()
        {
            if (fieldMetadata.TypeMetadata.GenericArguments.CheckIfItIsNullOrEmpty())
                return "";
            StringBuilder builder = new StringBuilder();
            builder.Append("<");
            foreach (TypeMetadata arg in fieldMetadata.TypeMetadata.GenericArguments)
                builder.Append(arg.TypeName + ",");
            builder.Remove(builder.Length - 1, 1);
            builder.Append(">");
            return builder.ToString();
        }
    }
}