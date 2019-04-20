using Nez;

namespace ARA2D.Components
{
    public class Movable : Component
    {
        public enum MoveSet {NULL, NORTH, EAST, SOUTH, WEST };
        public MoveSet CurrentMove = MoveSet.NULL;
    }
}
