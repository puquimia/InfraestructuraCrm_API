using CRM.LIB.EN;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.LIB.AD
{
    public class DUsuarios
    {
        private readonly string _connectionString;

        public DUsuarios(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConnectionString");
        }

        public async Task<Usuario>Autenticar(string cuenta, string contrasena)
        {
            using var connection = new SqlConnection(_connectionString);
            DynamicParameters dPars = new DynamicParameters();
            dPars.Add("Cuenta", cuenta, System.Data.DbType.AnsiString);
            dPars.Add("Contrasena", contrasena, System.Data.DbType.AnsiString);
            var usuario = await connection.QueryFirstOrDefaultAsync<Usuario>("SELECT Id, Cuenta FROM GEN.Usuario WHERE Cuenta = @Cuenta AND Contrasena = @Contrasena", dPars);
            return (Usuario)usuario;
        }
    }
}
