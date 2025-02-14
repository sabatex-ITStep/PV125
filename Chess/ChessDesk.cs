﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    public class ChessDesk:ICloneable
    {
        Figure[,] _figures = new Figure[8, 8];

        public FigureColor? Shach = null;

        
        public List<Figure> KilledFigures { get;} = new List<Figure> { };
        
        public void Clear()
        {
            for (var row = 2;row < 6; row++)
            {
                for (var column = 0; column < 8; column++) 
                {
                    _figures[row, column] = null;
                }
            }
            // pawn
            for (var column = 0; column < 8; column++)
            {
                _figures[1,column] = new Pawn(FigureColor.Black);
                _figures[6, column] = new Pawn(FigureColor.White);
            }
            // rook
            _figures[0,0] = new Rock(FigureColor.Black);
            _figures[0,7] = new Rock(FigureColor.Black);
            _figures[7,0] = new Rock(FigureColor.White);
            _figures[7,7] = new Rock(FigureColor.White);

            _figures[0,1] = new Horse(FigureColor.Black);
            _figures[0,6] = new Horse(FigureColor.Black);
            _figures[7,1] = new Horse(FigureColor.White );
            _figures[7,6] = new Horse(FigureColor.White);

            _figures[0,2] = new Bishop(FigureColor.Black );
            _figures[0,5] = new Bishop(FigureColor.Black);
            _figures[7,2] = new Bishop(FigureColor.White );
            _figures[7,5] = new Bishop(FigureColor.White);

            _figures[0,3] = new King(FigureColor.Black );
            _figures[0,4] = new Queen(FigureColor.Black );
            _figures[7,3] = new King(FigureColor.White );
            _figures[7,4] = new Queen(FigureColor.White );


        }

        public Figure this[int row, int column]
        {
            get=>_figures[row, column];
        }

        public Figure this[ChessPoint point]
        {
            get => _figures[point.Row, point.Column];
        }

        public FigureColor ActiveColor { get; set; }
        public void Move(ChessPoint source, ChessPoint destination)
        {
            var figure = _figures[source.Row, source.Column];
            if (figure == null)
                throw new Exception($"Figure not found in ({source.Row}.{source.Column}) ");
            if (figure.FigureColor != ActiveColor)
                throw new Exception($"Must be move figure color - {ActiveColor}");
            if (!figure.CheckMove(source, destination, this))
                throw new Exception("Figure can't move to position");
            var destinationFigure = _figures[destination.Row, destination.Column];
            if (destinationFigure != null)
            {
                KilledFigures.Add(destinationFigure);
            }
            figure.IsFirstMove = false;
            _figures[destination.Row, destination.Column] = figure;
            _figures[source.Row, source.Column] = null;
            var activeColor = ActiveColor == FigureColor.Black ? FigureColor.White : FigureColor.Black;
            
            if (CheckShach(activeColor))
            {
                Shach = activeColor;
            }
            ActiveColor = activeColor;
        }

        public ChessPoint GetKingPoint(FigureColor figureColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    King figure = _figures[row, column] as King;
                    if (figure != null)
                    {
                        if (figure.FigureColor == figureColor) 
                            return new ChessPoint(row, column);
                    }
                }
            }
            throw new Exception("King nor exist");

        }
        public IEnumerable<ChessPoint> GetFiguresPoint(FigureColor figureColor)
        {
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    var figure = _figures[row, column];
                    if (figure != null)
                    {
                        if (figure.FigureColor.Equals(figureColor))
                            yield return new ChessPoint(row, column);
                    }
                }
            }
        }

        public object Clone()
        {
            var result = new ChessDesk();
            for (int row = 0; row < 8; row++)
            {
                for (int column = 0; column < 8; column++)
                {
                    result._figures[row, column] = this._figures[row, column];
                }
            }
            return result;
        }
    
        public bool CheckShach(FigureColor kingColor)
        {
            var kingPoint = GetKingPoint(kingColor);
            var figures = GetFiguresPoint(kingColor == FigureColor.Black?FigureColor.White:FigureColor.Black);
            foreach (var figurePoint in figures)
            {
                var figure = _figures[figurePoint.Row, figurePoint.Column];
                if (figure.CheckMove(figurePoint,kingPoint,this))
                    return true;
            }
            return false;
        }
        

    
    
    }
}
