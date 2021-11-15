namespace CityPuzzle.Classes
{
    public interface IUserVerifier
    {
        //Called by CheckPassword in User.cs
        bool CPass(string name, string pass);
        //Called by CheckUser in User.cs
        bool CUser(string name);
        //Called by PassToHash in User.cs
        string PToH(string pass);
        //Used by CPass
        bool PassVer(string pass, string passwordHash);
        bool CheckHashPass(string name, string pass);
    }
}