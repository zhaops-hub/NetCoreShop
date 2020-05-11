namespace CoreShopUms.Infrastructure.Entity
{
    public class User
    {
        /// <summary>
        ///     唯一标识
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        ///     登录账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        ///     登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     真实姓名
        /// </summary>
        public string RealName { get; set; }
    }
}