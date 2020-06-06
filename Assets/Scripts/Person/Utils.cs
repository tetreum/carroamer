using UnityEngine;

namespace Peque.Person
{
    public class Utils : MonoBehaviour
    {
        public static int sanitizeStatValue(int val, int min = 0, int max = 100) {
            if (val < min) {
                val = min;
            } else if (val > max) {
                val = max;
            }
            return val;
        }
    }
}

