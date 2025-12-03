using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Exceptions
{
    internal class UserNotFoundException: ApplicationException
    {
        public UserNotFoundException(string name, object key): base($"Entity {name} - {key} is not found.")
        {
            
        }
    }
}
