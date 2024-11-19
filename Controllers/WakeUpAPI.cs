using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;

namespace API_ATS_WakeUp.Controllers
{
	[Route("[Action]")]
	[ApiController]
	public class WakeUpAPI : ControllerBase
	{
		private const string JsonFilePath = "apis.json";

		public class APIInfo
		{
			public string APIName { get; set; }
			public string URL { get; set; }
			public string method { get; set; }
			public string data { get; set; }
		}

		[HttpGet]
		public IActionResult run() { 
		
			return Ok("Run... WakeUpAPI V. 0.0.1");
		}


		[Route("/")]
		[HttpGet]
		public IActionResult runs()
		{

			return Ok("Run... WakeUpAPI V. 0.0.1");
		}

		[HttpGet]
		public IActionResult GetAllAPIs()
		{
			// If the JSON file doesn't exist, create an empty file
			if (!System.IO.File.Exists(JsonFilePath))
			{
				System.IO.File.WriteAllText(JsonFilePath, "[]"); // Initialize with an empty JSON array
			}

			// Read data from the file
			var jsonData = System.IO.File.ReadAllText(JsonFilePath);
			var apiList = JsonConvert.DeserializeObject<List<APIInfo>>(jsonData) ?? new List<APIInfo>();
			return Ok(apiList);
		}

		[HttpGet]
		public IActionResult DeleteAPI(string apiName)
		{
			// Check if the JSON file exists
			if (!System.IO.File.Exists(JsonFilePath))
			{
				return NotFound("JSON file not found!");
			}

			// Read data from the file
			var jsonData = System.IO.File.ReadAllText(JsonFilePath);
			var apiList = JsonConvert.DeserializeObject<List<APIInfo>>(jsonData) ?? new List<APIInfo>();

			// Find and remove the API by name
			var apiToDelete = apiList.FirstOrDefault(api => api.APIName == apiName);
			if (apiToDelete == null)
			{
				return NotFound($"API with name '{apiName}' not found.");
			}

			apiList.Remove(apiToDelete);

			// Save the updated data back to the file
			System.IO.File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(apiList, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));
			return Ok($"API with name '{apiName}' has been deleted.");
		}


		[HttpPost]
		public IActionResult UpdateAPIs(APIInfo newApi)
		{
			try
			{

			
			List<APIInfo> apiList;

			// If the JSON file doesn't exist, create it and initialize an empty list
			if (!System.IO.File.Exists(JsonFilePath))
			{
				apiList = new List<APIInfo>();
				System.IO.File.WriteAllText(JsonFilePath, "[]"); // Create the file with an empty array
			}
			else
			{
				// Read the existing file
				var jsonData = System.IO.File.ReadAllText(JsonFilePath);
				apiList = JsonConvert.DeserializeObject<List<APIInfo>>(jsonData) ?? new List<APIInfo>();
			}

			// Check if the API already exists; if so, update it
			var existingApi = apiList.Find(api => api.APIName == newApi.APIName);
			if (existingApi != null)
			{
				existingApi.URL = newApi.URL;
			}
			else
			{
				apiList.Add(newApi);
			}

			// Save updated data back to the file
			System.IO.File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(apiList, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));
			return Ok("API updated successfully.");
			}
			catch (Exception ex)
			{

				return Ok("API updated Fail."+ ex.Message);

			}
		}
		[HttpPost]
		public IActionResult UpdateAPI(APIInfo newApi)
		{
			try
			{
				List<APIInfo> apiList;

				// Ensure file path is accessible and create the file if it does not exist
				if (!System.IO.File.Exists(JsonFilePath))
				{
					using (var stream = System.IO.File.Create(JsonFilePath))
					{
						var emptyData = Encoding.UTF8.GetBytes("[]");
						stream.Write(emptyData, 0, emptyData.Length);
					}
				}

				// Read the existing file
				var jsonData = System.IO.File.ReadAllText(JsonFilePath);
				apiList = JsonConvert.DeserializeObject<List<APIInfo>>(jsonData) ?? new List<APIInfo>();

				// Check if the API already exists; if so, update it
				var existingApi = apiList.Find(api => api.APIName == newApi.APIName);
				if (existingApi != null)
				{
					existingApi.URL = newApi.URL;
				}
				else
				{
					apiList.Add(newApi);
				}

				// Save updated data back to the file
				System.IO.File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(apiList, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));
				return Ok("API updated successfully.");
			}
			catch (Exception ex)
			{
				return StatusCode(500, $"API update failed: {ex.Message}");
			}
		}

		// Trigger GET requests to all APIs and return their responses
		[HttpGet]
		public async Task<IActionResult> WakeAllAPIs()
		{
			if (!System.IO.File.Exists(JsonFilePath))
				return NotFound("JSON file not found!");

			var jsonData = System.IO.File.ReadAllText(JsonFilePath);
			var apiList = JsonConvert.DeserializeObject<List<APIInfo>>(jsonData) ?? new List<APIInfo>();
			var responses = new List<object>();

			using var httpClient = new HttpClient();
			foreach (var api in apiList)
			{
				try
				{
					var response = await httpClient.GetAsync(api.URL);
					responses.Add(new
					{
						APIName = api.APIName,
						URL = api.URL,
						StatusCode = response.StatusCode,
						Response = await response.Content.ReadAsStringAsync()
					});
				}
				catch (Exception ex)
				{
					responses.Add(new
					{
						APIName = api.APIName,
						URL = api.URL,
						Error = ex.Message
					});
				}
			}

			return Ok(responses);
		}


	}
}
