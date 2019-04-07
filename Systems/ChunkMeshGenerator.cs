﻿using Nez;
using ARA2D.Components;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ARA2D.Systems
{
    public class ChunkMeshGenerator : ProcessingSystem
    {
        // TODO: Implement variable handling depending on current performance     
        // We could do this by only handling so many ChunkGenerated events per frame, and after that queueing them up
        // public int HandlePerFrame = 5;

        readonly HashSet<ChunkCoords> LoadedMeshes;
        readonly Texture2D chunkTextures;

        // TODO: Handle a ChunkRemoved event
        public ChunkMeshGenerator(Texture2D chunkTextures)
        {
            this.chunkTextures = chunkTextures;
            LoadedMeshes = new HashSet<ChunkCoords>();
            Events.OnTileChunkGenerated += TileChunkGenerated;
        }

        public void TileChunkGenerated(ChunkCoords coords, TileChunk chunk)
        {
            if (LoadedMeshes.Contains(chunk.Coords))
            {
                return;
            }

            var vertices = CreateVertexArray(chunk.BaseTiles);
            var indices = CreateIndicesArray();
            var mesh = CreateMesh(vertices, indices);

            Entity meshEntity = new Entity($"ChunkMesh{coords.Cx},{coords.Cy}");
            meshEntity.addComponent(mesh);
            scene.addEntity(meshEntity);
            meshEntity.position = chunk.Coords.ToWorldCoords();

            LoadedMeshes.Add(chunk.Coords);
        }

        public bool ChunkLoaded(ChunkCoords coords) => LoadedMeshes.Contains(coords);

        static VertexPositionColorTexture[] CreateVertexArray(short[,] baseTileTypes)
        {
            var vertices = new VertexPositionColorTexture[TileChunk.Size * TileChunk.Size * 4];

            int vi = 0;
            for (int y = 0; y < TileChunk.Size; y++)
            {
                for (int x = 0; x < TileChunk.Size; x++)
                {
                    int blockIndex = baseTileTypes[x, y];
                    for (int corner = 0; corner < 4; corner++, vi++)
                    {
                        int cx = corner % 2;
                        int cy = corner / 2;
                        vertices[vi].Position = new Vector3((x + cx) * Tile.Size, (y+cy) * Tile.Size, 0 );
                        vertices[vi].TextureCoordinate = GetUVCoordsFromIndex(blockIndex, corner);
                        vertices[vi].Color = Color.White;
                    }
                }
            }

            return vertices;
        }

        /// <summary>
        /// Get texture coordinates for the block index given
        /// </summary>
        /// <param name="blockIndex"></param>
        /// <param name="corner">The corner of the block wanted
        /// 0 = Top-left
        /// 1 = Top-right
        /// 2 = Bottom-left
        /// 3 = Bottom-right
        /// </param>
        /// <returns></returns>
        static Vector2 GetUVCoordsFromIndex(int blockIndex, int corner)
        {
            // TODO: pull texture info from JSON
            int width = 4;
            int height = 4;

            int x = blockIndex % width + corner % 2;
            int y = blockIndex / width + corner / 2;

            return new Vector2((float) x / width, (float) y / height);
        }

        static short[] CreateIndicesArray()
        {
            short[] indices = new short[TileChunk.Size * TileChunk.Size * 6];
            int i = 0;
            int vi = 0;
            for (int y = 0; y < TileChunk.Size; y++)
            {
                for (int x = 0; x < TileChunk.Size; x++, vi += 4)
                {
                    indices[i++] = (short) vi;
                    indices[i++] = (short) (vi + 1);
                    indices[i++] = (short) (vi + 3);
                    indices[i++] = (short) vi;
                    indices[i++] = (short) (vi + 3);
                    indices[i++] = (short) (vi + 2);
                }
            }
            return indices;
        }

        RenderableComponent CreateMesh(VertexPositionColorTexture[] vertices, short[] indices)
        {
            UvMesh m = new UvMesh();
            m.SetVertices(vertices);
            m.SetIndices(indices);
            m.RecalculateBounds();
            m.SetTexture(chunkTextures);
            m.debugRenderEnabled = true;
            return m;
        }

        public override void process()
        {
            
        }
    }
}
