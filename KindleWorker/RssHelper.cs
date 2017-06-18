using System;
using System.Net.Http;


namespace KindleWorker {
    public class RssHelper :IDisposable{

        private HttpClient _httpClient;


        public RssHelper(string url) {
            _httpClient = new HttpClient();
        }

        public void Dispose() {
            _httpClient?.Dispose();
            _httpClient = null;
        }
    }
}
