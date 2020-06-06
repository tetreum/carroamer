using System.Collections.Generic;
using UnityEngine;

namespace Peque.NPC
{
    public class DialogSetup : IDialogSetup
    {
        public DialogPanel dialogPanel;

        public DialogSetup()
        {
            dialogPanel = DialogPanel.Instance;

            DialogPanel.dialogs = getDialogs();
        }

        /*
         All game dialogs
         Dictionary key string is used as identifier, it must be unique and easy to understand what will say
        */
        private Dictionary<string, Dialog> getDialogs ()
        {
            Dictionary<string, Dialog> dialogs = new Dictionary<string, Dialog>() {
                {"LetMeIntroduceMyself", new Dialog("Hi, let me introduce myself.\n My name is {{receiverName}}")},
                {"WhatDoYouWant", new Dialog("¿What do you want?", (Dialog currentDialog, string replyId, Person.Person sender, Person.Person receiver) => {
                    switch (replyId)
                    {
                        case "LetMeIntroduceMyself":
                            DialogPanel.Instance.showDialog("ItsAPleasure");
                            break;
                    }
                })},
                {"OpenDoorUnkownVisitor", new Dialog("¿What do you want?", (Dialog currentDialog, string replyId, Person.Person sender, Person.Person receiver) => {
                    switch (replyId)
                    {
                        case "LetMeIntroduceMyself":
                            DialogPanel.Instance.showDialog("ItsAPleasure");
                            break;
                    }
                })},
                {"OpenDoorUnexpected", new Dialog("Oh, i didn't expect you")},
                {"OpenDoorExpected", new Dialog("Hey, come in!")},
                {"GotNothingToSay", new Dialog("Stop speaking")},
                {"ItsAPleasure", new Dialog("It's a pleasure \n My name is {{senderName}}")},
                {"SorryIDontKnowYouEnough", new Dialog("Sorry i don't know much about you yet")},
                {"HereYouHaveIt", new Dialog("There you have")},
                {"GiveMeYourPhone", new Dialog("I would like to have your phone number", (Dialog currentDialog, string replyId, Person.Person sender, Person.Person receiver) => {
                    if (sender.getFeelingsFor(receiver).arousal < 40) {
                        DialogPanel.Instance.showDialog("SorryIDontKnowYouEnough");
                    } else {
                        DialogPanel.Instance.showDialog("HereYouHaveIt");
                        receiver.getData().contacts.Add(sender.id, 0);
                        sender.getData().contacts.Add(receiver.id, 0);
                    }
                })},
            };

            return dialogs;
        }

        /*
         When sender starts a conversation with receiver, this method is fired to know which replies
         can receiver make
        */
        public override List<Dialog> getAvailableReplies(string dialogId, Person.Person sender, Person.Person receiver)
        {
            var spokenDialogs = receiver.getData().spokenDialogs[sender.id].asked;
            List<Dialog> dialogs = new List<Dialog>();
            bool exitDialog = true;

            // first conversation between them
            if (!receiver.knows(sender)) {
                dialogs.Add(dialogPanel.getDialog("LetMeIntroduceMyself"));
            }

            switch (dialogId) {
                case "WhatDoYouWant":
                    if (!receiver.isInContacts(sender)) {
                        dialogs.Add(dialogPanel.getDialog("GiveMeYourPhone"));
                    }
                    break;
            }

            if (exitDialog) {
                dialogs.Add(dialogPanel.getDialog("GotNothingToSay"));
            }

            return dialogs;
        }
    }
}