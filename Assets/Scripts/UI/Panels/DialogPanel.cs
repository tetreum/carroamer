using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Peque;
using Peque.Person;
using Peque.UI;
using Peque.NPC;

public class DialogPanel : MonoBehaviour
{
    public Text message;
    public GameObject replyPrefab;
    public Transform replyContainer;
    public Text personName;
    public string possibleReplyId; // used by DialogItem to set the dialog that player is hovering

    public static DialogPanel Instance;
    public static Dialog currentDialog;
    public static Dictionary<string, Dialog> dialogs = new Dictionary<string, Dialog>();
    public static IDialogSetup logic;

    private int lastSender;
    private int lastReceiver;
    private Button firstButton;

    public void init() {
        Instance = this;
        logic = new DialogSetup();
    }

    private void OnEnable() {
        StartCoroutine(selectFirstItem());
    }

    // Unity hack as Select() method doesn't work in OnEnable
    IEnumerator selectFirstItem() {
        yield return new WaitForSeconds(0.1f);

        try {
            firstButton.GetComponent<Button>().Select();
        } catch (Exception) { } // maybe the container its empty
    }

    public void showDialog (string dialog) {
        showDialog(dialog, lastSender, lastReceiver);
    }

    public void showDialog (string dialog, int senderId, int receiverId)
    {
        if (!dialogs.ContainsKey(dialog)) {
            Debug.LogError("DialogPanel - Missing dialog text for " + dialog);
            return;
        }
        lastSender = senderId;
        lastReceiver = receiverId;
        Person sender = NPCManager.Instance.getPerson(senderId);
        Person receiver = NPCManager.Instance.getPerson(receiverId);
        
        deletePreviousReplies();

        // make characters look each other
        receiver.transform.LookAt(sender.face);
        receiver.talked = sender;
        sender.talked = receiver;

        message.text = dialogs[dialog].text;
        if (sender.knows(receiver)) {
            personName.text = sender.fullName;
        } else {
            personName.text = "";
        }
        
        List<Dialog> replies = getAvailableReplies(dialog, senderId, receiverId);
        
        if (replies.Count == 0) {
            replyContainer.gameObject.SetActive(false);
        } else {
            replyContainer.gameObject.SetActive(true);

            foreach (Dialog reply in replies)
            {
                GameObject button = (GameObject)Instantiate(replyPrefab, replyContainer);
                button.name = "TemporalReplyButton";
                button.GetComponentInChildren<Text>().text = reply.text;
                button.GetComponent<DialogItem>().id = reply.id;

                AddListener(button.GetComponent<Button>(), reply.id);

                button.SetActive(true);

                if (firstButton == null) {
                    firstButton = button.GetComponent<Button>();
                }
            }

            // maybe its already frozen because of another thing, dont set it frozen by dialog 
            // or it will get unfrozen when dialog stops
            if (!Player.Instance.isFrozen) {
                Player.Instance.freeze(Player.FreezeReason.Dialog);
            }
            Cursors.setLocked();
        }       

        currentDialog = getDialog(dialog);

        gameObject.SetActive(true);
        firstButton.GetComponent<Button>().Select();
    }

    void AddListener(Button b, string dialogId)
    {
        b.onClick.AddListener(() => reply(dialogId));
    }

    void deletePreviousReplies ()
    {
        try
        {
            var children = new List<GameObject>();

            foreach (Transform child in replyContainer)
            {
                if (child.name == "TemporalReplyButton") {
                    children.Add(child.gameObject);
                }
            }
            children.ForEach(child => Destroy(child));

        } catch {}
        firstButton = null;
    }

    public Dialog getDialog (string dialog)
    {
        if (!dialogs.ContainsKey(dialog)) {
            Debug.LogError("DialogPanel - Missing dialog text for " + dialog);
            return null;
        }

        Dialog d = dialogs[dialog];
        d.id = dialog;

        return d;
    }

    private List<Dialog> getAvailableReplies(string dialog, int senderId, int receiverId)
    {
        Person sender = NPCManager.Instance.getPerson(senderId);
        Person receiver = NPCManager.Instance.getPerson(receiverId);

        if (!receiver.getData().spokenDialogs.ContainsKey(senderId)) {
            receiver.getData().spokenDialogs.Add(senderId, new DialogData(receiverId, senderId));
        }
        
        return logic.getAvailableReplies(dialog, sender, receiver);
    }

    public void reply (string replyId)
    {
        if (replyId == "GotNothingToSay") {
            close();
            return;
        }
        /*
        Person lastSenderPerson = ModManager.getPerson(lastSender);
        lastSenderPerson.setSpokenDialog(lastReceiver, currentDialog.id, replyId);

        if (currentDialog.OnReply == null) {
            close();
            return;
        } else {
            currentDialog.OnReply(currentDialog, replyId, lastSenderPerson, ModManager.getPerson(lastReceiver));
        }
        */
    }

    public void close()
    {
        currentDialog = null;
        firstButton = null;
        Cursors.setLocked();

        if (Player.Instance.freezeReason == Player.FreezeReason.Dialog) {
            Player.Instance.unFreeze();
        }
        /*
        ModManager.getPerson(lastReceiver).talked = null;
        ModManager.getPerson(lastSender).talked = null;
        */
        gameObject.SetActive(false);
    }
}