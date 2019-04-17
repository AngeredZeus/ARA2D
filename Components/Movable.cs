using Nez;

namespace ARA2D.Components
{
    public class Movable : Component
    {
        public enum MoveSet {NULL, NORTH, EAST, SOUTH, WEST };
        MoveSet CurrentMove = MoveSet.NULL;



        //TODO Weird C# auto get/set
        public void setMove(MoveSet m)
        {
            CurrentMove = m;
        }
        public MoveSet getMove()
        {
            return CurrentMove;
        }
    }
}
