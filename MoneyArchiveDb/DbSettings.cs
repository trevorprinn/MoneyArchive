using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb {
    public class DbSettings {
        static DbSettings() {
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Babbacombe Computers Ltd", "MoneyArchive", "settings.json");
        }

        public string ConnectionString { get; set; }

        [JsonExtensionData]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("CodeQuality", "IDE0051:Remove unused private members", Justification = "Used by JsonConvert")]
        IDictionary<string, object> otherSettings { get; set; }

        public static string SettingsPath { get; private set; }

        public static DbSettings Load() {
            if (File.Exists(SettingsPath)) {
                using var r = new StreamReader(SettingsPath);
                return JsonConvert.DeserializeObject<DbSettings>(r.ReadToEnd());
            }
            return new DbSettings();
        }

        public virtual void Save() {
            using var w = new StreamWriter(SettingsPath);
            w.Write(JsonConvert.SerializeObject(this));
        }
    }
}
