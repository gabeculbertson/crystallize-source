using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IDayEvent {

    List<IDayEvent> Branches { get; set; }

    void SetNextBranch(int branch);

}
