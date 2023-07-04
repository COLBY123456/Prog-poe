using System.Windows;

namespace RecipeApp
{
    public partial class RecipeDetailsWindow : Window
    {
        public RecipeDetailsWindow(Recipe recipe)
        {
            InitializeComponent();
            DataContext = recipe;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
2
