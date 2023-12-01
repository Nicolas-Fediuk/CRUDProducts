using CRUDProducts.UI.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Azure.Core;

namespace CRUDProducts.Services
{
    public class Data : IData
    {

        private readonly string connectionString;

        private SqlConnection connection;
        public Data(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("Test");
            connection = new SqlConnection(connectionString);
        }

        public async Task<bool> CheckMail(Users user)
        {

            var result = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT COUNT(*) FROM USERS WHERE MAIL_USR = '"+user.MAIL_USR +"'" );

            return result == 1;
        }

        public async Task<bool> CheckPass(Users user)
        {
            var result = await connection.QueryFirstOrDefaultAsync<int>(@"SELECT 1 FROM USERS WHERE MAIL_USR = @MAIL_USR AND PASSWORD_USR = @PASSWORD_USR", user);

            return result == 1;
        }

        public async Task AddNewUser(Users user)
        {
            await connection.ExecuteAsync(@"INSERT INTO USERS (MAIL_USR, PASSWORD_USR)
                                            VALUES(@MAIL_USR,@PASSWORD_USR)", user);
        }

        public async Task<IEnumerable<Product>> LoadProducts()
        {
            return await connection.QueryAsync<Product>("select * from PRODUCTS");
            
        }

        public async Task AddNewProduct(Product product)
        {
            int maxOrden = await MaxOrden();

            await connection.ExecuteAsync(@"INSERT INTO PRODUCTS(NAME_PRO, USER_PRO, STOCK_PRO, VALUE_PRO, ORDER_PRO)
                                            VALUES('"+product.NAME_PRO+"', '"+product.USER_PRO+"', "+product.STOCK_PRO+", "+product.VALUE_PRO+", "+maxOrden+")");
        }

        public async Task<int> MaxOrden()
        {
            var order = await connection.QueryFirstOrDefaultAsync<int>("select MAX(ORDER_PRO) from PRODUCTS");

            return order+1;
        }

        public async Task<Product> GetProduct(string name)
        {
            return await connection.QueryFirstOrDefaultAsync<Product>(@"select * from Products where NAME_PRO = '"+name+"'");
        }

        public async Task EditProductExist(Product product)
        {
            await connection.ExecuteAsync(@"update PRODUCTS set STOCK_PRO="+product.STOCK_PRO+ ", VALUE_PRO="+product.VALUE_PRO+" where NAME_PRO = '"+product.NAME_PRO+"'");
        }
    }
}
