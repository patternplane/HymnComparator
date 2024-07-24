using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace HYMNModifier
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(viewmodel.VMMainWindow viewmodel)
        {
            InitializeComponent();

            this.DataContext = viewmodel;
            this.SetBinding(DoCompareCommandProperty, new Binding("CDoCompare"));
            this.SetBinding(SelectionChangedCommandProperty, new Binding("CSelectionChanged"));
            this.SetBinding(RecheckCommandProperty, new Binding("CRecheck"));
            this.SetBinding(ReGenerateCommandProperty, new Binding("CReGenerate"));
            this.SetBinding(SaveCommandProperty, new Binding("CSave"));
        }

        // ======================= Binding Properties =======================

        public static readonly DependencyProperty DoCompareCommandProperty =
        DependencyProperty.Register(
            name: "DoCompareCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(MainWindow));

        public ICommand DoCompareCommand
        {
            get => (ICommand)GetValue(DoCompareCommandProperty);
            set => SetValue(DoCompareCommandProperty, value);
        }

        public static readonly DependencyProperty SelectionChangedCommandProperty =
        DependencyProperty.Register(
            name: "SelectionChangedCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(MainWindow));

        public ICommand SelectionChangedCommand
        {
            get => (ICommand)GetValue(SelectionChangedCommandProperty);
            set => SetValue(SelectionChangedCommandProperty, value);
        }

        public static readonly DependencyProperty RecheckCommandProperty =
        DependencyProperty.Register(
            name: "RecheckCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(MainWindow));

        public ICommand RecheckCommand
        {
            get => (ICommand)GetValue(RecheckCommandProperty);
            set => SetValue(RecheckCommandProperty, value);
        }

        public static readonly DependencyProperty ReGenerateCommandProperty =
        DependencyProperty.Register(
            name: "ReGenerateCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(MainWindow));

        public ICommand ReGenerateCommand
        {
            get => (ICommand)GetValue(ReGenerateCommandProperty);
            set => SetValue(ReGenerateCommandProperty, value);
        }

        public static readonly DependencyProperty SaveCommandProperty =
        DependencyProperty.Register(
            name: "SaveCommand",
            propertyType: typeof(ICommand),
            ownerType: typeof(MainWindow));

        public ICommand SaveCommand
        {
            get => (ICommand)GetValue(SaveCommandProperty);
            set => SetValue(SaveCommandProperty, value);
        }

        // ======================= Event Handler =======================

        private void EH_fileopen1(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UE_FileTextBox1.Text = fd.FileName;
                UE_FileTextBox1.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void EH_fileopen2(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog fd = new System.Windows.Forms.OpenFileDialog();
            if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                UE_FileTextBox2.Text = fd.FileName;
                UE_FileTextBox2.GetBindingExpression(TextBox.TextProperty).UpdateSource();
            }
        }

        private void EH_StartButtonClick(object sender, RoutedEventArgs e)
        {
            DoCompareCommand.Execute(null);
        }

        private void EH_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0 && (int)e.AddedItems[0] >= 1 && (int)e.AddedItems[0] <= 645)
                SelectionChangedCommand.Execute(e.AddedItems[0]);
        }

        private void EH_RecheckButtonClick(object sender, RoutedEventArgs e)
        {
            RecheckCommand.Execute(null);
        }

        private void EH_ReGenerateButtonClick(object sender, RoutedEventArgs e)
        {
            ReGenerateCommand.Execute(null);
        }

        private void EH_SaveButtonClick(object sender, RoutedEventArgs e)
        {
            SaveCommand.Execute(null);
        }
    }
}
