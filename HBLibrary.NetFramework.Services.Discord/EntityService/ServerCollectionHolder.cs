using HB.NETF.Discord.NET.Toolkit.Models.Collections;
using System.Collections.Generic;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService.Holder {
    public class ServerCollectionHolder : IServerCollectionHolder {
        private readonly IDictionary<string, DiscordServerCollection> serverCollections = new Dictionary<string, DiscordServerCollection>();

        public DiscordServerCollection Get(string key) {
            if (!Has(key))
                return new DiscordServerCollection();

            return serverCollections[key];
        }

        public bool Has(string key) => serverCollections.ContainsKey(key);

        public void Hold(string key, DiscordServerCollection serverCollection) => serverCollections[key] = serverCollection;
    }
}
