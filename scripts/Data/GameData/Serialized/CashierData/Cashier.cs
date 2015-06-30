using UnityEngine;
using System.Collections;

namespace CrystallizeData {
	public class Cashier : StaticSerializedJobGameData {
		protected override void PrepareGameData() {
			Initialize("Cashier");
			AddTask<CashierSellGoods>();
		}

	}
}
