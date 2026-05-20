using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using System.Windows.Controls;
using WordRepeat.Models;


namespace WordRepeat.Views
{
    public partial class NotesView : UserControl
    {
        private ServiceProvider _serviceProvider;
        private AppData _appData;

        public NotesView(ServiceProvider serviceProvider, AppData appData)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
            _appData = appData;
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void CancelEditButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void FilterCategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void NotesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void FirstPageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void PrevPageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void LastPageButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
