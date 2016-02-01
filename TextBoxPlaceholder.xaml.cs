using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace GabrielControls
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class TextBoxPlaceholder : UserControl, INotifyPropertyChanged
    {
        #region Properties

        private Visibility _PlaceholderVisibility;

        public Visibility PlaceholderVisibility { get { return _PlaceholderVisibility; } set { _PlaceholderVisibility = value; OnPropertyChanged(); } }

        public string Placeholder
        {
            get { return (string)GetValue(PlaceholderProperty); }
            set { SetValue(PlaceholderProperty, value); OnPropertyChanged(); }
        }

        // Using a DependencyProperty as the backing store for Placeholder.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlaceholderProperty =
            DependencyProperty.Register("Placeholder", typeof(string), typeof(TextBoxPlaceholder), new PropertyMetadata(""));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); OnPropertyChanged(); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBoxPlaceholder), new PropertyMetadata(""));

        public int MaxLength
        {
            get { return (int)GetValue(MaxLengthProperty); }
            set { SetValue(MaxLengthProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MaxLength.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MaxLengthProperty =
            DependencyProperty.Register("MaxLength", typeof(int), typeof(TextBoxPlaceholder), new PropertyMetadata(int.MaxValue));

        public TextWrapping TextWrapping
        {
            get { return (TextWrapping)GetValue(TextWrappingProperty); }
            set { SetValue(TextWrappingProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TextWrapping.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextWrappingProperty =
            DependencyProperty.Register("TextWrapping", typeof(TextWrapping), typeof(TextBoxPlaceholder), new PropertyMetadata(TextWrapping.NoWrap));

        #endregion Properties

        public TextBoxPlaceholder()
        {
            InitializeComponent();
            PlaceholderVisibility = Visibility.Visible;
            DataContext = this;
        }

        private void VerificaNecessidadePlaceholder(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace((sender as TextBox).Text))
            {
                PlaceholderVisibility = Visibility.Hidden;
            }
            else
            {
                PlaceholderVisibility = Visibility.Visible;
            }
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion INotifyPropertyChanged Members
    }
}