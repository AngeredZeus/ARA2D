using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using Nez;
using System;
using ARA2D.Components;

namespace ARA2D.Systems
{
    public class MovementRequest : EntityProcessingSystem
    {
        readonly MovementRequest movementRequest;
        readonly TileEntitySystem tileEntitySystem;


        private Dictionary<int, Vector2> MovementRequests;

        public override void process(Entity entity)
        {
            //Only Contains logic
            //Phase 1: 
            //Collect the movement requests.

            //TODO: Probably a better way to clean this up
            if(entity.getComponent<Movable>().getMove() == Movable.MoveSet.NULL)
            {
                return;
            }else if (entity.getComponent<Movable>().getMove() == Movable.MoveSet.NORTH)
            {
                
                //Store the wanted position, and ID
                MovementRequests.Add(entity.id, new Vector2(entity.position.X, entity.position.Y + 1));

            }
            else if (entity.getComponent<Movable>().getMove() == Movable.MoveSet.EAST)
            {
                MovementRequests.Add(entity, new Vector2(entity.position.X + 1, entity.position.Y));

            }
            else if (entity.getComponent<Movable>().getMove() == Movable.MoveSet.SOUTH)
            {
                MovementRequests.Add(entity.id, new Vector2(entity.position.X, entity.position.Y - 1));

            }
            else if (entity.getComponent<Movable>().getMove() == Movable.MoveSet.WEST)
            {
                MovementRequests.Add(entity.id, new Vector2(entity.position.X - 1, entity.position.Y));

            }
            //This should work so far..


            //Phase 2: 
            //Process the movement requests.

            //TODO: Use LINQ (?) or make this loop better somehow
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
            
            //Process requests by changing the entities position.
            foreach (var mv1 in MovementRequests)
            {
                if (tileEntitySystem.CanPlaceTileEntity(entity, mv1.Value.X, mv1.Value.Y))
                {

                }
            }

        }
        public MovementRequest(MovementRequest movementRequest, TileEntitySystem tileEntitySystem) : base(new Matcher().all(typeof(Movable)))
        {
            this.movementRequest = movementRequest;
            this.tileEntitySystem = tileEntitySystem;
        }
    }
}
