using System.Text;

namespace ChessUtilities.Library.Chess;

public class Chessboard
{
    public static Chessboard EmptyBoard { get =>  new Chessboard(ForsythEdwardsNotation.Empty); }    
    
    private char[,] _board = {
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'},
        {'\0','\0','\0','\0','\0','\0','\0','\0'}
    };

    public Chessboard(ForsythEdwardsNotation? gameState = null)
    {
        FromForsythEdwardsString(gameState?.BoardState ?? ForsythEdwardsNotation.Start.BoardState);
    }

    public void Move(Move move)
    {
        _board[move.Target.Rank, move.Target.File] = move.Piece.ToChar();
        _board[move.Source.Rank, move.Source.File] = '\0';
    }
    
    public override string ToString()
    {        
        List<StringBuilder> ranks = new List<StringBuilder>();

        for(int rank = 7; rank > -1; rank--)
        {
            ranks.Add(new StringBuilder());
            int emptySpaces = 0;
            for(int file = 7; file > -1; file--)
            {
                if(_board[rank,file] == '\0')
                {
                    emptySpaces++;
                    if(emptySpaces == 8 || file == 0)
                    {
                        ranks[7 - rank].Append(emptySpaces);
                    }
                }
                else
                {
                    if(emptySpaces > 0)
                    {
                        ranks[7 - rank].Append(emptySpaces);
                        emptySpaces = 0;
                        ranks[7 - rank].Append(_board[rank,file]);
                    }
                    else
                    {
                       ranks[7 - rank].Append(_board[rank,file]);
                    }
                }
            }
        }

        return string.Join('/', ranks.Select(r=>r.ToString()));
    }
    
    public string DumpBoard()
    {
        StringBuilder boardString = new StringBuilder("[ ");
        for(int rank = 7; rank > -1; rank--)
        {
            
            for(int file = 7; file > -1; file--)
            {
                switch(_board[rank,file])
                {
                    case '\0':
                    {
                        boardString.Append(" ");
                        break;
                    }
                    default:
                    {
                        boardString.Append(_board[rank,file]);
                        break;
                    }
                }
            }            
        }
        boardString.Append(" ]");
        return boardString.ToString();
    }

    private void FromForsythEdwardsString(string position)
    {
        // FEN starts at rank 8, file 8 and works it's way along ranks to rank 1, file 1 
        int rank = 7;
        int file = 7;
        
        foreach(char token in position)
        {    
            if(token >= '1' && token <= '8')
            {                
                int emptySpaces = token - '0';
                for(int i = 0; i < emptySpaces; i++)
                {
                    _board[rank,file-i] = '\0';
                }
                file -=emptySpaces;                 
            }
            else if(token == '/')
            {
                rank--;
                file = 7;
            }
            else
            {
                _board[rank,file--] = token;
            }            
        }
    }
}