using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using Logging;
using ViewModel;

namespace CommandLine
{
    public class CLView
    {
        private Logger logger;
        private ViewModelBase viewModel;
        private int selection;
        private List<KeyValuePair<TreeViewItem, int>> tree = new List<KeyValuePair<TreeViewItem, int>>();
        private int treeCounter = 0;
        private int treeIndentation = 0;
        private int previousItemsCount;
        private List<NotifyCollectionChangedEventArgs> itemsChanged = new List<NotifyCollectionChangedEventArgs>();

        public CLView(Logger logger)
        {
            this.logger = logger;
            viewModel = new ViewModelBase(new CLFileSelector(), logger);
            selection = viewModel.Items.Count;
            previousItemsCount = viewModel.Items.Count;
        }

        public void Run()
        {
            do
            {
                FillTree();
                DisplayTree();
                previousItemsCount = viewModel.Items.Count;
                string input = ReadLineWithCancel();
                if (input == null)
                    break;
                ParseInput(input);
            } while (true);
        }

        private void ParseInput(string input)
        {
            if (Int32.TryParse(input, out selection))
            {
                if (IsChoiceValid())
                    logger.Write(SeverityEnum.Information, "Element was selected " + selection);
                else
                    logger.Write(SeverityEnum.Error, "Selected element does not exist " + selection);
                ChooseItem();
            }

            else if (input.ToLower().Equals("load"))
            {
                Console.Clear();
                tree.Clear();
                if (viewModel.ReadCommand.CanExecute(null))
                    viewModel.ReadCommand.Execute(null);
                viewModel.Items.CollectionChanged += ItemsChangedEventHandler;
            }
            else
            {
                logger.Write(SeverityEnum.Error, "Entered: " + input);
            }
        }

        private void FillTree()
        {
            if (IsChoiceValid())
            {
                int i = 0;
                foreach (TreeViewItem item in viewModel.Items)
                {
                    try
                    {
                        tree.Insert(treeCounter + i + selection - 1, new KeyValuePair<TreeViewItem, int>(item, treeIndentation));
                        logger.Write(SeverityEnum.Information, "The element has been successfully inserted into the tree");
                    }
                    catch
                    {
                        logger.Write(SeverityEnum.Error, "It was attempted to insert an element beyond the size of the tree under the index " + (treeCounter + i + selection - 1));
                    }
                    i++;
                }
                treeIndentation++;
                treeCounter += selection;
            }
        }

        private void DisplayTree()
        {
            Console.Clear();
            int[] numbers = new int[treeIndentation];
            for (int i = 0; i < tree.Count; i++)
            {
                numbers[tree.ElementAt(i).Value]++;
                string singlePadding = "  ";
                StringBuilder padding = new StringBuilder();
                for (int j = 0; j < tree.ElementAt(i).Value; j++)
                    padding.Append(singlePadding);
                Console.WriteLine(padding.ToString() + numbers[tree.ElementAt(i).Value] + ". "
                    + tree.ElementAt(i).Key.ToString() + " "
                    + (tree.ElementAt(i).Key.IsExpandable() ? "(+)" : "(-)"));
            }
        }

        private void ChooseItem()
        {
            viewModel.Select(selection);
        }

        private bool IsChoiceValid()
        {
            return selection != 0 && previousItemsCount >= selection;
        }

        private string ReadLineWithCancel()
        {
            string result = null;

            StringBuilder buffer = new StringBuilder();

            ConsoleKeyInfo info = Console.ReadKey(true);
            while (info.Key != ConsoleKey.Enter && info.Key != ConsoleKey.Escape)
            {
                Console.Write(info.KeyChar);
                buffer.Append(info.KeyChar);
                info = Console.ReadKey(true);
            }

            if (info.Key == ConsoleKey.Enter)
            {
                result = buffer.ToString();
            }
            Console.WriteLine();
            return result;
        }

        private void ItemsChangedEventHandler(object sender, NotifyCollectionChangedEventArgs e)
        {
            itemsChanged.Add(e);
            if (e.Action.ToString() == "Add")
            {
                selection = 1;
                previousItemsCount = 1;
                treeCounter = 0;
                treeIndentation = 0;
                FillTree();
                DisplayTree();
            }
        }
    }
}
