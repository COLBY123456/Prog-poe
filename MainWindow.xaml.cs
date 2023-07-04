using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RecipeApp
{
    public partial class MainWindow : Window
    {
        private List<Recipe> recipes = new List<Recipe>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddRecipe_Click(object sender, RoutedEventArgs e)
        {
            string recipeName = RecipeNameTextBox.Text.Trim();

            if (string.IsNullOrEmpty(recipeName))
            {
                MessageBox.Show("Please enter a recipe name.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Recipe recipe = new Recipe(recipeName);
            recipes.Add(recipe);

            RecipeNameTextBox.Clear();
        }

        private void DisplayRecipes_Click(object sender, RoutedEventArgs e)
        {
            RecipeListView.ItemsSource = null;
            RecipeListView.ItemsSource = recipes;
        }

        private void FilterRecipes_Click(object sender, RoutedEventArgs e)
        {
            FilterRecipesWindow filterWindow = new FilterRecipesWindow(recipes);
            filterWindow.ShowDialog();

            if (filterWindow.FilteredRecipes != null)
            {
                RecipeListView.ItemsSource = null;
                RecipeListView.ItemsSource = filterWindow.FilteredRecipes;
            }
        }

        private void SelectRecipe_Click(object sender, RoutedEventArgs e)
        {
            Recipe selectedRecipe = RecipeListView.SelectedItem as Recipe;

            if (selectedRecipe != null)
            {
                RecipeDetailsWindow detailsWindow = new RecipeDetailsWindow(selectedRecipe);
                detailsWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a recipe from the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class Recipe
    {
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public List<string> Steps { get; set; }

        public int TotalCalories
        {
            get { return Ingredients.Sum(ingredient => ingredient.Calories); }
        }

        public Recipe(string name)
        {
            Name = name;
            Ingredients = new List<Ingredient>();
            Steps = new List<string>();
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public int Calories { get; set; }
        public string FoodGroup { get; set; }
    }
}
