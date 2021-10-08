using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoneyArchiveDb.QifImport {
    public class QifFile {

        public QifTransaction[] Transactions { get; private set; }
        public string AccountName { get; private set; }

        public void Load(TextReader qifReader, string accountName) {
            // Ignore the first line
            AccountName = accountName;
            qifReader.ReadLine();
            List<QifTransaction> trans = new();
            while (true) {
                var record = new QifRecord(qifReader);
                if (record.IsEmpty) break;
                trans.Add(new QifTransaction(record));
            }
            Transactions = trans.ToArray();
        }

        public void Load(string file, string accountName = null) {
            if (accountName == null) accountName = Path.GetFileNameWithoutExtension(file);
            using var reader = new StreamReader(file);
            Load(reader, accountName);
        }
    }
}
