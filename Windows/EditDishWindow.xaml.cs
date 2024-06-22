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
    /// Логика взаимодействия для EditDishWindow.xaml
    /// </summary>
    public partial class EditDishWindow : Window
    {
        private List<Ingredients> availableIngredients;
        private List<DishIngredientViewModel> dishIngredients;
        private Dishes currentDish;
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

        public EditDishWindow(int dishId)
        {
            InitializeComponent();
            LoadIngredients();
            dishIngredients = new List<DishIngredientViewModel>();
            LoadDish(dishId);
        }

        private void LoadIngredients()
        {
            using (var context = Database)
            {
                availableIngredients = context.Ingredients.ToList();
                IngredientsComboBox.ItemsSource = availableIngredients;
            }
        }

        private void LoadDish(int dishId)
        {
            using (var context = Database)
            {
                currentDish = context.Dishes.Include("DishIngredients").FirstOrDefault(d => d.DishID == dishId);
                if (currentDish != null)
                {
                    DishNameTextBox.Text = currentDish.Name;

                    isPreparing.IsChecked = currentDish.IsPreparation;
                    foreach (var di in currentDish.DishIngredients)
                    {
                        var ingredient = context.Ingredients.FirstOrDefault(i => i.IngredientID == di.IngredientID);
                        if (ingredient != null)
                        {
                            dishIngredients.Add(new DishIngredientViewModel
                            {
                                Ingredient = ingredient,
                                Quantity = (int)di.Quantity
                            });
                        }
                    }
                    IngredientsListBox.ItemsSource = dishIngredients;
                }
                else
                {
                    MessageBox.Show("Блюдо не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
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
            IngredientsListBox.Items.Refresh();
        }

        private void RemoveIngredientButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var dishIngredient = button?.Tag as DishIngredientViewModel;
            if (dishIngredient != null)
            {
                dishIngredients.Remove(dishIngredient);
                IngredientsListBox.Items.Refresh();
            }
        }

        private void SaveDishButton_Click(object sender, RoutedEventArgs e)
        {
            string dishName = DishNameTextBox.Text;
            if (string.IsNullOrEmpty(dishName))
            {
                MessageBox.Show("Введите название блюда.");
                return;
            }

            using (var context = Database)
            {
                var dish = context.Dishes.FirstOrDefault(d => d.DishID == currentDish.DishID);
                if (dish != null)
                {
                    dish.Name = dishName;
                    dish.IsPreparation = (bool)isPreparing.IsChecked;

                    var existingIngredients = context.DishIngredients.Where(di => di.DishID == dish.DishID).ToList();
                    context.DishIngredients.RemoveRange(existingIngredients);

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
                else
                {
                    MessageBox.Show("Блюдо не найдено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            MessageBox.Show("Изменения сохранены успешно.");
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
