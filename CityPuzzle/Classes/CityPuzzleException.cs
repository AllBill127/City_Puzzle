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
    [Serializable]
    public class MultiRegistrationException : Exception
    {
        public Room CurrentRoom;
        public MultiRegistrationException(Room room)
            : base(String.Format("Jus jau registruotas šiame zaidyme. Ar norite testi zaidima "+ room.Id))
        {
            CurrentRoom = room;
        }

        public MultiRegistrationException(string message, Room room)
             : base(String.Format(message))
        {
            CurrentRoom = room;
        }
    }

    [Serializable]
    public class RoomFullException : Exception
    {
        public RoomFullException()
            : base(String.Format("Deja, kambarys yra pilnas "))
        {}

        public RoomFullException(string message)
             : base(String.Format(message))
        {}
    }
    [Serializable]
    public class RoomNotExistException : Exception
    {
        public RoomNotExistException()
            : base(String.Format("Deja,tokio kambario nera"))
        { }

        public RoomNotExistException(string message)
             : base(String.Format(message))
        { }
    }

    [Serializable]
    public class TypeNotExistException : Exception
    {
    public TypeNotExistException()
        : base(String.Format("Error: bad call- GetAdress<T>(T item)"))
    { }
    }
    [Serializable]
    public class APIFailedSaveException : Exception
    {
        public APIFailedSaveException()
            : base(String.Format("Error:failed saved"))
        { }
        public APIFailedSaveException(string message)
            : base(String.Format(message))
        { }
    }
    [Serializable]
    public class APIFailedDeleteException : Exception
    {
        public APIFailedDeleteException()
            : base(String.Format("Error:failed delete"))
        { }
        public APIFailedDeleteException(string message)
            : base(String.Format(message))
        { }
    }
}
