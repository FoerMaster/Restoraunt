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
    /// Логика взаимодействия для CreatingIngridient.xaml
    /// </summary>
    public partial class CreatingIngridient : Window
    {
        public CreatingIngridient(DataBase.FoodDBEntities database)
        {
            InitializeComponent();
        }

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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Введите название ингредиента.");
                return;
            }

            if (!double.TryParse(CaloriesTextBox.Text, out double calories))
            {
                MessageBox.Show("Введите корректное значение калорий.");
                return;
            }

            if (!double.TryParse(WeightTextBox.Text, out double weight))
            {
                MessageBox.Show("Введите корректное значение веса.");
                return;
            }

            using (Database)
            {
                var ingredient = new Ingredients
                {
                    Name = name,
                    Calories = (int)calories,
                    Weight = (int)weight,
                };
                
                Database.Ingredients.Add(ingredient);
                Database.SaveChanges();
            }

            MessageBox.Show("Ингредиент добавлен успешно.");
            this.Close();
        }
    }
}
