namespace Ecommerce_Website_Backend.Common.Constants
{
    public class ValidationConstants
    {
        public static class ProductCategory
        {
            public const int ProductCategoryNameMaxLength = 20;
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


    }
}
