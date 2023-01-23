using AndoIt.Common.Interface;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;

namespace AndoIt.Common.Test
{
    public class HttpClientAdapterFake : IHttpClientAdapter
    {
        private ILog log;
        private Semaphore semaphore = new Semaphore(1, 1); // Inicializo con semáforo abierto

        public HttpClientAdapterFake(ILog log, string fakeReturnValue, int milliseconds)
        {
            this.log= log ?? throw new NotImplementedException("log");
            this.FakeReturnValue = fakeReturnValue;
            this.Milliseconds = milliseconds;
        }

        public int Milliseconds { get; set; }
        public string FakeReturnValue { get; set; }
        public int TimesCalled { get; set; } = 0;

        private string UniversalResponse()
        {
            Thread.Sleep(this.Milliseconds);
            this.semaphore.WaitOne();
            this.TimesCalled++;
            return this.FakeReturnValue;
        }

        public AuthenticationHeaderValue AuthenticationHeaderValue { get => null; set { } }
        public HttpClientAdapter.ILog LogListener { get => null; set { } }
        public int? TimeoutSeconds { get => null; set { } }

        
        public string AllCookedUpDelete(string url, string urn, System.Net.NetworkCredential credentials = null)
        {
            return UniversalResponse();
        }

        public string AllCookedUpGet(string url, System.Net.NetworkCredential credentials = null)
        {
            return UniversalResponse();
        }

        public T AllCookedUpGet<T>(string url, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public void AllCookedUpMoveFile(string url, string completeFileAddress, System.Net.NetworkCredential credentials = null)
        {            
        }

        public void AllCookedUpPatch(string url, string body, System.Net.NetworkCredential credentials = null)
        {         
        }

        public string AllCookedUpPost(string url, object body, System.Net.NetworkCredential credentials = null)
        {
            return UniversalResponse();
        }

        public string AllCookedUpPost(string url, string body, System.Net.NetworkCredential credentials = null)
        {
            return UniversalResponse();            
        }

        public T AllCookedUpPost<T>(string url, object body, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public string AllCookedUpPut(string url, object body, System.Net.NetworkCredential credentials = null)
        {
            return UniversalResponse();            
        }

        public T AllCookedUpPut<T>(string url, object body, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public string AllCookedUpSoap(string url, string body, System.Net.NetworkCredential credentials = null)
        {
            return UniversalResponse();            
        }

        public void AllCookedUpUploadFile(string url, string completeFileAddress, System.Net.NetworkCredential credentials = null)
        {
        }

        public HttpClient GetDisposableHttpClient(string url, System.Net.NetworkCredential credentials = null, string mediaTypeHeaderValue = null)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage StandardDelete(string url, string urn, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage StandardGet(string url, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public System.Net.WebResponse StandardPatch(string url, object body, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage StandardPost(string url, string body, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public HttpResponseMessage StandardPut(string url, object body, System.Net.NetworkCredential credentials = null)
        {
            throw new NotImplementedException();
        }

        public void Thaw()
        {
            this.log.Info("Start", new StackTrace());
            this.semaphore.Release();
            this.log.Info("End", new StackTrace());
        }

        public void Freeze()
        {
            this.log.Info("Start", new StackTrace());
            this.semaphore = new Semaphore(0, 1);
            this.log.Info("End", new StackTrace());
        }
    }    
}
