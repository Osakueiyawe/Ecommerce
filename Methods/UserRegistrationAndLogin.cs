using Ecommerce_API.Models;
namespace Ecommerce_API.Methods
{
    public class UserRegistrationAndLogin : IUserRegistrationAndLogin
    {
        private readonly IDatabaseConnection _databaseConnection;

        public UserRegistrationAndLogin(IDatabaseConnection databaseConnection)
        {
            _databaseConnection = databaseConnection;
        }

        public async Task<NewUserRegistrationResponse> UserRegistration(NewUserRegistrationRequest userdetails)
        {
            NewUserRegistrationResponse response = new NewUserRegistrationResponse();
            try
            {
                if (userdetails.phonenumber.Length != 11)
                {
                    response.responsecode = "01";
                    response.responsemessage = "Invalid Phone number";
                }
                else if (userdetails.password != userdetails.confirmpassword)
                {
                    response.responsecode = "01";
                    response.responsemessage = "Password must be same as confirm password field";
                }
                else if (await _databaseConnection.CheckforExistingUser(userdetails))
                {
                    response.responsecode = "01";
                    response.responsemessage = "User already exists";
                }
                else
                {
                    bool createuser = await _databaseConnection.CreateNewUser(userdetails);
                    if (createuser)
                    {
                        response.responsecode = "00";
                        response.responsemessage = "successful";
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        public async Task<LoginResponse> Login(LoginRequest logindetails)
        {
            LoginResponse response = new LoginResponse();
            try
            {
                int getuser = await _databaseConnection.CheckUserDetails(logindetails);
                if (getuser > 0)
                {
                    LoginDetails login = new LoginDetails();
                    login.userid = getuser;
                    login.logintime = DateTime.Now;
                    bool logdata = await _databaseConnection.LogLogindetails(login);
                    if (logdata)
                    {
                        response.responsecode = "00";
                        response.responsemessage = "user retrieved successfully";
                    }
                    else
                    {
                        response.responsecode = "01";
                        response.responsemessage = "user not retrieved successfully";
                    }
                    
                }
                else
                {
                    response.responsecode = "01";
                    response.responsemessage = "user not retrieved successfully";
                }
            }
            catch
            {

            }
            return response;
        }
    }
}
