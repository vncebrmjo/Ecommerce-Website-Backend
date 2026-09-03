namespace Ecommerce_Website_Backend.Common.Constants
{
    public class ValidationConstants
    {
        public static class ProductCategory
        {
            public const int ProductCategoryNameMaxLength = 50;
            public const int ProductCategoryDescriptionMaxLength = 50;
        }
        public static class User
        {
            public const int UserNameMaxLength = 25;
            public const int EmailMaxLength = 100;
            public const int PasswordMinLength = 8;
            public const int PasswordMaxLength = 100;
            public const int FirstNameMaxLength = 50;
            public const int LastNameMaxLength = 50;
        }
        public static class Product
        {
            public const int ProductNameMaxLength = 100;
            public const int ProductDescriptionMaxLength = 500;
            public const int SkuMaxLength = 50;
            public const double PriceMin = 0.01;
            public const double PriceMax = 1_000_000;
        }


    }
}
