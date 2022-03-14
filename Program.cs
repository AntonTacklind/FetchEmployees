using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;

namespace ListEmployees
{

    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Starting fetch...");

            HttpClient client = new HttpClient();
            string location = "https://dummy.restapiexample.com/api/v1/employees";

            try
            {
                Task<string> contentTask = GetJson(client, location);
                contentTask.Wait();
                string content = contentTask.Result;

                int desiredWidthForName = 8 * 2; //3x \t
                string niceLine = "";
                for (int i = 0; i < 48; i++)
                {
                    niceLine += "_";
                }

                Console.WriteLine("Name \t\t\t Age \t Salary");
                using (JsonDocument document = JsonDocument.Parse(content))
                {
                    foreach (JsonElement element in document.RootElement.GetProperty("data").EnumerateArray())
                    {
                        int id = element.GetProperty("id").GetInt32();
                        string name = element.GetProperty("employee_name").GetString();
                        int salary = element.GetProperty("employee_salary").GetInt32();
                        int age = element.GetProperty("employee_age").GetInt32();

                        //Modifications
                        name = name.PadRight(desiredWidthForName);

                        Console.WriteLine(niceLine);
                        Console.WriteLine($"{name} \t {age} \t {salary}");
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            //Console.WriteLine("Complete!");
        }

        static async Task<string> GetJson(HttpClient client, string location)
        {
            HttpResponseMessage response = await client.GetAsync(location);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                return content;
            }
            throw new ApplicationException("Failed to fetch data. Status Code : " + response.StatusCode + ". Message : " + response.RequestMessage);
        }
    }
}
