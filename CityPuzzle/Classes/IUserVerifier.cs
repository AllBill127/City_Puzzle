namespace CityPuzzle.Classes
{
    public interface IUserVerifier
    {
        //Checks password
        bool CPass(string name, string pass);
        //Checks username
        bool CUser(string name);
        //Hashes password
        string PToH(string pass);
        //Verifies hashed password
        bool PassVer(string pass, string passwordHash);
        //Checks hashed password
        bool CheckHashPass(string name, string pass);
    }
}