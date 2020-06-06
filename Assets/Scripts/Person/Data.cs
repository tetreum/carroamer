using System.Collections.Generic;

namespace Peque.Person
{
    public class Data
    {
        public int id;
        public string firstName;
        public string surnames;
        public int houseId;

        public int age;
        public int health;
        public int hungry;
        public int strength = 10;
        public int stamina;
        public int intelligence;

        public Player.FreezeReason? freezeReason;
        public Weapons.Weapon weapon = new Weapons.Weapon(Weapons.Weapon.Type.Hand, 5, 10, 2);
        public Inventory.ItemData[] inventory = new Inventory.ItemData[0];

        public Dictionary<int, DialogData> spokenDialogs = new Dictionary<int, DialogData> ();
        public Dictionary<int, Feelings> feelings = new Dictionary<int, Feelings> ();
        public Dictionary<int, int> contacts = new Dictionary<int, int> ();
    }

    public class Feelings
    {
        public int _loyalty = 0;
        public int _arousal = 0;
        public int _badness = 10;
        
        public int loyalty {
            get {
                return loyalty;
            }
            set {
                loyalty = Utils.sanitizeStatValue(value);
            }
        }
        public int arousal {
            get {
                return arousal;
            }
            set {
                arousal = Utils.sanitizeStatValue(value);
            }
        }
        public int badness {
            get {
                return badness;
            }
            set {
                badness = Utils.sanitizeStatValue(value);
            }
        }
    }

    public class DialogData
    {
        public int myId;
        public int npcId;

        public DialogData (int selfId, int otherId)
        {
            myId = selfId;
            npcId = otherId;
        }

        public Dictionary<string, string> asked = new Dictionary<string, string>(); // questions he made
        public Dictionary<string, string> replied  // replies he made
        {
            get {
                return new Dictionary<string, string>();
                /*
                var spokenDialogs = ModManager.getPersonData(npcId).spokenDialogs;

                if (!spokenDialogs.ContainsKey(myId)) {
                    return new Dictionary<string, string>();
                }

                return spokenDialogs[myId].asked;
                */
            }
        }
    }
}