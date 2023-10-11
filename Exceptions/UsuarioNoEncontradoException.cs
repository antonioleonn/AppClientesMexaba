using System;

namespace AppClientesMexaba.Exceptions
{
    public class UsuarioNoEncontradoException: Exception
    {
        public UsuarioNoEncontradoException() : base() { }

        public UsuarioNoEncontradoException(string message) : base(message) { }
    }
}
