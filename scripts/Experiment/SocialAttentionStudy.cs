using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Crystallize.Experiment {
    public class SocialAttentionStudy : MonoBehaviour {

        HashSet<int> providedFeedback = new HashSet<int>();

        // Use this for initialization
        void Start() {
            GameSettings.SetFlag(UIFlags.LockEnglishChat, true);
            GameSettings.SetFlag(NetworkFlags.LockAnimation, true);
            GameSettings.SetFlag(NetworkFlags.LockEnglishText, true);

            CrystallizeEventManager.PlayerState.OnQuestStateChanged += HandleOnQuestStateChanged;
            CrystallizeEventManager.Environment.OnPersonAnimationRequested += HandleOnPersonAnimationRequested;
        }

        void HandleOnPersonAnimationRequested(object sender, PersonAnimationEventArgs e) {
            var id = PlayerManager.main.GetPlayerID(e.TargetObject);
            Debug.Log("Animation requested: " + id + "; " + PlayerManager.main.PlayerID);

            string text = null;
            switch (e.AnimationType) {
                case PersonAnimationType.Happy:
                    text = "nice!";
                    if (id == PlayerManager.main.PlayerID) {
                        StartCoroutine(PlayDelayedAnimation(PersonAnimationType.Thanks));
                    }
                    break;

                case PersonAnimationType.Wave:
                    text = "hi!";
                    if (id == PlayerManager.main.PlayerID) {
                        StartCoroutine(PlayDelayedAnimation(PersonAnimationType.Wave));
                    }
                    break;

                case PersonAnimationType.Thanks:
                    text = "thanks :)";
                    break;
            }

            if (text != null) {
                CrystallizeEventManager.Network.RaiseEnglishLineInput(this, new TextEventArgs("Player " + id + ": " + text));
            }
        }

        void HandleOnQuestStateChanged(object sender, QuestStateChangedEventArgs e) {
            if (providedFeedback.Contains(e.QuestID)) {
                return;
            }

            if (e.PlayerID != PlayerManager.main.PlayerID) {
                return;
            }

            var qd = e.GetQuestInstance();
            if (qd.GetObjectiveState(0).IsComplete) {
                GetFeedbackFromPartner();
                providedFeedback.Add(e.QuestID);
            }
        }

        void GetFeedbackFromPartner() {
            StartCoroutine(PlayDelayedAnimation(PersonAnimationType.Happy));
        }

        IEnumerator PlayDelayedAnimation(PersonAnimationType type) {
            yield return new WaitForSeconds(3f);

            var args = new PersonAnimationEventArgs(PlayerManager.main.OtherGameObject, type);
            CrystallizeEventManager.Environment.RaisePersonAnimationRequested(this, args);
        }

    }
}