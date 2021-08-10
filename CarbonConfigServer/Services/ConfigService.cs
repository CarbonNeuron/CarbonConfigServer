using System.Collections.Generic;
using CarbonConfigServer.Models;
using MongoDB.Driver;

namespace CarbonConfigServer.Services
{
    public class ConfigService
    {
        private readonly IMongoCollection<AppConfig> _AppConfigs;
        public ConfigService(IConfigStoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _AppConfigs = database.GetCollection<AppConfig>(settings.CollectionName);
        }

        public List<AppConfig> Get() =>
            _AppConfigs.Find(config => true).ToList();

        public AppConfig Get(string id) =>
            _AppConfigs.Find<AppConfig>(config => config.Id == id).FirstOrDefault();
        
        public AppConfig Create(AppConfig appConfig)
        {
            _AppConfigs.InsertOne(appConfig);
            return appConfig;
        }
        public void Update(string id, AppConfig AppConfigIn) =>
            _AppConfigs.ReplaceOne(appConfig => appConfig.Id == id, AppConfigIn);

        public void Remove(AppConfig AppConfigIn) =>
            _AppConfigs.DeleteOne(config => config.Id == AppConfigIn.Id);

        public void Remove(string id) => 
            _AppConfigs.DeleteOne(config => config.Id == id);
    }
}