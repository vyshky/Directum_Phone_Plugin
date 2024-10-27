using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;
using Directum_Plugin.DTO;
using System.Linq;
using System.Net;

namespace Directum_Plugin
{
    [Author(Name = "Alexandr Deduhin")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private static readonly HttpClient client = new HttpClient();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            logger.Info("Loading employees");
            List<EmployeesDTO> employeesList = new List<EmployeesDTO>();
            using (var httpClient = new HttpClient())
            {
                HttpResponseMessage response = GetAsync(httpClient).Result;
                if (response.IsSuccessStatusCode)
                {
                    string jsonString = ReadAsStringAsync(response).Result;

                    JObject jObject = JObject.Parse(jsonString);

                    List<User> usersList = JsonConvert.DeserializeObject<UsersList>(jsonString).Users;
                    
                    for (int i = 0; i < usersList.Count; ++i)
                    {
                        var employee = new EmployeesDTO()
                        {
                            Name = usersList[i].Name,
                        };
                        employee.AddPhone(usersList[i].Phone);
                        employeesList.Add(employee);
                    }
                   
                }
                else
                {
                   logger.Error($"Error: {response.StatusCode}");
                }
            }
            logger.Info($"Loaded {employeesList.Count()} employees");
            return employeesList.Cast<DataTransferObject>().ToList();
        }
        static async Task<HttpResponseMessage> GetAsync(HttpClient httpClient)
        {
            return await httpClient.GetAsync("https://dummyjson.com/users");
        }

        static async Task<string> ReadAsStringAsync(HttpResponseMessage response)
        {
            return await response.Content.ReadAsStringAsync();
        }
    }
}
