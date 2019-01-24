using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

interface IMoveable
{
    void OnMoveStarted();
    void OnMove(UnityEngine.Vector3 position);
    void OnMoveEnded();
    void OnCancelMove();
}
