using System.Collections;

namespace ANSEI.GemsCollector.Game
{
    public class GemData
    {
        internal int Row = 0;
        internal int Column = 0;
        internal int ColorId = 0;

        internal void Init(int row, int column, int colorId)
        {
            Row = row;
            Column = column;
            ColorId = colorId;
        }

        internal void UpdateData(int row, int column)
        {
            Row = row;
            Column = column;
        }

        internal bool Nearby(GemData data)
        {
            if (data.ColorId != ColorId)
                return false;

            return (data.Row >= Row - 1 && data.Row <= Row + 1) &&
                   (data.Column >= Column - 1 && data.Column <= Column + 1);

        }

        public override bool Equals(object obj)
        {
            var data = (GemData)obj;
            return data.Row == Row && data.Column == Column && data.ColorId == ColorId;
        }
    }
}