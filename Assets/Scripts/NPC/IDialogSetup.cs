using System.Collections.Generic;
using Peque;
using Peque.Person;

public abstract class IDialogSetup {
    public abstract List<Dialog> getAvailableReplies(string dialogId, Person sender, Person receiver);
}
