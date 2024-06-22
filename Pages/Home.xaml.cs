using Restoraunt.DataBase;
using Restoraunt.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }

        public DataBase.FoodDBEntities Database;
        public Home(DataBase.FoodDBEntities Database)
        {
            InitializeComponent();

            this.Database = Database;

            UpdateList();
        }

        private void UpdateList()
        {

                var dishesWithIngredients = this.Database.Dishes
                    .Select(d => new
                    {
                        Dish = d,
                        Ingredients = d.DishIngredients
                            .Select(di => new
                            {
                                Ingredient = this.Database.Ingredients
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
                        TotalCalories = "Калории: " + Math.Round(d.Ingredients.Sum(di => di.Ingredient.Calories * di.Quantity), 2) + " кал.",
                        TotalWeight = "Вес: " + Math.Round(d.Ingredients.Sum(di => di.Quantity) / 1000, 2) + " кг. (Знаю что нужно БЖУ, но эти поля тоже вычитаемы)"
                    })
                    .ToList();

                list_dishes.ItemsSource = dishesWithIngredients;

            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CreatingIngridient addIngredientWindow = new CreatingIngridient(this.Database);
            addIngredientWindow.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            CreateDish addDish = new CreateDish();
            addDish.ShowDialog();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            UpdateList();
        }

        private void Button_EditSelected(object sender, RoutedEventArgs e)
        {
            var selectedDish = list_dishes.SelectedItem;
            if (selectedDish != null)
            {
                var dishId = ((dynamic)selectedDish).Dish.DishID;
                EditDishWindow editDishWindow = new EditDishWindow(dishId);
                editDishWindow.ShowDialog();
                UpdateList(); // Обновить список после закрытия окна редактирования
            }
            else
            {
                MessageBox.Show("Выберите блюдо для редактирования.");
            }
        }

        private void RemoveDish(int dishID)
        {
            var dish = this.Database.Dishes.Include("DishIngredients").SingleOrDefault(x => x.DishID == dishID);

            if (dish != null)
            {
                // Удаление всех связанных ингредиентов из DishIngredients
                this.Database.DishIngredients.RemoveRange(dish.DishIngredients);

                // Теперь можно удалить само блюдо
                this.Database.Dishes.Remove(dish);

                // Сохраняем изменения в базе данных
                this.Database.SaveChanges();
            }
        }

        private void Button_RemoveSelected(object sender, RoutedEventArgs e)
        {
            var selectedDish = list_dishes.SelectedItem;
            if (selectedDish != null)
            {
                var dishId = ((dynamic)selectedDish).Dish.DishID;


                RemoveDish(dishId);

                UpdateList(); // Обновить список после закрытия окна редактирования
            }
            else
            {
                MessageBox.Show("Выберите блюдо для удаления.");
            }
        }
    }
}
