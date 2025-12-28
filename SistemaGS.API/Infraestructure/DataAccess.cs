using Dapper;
using Microsoft.Data.SqlClient;
using SistemaGS.DTO.AuthDTO;

namespace SistemaGS.API.Infraestructure
{
    public class DataAccess : IDisposable
    {
        private SqlConnection connection;
        //private SqlConnection? connection;
        //private bool disposed = false; 
        public DataAccess(IConfiguration configuration)
        {
            string connectionString = configuration.GetConnectionString("CadenaSQL")!;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }
        public void Dispose()
        {
            if (connection != null)
            {
                connection.Dispose();
                connection.Close();
            }
        }
        /*
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (connection != null)
                    {
                        connection.Dispose();
                        connection = null;
                    }
                }
            }
        }
        ~DataAccess() 
        { 
            Dispose(false); 
        }
        */
        public bool InsertRefreshToken(RefreshToken refreshToken, int Cedula)
        {
            string sql = "INSERT INTO [RefreshToken] (Token, CreatedDate, Expires, Enabled, Cedula) VALUES (@token, @createddate, @expires, @enabled, @cedula)";
            
            int result = connection.Execute(sql,new
            {
                refreshToken.Token,
                refreshToken.CreatedDate,
                refreshToken.Expires,
                refreshToken.Enabled,
                Cedula
            });
            
            return result > 0; 
        }
        public bool DisableUserTokenByCedula(int Cedula)
        {
            string sql = "UPDATE [RefreshToken] Set [Enabled] = 0 WHERE [Cedula] = @cedula";

            int result = connection.Execute(sql, new
            {
                Cedula
            });

            return result > 0;
        }
        public bool DisableUserToken(string token)
        {
            string sql = "UPDATE [RefreshToken] Set [Enabled] = 0 WHERE [Token] = @token";

            int result = connection.Execute(sql, new
            {
                token
            });

            return result > 0;
        }
        public bool IsRefreshTokenValid(string token)
        {
            string sql = "SELECT COUNT(1) FROM RefreshToken WHERE [Token] = @token AND [Enabled] = 1 AND [Expires] >= CAST(GETDATE() AS DATE)";

            int result = connection.ExecuteScalar<int>(sql, new
            {
                token
            });

            return result > 0;
        }
        public int FindUserByToken(string token)
        {
            string sql = "SELECT [Cedula] FROM [RefreshToken] WHERE [Token] = @token";
            return connection.QueryFirstOrDefault<int>(sql, new { token });
        }
    }
}
