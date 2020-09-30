﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace PandaMonogame
{
    public interface ISpatialGridObject
    {
        IntXY SpatialCellPosition { get; set; }
    }

    public struct SpatialGridCell<T> where T : ISpatialGridObject
    {
        public int X;
        public int Y;
        public List<T> CellObjects;

        public SpatialGridCell(int x, int y)
        {
            X = x;
            Y = y;
            CellObjects = new List<T>();
        }

        public void Remove(T obj)
        {
            CellObjects.Remove(obj);
        }

        public void Add(T obj)
        {
            CellObjects.Add(obj);
        }
    } // SpatialGridCell

    public class SpatialGrid<T> where T : ISpatialGridObject
    {
        public SpatialGridCell<T>[] Grid;

        public int GridWidth { get; set; }
        public int GridHeight { get; set; }
        public int CellWidth { get; set; }
        public int CellHeight { get; set; }

        public int GridWidthUnits { get; set; }
        public int GridHeightUnits { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gridWidth">In number of cells.</param>
        /// <param name="gridHeight">In number of cells.</param>
        /// <param name="cellWidth">In number of units.</param>
        /// <param name="cellHeight">In number of units.</param>
        public SpatialGrid(int gridWidth, int gridHeight, int cellWidth, int cellHeight)
        {
            GridWidth = gridWidth;
            GridHeight = gridHeight;
            CellWidth = cellWidth;
            CellHeight = cellHeight;

            GridWidthUnits = GridWidth * CellWidth;
            GridHeightUnits = GridHeight * CellHeight;

            Grid = new SpatialGridCell<T>[GridWidth * GridHeight];

            for (var y = 0; y < GridHeight; y++)
            {
                for (var x = 0; x < GridWidth; x++)
                {
                    Grid[x + GridWidth * y] = new SpatialGridCell<T>(x, y);
                }
            }
        }

        public bool UpdateCellPosition(Vector2 position, T obj)
        {
            var newCellPos = GetCellPosition(position);

            if (obj.SpatialCellPosition != newCellPos)
            {
                var cell = GetGridCell(obj);
                cell.Remove(obj);

                obj.SpatialCellPosition = newCellPos;

                cell = GetGridCell(obj);
                cell.Add(obj);

                return true;
            }

            return false;
        }

        public IntXY GetCellPosition(Vector2 position)
        {
            return new IntXY((int)position.X / CellWidth, (int)position.Y / CellHeight);
        }

        public SpatialGridCell<T> GetGridCell(T obj)
        {
            return Grid[obj.SpatialCellPosition.X + GridWidth * obj.SpatialCellPosition.Y];
        }

        public List<T> GetGridCellObjects(int x, int y)
        {
            return Grid[x + GridWidth * y].CellObjects;
        }
    } // SpatialGrid
}
