using System;
using log4net;
using KindleWorker.Models;
using Dapper;

namespace KindleWorker {
    public class DbWorkHelper {

        private static ILog Logger = log4net.LogManager.GetLogger(typeof(DbWorkHelper));

        public DbWorkHelper() {
            var dbtype = System.Configuration.ConfigurationManager.AppSettings["db"];



        }

        public void Init() {
            try {
                
            } catch (Exception ex) {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}
