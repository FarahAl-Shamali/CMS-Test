using System;
using System.Collections.Generic;
using System.Text;

namespace CMS.Core.Exceptions
{
    public class DuplicatedEmailOrPhoneException : Exception
    {
        public DuplicatedEmailOrPhoneException(): base("Duplicated Email Or Phone Exception")
        {

        }
    }
}
