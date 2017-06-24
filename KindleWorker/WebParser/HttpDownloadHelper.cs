using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;
using log4net;

namespace KindleWorker.WebParser {
    /// <summary>
    ///     下载网页
    ///     TODO: 下载内容解压缩
    /// </summary>
    public class HttpDownloadHelper {
        private static readonly ILog Logger = LogManager.GetLogger(typeof(HttpDownloadHelper));

        private static readonly string PathRoot = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "web");
        private readonly HttpWorkDefine _workitem;
        private string _targetPath;

        public HttpDownloadHelper(HttpWorkDefine workItem) {
            _workitem = workItem;
        }

        public bool CheckFileDownloaded() {
            return Directory.Exists(_targetPath);
        }

        public async Task<bool> Do() {
            _targetPath = Path.Combine(PathRoot, _workitem.RssId, _workitem.RssItemId);
            

            if (!Directory.Exists(_targetPath))
                Directory.CreateDirectory(_targetPath);

            var targetFile = Path.Combine(_targetPath, $"{_workitem.RssItemId}.html");
                
            try {
                using (var c = new HttpClient()) {
                    var result = await c.GetAsync(_workitem.Url);
                    if (!result.IsSuccessStatusCode)
                        return false;
                    
                    var buff = await result.Content.ReadAsByteArrayAsync();
                    if (result.Content.Headers.ContentEncoding.Contains("gzip")) {
                        buff = Decompress(buff);
                    }
                    
                    File.WriteAllBytes(targetFile,buff);
                    
                    return true;
                } 
            } catch (Exception ex) {
                Logger.Error(ex.Message);
                return false;
            }
        }

        public static byte[] Decompress(byte[] zippedData) {
            var ms = new MemoryStream(zippedData);
            var compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            var outBuffer = new MemoryStream();
            var block = new byte[1024];
            while (true) {
                var bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }
    }
}