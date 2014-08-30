using Otter;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Contraste {
	public class StateMachineCoroutine : Component {

		#region Public Fields
		public Dictionary<string, StateCoroutine> states = new Dictionary<string, StateCoroutine>();
		public Coroutine Coroutine = new Coroutine();
		public string Current;
		public string Last;
		public Action OnChange;
		#endregion

		#region Constructor
		public StateMachineCoroutine()
			: base() {
		}
		#endregion

		#region Component Overrides
		public override void Removed() {
			Coroutine.StopAll();
		}

		public override void Update() {
			base.Update();

			if(Coroutine.Running) {
				Coroutine.Update();
			}
		}
		#endregion

		#region Constructor
		public void ChangeState<TState>(TState state) {
			Coroutine.StopAll();
			Coroutine.Start(states[state.ToString()].Update());
			Last = Current;
			Current = state.ToString();
			if(OnChange != null) {
				OnChange();
			}
		}
		#endregion

		#region Public Methods
		public void Populate<TState>() {
			foreach(var name in Enum.GetValues(typeof(TState))) {
				AddState(name);
			}
		}

		public void AddState(string name, Func<IEnumerator> state) {
			states.Add(name, new StateCoroutine(state));
		}

		public void AddState<TState>(TState state) {
			StateCoroutine s;
			MethodInfo mi;
			mi = Entity.GetType().GetMethod(state.ToString(), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy);
			if(mi == null) Entity.GetType().GetMethod(state.ToString(), BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public);
			if(mi != null) {
				s = new StateCoroutine((Func<IEnumerator>)Delegate.CreateDelegate(typeof(Func<IEnumerator>), Entity, mi));
				AddState(state.ToString(), s);
			}
		}

		public void AddState(string name, StateCoroutine state) {
			states.Add(name, state);
		}

		public IEnumerator WaitForSeconds(float seconds) {
			yield return Coroutine.WaitForSeconds(seconds);
		}

		public IEnumerator WaitForFrames(int frames) {
			yield return Coroutine.WaitForFrames(frames);
		}

		public IEnumerator WaitForEvent(string id) {
			yield return Coroutine.WaitForEvent(id);
		}

		public void PublishEvent(string id) {
			Coroutine.PublishEvent(id);
		}

		public void PublishEvent(Enum id) {
			Coroutine.PublishEvent(id);
		}
		#endregion
	}

	public class StateCoroutine {
		public Func<IEnumerator> OnUpdate;
		public StateCoroutine(Func<IEnumerator> onUpdate) {
			OnUpdate = onUpdate;
		}

		public IEnumerator Update() {
			yield return OnUpdate();
		}
	}

}
