using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp
{
    public partial class FilterRecipesWindow : Window
    {
        private List<Recipe> allRecipes;
        public List<Recipe> FilteredRecipes { get; private set; }

        public FilterRecipesWindow(List<Recipe> recipes)
        {
            InitializeComponent();
            allRecipes = recipes;
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {
            FilteredRecipes = allRecipes;

            if (IngredientRadioButton.IsChecked == true)
            {
                string ingredientName = FilterTextBox.Text.Trim();
                FilteredRecipes = FilteredRecipes.Where(recipe =>
                    recipe.Ingredients.Any(ingredient =>
                        ingredient.Name.Equals(ingredientName, System.StringComparison.OrdinalIgnoreCase))).ToList();
            }
            else if (FoodGroupRadioButton.IsChecked == true)
            {
                string foodGroup = FilterTextBox.Text.Trim();
                FilteredRecipes = FilteredRecipes.Where(recipe =>
                    recipe.Ingredients.Any(ingredient =>
                        ingredient.FoodGroup.Equals(foodGroup, System.StringComparison.OrdinalIgnoreCase))).ToList();
            }
            else if (CaloriesRadioButton.IsChecked == true)
            {
                int maxCalories = (int)CaloriesSlider.Value;
                FilteredRecipes = FilteredRecipes.Where(recipe => recipe.TotalCalories <= maxCalories).ToList();
            }

            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void FilterOption_Checked(object sender, RoutedEventArgs e)
        {
            if (IngredientRadioButton.IsChecked == true || FoodGroupRadioButton.IsChecked == true)
            {
                FilterTextBox.Visibility = Visibility.Visible;
                CaloriesSlider.Visibility = Visibility.Collapsed;
            }
            else if (CaloriesRadioButton.IsChecked == true)
            {
                FilterTextBox.Visibility = Visibility.Collapsed;
                CaloriesSlider.Visibility = Visibility.Visible;
            }
        }
    }
}
