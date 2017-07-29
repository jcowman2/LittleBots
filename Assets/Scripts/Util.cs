using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Util {

    public static bool CloseEnough(Transform a, Transform b, float margin = 0) {
        return Vector2.Distance(a.position, b.position) <= margin;
    }

}
