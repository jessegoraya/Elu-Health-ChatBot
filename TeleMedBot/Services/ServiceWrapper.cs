using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Person.Models;
using Newtonsoft.Json;
using System.Web;
using TeleMedBot.Models;
using System.Text;

namespace Client_EMR.Services
{
    public class ServiceWrapper
    {
        public static async Task<Person.Models.Person> GetNextApptforDoc (string tenant, string appname, string doctor)
        {
            //url from workflow: 
            
            try
            {
                //prepare paramters to send to logic app by putting them into JSON                     
                //see article https://www.codeproject.com/Articles/1136839/Using-Azure-Logic-Apps-with-HTTP-Requests

                //json properties pulled from Request model (under Models folder)
                var json = new Request();
                json.properties = new Properties();

                json.properties.appname = new AppName();
                json.properties.appname.value = appname;
                json.properties.appname.type = "string";

                json.properties.tenant = new Tenant();
                json.properties.tenant.value = tenant;
                json.properties.tenant.type = "string";

                json.properties.doctor = new Doctor();
                json.properties.doctor.value = doctor;
                json.properties.doctor.type = "string";

                string jsonStr = JsonConvert.SerializeObject(json);
                string url = "https://prod-20.eastus.logic.azure.com:443/workflows/61055bd24809473398ba94ad694a6f7b/triggers/request/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Frequest%2Frun&sv=1.0&sig=D54PpxSp4fi8PrrM8LeAo1jkCPsj-6Xq3h7Wrb5S4Rk";

                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);

                    //HttpResponseMessage response = await client.GetAsync("/appname/" + encodedappname + "/doctor/" + doctor + "/tenant/" + encodedtenant);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        Person.Models.Person nextPatient = JsonConvert.DeserializeObject<Person.Models.Person>(data);
                        return nextPatient;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public static async Task<List<Person.Models.Person>> GetPersonByName(string name)
        {
            string url = "https://bloomskyperson.azurewebsites.net/";
            //List<Person.Models.Person> patients = await GetPatients(url);
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    string capitalizeName = FirstCharToUpper(name);
                    string encodedName = GetURLEncodeString(capitalizeName);

                    HttpResponseMessage response = await client.GetAsync("api/Person/GetPersonByName/GeannieandNicky/Discharge/Patient/" + encodedName);
                    //HttpResponseMessage response = await client.GetAsync("Api/Person/GetPersonByName?tenant=GeannieandNicky&appname=Discharge&objecttype=Patient&name=" + encodedName);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        List<Person.Models.Person> patients = JsonConvert.DeserializeObject<List<Person.Models.Person>>(data);
                        return patients;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public static async Task<string> UpdatePerson (Person.Models.Person Patient)
        {
            var json = new OverviewRequest();
            json.properties = new OverviewProps();

            //json.properties.requestPerson = new Person.Models.Person();
            json.properties.requestPerson = Patient;

            string jsonStr = JsonConvert.SerializeObject(json);
            string url = "https://prod-27.eastus.logic.azure.com:443/workflows/63b68fb6fa344ddc86b758078e17a90d/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=YWCTQQYNwXnwPfzVmSp5CMjdgPewFp2TGTfGWjVM_Ac";
            try
            { 
                using (var client = new HttpClient())
                {
                    var content = new StringContent(jsonStr, Encoding.UTF8, "application/json");
                    HttpResponseMessage response = await client.PostAsync(url, content);
                    //HttpResponseMessage response = await client.GetAsync("/appname/" + encodedappname + "/doctor/" + doctor + "/tenant/" + encodedtenant);
                    if (response.IsSuccessStatusCode)
                    {
                        string data = await response.Content.ReadAsStringAsync();
                        //Person.Models.Person nextPatient = JsonConvert.DeserializeObject<Person.Models.Person>(data);
                        return data;
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }

            return null;
        }

        public static string GetURLEncodeString(string name)
        {
            string encodedName = name.Replace("&", "%26");
            encodedName = name.Replace(" ", "%20");
            return encodedName;
        }

        public static string FirstCharToUpper(string name)
        {
            char[] nameChars = name.ToCharArray();

            if (nameChars.Length >= 1)
            {
                if (char.IsLower(nameChars[0]))
                {
                    nameChars[0] = char.ToUpper(nameChars[0]);
                }
            }

            for (int i = 1; i < nameChars.Length; i++)
            {
                if (nameChars[i - 1] == ' ')
                {
                    if (char.IsLower(nameChars[i]))
                    {
                        nameChars[i] = char.ToUpper(nameChars[i]);
                    }
                }
            }

            return new string(nameChars);
        }
    }
}
