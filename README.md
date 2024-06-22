```sql
-- Создаем базу данных
CREATE DATABASE FoodDB;
GO

-- Используем созданную базу данных
USE FoodDB;
GO

-- Создаем таблицу Ингредиентов
CREATE TABLE Ingredients (
    IngredientID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Calories INT NOT NULL,
    Weight DECIMAL(10, 2) NOT NULL
);
GO

-- Создаем таблицу Блюд
CREATE TABLE Dishes (
    DishID INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    IsPreparation BIT NOT NULL
);
GO

-- Создаем связующую таблицу для блюд и ингредиентов
CREATE TABLE DishIngredients (
    DishIngredientID INT IDENTITY(1,1) PRIMARY KEY,
    DishID INT NOT NULL FOREIGN KEY REFERENCES Dishes(DishID),
    IngredientID INT NOT NULL FOREIGN KEY REFERENCES Ingredients(IngredientID),
    Quantity DECIMAL(10, 2) NOT NULL
);
GO

-- Используем созданную базу данных
USE FoodDB;
GO

-- Добавляем тестовые данные в таблицу Ингредиентов
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Chicken Breast', 165, 100.00);
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Olive Oil', 120, 13.50);
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Garlic', 5, 3.00);
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Salt', 0, 1.00);
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Black Pepper', 0, 1.00);
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Lemon', 17, 58.00);
INSERT INTO Ingredients (Name, Calories, Weight) VALUES ('Parsley', 4, 10.00);
GO

-- Добавляем тестовые данные в таблицу Блюд
INSERT INTO Dishes (Name, IsPreparation) VALUES ('Grilled Chicken Breast', 1);
INSERT INTO Dishes (Name, IsPreparation) VALUES ('Lemon Garlic Sauce', 1);
GO

-- Добавляем тестовые данные в таблицу DishIngredients
-- Ингредиенты для Grilled Chicken Breast
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (1, 1, 200.00); -- Chicken Breast
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (1, 2, 10.00);  -- Olive Oil
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (1, 4, 2.00);   -- Salt
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (1, 5, 1.00);   -- Black Pepper

-- Ингредиенты для Lemon Garlic Sauce
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (2, 2, 20.00);  -- Olive Oil
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (2, 3, 5.00);   -- Garlic
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (2, 6, 58.00);  -- Lemon
INSERT INTO DishIngredients (DishID, IngredientID, Quantity) VALUES (2, 7, 10.00);  -- Parsley
GO
```
