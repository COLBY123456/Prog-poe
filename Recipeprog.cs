using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class Recipe
{
    public string Name { get; private set; }
    public List<Ingredient> Ingredients { get; private set; }
    public List<string> Steps { get; private set; }

    public Recipe(string name)
    {
        Name = name;
        Ingredients = new List<Ingredient>();
        Steps = new List<string>();
    }

    public void AddIngredient(Ingredient ingredient)
    {
        if (ingredient != null)
        {
            Ingredients.Add(ingredient);
        }
        else
        {
            throw new ArgumentNullException(nameof(ingredient), "Ingredient cannot be null.");
        }
    }

    public void AddStep(string step)
    {
        if (!string.IsNullOrWhiteSpace(step))
        {
            Steps.Add(step);
        }
    }

    public int CalculateTotalCalories()
    {
        int totalCalories = 0;
        foreach (var ingredient in Ingredients)
        {
            totalCalories += ingredient.Calories;
        }
        return totalCalories;
    }

    private string DebuggerDisplay => Name;
}

public class Ingredient
{
    public string Name { get; set; }
    public int Calories { get; set; }
    public string FoodGroup { get; set; }

    public Ingredient(string name, int calories, string foodGroup)
    {
        Name = name;
        Calories = calories;
        FoodGroup = foodGroup;
    }
}

public class Program
{
    private static List<Recipe> recipes = new List<Recipe>();

    public delegate void CaloriesExceededHandler(Recipe recipe);

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Add Recipe");
            Console.WriteLine("2. Display Recipes");
            Console.WriteLine("3. Filter Recipes");
            Console.WriteLine("4. Select Recipe");
            Console.WriteLine("5. Exit");
            Console.WriteLine("Enter your choice:");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddRecipe();
                    break;
                case "2":
                    DisplayRecipes();
                    break;
                case "3":
                    FilterRecipes();
                    break;
                case "4":
                    SelectRecipe();
                    break;
                case "5":
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    private static void AddRecipe()
    {
        Console.WriteLine("Enter recipe name:");
        string name = Console.ReadLine();

        Recipe recipe = new Recipe(name);

        recipes.Add(recipe);
        recipes.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));

        Console.WriteLine("Enter ingredient details (or type 'done' to finish):");
        while (true)
        {
            Console.WriteLine("Ingredient Name:");
            string ingredientName = Console.ReadLine();
            if (ingredientName.ToLower() == "done")
                break;

            Console.WriteLine("Calories:");
            int calories;
            bool validCalories = int.TryParse(Console.ReadLine(), out calories);
            if (!validCalories)
            {
                Console.WriteLine("Invalid input. Calories must be a valid integer.");
                continue;
            }

            Console.WriteLine("Food Group:");
            string foodGroup = Console.ReadLine();

            Ingredient ingredient = new Ingredient(ingredientName, calories, foodGroup);
            recipe.AddIngredient(ingredient);
        }

        Console.WriteLine("Enter recipe steps (or type 'done' to finish):");
        while (true)
        {
            Console.WriteLine("Step:");
            string step = Console.ReadLine();
            if (step.ToLower() == "done")
                break;

            recipe.AddStep(step);
        }
    }

    private static void DisplayRecipes(IEnumerable<Recipe> recipes)
    {
        Console.WriteLine("Recipes:");
        if (!recipes.Any())
        {
            Console.WriteLine("No recipes found.");
        }
        else
        {
            foreach (var recipe in recipes)
            {
                Console.WriteLine($"Recipe: {recipe.Name}");
                Console.WriteLine("Ingredients:");
                foreach (var ingredient in recipe.Ingredients)
                {
                    Console.WriteLine($"- {ingredient.Name} ({ingredient.Calories} calories)");
                }
                Console.WriteLine("Steps:");
                foreach (var step in recipe.Steps)
                {
                    Console.WriteLine($"- {step}");
                }
                Console.WriteLine();
            }
        }
    }

    private static void DisplayRecipes()
    {
        DisplayRecipes(recipes);
    }

    private static void SelectRecipe()
    {
        Console.WriteLine("Enter recipe name:");
        string recipeName = Console.ReadLine();

        Recipe selectedRecipe = recipes.Find(r => r.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase));

        if (selectedRecipe != null)
        {
            Console.WriteLine($"Selected Recipe: {selectedRecipe.Name}");
            Console.WriteLine("Ingredients:");
            foreach (var ingredient in selectedRecipe.Ingredients)
            {
                Console.WriteLine($"- {ingredient.Name} ({ingredient.Calories} calories)");
            }
            Console.WriteLine("Steps:");
            foreach (var step in selectedRecipe.Steps)
            {
                Console.WriteLine($"- {step}");
            }
            int totalCalories = selectedRecipe.CalculateTotalCalories();
            Console.WriteLine("Total Calories: " + totalCalories);

            if (totalCalories > 300)
            {
                Console.WriteLine("Warning: Calories exceed 300!");
                HandleCaloriesExceeded(selectedRecipe);
            }
        }
        else
        {
            Console.WriteLine("Recipe not found.");
        }
    }

    private static void HandleCaloriesExceeded(Recipe recipe)
    {
        Console.WriteLine("Warning: Calories exceed 300 in recipe " + recipe.Name);
    }

    private static void FilterRecipes()
    {
        Console.WriteLine("Filter by:");
        Console.WriteLine("1. Ingredient");
        Console.WriteLine("2. Food Group");
        Console.WriteLine("3. Maximum Calories");
        Console.WriteLine("Enter your choice:");
        string filterChoice = Console.ReadLine();

        switch (filterChoice)
        {
            case "1":
                FilterByIngredient();
                break;
            case "2":
                FilterByFoodGroup();
                break;
            case "3":
                FilterByMaximumCalories();
                break;
            default:
                Console.WriteLine("Invalid choice. Please try again.");
                break;
        }
    }

    private static void FilterByIngredient()
    {
        Console.WriteLine("Enter ingredient name:");
        string ingredientName = Console.ReadLine();

        var filteredRecipes = recipes.Where(recipe =>
            recipe.Ingredients.Any(ingredient =>
                ingredient.Name.Equals(ingredientName, StringComparison.OrdinalIgnoreCase)));

        DisplayRecipes(filteredRecipes);
    }

    private static void FilterByFoodGroup()
    {
        Console.WriteLine("Enter food group:");
        string foodGroup = Console.ReadLine();

        var filteredRecipes = recipes.Where(recipe =>
            recipe.Ingredients.Any(ingredient =>
                ingredient.FoodGroup.Equals(foodGroup, StringComparison.OrdinalIgnoreCase)));

        DisplayRecipes(filteredRecipes);
    }

    private static void FilterByMaximumCalories()
    {
        Console.WriteLine("Enter maximum calories:");
        int maximumCalories;
        bool validCalories = int.TryParse(Console.ReadLine(), out maximumCalories);
        if (!validCalories)
        {
            Console.WriteLine("Invalid input. Maximum calories must be a valid integer.");
            return;
        }

        var filteredRecipes = recipes.Where(recipe => recipe.CalculateTotalCalories() <= maximumCalories);

        DisplayRecipes(filteredRecipes);
    }
}
