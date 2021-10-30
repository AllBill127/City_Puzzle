using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    [Serializable]
    public class BadInputdException : Exception
    {
        public BadInputdException(string message)
            : base(message)
        { }

        public BadInputdException(string message, string name)
             : base("Prasau "+ name+" sudaryti"+ message)
        {}
        }

    [Serializable]
    public class EmptyInputdException : Exception
    {
        public EmptyInputdException()
            : base(String.Format("Rastas tuscias laukas"))
        { }

        public EmptyInputdException(string message)
             : base(String.Format("Rastas tuscias laukas- ", message))
        {}
        }
    }
