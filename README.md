# Skestam_Kartu
Encounter me style app by Aurimas Adlys, Aleksandras Bilevičius, Justas Dragūnas, Rokas Gervetauskas    
Api Link -https://github.com/Justuxs/CityPuzzleAPI

CityPuzzle\Classes\Sql ReadUsers() sometimes breaks because (probbably) GetStringAsync() does not respond in 3s and exception is thrown by CityPuzzle\Rest Services\Client\HttpClientRequest SendCommand() method. A reload of the app solved this issue (2021-12-16.21:15)
