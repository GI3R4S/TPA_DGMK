using LoggerBase;
using System.Collections.ObjectModel;

namespace ViewModel
{
    public abstract class TreeViewItem
    {
        protected Logger logger;
        private bool isExpanded;

        public ObservableCollection<TreeViewItem> Children { get; } = new ObservableCollection<TreeViewItem>();
        public abstract string Name { get; }
        public bool IsExpanded
        {
            get
            {
                return isExpanded;
            }
            set
            {
                if (value != isExpanded)
                {
                    isExpanded = value;
                    if (isExpanded)
                        LoadChildren();
                }
            }
        }

        protected virtual void LoadChildren()
        {
            Children.Clear();
            logger.Write(SeverityEnum.Information, "The element's children started to be loaded " + Name);
        }
        protected void FinishedLoadingChildren()
        {
            logger.Write(SeverityEnum.Information, "The element's children were loaded " + Name);
        }
        protected virtual bool CanLoadChildren()
        {
            return true;
        }
        public bool IsExpandable()
        {
            return !Children.CheckIfItIsNullOrEmpty();
        }
    }
}
