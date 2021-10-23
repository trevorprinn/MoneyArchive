using System;
using System.Text.RegularExpressions;

namespace MoneyArchiveDb.QifImport {
	public abstract class Field {
		public static Field Create(string line) {
			string data = line[1..];
			if (string.IsNullOrWhiteSpace(line)) throw new Exception("Blank Line");
            return line[0] switch {
                'D' => new DateField(data),
                'T' or 'U' => new AmountField(data),
                'M' => new MemoField(data),
                'C' => new ClearedStatusField(data),
                'N' => new ChequeNumberField(data),
                'P' => new PayeeField(data),
                'L' => data.StartsWith('[') ? new TransferField(data) : new CategoryField(data),
                'S' => data.StartsWith('[') ? new SplitTransfer(data) : new SplitCategory(data),
                'E' => new SplitMemo(data),
                '$' => new SplitAmount(data),
                _ => throw new Exception($"Unknown Field Type {line[0]}"),
            };
        }
	}

	class DateField : Field {
		public DateTime Value { get; private set; }

		static readonly Regex _date = new(@"(?'day'^\d{1,2})/(?'month'\d{1,2})(?'cent'/|')(?'year'\d\d)$");

		public DateField(string data) {
			var m = _date.Match(data);
			if (!m.Success) throw new Exception($"Date Parse Failure: '{data}'");
			var day = int.Parse(m.Groups["day"].Value);
			var month = int.Parse(m.Groups["month"].Value);
			var year = int.Parse(m.Groups["year"].Value);
			year += m.Groups["cent"].Value == "/" ? 1900 : 2000;
			Value = new DateTime(year, month, day);
		}
	}

	class AmountField : Field {

		public decimal Value { get; private set; }

		public AmountField(string data) {
			Value = decimal.Parse(data);
		}
	}

	abstract class StringField : Field {
		public string Value { get; protected set; }
		public StringField(string data) {
			Value = data;
		}
	}

	class MemoField : StringField {
		public MemoField(string data) : base(data) { }
	}

	class ClearedStatusField : Field {
		public char Value { get; private set; }
		public ClearedStatusField(string data) {
			Value = string.IsNullOrWhiteSpace(data) || data.Length == 0 || data[0] == 0 ? ' ' : data[0];
		}
	}

	class ChequeNumberField : Field {
		public int? Number { get; private set; }
		public string Value { get; private set; }
		public ChequeNumberField(string data) {
			Number = int.TryParse(data, out int n) ? n : null;
			if (Number == null) Value = data;
		}
	}

	class PayeeField : StringField {
		public PayeeField(string data) : base(data) { }
	}

	class CategoryField : StringField {
		public CategoryField(string data) : base(data) { }
	}

	class TransferField : StringField {
		public TransferField(string data) : base(null) {
			if (!data.StartsWith('[') || !data.EndsWith(']')) throw new Exception($"Invalid transfer account name: '{data}'");
			Value = data[1..^1];
		}
	}

	class SplitCategory : CategoryField {
		public SplitCategory(string data) : base(data) { }
	}

	class SplitTransfer : TransferField {
		public SplitTransfer(string data) : base(data) { }
	}

	class SplitMemo : MemoField {
		public SplitMemo(string data) : base(data) { }
	}

	class SplitAmount : AmountField {
		public SplitAmount(string data) : base(data) { }
	}
}
