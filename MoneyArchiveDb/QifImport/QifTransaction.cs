using System;
using System.Collections.Generic;
using System.Linq;

namespace MoneyArchiveDb.QifImport {
	public class QifTransaction {
		public DateTime Date { get; private set; }
		public decimal Amount { get; private set; }
		public char Status { get; private set; }
		public string Memo { get; private set; }
		public int? ChequeNumber { get; private set; }
		public string TransType { get; private set; }
		public string Payee { get; private set; }
		public string Transfer { get; private set; }
		public string Category { get; private set; }
		public Split[] Splits { get; private set; }

		public class Split {
			public string Category { get; internal set; }
			public string Transfer { get; internal set; }
			public string Memo { get; internal set; }
			public decimal Amount { get; internal set; }
		}

		public QifTransaction(QifRecord r) {
			List<Split> splits = new();
			Split currSplit = null;

			foreach (Field f in r.Fields) {
				if (f is SplitCategory sc) splits.Add(currSplit = new Split { Category = sc.Value });
				else if (f is SplitTransfer st) splits.Add(currSplit = new Split { Transfer = st.Value });
				else if (f is SplitMemo sm) currSplit.Memo = sm.Value;
				else if (f is SplitAmount sa) currSplit.Amount = sa.Value;
				else if (f is DateField d) Date = d.Value;
				else if (f is AmountField a) Amount = a.Value;
				else if (f is MemoField m) Memo = m.Value;
				else if (f is ClearedStatusField s) Status = s.Value;
				else if (f is ChequeNumberField c) {
					ChequeNumber = c.Number;
					TransType = c.Value;
				} else if (f is PayeeField p) Payee = p.Value;
				else if (f is TransferField t) Transfer = t.Value;
				else if (f is CategoryField cat) Category = cat.Value;
				else throw new Exception($"Unexpected field type '{f.GetType().Name}'");
			}
			if (splits.Any()) Splits = splits.ToArray();
		}
	}
}
