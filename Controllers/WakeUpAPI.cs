using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.Text.Json;
using System.Diagnostics;
using Newtonsoft.Json.Linq;

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
		public class API_update
		{
			public string APIName { get; set; }
			public string URL { get; set; }
			public string method { get; set; }
			public object data { get; set; }
		}
		[HttpGet]
		public IActionResult run()
		{

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
			var apiList = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(jsonData);
			return Ok(apiList);
		}

		[HttpGet]
		public IActionResult GetAllAPI_test()
		{
			// If the JSON file doesn't exist, create an empty file
			if (!System.IO.File.Exists(JsonFilePath))
			{
				System.IO.File.WriteAllText(JsonFilePath, "[]"); // Initialize with an empty JSON array
			}

			// Read data from the file
			var jsonData = System.IO.File.ReadAllText(JsonFilePath);
			var trt = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(jsonData);
			return Ok(trt);
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
					existingApi.method = newApi.method;
					existingApi.data = newApi.data;
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

				return Ok("API updated Fail." + ex.Message);

			}
		}
		[HttpPost]
		public IActionResult UpdateAPI_s(APIInfo newApi)
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
					existingApi.method = newApi.method;
					existingApi.data = newApi.data;
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
		[HttpPost]
		public IActionResult UpdateAPI(API_update newApi)
		{
			try
			{
				List<API_update> apiList;

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
				apiList = JsonConvert.DeserializeObject<List<API_update>>(jsonData) ?? new List<API_update>();
				//apiList = System.Text.Json.JsonSerializer.Deserialize<JsonElement>(jsonData);
			

				// Check if the API already exists; if so, update it
				var existingApi = apiList.Find(api => api.APIName == newApi.APIName);
				if (existingApi != null)
				{
					existingApi.URL = newApi.URL;
					existingApi.method = newApi.method;
					//ValueKind = Object : "{"sdsd":"asdad","sdd":{"sdsd":"asdasd"}}"
					existingApi.data = JObject.Parse(newApi.data.ToString());
				}
				else
				{
					newApi.data= JObject.Parse(newApi.data.ToString()); ;
					apiList.Add(newApi);
				}

				// Save updated data back to the file
				//System.IO.File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(apiList, (Newtonsoft.Json.Formatting)System.Xml.Formatting.Indented));
				System.IO.File.WriteAllText(JsonFilePath, JsonConvert.SerializeObject(apiList, Newtonsoft.Json.Formatting.Indented));
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
			Stopwatch stopwatch = new Stopwatch();
			
			while (true)
			{
				stopwatch.Start();
				var startTime = stopwatch.ElapsedMilliseconds;
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
				var timeTakenForRequest = stopwatch.ElapsedMilliseconds - startTime;

				// Check if the time taken exceeds 500 ms for this request
				if (timeTakenForRequest < 500)
				{
					break;  // Switch to while loop if this request takes more than 500ms
				}
				Debug.WriteLine($"1 Time taken: {stopwatch.ElapsedMilliseconds} | {timeTakenForRequest} milliseconds");
				stopwatch.Reset();
				
			}
			
				stopwatch.Stop();



			return Ok(responses);
		}

		[HttpGet]
		public async Task<IActionResult> WakeAllAPI()
		{
			if (!System.IO.File.Exists(JsonFilePath))
				return NotFound("JSON file not found!");

			var jsonData = System.IO.File.ReadAllText(JsonFilePath);
			var apiList = JsonConvert.DeserializeObject<List<API_update>>(jsonData) ?? new List<API_update>();
			var responses = new List<object>();

			using var httpClient = new HttpClient();
			foreach (var api in apiList)
			{
				try
				{
					

					if (api.method?.ToUpper() == "POST")
					{
						// Use the `data` field as the JSON payload for POST requests

						StringContent content = new StringContent(api.data.ToString(), Encoding.UTF8, "application/json");
						var response = await httpClient.PostAsync(api.URL, content);
						// Capture the response details

						responses.Add(new
						{
							APIName = api.APIName,
							URL = api.URL,
							Method = api.method,
							StatusCode = response.StatusCode,
							Response = await response.Content.ReadAsStringAsync()
						});
					}
					else
					{
						// Default to GET requests if the method is not POST
						var response = await httpClient.GetAsync(api.URL);
						// Capture the response details

						responses.Add(new
						{
							APIName = api.APIName,
							URL = api.URL,
							Method = api.method,
							StatusCode = response.StatusCode,
							Response = await response.Content.ReadAsStringAsync()
						});
					}

					
				}
				catch (Exception ex)
				{
					// Handle errors
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
		[HttpGet]
		public async Task<IActionResult> WakeAllAPI_GETPOST()
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


					if (api.method?.ToUpper() == "POST")
					{
						// Use the `data` field as the JSON payload for POST requests
						var content = new StringContent(api.data ?? "{}", Encoding.UTF8, "application/json");
						var response = await httpClient.PostAsync(api.URL, content);
						// Capture the response details

						responses.Add(new
						{
							APIName = api.APIName,
							URL = api.URL,
							Method = api.method,
							StatusCode = response.StatusCode,
							Response = await response.Content.ReadAsStringAsync()
						});
					}
					else
					{
						// Default to GET requests if the method is not POST
						var response = await httpClient.GetAsync(api.URL);
						// Capture the response details

						responses.Add(new
						{
							APIName = api.APIName,
							URL = api.URL,
							Method = api.method,
							StatusCode = response.StatusCode,
							Response = await response.Content.ReadAsStringAsync()
						});
					}


				}
				catch (Exception ex)
				{
					// Handle errors
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
		[HttpPost]
		public IActionResult Test(object data)
		{
			// Example of a dynamic JSON object
			var js = new { sada = "asdasd" };

			// Prepare the response list
			var responses = new List<object>();

			// Add a response object to the list
			responses.Add(new
			{
				APIName = "1122",
				URL = "1122",
				Error = new
				{
					aasd = "asdasd",
					ggg = js
				}
			});

			// Return the response
			return Ok(data);
		}

	}
}
