namespace Ecommerce_Website_Backend.Common.Constants
{
    public static class UserRoles
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";
        public const string Merchant = "Merchant";
        public const string SuperAdmin = "SuperAdmin";

        public static readonly string[] All = [
            Admin, 
            Customer, 
            Merchant, 
            SuperAdmin
        ];
    }

    
}
