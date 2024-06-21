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

namespace Restoraunt.Pages
{
    /// <summary>
    /// Логика взаимодействия для Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public Home(FoodDBEntities Database)
        {
            InitializeComponent();


            using (var context = Database)
            {
                var dishesWithIngredients = context.Dishes
                    .Select(d => new
                    {
                        Dish = d,
                        Ingredients = d.DishIngredients
                            .Select(di => new
                            {
                                Ingredient = context.Ingredients
                                    .Where(ing => di.IngredientID == ing.IngredientID)
                                    .Select(ing => new
                                    {
                                        ing.Name,
                                        ing.Calories,
                                        ing.Weight
                                    })
                                    .FirstOrDefault(),
                                Quantity = di.Quantity
                            })
                            .ToList()
                    })
                    .ToList()
                    .Select(d => new
                    {
                        Dish = d.Dish,
                        Ingredients = d.Ingredients.Select(di => new
                        {
                            di.Ingredient.Name,
                            di.Ingredient.Calories,
                            di.Ingredient.Weight,
                            di.Quantity
                        }).ToList(),
                        TotalCalories = "Калории: " + Math.Round(d.Ingredients.Sum(di => di.Ingredient.Calories * di.Quantity),2) + "кал.",
                        TotalWeight = "Вес: " + Math.Round(d.Ingredients.Sum(di => di.Quantity) / 1000, 2) + " кг."
                    })
                    .ToList();

                list_dishes.ItemsSource = dishesWithIngredients;

            }
        }
    }
}
