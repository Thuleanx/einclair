using Thuleanx.AI.Core;

namespace Thuleanx.Interaction {
    public class PlayerForceDirection : CustomEmitter {
        public enum LR {
            Left, 
            Right
        }
        public LR Dir;
        public override void Execute() {
            if (Context.ReferenceManager.Player)
                Context.ReferenceManager.Player.ForceDirection(Dir == LR.Right);
        }
    }
}