using System.Globalization;
using WebApiDemo.Models;

namespace WebApiDemo.Models.Repositories
{
    public static class ShirtRepository
    {
        private static List<Shirt> shirts = new List<Shirt>()
        {
            new Shirt { ShirtId = 1, Brand = "Nike", Color = "Red", Gender ="Men", Size = 10 , Price = 10},
            new Shirt { ShirtId = 2, Brand = "Adidas", Color = "Blue", Gender ="Men", Size = 8 , Price = 10},
            new Shirt { ShirtId = 3, Brand = "Puma", Color = "Green", Gender ="Women",  Size = 6 , Price = 10},
            new Shirt { ShirtId = 4, Brand = "Reebok", Color = "Yellow",Gender ="Women",  Size = 4, Price = 10}
        };

        public static List<Shirt> GetShirts()
        {
            return shirts;
        }

        public static bool ShirtExists(int id)
        {
            return shirts.Any(s => s.ShirtId == id);
        }

        public static Shirt? GetShirtById(int id)
        {
            return shirts.FirstOrDefault(s => s.ShirtId == id);
        }

        public static Shirt? GetShirtByProperties(string? brand, string? color, string? gender, int? size)
        {
            return shirts.FirstOrDefault
            (s =>
            !string.IsNullOrWhiteSpace(brand) &&
            !string.IsNullOrWhiteSpace(s.Brand) &&
            s.Brand.Equals(brand, StringComparison.OrdinalIgnoreCase) &&
             !string.IsNullOrWhiteSpace(color) &&
            !string.IsNullOrWhiteSpace(s.Color) &&
            s.Color.Equals(color, StringComparison.OrdinalIgnoreCase) &&
             !string.IsNullOrWhiteSpace(gender) &&
            !string.IsNullOrWhiteSpace(s.Gender) &&
            s.Gender.Equals(gender, StringComparison.OrdinalIgnoreCase) &&
            size.HasValue &&
            s.Size.HasValue &&
            size.Value == s.Size.Value);


        }

        public static void AddShirt(Shirt shirt)
        {
            int maxId = shirts.Max(s => s.ShirtId);
            shirt.ShirtId = maxId + 1;

            shirts.Add(shirt);
        }


        public static void UpdateShirt(Shirt shirt)
        {
            var shirtToUpdate = shirts.First(s => s.ShirtId == shirt.ShirtId);
            shirtToUpdate.Brand = shirt.Brand;
            shirtToUpdate.Color = shirt.Color;
            shirtToUpdate.Gender = shirt.Gender;
            shirtToUpdate.Size = shirt.Size;
            shirtToUpdate.Price = shirt.Price;

        }

        public static void DeleteShirt(int id)
        {
            var shirtToDelete = GetShirtById(id);
            if (shirtToDelete != null)
            {
                shirts.Remove(shirtToDelete);
            }
        }
    }
}
