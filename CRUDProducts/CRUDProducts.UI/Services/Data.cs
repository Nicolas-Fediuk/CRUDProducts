using CRUDProducts.UI.Models.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Data.SqlClient;
using Azure.Core;
using System.Globalization;

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

            string query = @"
            INSERT INTO PRODUCTS(NAME_PRO, USER_PRO, STOCK_PRO, VALUE_PRO, ORDER_PRO)
            VALUES(@NamePro, @UserPro, @StockPro, @ValuePro, @MaxOrder)";

            CultureInfo culture = CultureInfo.InvariantCulture;

            if (decimal.TryParse(product.VALUE_PRO, NumberStyles.AllowDecimalPoint, culture, out decimal valueProDecimal))
            {
                await connection.ExecuteAsync(query, new
                {
                    NamePro = product.NAME_PRO,
                    UserPro = product.USER_PRO, 
                    StockPro = product.STOCK_PRO,
                    ValuePro = valueProDecimal,
                    MaxOrder = maxOrden
                });
            }
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
            //await connection.ExecuteAsync(@"update PRODUCTS set STOCK_PRO="+product.STOCK_PRO+ ", VALUE_PRO="+decimal.Parse(product.VALUE_PRO.ToString())+" where NAME_PRO = '"+product.NAME_PRO+"'");

            string query = @"
            UPDATE PRODUCTS 
            SET STOCK_PRO = @StockPro, VALUE_PRO = @ValuePro 
            WHERE NAME_PRO = @NamePro";

            CultureInfo culture = CultureInfo.InvariantCulture;

            if (decimal.TryParse(product.VALUE_PRO, NumberStyles.AllowDecimalPoint, culture, out decimal valueProDecimal))
            {
                await connection.ExecuteAsync(query, new
                {
                    StockPro = product.STOCK_PRO,
                    ValuePro = valueProDecimal,
                    NamePro = product.NAME_PRO
                });
            }
        }

        public async Task Delete(string name)
        {
           await connection.ExecuteAsync(@"delete from PRODUCTS where NAME_PRO = '" + name + "'");
        }

        public async Task<bool> checkProductExist(string name)
        {
           int val = await connection.QueryFirstOrDefaultAsync<int>(@"select COUNT(UPPER(NAME_PRO)) from Products where NAME_PRO = '" + name.ToUpper() + "'");

           return (val == 1) ? true : false;
        }
    }
}
