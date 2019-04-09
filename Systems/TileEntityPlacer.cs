﻿using Microsoft.Xna.Framework;
using Nez;
using System;

namespace ARA2D.Systems
{
    public class TileEntityPlacer : ProcessingSystem
    {
        TileEntitySystem tileEntitySystem;
        ITileEntity template;
        Color validPlacementColor = new Color(255, 255, 255, 180);
        Color invalidPlacementColor = new Color(255, 64, 64, 180);

        public TileEntityPlacer(TileEntitySystem tileEntitySystem)
        {
            this.tileEntitySystem = tileEntitySystem;
        }

        public void SetTemplate(ITileEntity template)
        {
            if (template == null)
            {
                if (this.template?.Entity != null) this.template.Entity.destroy();
                this.template = template;
                return;
            }
            this.template = template;
            template.CreateEntity(scene, 0, 0);
        }

        public override void process()
        {
            if (template?.Entity == null) return;

            // Adjust tileEntityToPlace ghost position
            var mousePoint = scene.camera.screenToWorldPoint(Input.mousePosition);
            var anchorPoint = mousePoint /= Tile.Size;

            anchorPoint.X = template.Width % 2 == 0
                ? (float)Math.Round(anchorPoint.X)
                : (float)Math.Round(anchorPoint.X + .5f) - .5f;
            anchorPoint.Y = template.Height % 2 == 0
                ? (float)Math.Round(anchorPoint.Y)
                : (float)Math.Round(anchorPoint.Y + .5f) - .5f;
            anchorPoint.X -= template.Width * .5f;
            anchorPoint.Y -= template.Height * .5f;

            template.Entity.position = anchorPoint * Tile.Size;

            long anchorX = (long)Math.Round(anchorPoint.X);
            long anchorY = (long)Math.Round(anchorPoint.Y);

            var canPlace = tileEntitySystem.CanPlaceTileEntity(template, anchorX, anchorY);

            template.Sprite.setColor(canPlace ? validPlacementColor : invalidPlacementColor);

            // Place tile entity
            if (Input.leftMouseButtonDown && canPlace)
            {
                tileEntitySystem.PlaceTileEntity(template.Clone(), anchorX, anchorY);
            }
        }
    }
}