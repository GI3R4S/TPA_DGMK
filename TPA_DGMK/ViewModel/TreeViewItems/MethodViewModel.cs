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
            foreach (TypeMetadata genericArgument in methodMetadata.GenericArguments.ReturnEmptyIfItIsNull())
                base.Children.Add(new TypeViewModel(genericArgument, logger));
            base.FinishedLoadingChildren();
        }
        protected override bool CanLoadChildren()
        {
            return !(methodMetadata.ReturnType == null && methodMetadata.Parameters.CheckIfItIsNullOrEmpty() && methodMetadata.GenericArguments.CheckIfItIsNullOrEmpty());
        }
        public override string ToString()
        {
            return "(Method) " + methodMetadata.ToString();
        }
    }
}
