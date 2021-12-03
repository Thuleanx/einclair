using Thuleanx.AI.Core;

namespace Thuleanx.Interaction {
    public class PlayerLockEmitter : CustomEmitter {
        public string AnimTrigger = "Idle";

        public override void Execute() {
            if (Context.ReferenceManager.Player)
                Context.ReferenceManager.Player.Lock(AnimTrigger, (fin) => fin?(int) Player.PlayerState.Normal : -1);
        }
    }
}