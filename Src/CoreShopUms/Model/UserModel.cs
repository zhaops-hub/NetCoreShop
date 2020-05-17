using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreShopUms.Model
{
    public class UserModel
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
