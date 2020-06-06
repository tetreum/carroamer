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
                {"LetMeIntroduceMyself", new Dialog("Hola, permíteme presentarme.\n Mi nombre es {{receiverName}}")},
                {"WhatDoYouWant", new Dialog("¿Qué quieres?")},
                {"OpenDoorUnkownVisitor", new Dialog("¿Qué quieres?", (Dialog currentDialog, string replyId, Person.Person sender, Person.Person receiver) => {
                    switch (replyId)
                    {
                        case "LetMeIntroduceMyself":
                            DialogPanel.Instance.showDialog("ItsAPleasure");
                            break;
                    }
                })},
                {"OpenDoorUnexpected", new Dialog("Vaya, no te esperaba")},
                {"OpenDoorExpected", new Dialog("Ei, pasa, pasa")},
                {"GotNothingToSay", new Dialog("Dejar de hablar")},
                {"ItsAPleasure", new Dialog("Un placer conocerte \n Mi nombre es {{senderName}}")},
                {"SorryIDontKnowYouEnough", new Dialog("Lo siento, no te conozco lo suficiente")},
                {"HereYouHaveIt", new Dialog("Toma, aquí tienes")},
                {"GiveMeYourPhone", new Dialog("Me gustaría tener tu número de teléfono", (Dialog currentDialog, string replyId, Person.Person sender, Person.Person receiver) => {
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