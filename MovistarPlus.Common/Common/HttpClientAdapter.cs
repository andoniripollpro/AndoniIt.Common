﻿using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace MovistarPlus.Common
{
	public class HttpClientAdapter
    {
		public delegate void Log(string message);		

		private readonly HttpClientAdapter.ILog logListener;
		private readonly int? timeoutSeconds = null;

		public HttpClientAdapter(HttpClientAdapter.ILog logListener = null, int? timeoutSeconds = null)
		{
			this.logListener = logListener;
			this.timeoutSeconds = timeoutSeconds;
		}

		public T AllCookedUpPost<T>(string url, object body, NetworkCredential credentials = null)
        {
			return JsonConvert.DeserializeObject<T>(AllCookedUpPost(url, JsonConvert.SerializeObject(body), credentials));
		}
		public string AllCookedUpPost(string url, object body, NetworkCredential credentials = null)
		{
			return AllCookedUpPost(url, JsonConvert.SerializeObject(body), credentials);
		}
		public string AllCookedUpPost(string url, string body, NetworkCredential credentials = null)
		{
			this.logListener?.Message($"Antes del POST {url}. Credentials: {credentials.UserName} Body: {body}");

			using (var webApiClient = this.GetDisposableHttpClient(url, credentials))
			{
				StringContent content = new StringContent(body);
				content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
				var responseMessage = webApiClient.PostAsync(string.Empty, content).Result;
				this.logListener?.Message($"Response.StatusCode = {(int)responseMessage.StatusCode}");
				if (!responseMessage.IsSuccessStatusCode)
					throw new Exception(responseMessage.ReasonPhrase.ToString());
				return responseMessage.Content.ReadAsStringAsync().Result;
			}
		}
		private HttpClient GetDisposableHttpClient(string url, NetworkCredential credentials = null)
        {
            HttpClientHandler handler;
            if (credentials == null)
                handler = new HttpClientHandler();
            else
            {   
                handler = new HttpClientHandler() { Credentials = credentials };
                handler.PreAuthenticate = true;
                handler.Proxy = WebRequest.DefaultWebProxy;
                handler.Proxy.Credentials = CredentialCache.DefaultNetworkCredentials;
            }			

			HttpClient client = new HttpClient(handler)
            {
                BaseAddress = new Uri(url)				
			};

			if (credentials != null)
			{
				var encoding = Encoding.GetEncoding("iso-8859-1");
				string usuPassString = string.Format("{0}:{1}", credentials.UserName, credentials.Password);
				byte[] bytes = Encoding.UTF8.GetBytes(usuPassString);
				string oauthToken = Convert.ToBase64String(bytes);
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", oauthToken);
			}

			if (this.timeoutSeconds.HasValue)
			{
				client.Timeout = new TimeSpan (0, 0, this.timeoutSeconds.Value);
				this.logListener?.Message($"Timeout establecido a {this.timeoutSeconds.Value} segundos");
			}

			return client;
        }

		public string AllCookedUpDelete(string url, string urn, NetworkCredential credentials = null)
		{
			this.logListener?.Message($"Antes del DELETE {url}. Credentials: {credentials.UserName}. Deleted file: {urn}");
			string response = "ERROR";

			using (var webApiClient = this.GetDisposableHttpClient(url, credentials))
			{
				var responseMessage = webApiClient.DeleteAsync(urn).Result;
				this.logListener?.Message($"Response.StatusCode = {(int)responseMessage.StatusCode}");
				response = responseMessage.Content.ReadAsStringAsync().Result;
			}
			return response;
		}

		public void AllCookedUpPatch(string url, string body, NetworkCredential credentials = null)
		{
			this.logListener?.Message($"Antes del PATCH {url}. Credentials: {credentials.UserName}. Body: {body}");

			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "PATCH";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Credentials = credentials ?? CredentialCache.DefaultCredentials;

			var bytes = Encoding.UTF8.GetBytes(body);
			httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);

			using (WebResponse wresp = httpWebRequest.GetResponse())
			{
				this.logListener?.Message($"Response.StatusCode = {(int)((HttpWebResponse)wresp).StatusCode}");				
			};

			httpWebRequest = null;
		}

		private void SetCredentials(HttpClient wepApiClient, NetworkCredential credentials)
        {
            if (credentials != null)
            {
                wepApiClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Basic", Convert.ToBase64String(
                        ASCIIEncoding.ASCII.GetBytes(
                        string.Format("{0}:{1}", credentials.UserName, credentials.Password))));
            }
        }

        public string AllCookedUpPut(string url, object body, NetworkCredential credentials = null)
        {
			this.logListener?.Message($"Antes del PUT {url}. Credentials: {credentials.UserName} Body: {body}");
			string response = "ERROR";

            using (var wepApiClient = this.GetDisposableHttpClient(url, credentials))
            {
                StringContent content = new StringContent(body is string ? body.ToString() : JsonConvert.SerializeObject(body));
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                var responseMessage = wepApiClient.PutAsync(string.Empty, content).Result;
                if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception(responseMessage.ReasonPhrase.ToString());
				this.logListener?.Message($"Response.StatusCode = {(int)responseMessage.StatusCode}");
				response = string.Format("StatusCode: {0}, Status: {1}, Body: {2}",
                    responseMessage.StatusCode, responseMessage.ReasonPhrase, responseMessage.Content.ReadAsStringAsync().Result);
            }
            return response;
        }
		public T AllCookedUpPut<T>(string url, object body, NetworkCredential credentials = null)
		{
			string strResult = AllCookedUpPut(url, body, credentials);
			return JsonConvert.DeserializeObject<T>(strResult);
		}

		//public string AllCookedUpUploadFile(string url, string completeFileAddress, NetworkCredential credentials = null)
		//{
		//	this.logListener?.Message($"Antes del PUT {url}. Credentials: {credentials.UserName} CompleteFileAddress: {completeFileAddress}");
		//	string response = "ERROR";

		//	var webClient = new WebClient();
		//	webClient.Credentials = credentials;
		//	var result = webClient.UploadFile(url, "PUT", completeFileAddress);
		//	this.logListener?.Message($"Response.StatusCode = WebClient puede realizar muchas llamadas: 2XX");
		//	response = Encoding.Default.GetString(result);

		//	return response;
		//}
		public void AllCookedUpUploadFile(string url, string completeFileAddress, NetworkCredential credentials = null)
		{
			this.logListener?.Message($"Uploading {completeFileAddress} to {url}");

			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.ContentType = "application/json";
			httpWebRequest.Method = "PUT";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.Credentials = credentials ?? CredentialCache.DefaultCredentials;
			
			FromFileToStream(completeFileAddress, httpWebRequest.GetRequestStream());

			this.logListener?.Message($"Antes del PUT(UploadFile) {url}. Credentials: {credentials.UserName} File: {completeFileAddress}");
			using (WebResponse wresp = httpWebRequest.GetResponse())
			{
				this.logListener?.Message($"Response.StatusCode = {(int)((HttpWebResponse)wresp).StatusCode}");
			};

			httpWebRequest = null;
		}

		private void FromFileToStream(string completeFileAddress, Stream rs)
		{
			FileStream fileStream = new FileStream(completeFileAddress, FileMode.Open, FileAccess.Read);
			byte[] buffer = new byte[4096];
			int bytesRead = 0;
			while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) != 0)
			{
				rs.Write(buffer, 0, bytesRead);
			}
			fileStream.Close();
			rs.Close();
		}

		public string AllCookedUpGet(string url, NetworkCredential credentials = null)
        {
			this.logListener?.Message($"Antes del GET {url}. Credentials: {credentials.UserName}");
			string response = "ERROR";

            using (var wepApiClient = this.GetDisposableHttpClient(url, credentials))
            {
                var responseMessage = wepApiClient.GetAsync(string.Empty).Result;
				this.logListener?.Message($"Response.StatusCode = {(int)responseMessage.StatusCode}");
				if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception(string.Format("HttpClientAdapter.AllCookedUpGet {0}", responseMessage.ReasonPhrase.ToString()));
                response = responseMessage.Content.ReadAsStringAsync().Result;
			}
            return response;
        }

        public T AllCookedUpGet<T>(string url, NetworkCredential credentials = null)
        {
			this.logListener?.Message($"Antes del GET {url}. Credentials: {credentials.UserName}");

			using (var wepApiClient = this.GetDisposableHttpClient(url, credentials))
            {
				var responseMessage = wepApiClient.GetAsync(string.Empty).Result;
				this.logListener?.Message($"Response.StatusCode = {(int)responseMessage.StatusCode}");
				if (!responseMessage.IsSuccessStatusCode)
                    throw new Exception(responseMessage.ReasonPhrase.ToString());
				var response = JsonConvert.DeserializeObject<T>(responseMessage.Content.ReadAsStringAsync().Result);
				return response;
            }
        }

		public interface ILog
		{
			void Message(string message);
		}
    }
}
