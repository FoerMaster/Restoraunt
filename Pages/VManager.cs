﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restoraunt.Pages
{
    internal class VManager
    {
        private static Home homeView;

        private static DataBase.FoodDBEntities database;

        private static DataBase.FoodDBEntities Database
        {
            get
            {
                if (true)
                {
                    database = new DataBase.FoodDBEntities();
                    if (database.Database.Exists() == false)
                    {
                        MessageBox.Show("Подключения к базе данных провалено.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                return database;
            }
        }

        public static Home HomeView
        {
            get
            {
                if (homeView == null)
                {
                    homeView = new Home(Database);
                }
                return homeView;
            }
        }
    }
}

