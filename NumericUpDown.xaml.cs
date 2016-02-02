using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GabrielControls
{
    /// <summary>
    /// Interaction logic for NumericUpDown.xaml
    /// </summary>
    public partial class NumericUpDown : UserControl, INotifyPropertyChanged
    {
        #region Properties

        public decimal MinValue
        {
            get { return (decimal)GetValue(MinValueProperty); }
            set { SetValue(MinValueProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register("MinValue", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(decimal.MinValue));

        public decimal MaxValue
        {
            get { return (decimal)GetValue(MaxValueProperty); }
            set { SetValue(MaxValueProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register("MaxValue", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(decimal.MaxValue));

        public int DecimalPlaces
        {
            get { return (int)GetValue(DecimalPlacesProperty); }
            set { SetValue(DecimalPlacesProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty DecimalPlacesProperty = DependencyProperty.Register("DecimalPlaces", typeof(int), typeof(NumericUpDown), new PropertyMetadata(0));

        public decimal Value
        {
            get { return (decimal)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); OnPropertyChanged(); }
        }

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(decimal), typeof(NumericUpDown), new PropertyMetadata(default(decimal), new PropertyChangedCallback(OnValueChanged)));

        private string _sValue;

        public string sValue { get { return _sValue; } set { _sValue = value; OnPropertyChanged(); } }

        #endregion Properties

        public NumericUpDown()
        {
            InitializeComponent();
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as NumericUpDown).sValue = (d as NumericUpDown).Value.ToString();
        }

        private void Aumentar_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.Parse(sValue) < decimal.MaxValue && decimal.Parse(sValue) + 1 <= MaxValue)
            {
                Value++;
                sValue = Value.ToString();
                OnPropertyChanged("Value");
            }
            else
            {
                Value = MaxValue;
                sValue = Value.ToString();
                OnPropertyChanged("Value");
            }
        }

        private void Diminuir_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.Parse(sValue) > decimal.MinValue && decimal.Parse(sValue) - 1 >= MinValue)
            {
                Value--;
                sValue = Value.ToString();
                OnPropertyChanged("Value");
            }
            else
            {
                Value = MinValue;
                sValue = Value.ToString();
                OnPropertyChanged("Value");
            }
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text == "-" && (sender as TextBox).CaretIndex == 0 && !(sender as TextBox).Text.Contains("-"))
            {
                e.Handled = false;
                return;
            }
            else if (((e.Text == "." || e.Text == ",") && (((sender as TextBox).Text.Contains(",") || (sender as TextBox).Text.Contains(".")) || DecimalPlaces == 0)) || (Regex.IsMatch(e.Text, @"[^0-9,.]")))
            {
                e.Handled = true;
                return;
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (sender as TextBox);
            var caret = textbox.CaretIndex;

            textbox.Text = textbox.Text.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(" ", "");

            if (textbox.Text == "")
            {
                textbox.Text = (default(decimal) > MinValue) ? default(decimal).ToString() : MinValue.ToString();
            }

            try
            {
                if (decimal.Parse(textbox.Text) > MaxValue)
                {
                    textbox.Text = MaxValue.ToString();
                }
                else if (decimal.Parse(textbox.Text) < MinValue)
                {
                    textbox.Text = MinValue.ToString();
                }
            }
            catch (OverflowException)
            {
                if (textbox.Text.StartsWith("-"))
                {
                    textbox.Text = MinValue.ToString();
                }
                else
                {
                    textbox.Text = MaxValue.ToString();
                }
            }

            decimal val;

            if (decimal.TryParse(textbox.Text, out val) && val != 0)
            {
                Value = val;
            }

            var zeros = "";
            for (int i = 0; i < DecimalPlaces; i++)
            {
                if (zeros == "")
                {
                    zeros += ".0";
                }
                else
                {
                    zeros += "0";
                }
            }

            if (val != 0)
            {
                textbox.Text = string.Format("{0:0" + zeros + "}", Value);
            }

            textbox.CaretIndex = caret;
        }

        private void TextBox_Pasting(object sender, DataObjectPastingEventArgs e)
        {
            var textbox = (sender as TextBox);

            if (e.DataObject.GetDataPresent(typeof(string)))
            {
                string text = (string)e.DataObject.GetData(typeof(string));
                text = Regex.Replace(text, @"[^0-9,.]", "");
                if (DecimalPlaces == 0 || ((sender as TextBox).Text.Contains(",") || (sender as TextBox).Text.Contains(".")))
                {
                    text = Regex.Replace(text, @"[,.]", "");
                }
                var dataobj = new DataObject();
                dataobj.SetData(DataFormats.Text, text);
                e.DataObject = dataobj;
            }
            else
            {
                e.CancelCommand();
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            decimal val;

            if (decimal.TryParse(textbox.Text, out val))
            {
                Value = val;
            }

            var zeros = "";
            for (int i = 0; i < DecimalPlaces; i++)
            {
                if (zeros == "")
                {
                    zeros += ".0";
                }
                else
                {
                    zeros += "0";
                }
            }

            textbox.Text = string.Format("{0:0" + zeros + "}", Value);
        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([System.Runtime.CompilerServices.CallerMemberName]string propertyName = "")
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