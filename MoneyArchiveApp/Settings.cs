using MoneyArchiveDb;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveApp {
    internal class Settings {

        static Settings() {
            SettingsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Babbacombe Computers Ltd", "MoneyArchive", "settings.json");
        }

        public static string SettingsPath { get; private set; }

        public static Settings Load() {
            if (File.Exists(SettingsPath)) {
                using var r = new StreamReader(SettingsPath);
                return JsonConvert.DeserializeObject<Settings>(r.ReadToEnd())!;
            }
            return new Settings();
        }

        public string? QifFolder { get; set; }

        public virtual void Save() {
            var folder = Path.GetDirectoryName(SettingsPath);
            if (!Directory.Exists(folder)) Directory.CreateDirectory(folder!);
            using var w = new StreamWriter(SettingsPath);
            w.Write(JsonConvert.SerializeObject(this));
        }
    }
}
