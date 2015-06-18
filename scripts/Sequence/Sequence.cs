using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Util;

namespace Unity.Sequence {
    public class Sequence {
        #region STATIC
        private static float time = 0.0f;

        protected static HashSet<Sequence> awake = new HashSet<Sequence>();
        protected static HashSet<Sequence> dormant = new HashSet<Sequence>();
        protected static PriorityQueue<float, Sequence> waiting = new PriorityQueue<float, Sequence>();
        protected static Queue<Sequence> stirring = new Queue<Sequence>();
        protected static Queue<Sequence> drowsy = new Queue<Sequence>();
        protected static Queue<Sequence> dying = new Queue<Sequence>();
		//protected static HashSet<Sequence> waitingDead = new HashSet<Sequence>();
		
		public static void ResetAll(){
			awake = new HashSet<Sequence>();
        	dormant = new HashSet<Sequence>();
        	waiting = new PriorityQueue<float, Sequence>();
        	stirring = new Queue<Sequence>();
       		drowsy = new Queue<Sequence>();
        	dying = new Queue<Sequence>();
		}
        //public static IEnumerable<Sequence> ListAll { get { return awake.Concat(dormant); } }

        public static void PlayFrame(){
			RemoveDying ();


			if (!waiting.IsEmpty) {
                while (waiting.Peek() <= time && !waiting.IsEmpty) {
                    Queue<Sequence> queue = waiting.DequeueAll();
                    while (queue.Count > 0) {
                        var seq = queue.Dequeue();
                        if (dormant.Contains(seq)) {
                            dormant.Remove(seq);
                            awake.Add(seq);
							if(seq.afterWait != null) {
								seq.afterWait();
								seq.afterWait = null;
							}
                        }
                    }
                }
            }
            while (stirring.Count > 0) {
                var waker = stirring.Dequeue();
                if (dormant.Contains(waker)) {
					dormant.Remove(waker);
				}
				if (!awake.Contains(waker)) {
					awake.Add(waker);
				}
            }

			//Debug.Log("Playing " + awake.Count + " sequences.");
            foreach (Sequence s in awake) {
                s.Play();
				
				if(s.Update != null) s.Update(s, EventArgs.Empty);
            }

            while (drowsy.Count > 0) {
                var sleeper = drowsy.Dequeue();
                if (awake.Contains(sleeper)) {
					awake.Remove(sleeper);
				}
                if (!dormant.Contains(sleeper)) {
					dormant.Add(sleeper);
				}
            }
			
			RemoveDying ();
            time += Time.deltaTime;
        }

		static void RemoveDying(){
			while (dying.Count > 0) {
				var dead = dying.Dequeue();
				awake.Remove(dead);
				dormant.Remove(dead);
				
				if(dead.OnDestroy != null) {
					dead.OnDestroy(dead, EventArgs.Empty);
				}
			}
		}
		
		static void Wait(Sequence seqr, float seconds) {
            waiting.Enqueue(time + seconds, seqr);
            drowsy.Enqueue(seqr);
        }
		
		static void Sleep(Sequence seq){
			drowsy.Enqueue(seq);
		}
		
		static void Awaken(Sequence seq){
			if(dormant.Contains(seq)){
				stirring.Enqueue(seq);
			}
		}

        static void Kill(Sequence seq) {
			dying.Enqueue(seq);
			seq.HandleOnDestroy();
        }
        #endregion

        protected IEnumerator enumerator;
		protected Action OnKill;
		protected Action afterWait;
		protected Sequence awaitingSequence;
		
		public virtual event EventHandler Update;
		public virtual event EventHandler OnDestroy;
		public virtual event EventHandler OnComplete;
		public virtual event EventHandler OnCancel;
		
		protected bool active = true;

		public bool Active{
			get{
				return active;
			}
		}
		
		public Action CompletionAction { get; set; }
		
        protected Sequence() {
			stirring.Enqueue(this);
            SequenceRunner.Run();
        }

        public Sequence(IEnumerator sequence) : this() {
            enumerator = sequence;
        }

        public virtual void Cancel() {
			if(awaitingSequence != null){
				awaitingSequence.Cancel();
			}

			if (active) {
				HandleOnCancel ();
				Kill (this);
			} 
        }
		
		public virtual void Sleep(){
			Sleep(this);
		}
		
		public virtual void Awaken(){
			Awaken(this);
		}
		
		public virtual void Wait(float seconds){
			Wait(this, seconds);
		}
		public virtual void Wait(float seconds, Action afterWait){
			Wait(this, seconds);
			this.afterWait = afterWait;
		}
		
		public virtual void WaitFor(IEnumerator sequence){
			Sleep(this);
			var seq = new Sequence(sequence);
			seq.OnKill = () => Awaken(this);
		}	

		public virtual void WaitFor(Sequence sequence){
			Sleep(this);
			awaitingSequence = sequence;
			sequence.OnKill = StopWaiting;
		}	

		public virtual void StopWaiting(){
			awaitingSequence = null;
			Awaken(this);
		}
			
        protected virtual void Play() {
            if (enumerator.MoveNext()) {
                if (enumerator.Current is Wait) {
                    Wait(((Wait)enumerator.Current).Seconds);
                } else if(enumerator.Current is Sequence){
					WaitFor((Sequence)enumerator.Current);
				}
            }
            else {
				active = false;
				HandleOnComplete();
				if(OnKill != null) {
					OnKill();
				}
                Kill(this);
            }
        }
		
		public static bool operator true(Sequence s){
			if(s == null){
				return false;
			} 
			//Debug.Log("Returning active. Active = " + s.active);
			return s.active;
		}
		
		public static bool operator false(Sequence s){
			if(s == null){
				return true;
			}
			return !s.active;
		}
		
		public static bool operator ! (Sequence s){
			if(s){
				return false;
			} else {
				return true;
			}
		}
		
		public static implicit operator bool (Sequence seq){
			if(seq == null){
				return false;
			} 
			if (seq.awaitingSequence != null) {
				return seq.awaitingSequence;
			}
			return seq.active;
		}
		
		protected void HandleOnDestroy(){
			active = false;
			if(OnDestroy != null){
				OnDestroy(this, new EventArgs());
			}
			OnDestroy = null;
		}
		
		protected void HandleOnComplete(){
			//Debug.Log("Handling completed action.");
			if(OnComplete != null){
				OnComplete(this, new EventArgs());
			}
			OnComplete = null;
			
			if(CompletionAction != null){
				CompletionAction();
			}
		}
		
		
		protected void HandleOnCancel(){
			if(OnCancel != null){
				OnCancel(this, new EventArgs());
			}
			OnCancel = null;
		}
		
    }
}
