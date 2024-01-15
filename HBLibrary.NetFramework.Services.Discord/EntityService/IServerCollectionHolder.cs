using HB.NETF.Discord.NET.Toolkit.Models.Collections;

namespace HB.NETF.Discord.NET.Toolkit.Services.EntityService.Holder {
    public interface IServerCollectionHolder {
        DiscordServerCollection Get(string key);
        void Hold(string key, DiscordServerCollection serverCollection);
        bool Has(string key);
    }
}
