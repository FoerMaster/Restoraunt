//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Restoraunt
{
    using System;
    using System.Collections.Generic;
    
    public partial class DishIngredients
    {
        public int DishIngredientID { get; set; }
        public int DishID { get; set; }
        public int IngredientID { get; set; }
        public decimal Quantity { get; set; }
    
        public virtual Dishes Dishes { get; set; }
        public virtual Ingredients Ingredients { get; set; }
    }
}