using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace LibraryApp_03._04
{
    public partial class MainWindow : Window
    {
        private LibraryContext _context;
        private LibraryViewModel _viewModel;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new LibraryViewModel(new LibraryContext());
            DataContext = _viewModel;
            _context = new LibraryContext();
        }

       
        private void AuthorComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedAuthor = AuthorComboBox.SelectedItem as Author;
            if (selectedAuthor != null)
            {
                _viewModel.FilterBooksByAuthor(selectedAuthor); 
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = SearchTextBox.Text;
            _viewModel.FilterBooksBySearch(searchText);  
        }
    }
}
