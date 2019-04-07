﻿using ARA2D.Components;
using Nez;

namespace ARA2D
{
    public static class Events
    {
        // TODO: I should probably remove this type of code.
        // Using ECS for an event queue sounds like a cool idea, but causes some overhead, and also
        // doesn't allow for multiple systems to respond to an entity event
        public static void ChunkGenerated(ChunkCoords coords, Chunk chunk)
        {
            var entity = new Entity($"ChunkGenerated{coords.Cx},{coords.Cy}");
            entity.addComponent(new ChunkGeneratedEvent(coords, chunk));
            Core.scene.addEntity(entity);
        }

        public static void ChunkRemoved(ChunkCoords coords)
        {
            var entity = new Entity($"ChunkRemoved{coords.Cx},{coords.Cy}");
            entity.addComponent(new ChunkRemovedEvent(coords));
            Core.scene.addEntity(entity);
        }
    }
}
