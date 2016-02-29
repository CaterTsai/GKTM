using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

static class constParameter
{
    public static readonly float cMOUSE_TRIGGER_DIST = 20.0f;

    //Vertex
    public static readonly int cVERTEX_NUM_INIT = 14;
    public static readonly int cVERTEX_NUM_MAX = 30;

    //Edge
    public static readonly int cEDGE_NUM_INIT = 21;
    public static readonly int cEDGE_NUM_MAX = 50;
    
    //Player
    public static readonly float cPLAYER_JUMP_HEIGHT = 3.0f;
    public static readonly float cPLAYER_JUMP_DURATION = 0.5f;
    public static readonly float cPLAYER_DROP_TRIGGER = -10.0f;

    //Game
    public static readonly int cNORMAL_ISLAND_SCORE = 2;
    public static readonly int cSPECIAL_ISLAND_SCORE = 10;

    //PlayerPref
    public static readonly string cPPREF_SCORE = "GKTM_SCORE";
}
