﻿using AccSol.Models;
using AccSol.ViewModels;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AccSol.Services
{
    public class DepartmentService : CommonService<Department, IReportItem>
    {
        private readonly HttpClient _httpClient;
        public DepartmentService(HttpClient httpClient) : base(httpClient)
        {
            _httpClient = httpClient;
        }

        public override async Task<IEnumerable<Department>?> GetAll()
        {
            IEnumerable<Department>? list = null;

            try
            {
                _httpClient.DefaultRequestHeaders.Accept.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                list = await _httpClient.GetFromJsonAsync<IEnumerable<Department>>("Departments/GetAll");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return list;
        }

        public override async Task<Department?> Get(int id)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = await _httpClient.GetAsync($"Departments/Get/{id}");
            Department? department = null;
            try
            {
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    department = await response.Content.ReadFromJsonAsync<Department>();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return department;
        }


        public override async Task<Department?> Save(Department? model)
        {
            if (model == null)
            {
                throw new Exception("Invalid or empty model.");
            }

            Department? postedModel = null;
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var getTask = _httpClient.PostAsJsonAsync("Departments/Save", model);

            getTask.Wait();
            HttpResponseMessage response = getTask.Result;
            response.EnsureSuccessStatusCode();
            if (response.IsSuccessStatusCode)
            {

                try
                {
                    postedModel = await response.Content.ReadFromJsonAsync<Department>();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            return postedModel;
        }
        
        public override async Task Delete(int id)
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var getTask = _httpClient.PostAsJsonAsync<int>("Departments/Delete", id);

            try
            {

                await getTask;
                var response = getTask.Result;
                
                if (response == null)
                {
                    throw new Exception("There was an error in deleting the data.");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
