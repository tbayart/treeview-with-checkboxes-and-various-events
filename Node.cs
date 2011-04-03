using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Collections;
using System;

namespace WpfApplication
{
    public class Node : INotifyPropertyChanged
    {
        public Node()
        {
            this.id = Guid.NewGuid().ToString();
        }

        private ObservableCollection<Node> children = new ObservableCollection<Node>();
        private ObservableCollection<Node> parent = new ObservableCollection<Node>();
        private string text;
        private string id;
        private bool? isChecked = true;
        private bool isExpanded;
        
        public ObservableCollection<Node> Children
        {
            get { return this.children; }
        }

        public ObservableCollection<Node> Parent
        {
            get { return this.parent; }
        }

        public bool? IsChecked
        {
            get { return this.isChecked; }
            set
            {
                this.isChecked = value;
                RaisePropertyChanged("IsChecked");
            }
        }

        public string Text
        {
            get { return this.text; }
            set
            {
                this.text = value;
                RaisePropertyChanged("Text");
            }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                isExpanded = value;
                RaisePropertyChanged("IsExpanded");
            }
        }

        public string Id
        {
            get { return this.id; }
            set
            {
                this.id = value;
            }
        }
        
        public event PropertyChangedEventHandler PropertyChanged;
        private void RaisePropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            int countCheck = 0;
            if (propertyName == "IsChecked")
            {
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count == 0 && this.Children.Count != 0)
                {
                    CheckedTreeParent(this.Children, this.IsChecked);
                }
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count > 0 && this.Children.Count > 0)
                {
                    CheckedTreeChildMiddle(this.Parent, this.Children, this.IsChecked);
                }
                if (this.Id == CheckBoxId.checkBoxId && this.Parent.Count > 0 && this.Children.Count == 0)
                {
                    CheckedTreeChild(this.Parent, countCheck);
                }
            }            
        }

        private void CheckedTreeChildMiddle(ObservableCollection<Node> itemsParent, ObservableCollection<Node> itemsChild, bool? isChecked)
        {
            int countCheck = 0;
            CheckedTreeParent(itemsChild, isChecked);
            CheckedTreeChild(itemsParent, countCheck);
        }

        private void CheckedTreeParent(ObservableCollection<Node> items, bool? isChecked)
        {
            foreach (Node item in items)
            {
                item.IsChecked = isChecked;
                if (item.Children.Count != 0) CheckedTreeParent(item.Children, isChecked);
            }
        }

        private void CheckedTreeChild(ObservableCollection<Node> items, int countCheck)
        {            
            bool isNull = false;
            foreach (Node paren in items)
            {
                foreach (Node child in paren.Children)
                {
                    if (child.IsChecked == true || child.IsChecked == null)
                    {
                        countCheck++;
                        if (child.IsChecked == null)
                            isNull = true;
                    }
                }
                if (countCheck != paren.Children.Count && countCheck != 0) paren.IsChecked = null;
                else if (countCheck == 0) paren.IsChecked = false;
                else if (countCheck == paren.Children.Count && isNull) paren.IsChecked = null;
                else if (countCheck == paren.Children.Count && !isNull) paren.IsChecked = true;
                if (paren.Parent.Count != 0) CheckedTreeChild(paren.Parent, 0);
            }
        }
    }

    public struct CheckBoxId
    {
        public static string checkBoxId;
    }
}
