using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System;

namespace WpfApplication
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Node> Nodes { get; private set; }

        public MainWindow()
        {
            Nodes = new ObservableCollection<Node>();
            InitializeComponent();
            FillingTree();
        }

        /// <summary>
        /// Filling tree different values
        /// </summary>
        private void FillingTree()
        {
            Nodes.Clear();
            for (int i = 0; i < 5; i++)
            {
                var level_1_items = new Node() { Text = " Level 1 Item " + (i + 1) };
                for (int j = 0; j < 2; j++)
                {
                    var level_2_items = new Node() { Text = " Level 2 Item " + (j + 1) };
                    level_2_items.Parent.Add(level_1_items);
                    level_1_items.Children.Add(level_2_items);
                    for (int n = 0; n < 2; n++)
                    {
                        var level_3_items = new Node() { Text = " Level 3 Item " + (n + 1) };
                        level_3_items.Parent.Add(level_2_items);
                        level_2_items.Children.Add(level_3_items);
                    }
                }

                Nodes.Add(level_1_items);
            }
            treeView.ItemsSource = Nodes;
        }

        /// <summary>
        /// Take Id from CheckBox Uid and transfer value to CheckBoxId struct
        /// </summary>
        /// <param name="sender">The CheckBox clicked.</param>
        /// <param name="e">Parameters associated to the mouse event.</param>
        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            CheckBox currentCheckBox = (CheckBox)sender;
            CheckBoxId.checkBoxId = currentCheckBox.Uid;
        }

        /// <summary>
        /// Take Id from CheckBox Uid and transfer value to CheckBoxId struct
        /// </summary>
        /// <param name="sender">The CheckBox check with space button.</param>
        /// <param name="e">Parameters associated to the mouse event.</param>
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key==Key.Space)
            {
                CheckBox currentCheckBox = (CheckBox)sender;
                CheckBoxId.checkBoxId = currentCheckBox.Uid;
            }
        }



        #region Check, Uncheck and Invert all checkboxes

        private void buttonCheckAll_Click(object sender, RoutedEventArgs e)
        {
            CheckedTree(Nodes, true);
        }

        private void buttonUncheckAll_Click(object sender, RoutedEventArgs e)
        {
            CheckedTree(Nodes, false);
        }

        private void CheckedTree(ObservableCollection<Node> items, bool isChecked)
        {
            foreach (Node item in items)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count != 0) CheckedTree(item.Children, isChecked);
            }
        }

        private void buttonInvert_Click(object sender, RoutedEventArgs e)
        {
            CheckBoxId.checkBoxId = "";
            InvertTree(Nodes);
        }

        private void InvertTree(ObservableCollection<Node> items)
        {
            foreach (Node item in items)
            {
                if (item.IsChecked != null)
                {
                    if (item.IsChecked == true) item.IsChecked = false;
                    else item.IsChecked = true;
                    if (item.Parent.Count!=0 && item.Parent[0].IsChecked == true) item.IsChecked = true;
                    if (item.Parent.Count != 0 && item.Parent[0].IsChecked == false) item.IsChecked = false;
                }
                if (item.Children.Count != 0) InvertTree(item.Children);
            }
        }

        #endregion

        #region Expand and Collapse items

        private void buttonExpand_Click(object sender, RoutedEventArgs e)
        {
            ExpandTree(Nodes, true);
        }

        private void buttonCollapse_Click(object sender, RoutedEventArgs e)
        {
            ExpandTree(Nodes, false);
        }

        private void ExpandTree(ObservableCollection<Node> items, bool isExpanded)
        {
            foreach (Node item in items)
            {
                item.IsExpanded = isExpanded;
                if (item.Children.Count != 0) ExpandTree(item.Children, isExpanded);
            }
        }

        #endregion


    }
}
