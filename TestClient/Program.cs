using System;
using KindleWorker;

namespace TestClient {
    class MainClass {
        public static void Main(string[] args) {

            KindleWorker.KindleWorker w = new KindleWorker.KindleWorker();
            w.Init();
        }
    }
}
