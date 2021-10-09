using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoneyArchiveDb.QifImport {
	public class QifRecord {
		List<Field> _fields = new();
		public Field[] Fields => _fields.ToArray();

		public bool IsEmpty => !_fields.Any();

		public QifRecord(TextReader reader) {
			var line = reader.ReadLine();
			if (string.IsNullOrWhiteSpace(line)) return;
			while (line[0] != '^') {
				_fields.Add(Field.Create(line));
				line = reader.ReadLine();
			}
		}
	}
}
