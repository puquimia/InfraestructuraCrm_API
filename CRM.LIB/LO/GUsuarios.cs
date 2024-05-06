using CRM.LIB.AD;
using CRM.LIB.EN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XSystem.Security.Cryptography;

namespace CRM.LIB.LO
{
    public class GUsuarios
    {
        private readonly DUsuarios _usuariosRepository;
        public GUsuarios(DUsuarios usuariosRepository)
        {
            _usuariosRepository = usuariosRepository;
        }

        public async Task<Usuario> Autenticar(string cuenta, string contrasena)
        {
            return await _usuariosRepository.Autenticar(cuenta, Hash(contrasena));
        }

        public static string Hash(string contrasena)
        {
            return Convert.ToBase64String(new SHA256Managed().ComputeHash(Encoding.UTF8.GetBytes(contrasena)));
        }
    }
}
