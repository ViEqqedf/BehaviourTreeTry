using BehaviourTree;
using BehaviourTree.Nodes;
using BehaviourTree.Precondition;

public class MoveAttatckAI : BTTree
{
    private const string DESTINATION = "Destination";
    private const string ORC_NAME = "Orc";
    private const string GOBLIN_NAME = "Goblin";
    private const string RUN_ANIMATION = "Run";
    private const string FIGHT_ANIMATION = "Fight";
    private const string IDLE_ANIMATION = "Idle";
    public float speed;
    public float sightForOrc;
    public float sightForGoblin;
    public float fightDistance;

    protected override void Init() {
        base.Init();

        root = new BTPrioritySelector("Root");

        // Move
        BTParallel move = new BTParallel("Move", BTParallel.ParallelFunction.Or);
        move.AddChild(new Move(DESTINATION, speed));
        move.AddChild(new PlayAnimation(RUN_ANIMATION));

        // Escape
        InSightPrec orcInSight = new InSightPrec(sightForOrc, ORC_NAME);
		FindEscapeDestination findEscapeDestination = new FindEscapeDestination(
            ORC_NAME, DESTINATION, sightForOrc);
        BTParallel escape = new BTParallel("Escape", BTParallel.ParallelFunction.Or, orcInSight);
        escape.AddChild(findEscapeDestination);
        escape.AddChild(move);
        root.AddChild(escape);

        // Fight
        InSightPrec gobInSight = new InSightPrec(sightForGoblin, GOBLIN_NAME);
		FindToTargetDestination findFightDestination = new FindToTargetDestination(
            GOBLIN_NAME, DESTINATION, fightDistance * 0.9f);
        BTSequence fight = new BTSequence("Fight", gobInSight);
        BTParallel fightParallel = new BTParallel("FightParallel", BTParallel.ParallelFunction.Or);
        fightParallel.AddChild(findFightDestination);
        fightParallel.AddChild(move);
        InSightPrec gobInFightDistance = new InSightPrec(fightDistance, GOBLIN_NAME);
        fight.AddChild(fightParallel);
        fight.AddChild(new PlayAnimation(FIGHT_ANIMATION, gobInFightDistance));
        root.AddChild(fight);

        //Idle
        root.AddChild(new PlayAnimation(IDLE_ANIMATION));
    }
}
