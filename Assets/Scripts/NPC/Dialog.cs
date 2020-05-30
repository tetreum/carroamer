using System.Collections.Generic;

namespace Peque
{
    [System.Serializable]
    public class Dialog
    {
        public string id;
        public string text;
        public string nextDialog;
        public dialogCallback OnReply;
        public bool isTranslated = false;
        public Dictionary<string, string> varList;

        public delegate void dialogCallback(Dialog currentDialog, string replyId, Person.Person sender, Person.Person receiver);

        public Dialog(string txt)
        {
            text = txt;
        }

        public Dialog(string txt, Dictionary<string, string> vars)
        {
            text = txt;
            varList = vars;
        }

        public Dialog(string txt, string next)
        {
            text = txt;
            nextDialog = next;
        }

        public Dialog(string txt, string next, Dictionary<string, string> vars)
        {
            text = txt;
            nextDialog = next;
            varList = vars;
        }

        public Dialog(string txt, string next, dialogCallback cb)
        {
            text = txt;
            nextDialog = next;
            OnReply = cb;
        }

        public Dialog(string txt, string next, dialogCallback cb, Dictionary<string, string> vars)
        {
            text = txt;
            nextDialog = next;
            OnReply = cb;
            varList = vars;
        }

        public Dialog(string txt, dialogCallback cb)
        {
            text = txt;
            OnReply = cb;
        }

        public Dialog(string txt, dialogCallback cb, Dictionary<string, string> vars)
        {
            text = txt;
            OnReply = cb;
            varList = vars;
        }
    }

}

