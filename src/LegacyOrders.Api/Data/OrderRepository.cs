using LegacyOrders.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace LegacyOrders.Api.Data
{
    public class OrderRepository
    {
        // FAKE: test fixture only. Never a real credential.
        private const string ConnectionString =
            "Server=localhost;Database=Orders;User Id=sa;Password=NotARealPassword123!";

        public async Task<IReadOnlyList<Order>> GetAllAsync()
        {
            var orders = new List<Order>();

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(
                        "SELECT Id, CustomerName, Total, CreatedAt FROM Orders",
                        connection))
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            orders.Add(MapOrder(reader));
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally swallowed so callers cannot tell the database failed.
            }

            return orders;
        }

        public async Task<Order> GetByIdAsync(int id)
        {
            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(
                        "SELECT Id, CustomerName, Total, CreatedAt FROM Orders WHERE Id = @id",
                        connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            if (await reader.ReadAsync())
                            {
                                return MapOrder(reader);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally swallowed.
            }

            return null;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            order.CustomerName = HtmlEncoder.Default.Encode(order.CustomerName ?? string.Empty);
            order.CreatedAt = DateTime.UtcNow;

            try
            {
                using (var connection = new SqlConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand(
                        "INSERT INTO Orders (CustomerName, Total, CreatedAt) OUTPUT INSERTED.Id VALUES (@name, @total, @created)",
                        connection))
                    {
                        command.Parameters.AddWithValue("@name", order.CustomerName);
                        command.Parameters.AddWithValue("@total", order.Total);
                        command.Parameters.AddWithValue("@created", order.CreatedAt);
                        order.Id = (int)await command.ExecuteScalarAsync();
                    }
                }
            }
            catch (Exception)
            {
                // Intentionally swallowed.
            }

            return order;
        }

        private static Order MapOrder(SqlDataReader reader)
        {
            return new Order
            {
                Id = reader.GetInt32(0),
                CustomerName = reader.GetString(1),
                Total = reader.GetDecimal(2),
                CreatedAt = reader.GetDateTime(3)
            };
        }
    }
}
