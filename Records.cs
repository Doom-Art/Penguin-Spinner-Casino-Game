using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Penguin_Spinner_Casino_Game
{
    internal class Records
    {
        public Records(int bet, int payout)
        {
            Bet = bet;
            Payout = payout;
        }
        public Records(int bet, int payout, bool coinFlip, bool heads)
        {
            Bet = bet;
            Payout = payout;
            Heads = heads;
            Flip = coinFlip;
        }
        public Records(int bet, int payout, bool dice, int roll1, int roll2)
        {
            Bet = bet;
            Payout = payout;
            Dice = dice;
            Roll1 = roll1;
            Roll2 = roll2;
        }
        public int Bet
        {
            get; set;
        }
        public int Payout
        {
            get; set;
        }
        public bool Flip
        {
            get; set;
        }
        public bool Heads
        {
            get;
            set;
        }
        public bool Dice
        {
            get; set;
        }
        public int Roll1
        {
            get; set;
        }
        public int Roll2
        {
            get; set;
        }
        public override string ToString()
        {
            if (Flip)
            {
                return $"Bet:{Bet}, Payout:{Payout}, Coin Flip: {Flip}, Heads: {Heads}";
            }
            else if (Dice)
            {
                return $"Bet:{Bet}, Payout:{Payout}, Dice: {Dice}, Roll1: {Roll1}, Roll2: {Roll2}";
            }
            else
            {
                return $"Bet:{Bet}, Payout:{Payout}";
            }
        }
    }
}
