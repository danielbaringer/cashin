using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace ClassLibrary.Gaas.MongoDb
{
    public class MongoDbContext
    {

        private static string ConnectionString { get; set; }
        private static string DatabaseName { get; set; }
        private static bool IsSSL { get; set; }

        private IMongoDatabase _database { get; set; }

        public ObjectId novoObjectIdGuiid() { return ObjectId.GenerateNewId(); }

        public MongoDbContext()
        {
            try
            {
                SetConnectionString();
                SetDatabaseName("AmeGaas");
                SetIsSSL(true);

                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                }

                var mongoClient = new MongoClient(settings);

                SetMongoDbConn(mongoClient.GetDatabase(DatabaseName));

            }
            catch (Exception ex)
            {
                throw new Exception("Não foi possível se conectar com o servidor.", ex);
            }
        }

        private void SetConnectionString()
        {
            ConnectionString = ConfigurationManager.AppSettings["MONGO:CONN_STRING:AME-GAAS"];
        }

        private void SetDatabaseName(string value)
        {
            DatabaseName = value;
        }

        private void SetIsSSL(bool value)
        {
            IsSSL = value;
        }

        private void SetMongoDbConn(IMongoDatabase mongoConn)
        {
            _database = mongoConn;
        }

        public IMongoDatabase GetMongoDbConn()
        {
            return _database;
        }

    }
}
