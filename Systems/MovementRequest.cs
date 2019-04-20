using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Nez;
using System;
using ARA2D.Components;
using ARA2D.TileEntities;

namespace ARA2D.Systems
{
    public class MovementRequest : EntityProcessingSystem
    {
        readonly MovementRequest movementRequest;
        readonly TileEntitySystem tileEntitySystem;
        private int tempCount = 0; 

        private Dictionary<Entity, Vector2> MovementRequests;

        public override void process(Entity entity)
        {
            //Only Contains logic
            //Phase 1: 
            //Collect the movement requests.


            var move = entity.getComponent<Movable>().CurrentMove;
            var Worldpos = TileCoords.ToWorldSpace(entity.localPosition.X, entity.localPosition.Y);

            switch (move)
            {
                case Movable.MoveSet.NULL:
                    break;
                case Movable.MoveSet.NORTH:
                    MovementRequests.Add(entity, new Vector2(Worldpos.X, Worldpos.Y + 1.0f));
                    break;
                case Movable.MoveSet.EAST:
                    MovementRequests.Add(entity, new Vector2(Worldpos.X + 1.0f, Worldpos.Y));
                    break;
                case Movable.MoveSet.SOUTH:
                    MovementRequests.Add(entity, new Vector2(Worldpos.X, Worldpos.Y - 1.0f));
                    break;
                case Movable.MoveSet.WEST:
                    MovementRequests.Add(entity, new Vector2(Worldpos.X - 1.0f, Worldpos.Y));
                    break;
                default:
                    break;
            }
            tempCount++;


            if(tempCount == 20)
            {
                EvaluateMovementRequests();
                tempCount = 0;
            }

            //This should work so far..

            

        }
        public MovementRequest(MovementRequest movementRequest, TileEntitySystem tileEntitySystem) : base(new Matcher().all(typeof(Movable)))
        {
            this.movementRequest = movementRequest;
            this.tileEntitySystem = tileEntitySystem;
        }

        public void EvaluateMovementRequests()
        {

            //Phase 2: 
            //Process the movement requests.

            //TODO: Optimize This
            //Removes duplicates
            foreach (var mv1 in MovementRequests)
            {
                foreach (var mv2 in MovementRequests)
                {
                    if (mv1.Value == mv2.Value)
                    {
                        MovementRequests.Remove(mv1.Key);
                        MovementRequests.Remove(mv2.Key);
                    }
                }
            }
            foreach (var moveRequest in MovementRequests)
            {
                moveRequest.Key.setPosition(TileCoords.FromWorldSpace(moveRequest.Value.X, moveRequest.Value.Y));
                moveRequest.Key.getComponent<Movable>().CurrentMove = Movable.MoveSet.NULL;
                MovementRequests.Remove(moveRequest.Key);
            }

        }
    }
}
