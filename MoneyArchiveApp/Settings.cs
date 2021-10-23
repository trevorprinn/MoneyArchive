using MoneyArchiveDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveApp {
    internal class Settings : DbSettings {

        public new static Settings Load() {
            if (File.Exists(SettingsPath)) {
                using var r = new StreamReader(SettingsPath);
                return JsonConvert.DeserializeObject<Settings>(r.ReadToEnd())!;
            }
            return new Settings();
        }

    }
}
