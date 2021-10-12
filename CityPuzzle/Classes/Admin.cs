using System;
using System.Collections.Generic;
using System.Text;

namespace CityPuzzle.Classes
{
    public class Admin
    {
        public User User;
        public String RoomId;


        public Admin(String roomid,User user)
        {
            this.User = user;
            RoomId = roomid;
        }

        public void setRoomId(String id)
        {
            RoomId = id;
        }

    }
}
