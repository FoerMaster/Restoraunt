using Restoraunt.DataBase;
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
using System.Windows.Shapes;

namespace Restoraunt.Windows
{
    /// <summary>
    /// Логика взаимодействия для CreateDish.xaml
    /// </summary>
    public partial class CreateDish : Window
    {
        private List<Ingredients> availableIngredients;
        private List<DishIngredientViewModel> dishIngredients;
        private static DataBase.FoodDBEntities database;

        private static DataBase.FoodDBEntities Database
        {
            get
            {
                database = new DataBase.FoodDBEntities();
                if (database.Database.Exists() == false)
                {
                    MessageBox.Show("Подключения к базе данных провалено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return database;
            }
        }
        public CreateDish()
        {
            InitializeComponent();
            LoadIngredients();
            dishIngredients = new List<DishIngredientViewModel>();
        }
        private void LoadIngredients()
        {
            using (var context = Database)
            {
                availableIngredients = context.Ingredients.ToList();
                IngredientsComboBox.ItemsSource = availableIngredients;

            }
        }

        private void AddIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedIngredient = IngredientsComboBox.SelectedItem as Ingredients;
            if (selectedIngredient == null)
            {
                MessageBox.Show("Выберите ингредиент.");
                return;
            }

            if (!double.TryParse(QuantityTextBox.Text, out double quantity) || quantity <= 0)
            {
                MessageBox.Show("Введите корректное количество.");
                return;
            }

            var dishIngredient = new DishIngredientViewModel
            {
                Ingredient = selectedIngredient,
                Quantity = quantity
            };

            dishIngredients.Add(dishIngredient);
            IngredientsListBox.Items.Add(dishIngredient);
        }

        private void RemoveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dishIngredient = button?.Tag as DishIngredientViewModel;
            if (dishIngredient != null)
            {
                dishIngredients.Remove(dishIngredient);
                IngredientsListBox.Items.Remove(dishIngredient);
            }
        }

        private void AddDishButton_Click(object sender, RoutedEventArgs e)
        {
            string dishName = DishNameTextBox.Text;
            if (string.IsNullOrEmpty(dishName))
            {
                MessageBox.Show("Введите название блюда.");
                return;
            }

            using (var context = Database)
            {

                var dish = new Dishes
                {
                    Name = dishName,
                    IsPreparation = true,
                };

                context.Dishes.Add(dish);


                context.SaveChanges();

                foreach (var dishIngredient in dishIngredients)
                {
                    var dishIngredientEntity = new DishIngredients
                    {
                        DishID = dish.DishID,
                        IngredientID = dishIngredient.Ingredient.IngredientID,
                        Quantity = (int)dishIngredient.Quantity
                    };

                    context.DishIngredients.Add(dishIngredientEntity);
                }
                context.SaveChanges();
            }

            MessageBox.Show("Блюдо добавлено успешно.");
            this.Close();
        }
        public class DishIngredientViewModel
        {
            public Ingredients Ingredient { get; set; }
            public double Quantity { get; set; }
            public string Display => $"{Ingredient.Name} - {Quantity}";
        }
    }
}
