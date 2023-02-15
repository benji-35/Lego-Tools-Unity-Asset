using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace kap35 {
    namespace lego {
        [CreateAssetMenu(fileName = "NewDiscuss", menuName = "kap35/lego/Discuss", order = 1)]
        public class Discuss : ScriptableObject
        {

            public Discuss nextDiscussion = null;
            public string talker = "Person";
            public string text = "Hello World !";

            public string GetTalker()
            {
                return talker;
            }

            public string GetText()
            {
                return text;
            }

            public Discuss GetNextDiscussion()
            {
                return nextDiscussion;
            }

        }
    }
}