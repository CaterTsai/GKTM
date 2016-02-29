using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphyData
{
    #region Enum
    public enum eVertexType
    {
        eVertex_Normal = 0
        ,eVertex_Order1
        ,eVertex_Order2
        ,eVertex_Order3
        ,eVertex_Order4
    }
    #endregion

    public class GraphyData
    {
        #region Element
        private int _VertexNum = 0;
        public Dictionary<int, List<int>> _EdgeMap = new Dictionary<int, List<int>>(); //for checking connection        
        #endregion

        #region Property
        public int VertexNum { get { return _VertexNum; } }        
        #endregion

        #region Basic Method
        public GraphyData()
        {
        }
        #endregion

        #region Method
        //---------------------------------------------------
        public void removeVertex(int id)
        {
            _VertexNum--;

            var vList_ = _EdgeMap[id];

            foreach (var vid_ in vList_)
            {
                _EdgeMap[vid_].RemoveAll(item => item == id);
            }
            _EdgeMap.Remove(id);
        }

        //---------------------------------------------------
        public void removeEdge(int from, int to)
        {
            _EdgeMap[from].RemoveAll(item => item == to);
            _EdgeMap[to].RemoveAll(item => item == from);
        }

        //---------------------------------------------------
        public void clearGraphy()
        {
            _EdgeMap.Clear();
        }

        //---------------------------------------------------
        public void generateK4()
        {
            _VertexNum = 4;

            //edge map
            for (int from_ = 0; from_ < 4; from_++)
            {
                List<int> edgeList_ = new List<int>();
                for (int to_ = 0; to_ < 4; to_++)
                {  
                    if (from_ != to_)
                    {
                        edgeList_.Add(to_);
                    }
                }
                _EdgeMap.Add(from_, edgeList_);
            }
        }

        //---------------------------------------------------
        public void generateK33()
        {
            _VertexNum = 6;

            //edge map
            for (int idx_ = 0; idx_ < 6; idx_++)
            {
                List<int> edgeList_ = new List<int>();

                if (idx_ < 3)
                {
                    edgeList_.Add(3);
                    edgeList_.Add(4);
                    edgeList_.Add(5);
                }
                else
                {
                    edgeList_.Add(0);
                    edgeList_.Add(1);
                    edgeList_.Add(2);
                }
                _EdgeMap.Add(idx_, edgeList_);
            }
            
        }

        //---------------------------------------------------
        public void generatePetersen()
        {
            _VertexNum = 10;
            int iHalfSize_ = 5;
            //edge map
            for (int idx_ = 0; idx_ < _VertexNum; idx_++)
            {
                List<int> edgeList_ = new List<int>();
                if (idx_ < iHalfSize_)
                {
                    edgeList_.Add((idx_ + 1) % iHalfSize_);
                    edgeList_.Add((idx_ - 1 + iHalfSize_) % iHalfSize_);
                    edgeList_.Add(idx_ + iHalfSize_);
                }
                else
                {
                    int index_ = idx_ - iHalfSize_;
                    edgeList_.Add(((index_ + 2) % iHalfSize_) + iHalfSize_);
                    edgeList_.Add((index_ - 2 + iHalfSize_) % iHalfSize_ + iHalfSize_);
                    edgeList_.Add(index_);
                }
                _EdgeMap.Add(idx_, edgeList_);
            }
        }

        //---------------------------------------------------
        public void generateHeawood()
        {
            _VertexNum = 14;

            for (int idx_ = 0; idx_ < _VertexNum; idx_++)
            {
                List<int> edgeList_ = new List<int>();

                if (idx_ % 2 == 0)
                {
                    edgeList_.Add((idx_ + 1) % _VertexNum);
                    edgeList_.Add((idx_ - 1 + _VertexNum) % _VertexNum);
                    edgeList_.Add((idx_ - 5 + _VertexNum) % _VertexNum);
                }
                else
                {
                    edgeList_.Add((idx_ + 1) % _VertexNum);
                    edgeList_.Add((idx_ - 1 + _VertexNum) % _VertexNum);
                    edgeList_.Add((idx_ + 5) % _VertexNum);
                }
                _EdgeMap.Add(idx_, edgeList_);
            }
        }
        #endregion

    }
}
