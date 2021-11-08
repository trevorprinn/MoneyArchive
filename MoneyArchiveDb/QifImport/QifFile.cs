using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.QifImport {
    public class QifFile {

        private QifFile() { }

        public QifTransaction[] Transactions { get; private set; }
        public string AccountName { get; private set; }

        public static QifFile Load(TextReader qifReader, string accountName) {
            var file = new QifFile {
                AccountName = accountName
            };
            // Ignore the first line
            _ = qifReader.ReadLine();
            List<QifTransaction> trans = new();
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
