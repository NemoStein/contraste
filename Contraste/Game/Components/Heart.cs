using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Otter;

namespace Contraste {

	public class Heart : Component {
		#region Public Properties
		public int TotalLife { get; private set; }
		public int CurrentLife { get; private set; }
		#endregion

		#region Public Fields
		public Action OnDead;
		public Action OnDamaged;
		public Action OnHealed;
		#endregion

		#region Constructor
		public Heart(int totalLife)
			: base() {
			SetTotalLife(totalLife);
		}

		public Heart() {
			SetTotalLife(1);
		}
		#endregion

		#region Public Methods
		public void TakeDamage(int damage) {
			if(damage <= 0) return;
			CurrentLife -= damage;

			if(CurrentLife <= 0) {
				CurrentLife = 0;

				if(OnDead != null) {
					OnDead();
				}
			} else {
				if(OnDamaged != null) {
					OnDamaged();
				}
			}
		}

		public void RestoreLife(int ammount) {
			CurrentLife += ammount;

			if(CurrentLife > TotalLife) {
				CurrentLife = TotalLife;
			}
			if(OnHealed != null) {
				OnHealed();
			}
		}

		public void RestoreLife() {
			RestoreLife(TotalLife);
		}

		public void IncreaseTotal(int ammount, bool restore) {
			TotalLife += ammount;

			if(restore) {
				RestoreLife();
			}
		}

		public void DecreaseTotal(int ammount, bool restore) {
			TotalLife -= ammount;
			if(TotalLife < CurrentLife) {
				CurrentLife = TotalLife;
			}
			if(restore) {
				RestoreLife();
			}
		}

		public void SetTotalLife(int total) {
			TotalLife = CurrentLife = total;
		}

		#endregion
	}
}
