using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.QifImport {
    public class QifFile {

        private QifFile() { }

        public string Type { get; private set; }

        public QifTransaction[] Transactions { get; private set; }
        public string AccountName { get; private set; }

        public static QifFile Load(TextReader qifReader, string accountName) {
            var file = new QifFile {
                AccountName = accountName
            };
            var header = qifReader.ReadLine();
            file.Type = header[6..].Trim();
            List<QifTransaction> trans = new();
            if (file.Type == "Invst") return file;
            while (true) {
                var record = new QifRecord(qifReader);
                if (record.IsEmpty) break;
                trans.Add(new QifTransaction(record));
            }
            file.Transactions = trans.ToArray();
            return file;
        }

        public static QifFile Load(string file, string accountName = null) {
            if (accountName == null) accountName = Path.GetFileNameWithoutExtension(file);
            using var reader = new StreamReader(file, Encoding.Latin1);
            return Load(reader, accountName);
        }
    }
}
