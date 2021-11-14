using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace CityPuzzle.Classes
{
    [Serializable]
    public class BadInputdException : Exception
    {
        public Entry Field;
        public BadInputdException(string message,Entry entry)
            : base(message)
        {
            Field = entry;
        }

        public BadInputdException(string message, string name, Entry entry)
             : base("Prasau "+ name+" sudaryti"+ message)
        {
            Field = entry;
        }
    }

    [Serializable]
    public class EmptyInputdException : Exception
    {
        public Entry Field;
        public EmptyInputdException(Entry entry)
            : base(String.Format("Rastas tuscias laukas"))
        {
            Field = entry;
        }

        public EmptyInputdException(string message, Entry entry)
             : base(String.Format("Rastas tuscias laukas- ", message))
        {
            Field = entry;
        }
    }

    }
