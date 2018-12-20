using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using ViewModel;

namespace CommandLine
{
    public class CLView
    {

        private int selection = 0;
        private int treeCounter = 0;
        private int treeIndentation = 0;
        private int previousItemsCount = 0;
        private bool tmpToDeserialize;
        private List<KeyValuePair<TreeViewItem, int>> tree = new List<KeyValuePair<TreeViewItem, int>>();
        private List<NotifyCollectionChangedEventArgs> itemsChanged = new List<NotifyCollectionChangedEventArgs>();

        public ViewModelBase ViewModel { get; set; }

        [ImportingConstructor]
        public CLView()
        {
            ViewModel = new ViewModelBase();
            selection = ViewModel.Items.Count;
            previousItemsCount = ViewModel.Items.Count;
            Console.WriteLine("Write 'Load', 'Serialize' or 'Deserialize' at any time");
            Console.WriteLine("Press any key to cotinue: ");
            Console.ReadKey();
        }

        public void Run()
        {
            do
            {
                FillTree();
                DisplayTree();
                previousItemsCount = ViewModel.Items.Count;
                string input = Console.ReadLine();
                if (input == null)
                    break;
                ParseInput(input);
            } while (true);
        }

        private void ParseInput(string input)
        {
            if (Int32.TryParse(input, out selection))
            {
                ViewModel.Select(selection);
            }
            else if (input.ToLower().Equals("load"))
            {
                Console.Clear();
                tree.Clear();
                ViewModel.Items.CollectionChanged += ItemsChangedEventHandler;

                if (ViewModel.ReadCommand.CanExecute(null))
                {
                    ViewModel.ReadCommand.Execute(null);
                }
            }
            else if (input.ToLower().Equals("de"))
            {
                Console.Clear();
                tree.Clear();
                ViewModel.Items.CollectionChanged += ItemsChangedEventHandler;
                tmpToDeserialize = true;

                if (ViewModel.DeserializeCommand.CanExecute(null))
                {
                    ViewModel.DeserializeCommand.Execute(null);
                }
            }
            else if (input.ToLower().Equals("se"))
            {
                if (ViewModel.SerializeCommand.CanExecute(null))
                {
                    ViewModel.SerializeCommand.Execute(null);
                }
            }
        }

        private void FillTree()
        {
            if (IsChoiceValid())
            {
                int i = 0;
                foreach (TreeViewItem item in ViewModel.Items)
                {
                    tree.Insert(treeCounter + i + selection - 1, new KeyValuePair<TreeViewItem, int>(item, treeIndentation));
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

        private bool IsChoiceValid()
        {
            return selection != 0 && previousItemsCount >= selection;
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
                if (tmpToDeserialize)
                {
                    FillTree();
                    DisplayTree();
                    tmpToDeserialize = false;
                }
            }
        }
    }
}
